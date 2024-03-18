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
					TimeSetHoursMsg = $"{Math.Round(timeSets[j].Hours / 3600000, 2)} hours since last reset",
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

		await Clients.Caller.PrintTimeSet(newStopwatch.Id, newTimeSet.Id, "no records in this time set yet");
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

	public async Task PauseBtn(int timeIntervalId, bool isReset, bool clockWasStopped)
	{
    TimeInterval timeIntervalToEdit = await _timeIntervalRepo.GetByIdAsync(timeIntervalId);

    if (clockWasStopped)
		{
      timeIntervalToEdit.EndDate = DateTime.Now;
      timeIntervalToEdit.Hours = (timeIntervalToEdit.EndDate - timeIntervalToEdit.StartDate).TotalMilliseconds;
      _timeIntervalRepo.Update(timeIntervalToEdit);
    }

		TimeSet parentTimeSet = await _timeSetRepo.GetByIdAsync(timeIntervalToEdit.TimeSetId);
		parentTimeSet.Hours += timeIntervalToEdit.Hours;
		_timeSetRepo.Update(parentTimeSet);

		Stopwatch parentStopwatch = await _stopwatchRepo.GetByIdAsync(timeIntervalToEdit.StopwatchId);
		parentStopwatch.TotalHours = isReset ? 0: parentStopwatch.TotalHours + timeIntervalToEdit.Hours;
		_stopwatchRepo.Update(parentStopwatch);

		double roundedHours = Math.Round(timeIntervalToEdit.Hours / 3600000, 2);

		await Clients.Caller.PauseUpdate(parentStopwatch.Id, parentStopwatch.TotalHours);
		await Clients.Caller.ClockOutTimeInterval(timeIntervalToEdit.StopwatchId, timeIntervalId, timeIntervalToEdit.EndDate.ToString("t"), roundedHours);
	}

	public async Task ResetBtn(int projId, int stopwatchId, int timeIntervalId, bool clockWasStopped)
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

		await PauseBtn(timeIntervalId, isReset: true, clockWasStopped);
		await Clients.Caller.PrintTimeSet(stopwatchId, newTimeSet.Id, "no records in this time set yet");
	}

	public async Task TimeSetRefresh(int stopwatchId, int timeSetId)
	{
		TimeSet timeset = await _timeSetRepo.GetByIdAsync(timeSetId);
		string timeSetText = Math.Round(timeset.Hours / 3600000, 2).ToString();

		await Clients.Caller.TimeSetRefresh(stopwatchId, timeSetId, timeSetText);
	}



	public async Task EditTimeInterval(int timeIntervalId, object timeIntervalFromClient)
	{
		TimeInterval timeIntervalToEdit = await _timeIntervalRepo.GetByIdAsync(timeIntervalId);

		string timeIntervalFromClientAsString = timeIntervalFromClient.ToString();
		TimeInterval clientObj = JsonConvert.DeserializeObject<TimeInterval>(timeIntervalFromClientAsString);

		if (clientObj.EndDate > clientObj.StartDate)
		{
			// undo whatever affect the timeInterval had on these objects
			Stopwatch stopwatchToUpdate = await _stopwatchRepo.GetByIdAsync(timeIntervalToEdit.StopwatchId);
			stopwatchToUpdate.TotalHours -= timeIntervalToEdit.Hours;

			TimeSet timeSetToUpdate = await _timeSetRepo.GetByIdAsync(timeIntervalToEdit.TimeSetId);
			timeSetToUpdate.Hours -= timeIntervalToEdit.Hours;

			// update the timeInterval
			timeIntervalToEdit.StartDate = clientObj.StartDate;
			timeIntervalToEdit.EndDate = clientObj.EndDate;
			timeIntervalToEdit.Hours = Math.Round((double)(clientObj.EndDate - clientObj.StartDate).Milliseconds / 3600000, 2);

			// update related objects with the new timeInterval data
			stopwatchToUpdate.TotalHours += timeIntervalToEdit.Hours;
			timeSetToUpdate.Hours += timeIntervalToEdit.Hours;

			// save everything to the db
			_timeIntervalRepo.Update(timeIntervalToEdit);
			_timeSetRepo.Update(timeSetToUpdate);
			_stopwatchRepo.Update(stopwatchToUpdate);

			// callback needs to update html for timeSet, timeInterval hours, and perhaps stopwatch timer
		}
	}

	public async Task DelTimeInterval(int timeIntervalId)
	{
		TimeInterval timeIntervalToDel = await _timeIntervalRepo.GetByIdAsync(timeIntervalId);

		TimeSet timeSetToChange = await _timeSetRepo.GetByIdAsync(timeIntervalToDel.TimeSetId);
		List<TimeInterval> intervalQuantityList = await _timeIntervalRepo.GetAllFromTimeSet(timeSetToChange.Id);
		bool isLastIntervalInTimeSet = intervalQuantityList.Count == 1;

    Stopwatch stopwatchToChange = await _stopwatchRepo.GetByIdAsync(timeIntervalToDel.StopwatchId);
		bool isFromActiveTimeSet = stopwatchToChange.TotalHours == timeSetToChange.Hours; // while highly unlikely, this check could be wrong

    _timeIntervalRepo.Delete(timeIntervalToDel);

    if (isFromActiveTimeSet)
		{
			if (isLastIntervalInTimeSet)
			{
        timeSetToChange.Hours = 0;
        _timeSetRepo.Update(timeSetToChange);
				stopwatchToChange.TotalHours = 0;
				_stopwatchRepo.Update(stopwatchToChange);
      }
			else
			{
        timeSetToChange.Hours -= timeIntervalToDel.Hours;
        _timeSetRepo.Update(timeSetToChange);
        stopwatchToChange.TotalHours -= timeIntervalToDel.Hours;
        _stopwatchRepo.Update(stopwatchToChange);
      }
		}
		else
		{
      if (isLastIntervalInTimeSet)
      {
				_timeSetRepo.Delete(timeSetToChange);
      }
      else
      {
        timeSetToChange.Hours -= timeIntervalToDel.Hours;
        _timeSetRepo.Update(timeSetToChange);
      }
    }

    string timeSetTrMsg = $"{Math.Round(timeSetToChange.Hours / 3600000, 2)} hours since last reset";
    await Clients.Caller.DelTimeInterval(stopwatchToChange.Id, timeSetToChange.Id, timeIntervalId, isFromActiveTimeSet, isLastIntervalInTimeSet, timeSetTrMsg, stopwatchToChange.TotalHours);
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
	public double TimeElapsed { get; set; } = (endDate == DateTime.MinValue) ? 0 : Math.Round((endDate - startDate).TotalMilliseconds / 3600000, 2);
}

public class TimeSetDto
{
	public int Id { get; set; }
	public double TimeSetMili { get; set; }
	public string TimeSetHoursMsg { get; set; }
	public List<TimeIntervalDto> Intervals { get; set; }
}
