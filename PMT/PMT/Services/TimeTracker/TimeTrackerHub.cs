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
			List<TimeSetDto> timeSetDtoList = [];
			List<TimeSet> timeSets = await _timeSetRepo.GetAllFromStopwatch(stopwatches[i].Id);

			for (int j = 0; j < timeSets.Count; j++)
			{
				List<TimeIntervalDto> timeIntervalDtoList = [];
				List<TimeInterval> intervals = await _timeIntervalRepo.GetAllFromTimeSet(timeSets[j].Id);
				for (int k = 0; k < intervals.Count; k++)
				{
					TimeIntervalDto intervalDto = new(intervals[k].StartDate, intervals[k].EndDate);
					timeIntervalDtoList.Add(intervalDto);
				}

				TimeSetDto timeSetDto = new()
				{
					TimeSetHours = $"{timeSets[j].Hours} hours since last reset",
					Intervals = timeIntervalDtoList
				};
				timeSetDtoList.Add(timeSetDto);
			}

			await Clients.Caller.PrintStopwatch(stopwatches[i].Id, stopwatches[i].Name, timeSetDtoList);
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

		await Clients.Caller.ApplyStopwatchId(newStopwatch.Id);
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
		List<TimeInterval> relatedIntervals = await _timeIntervalRepo.GetAllFromStopwatch(stopwatchId);
		for (int i = 0; i < relatedIntervals.Count; i++)
		{
			_timeIntervalRepo.Delete(relatedIntervals[i]);
		}

		Stopwatch stopwatchToDel = await _stopwatchRepo.GetByIdAsync(stopwatchId);
		_stopwatchRepo.Delete(stopwatchToDel);

		await Clients.Caller.DelStopwatch(stopwatchId);
	}



	public async Task CreateTimeInterval(int projId, int stopwatchId)
	{
		AppUser appuser = GetUser();

		TimeInterval newTimeInterval = new()
		{
			ProjId = projId,
			AppUserId = appuser.Id,
			StopwatchId = stopwatchId,
			//TimeSetId = ???????????????????????
			StartDate = DateTime.Now.Date,
			Hours = 0
		};
		_timeIntervalRepo.Add(newTimeInterval);

		// render new shift
	}

	public async Task ResetBtn(int projId, int stopwatchId)
	{

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
			// compute hours
			_timeIntervalRepo.Update(timeIntervalToEdit);
		}

		// render shift changes
	}

	public async Task DelShift(int timeIntervalId)
	{
		TimeInterval timeIntervalToDel = await _timeIntervalRepo.GetByIdAsync(timeIntervalId);
		_timeIntervalRepo.Delete(timeIntervalToDel);

		// callback function deletes html
	}



	private AppUser GetUser()
	{
		string myId = _contextAccessor.HttpContext.User.FindFirstValue("Id");
		return _appUserRepo.GetById(myId);
	}
}

public class TimeIntervalDto(DateTime startDate, DateTime endDate)
{
	public string StartDate { get; set; } = startDate.ToString("M/d");
	public string ClockIn { get; set; } = startDate.ToString("t");
	public string ClockOut { get; set; } = endDate.ToString("t");
}

public class TimeSetDto
{
	public string TimeSetHours { get; set; }
	public List<TimeIntervalDto> Intervals { get; set; }
}
