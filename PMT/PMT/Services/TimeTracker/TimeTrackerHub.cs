using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;
using System.Security.Claims;

namespace PMT.Services.TimeTracker;

public class TimeTrackerHub(IStopwatchRepo stopwatchRepo,
	IHttpContextAccessor contextAccessor,
	IAppUserRepo appUserRepo,
	ITimeSetRepo timeSetRepo,
	ITimeIntervalRepo timeIntervalRepo) : Hub<ITimeTrackerHub>
{
	private readonly IStopwatchRepo _stopwatchRepo = stopwatchRepo;
	private readonly ITimeIntervalRepo _timeIntervalRepo = timeIntervalRepo;
	private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
	private readonly IAppUserRepo _appUserRepo = appUserRepo;
	private readonly ITimeSetRepo _timeSetRepo = timeSetRepo;



	public async Task GetStopwatches(int projId) // consider a different algorithm for this and measure the two
	{
		AppUser appuser = GetUser();

		List<Stopwatch> stopwatches = await _stopwatchRepo.GetAllFromUser(appuser.Id, projId);

		for (int i = 0; i < stopwatches.Count; i++)
		{
			bool clockIsRunning = false;
			DateTime clockRunningSince = DateTime.Now;

			List<TimeSetDto> timeSetDtoList = [];
			List<TimeSet> timeSets = await _timeSetRepo.GetAllFromStopwatch(stopwatches[i].Id);

			for (int j = 0; j < timeSets.Count; j++)
			{
				List<TimeIntervalDto> timeIntervalDtoList = [];
				List<TimeInterval> intervals = await _timeIntervalRepo.GetAllFromTimeSet(timeSets[j].Id);
				for (int k = 0; k < intervals.Count; k++)
				{
					TimeIntervalDto intervalDto = new(intervals[k].Id, intervals[k].StartDate, intervals[k].EndDate);
					timeIntervalDtoList.Add(intervalDto);
				}

				TimeSetDto timeSetDto = new()
				{
					Id = timeSets[j].Id,
					TimeSetMili = timeSets[j].Hours,
					TimeSetHoursMsg = $"{timeSets[j].Hours} hours since last reset",
					Intervals = timeIntervalDtoList
				};
				timeSetDtoList.Add(timeSetDto);

				if (j == timeSets.Count - 1 && intervals.Count > 0) // if the last Interval in the last TimeSet doesn't have an end date, the clock is still running
				{
					clockIsRunning = intervals[intervals.Count - 1].EndDate == DateTime.MinValue;
					clockRunningSince = intervals[intervals.Count - 1].StartDate;
				}
			}

			await Clients.Caller.PrintStopwatch(stopwatches[i].Id, stopwatches[i].Name, clockIsRunning, clockRunningSince, timeSetDtoList);
		}
	}

	public async Task CreateStopwatch(int projId)
	{
		AppUser appuser = GetUser();

		Stopwatch newStopwatch = new()
		{
			AppUserId = appuser.Id,
			ProjId = projId,
			Name = "New Stopwatch",
			TotalHours = 0
		};
		_stopwatchRepo.Add(newStopwatch);

		TimeSet newTimeSet = new()
		{
			ProjId = projId,
			AppUserId = appuser.Id,
			Hours = 0,
			StopwatchId = newStopwatch.Id
		};
		_timeSetRepo.Add(newTimeSet);

		await Clients.Caller.PrintTimeSet(newStopwatch.Id, newTimeSet.Id, "0 hours");
	}

	public async Task EditStopWatch(int stopwatchId, string newName)
	{
		Stopwatch stopwatchToEdit = await _stopwatchRepo.GetByIdAsync(stopwatchId);

		if (newName != string.Empty)
		{
			stopwatchToEdit.Name = newName;
			_stopwatchRepo.Update(stopwatchToEdit);
		}
	}

	public async Task DelStopWatch(int stopwatchId)
	{
		List <TimeSet> relatedTimeSets = await _timeSetRepo.GetAllFromStopwatch(stopwatchId);
		for (int i = 0; i < relatedTimeSets.Count; i++)
		{
			_timeSetRepo.Delete(relatedTimeSets[i]);
		}
		List<TimeInterval> relatedIntervals = await _timeIntervalRepo.GetAllFromStopwatch(stopwatchId);
		for (int i = 0; i < relatedIntervals.Count; i++)
		{
			_timeIntervalRepo.Delete(relatedIntervals[i]);
		}

		Stopwatch stopwatchToDel = await _stopwatchRepo.GetByIdAsync(stopwatchId);
		_stopwatchRepo.Delete(stopwatchToDel);

		await Clients.Caller.DelStopwatch(stopwatchId);
	}



	public async Task StartBtn(int projId, int stopwatchId, int timeSetId)
	{
		AppUser appuser = GetUser();

		TimeInterval newTimeInterval = new()
		{
			ProjId = projId,
			AppUserId = appuser.Id,
			StopwatchId = stopwatchId,
			TimeSetId = timeSetId,
			StartDate = DateTime.Now,
			Hours = 0
		};
		_timeIntervalRepo.Add(newTimeInterval);

		TimeIntervalDto dto = new(newTimeInterval.Id, newTimeInterval.StartDate, DateTime.MinValue);
		await Clients.Caller.PrintTimeInterval(stopwatchId, dto);
	}

	public async Task PauseBtn(int timeIntervalId)
	{
		TimeInterval timeIntervalToEdit = await _timeIntervalRepo.GetByIdAsync(timeIntervalId);
		timeIntervalToEdit.EndDate = DateTime.Now;
		timeIntervalToEdit.Hours = (timeIntervalToEdit.EndDate - timeIntervalToEdit.StartDate).TotalMilliseconds;
		_timeIntervalRepo.Update(timeIntervalToEdit);

		TimeSet parentTimeSet = await _timeSetRepo.GetByIdAsync(timeIntervalToEdit.TimeSetId);
		parentTimeSet.Hours += timeIntervalToEdit.Hours;
		_timeSetRepo.Update(parentTimeSet);

		Stopwatch parentStopwatch = await _stopwatchRepo.GetByIdAsync(timeIntervalToEdit.StopwatchId);
		parentStopwatch.TotalHours += timeIntervalToEdit.Hours;
		_stopwatchRepo.Update(parentStopwatch);

		await Clients.Caller.PauseUpdate(parentStopwatch.Id, parentTimeSet.Hours);
		await Clients.Caller.ClockOutTimeInterval(timeIntervalToEdit.StopwatchId, timeIntervalId, timeIntervalToEdit.EndDate.ToString("t"), timeIntervalToEdit.Hours);
	}

	public async Task ResetBtn(int projId, int stopwatchId, int timeIntervalId)
	{
		AppUser appuser = GetUser();

		TimeSet newTimeSet = new()
		{
			ProjId = projId,
			AppUserId = appuser.Id,
			Hours = 0,
			StopwatchId = stopwatchId
		};
		_timeSetRepo.Add(newTimeSet);

		await PauseBtn(timeIntervalId);

		Stopwatch parentStopwatch = await _stopwatchRepo.GetByIdAsync(stopwatchId);
		parentStopwatch.TotalHours = 0;
		_stopwatchRepo.Update(parentStopwatch);

		await Clients.Caller.PrintTimeSet(0, newTimeSet.Id, "0 hours");
	}

	public async Task EditTimeInterval(int timeIntervalId, object timeIntervalFromClient)
	{
		TimeInterval timeIntervalToEdit = await _timeIntervalRepo.GetByIdAsync(timeIntervalId);

		string timeIntervalFromClientAsString = timeIntervalFromClient.ToString();
		TimeInterval clientObj = JsonConvert.DeserializeObject<TimeInterval>(timeIntervalFromClientAsString);

		if (clientObj.EndDate > clientObj.StartDate)
		{
			timeIntervalToEdit.StartDate = clientObj.StartDate;
			timeIntervalToEdit.EndDate = clientObj.EndDate;
			timeIntervalToEdit.Hours = (clientObj.EndDate - clientObj.StartDate).Seconds / 3600;
			_timeIntervalRepo.Update(timeIntervalToEdit);
		}
	}

	public async Task DelTimeInterval(int timeIntervalId)
	{
		TimeInterval timeIntervalToDel = await _timeIntervalRepo.GetByIdAsync(timeIntervalId);
		_timeIntervalRepo.Delete(timeIntervalToDel);

		// needs to affect TimeSet

		// callback function deletes html and timeSet displays correct hours
	}



	private AppUser GetUser()
	{
		string myId = _contextAccessor.HttpContext.User.FindFirstValue("Id");
		return _appUserRepo.GetById(myId);
	}
}

public class TimeIntervalDto(int id, DateTime startDate, DateTime endDate)
{
	public int Id { get; set; } = id;
	public string StartDate { get; set; } = startDate.ToString("M/d");
	public string ClockIn { get; set; } = startDate.ToString("t");
	public string ClockOut { get; set; } = (endDate == DateTime.MinValue) ? string.Empty : endDate.ToString("t");
	public double TimeElapsed { get; set; } = (endDate == DateTime.MinValue) ? 0 : (endDate - startDate).TotalMilliseconds;
}

public class TimeSetDto
{
	public int Id { get; set; }
	public double TimeSetMili { get; set; }
	public string TimeSetHoursMsg { get; set; }
	public List<TimeIntervalDto> Intervals { get; set; }
}
