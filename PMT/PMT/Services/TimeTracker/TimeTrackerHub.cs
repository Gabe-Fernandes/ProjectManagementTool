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



	public async Task GetStopwatches() // consider a different algorithm for this and measure the two
	{
		AppUser appuser = GetUser();
		int projId = appuser.CurrentProjId;

		List<Stopwatch> stopwatches = await _stopwatchRepo.GetAllFromUser(appuser.Id, projId);

		for (int i = 0; i < stopwatches.Count; i++)
		{
			bool clockIsRunning = false;
			DateTime clockRunningSince = GetEasternTime();

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
					TimeSetMilli = timeSets[j].Milliseconds,
					TimeSetHoursMsg = $"{Math.Round(timeSets[j].Milliseconds / 3600000, 2)} hours since last reset",
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

	public async Task CreateStopwatch()
	{
		AppUser appuser = GetUser();
		int projId = appuser.CurrentProjId;

		Stopwatch newStopwatch = new()
		{
			AppUserId = appuser.Id,
			ProjId = projId,
			Name = "New Stopwatch",
			Milliseconds = 0
		};
		_stopwatchRepo.Add(newStopwatch);

		TimeSet newTimeSet = new()
		{
			ProjId = projId,
			AppUserId = appuser.Id,
			Milliseconds = 0,
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



	public async Task StartBtn(int stopwatchId, int timeSetId)
	{
		AppUser appuser = GetUser();
		int projId = appuser.CurrentProjId;

		TimeInterval newTimeInterval = new()
		{
			ProjId = projId,
			AppUserId = appuser.Id,
			StopwatchId = stopwatchId,
			TimeSetId = timeSetId,
			StartDate = GetEasternTime(),
			Milliseconds = 0
		};
		_timeIntervalRepo.Add(newTimeInterval);

		TimeIntervalDto dto = new(newTimeInterval.Id, newTimeInterval.StartDate, DateTime.MinValue);
		await Clients.Caller.PrintTimeInterval(stopwatchId, dto, 0, string.Empty, 0);
	}

	public async Task PauseBtn(int timeIntervalId, bool isReset, bool clockWasStopped)
	{
    TimeInterval timeIntervalToEdit = await _timeIntervalRepo.GetByIdAsync(timeIntervalId);

    if (clockWasStopped)
		{
      timeIntervalToEdit.EndDate = GetEasternTime();
      timeIntervalToEdit.Milliseconds = (timeIntervalToEdit.EndDate - timeIntervalToEdit.StartDate).TotalMilliseconds;
      _timeIntervalRepo.Update(timeIntervalToEdit);

			TimeSet parentTimeSet = await _timeSetRepo.GetByIdAsync(timeIntervalToEdit.TimeSetId);
			parentTimeSet.Milliseconds += timeIntervalToEdit.Milliseconds;
			_timeSetRepo.Update(parentTimeSet);
		}

		Stopwatch parentStopwatch = await _stopwatchRepo.GetByIdAsync(timeIntervalToEdit.StopwatchId);
		parentStopwatch.Milliseconds = isReset ? 0: parentStopwatch.Milliseconds + timeIntervalToEdit.Milliseconds;
		_stopwatchRepo.Update(parentStopwatch);

		double roundedHours = Math.Round(timeIntervalToEdit.Milliseconds / 3600000, 2);

		await Clients.Caller.PauseUpdate(parentStopwatch.Id, parentStopwatch.Milliseconds);
		await Clients.Caller.ClockOutTimeInterval(timeIntervalToEdit.StopwatchId, timeIntervalId, timeIntervalToEdit.EndDate.ToString("t"), roundedHours);
	}

	public async Task ResetBtn(int stopwatchId, int timeIntervalId, bool clockWasStopped)
	{
		AppUser appuser = GetUser();
		int projId = appuser.CurrentProjId;

		TimeSet newTimeSet = new()
		{
			ProjId = projId,
			AppUserId = appuser.Id,
			Milliseconds = 0,
			StopwatchId = stopwatchId
		};
		_timeSetRepo.Add(newTimeSet);

		await PauseBtn(timeIntervalId, isReset: true, clockWasStopped);
		await Clients.Caller.PrintTimeSet(stopwatchId, newTimeSet.Id, "no records in this time set yet");
	}

	public async Task TimeSetRefresh(int stopwatchId, int timeSetId)
	{
		TimeSet timeset = await _timeSetRepo.GetByIdAsync(timeSetId);
		string timeSetText = Math.Round(timeset.Milliseconds / 3600000, 2).ToString();

		await Clients.Caller.TimeSetRefresh(stopwatchId, timeSetId, timeSetText);
	}



	public async Task GetDatesToEdit(int timeIntervalId)
	{
		TimeInterval interval = await _timeIntervalRepo.GetByIdAsync(timeIntervalId);
		await Clients.Caller.PopulateEditIntervalModal(interval.StartDate, interval.EndDate);
	}

	public async Task CustomTimeInterval(int stopwatchId, object timeIntervalFromClient)
	{
		string timeIntervalFromClientAsString = timeIntervalFromClient.ToString();
		TimeInterval clientObj = JsonConvert.DeserializeObject<TimeInterval>(timeIntervalFromClientAsString);
		
		// the last timeSet in this query should be the current one
		List<TimeSet> timeSets = await _timeSetRepo.GetAllFromStopwatch(stopwatchId);
		TimeSet currentTimeSet = timeSets[timeSets.Count - 1];
		Stopwatch stopwatch = await _stopwatchRepo.GetByIdAsync(stopwatchId);

		if (clientObj.EndDate > clientObj.StartDate)
		{
			AppUser appuser = GetUser();
			int projId = appuser.CurrentProjId;

			TimeInterval newTimeInterval = new()
			{
				ProjId = projId,
				AppUserId = appuser.Id,
				StopwatchId = stopwatchId,
				TimeSetId = currentTimeSet.Id,
				StartDate = clientObj.StartDate,
				EndDate = clientObj.EndDate,
				Milliseconds = (clientObj.EndDate - clientObj.StartDate).TotalMilliseconds
			};
			_timeIntervalRepo.Add(newTimeInterval);

			// update related records
			currentTimeSet.Milliseconds += newTimeInterval.Milliseconds;
			_timeSetRepo.Update(currentTimeSet);
			stopwatch.Milliseconds += newTimeInterval.Milliseconds;
			_stopwatchRepo.Update(stopwatch);

			string timeSetMsg = $"{Math.Round(currentTimeSet.Milliseconds / 3600000, 2)} hours since last reset";
			TimeIntervalDto dto = new(newTimeInterval.Id, newTimeInterval.StartDate, newTimeInterval.EndDate);
			await Clients.Caller.PrintTimeInterval(stopwatchId, dto, currentTimeSet.Id, timeSetMsg, stopwatch.Milliseconds);
		}
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
			TimeSet timeSetToUpdate = await _timeSetRepo.GetByIdAsync(timeIntervalToEdit.TimeSetId);
			bool isActiveTimeSet = stopwatchToUpdate.Milliseconds == timeSetToUpdate.Milliseconds; // while highly unlikely, this check could be wrong

			timeSetToUpdate.Milliseconds -= timeIntervalToEdit.Milliseconds;
			if (isActiveTimeSet)
			{
				stopwatchToUpdate.Milliseconds -= timeIntervalToEdit.Milliseconds;
			}

			// update the timeInterval
			timeIntervalToEdit.StartDate = clientObj.StartDate;
			timeIntervalToEdit.EndDate = clientObj.EndDate;
			timeIntervalToEdit.Milliseconds = (clientObj.EndDate - clientObj.StartDate).TotalMilliseconds;

			// update related objects with the new timeInterval data
			timeSetToUpdate.Milliseconds += timeIntervalToEdit.Milliseconds;
			if (isActiveTimeSet)
			{
				stopwatchToUpdate.Milliseconds += timeIntervalToEdit.Milliseconds;
			}

			// save everything to the db
			_timeIntervalRepo.Update(timeIntervalToEdit);
			_timeSetRepo.Update(timeSetToUpdate);
			if (isActiveTimeSet)
			{
				_stopwatchRepo.Update(stopwatchToUpdate);
			}

			string timeSetMsg = $"{Math.Round(timeSetToUpdate.Milliseconds / 3600000, 2)} hours since last reset";
			TimeIntervalDto intervalDto = new(timeIntervalToEdit.Id, timeIntervalToEdit.StartDate, timeIntervalToEdit.EndDate);
			await Clients.Caller.EditTimeInterval(stopwatchToUpdate.Id, timeSetToUpdate.Id, timeIntervalId, isActiveTimeSet, stopwatchToUpdate.Milliseconds, timeSetMsg, intervalDto);
		}
	}

	public async Task DelTimeInterval(int timeIntervalId)
	{
		TimeInterval timeIntervalToDel = await _timeIntervalRepo.GetByIdAsync(timeIntervalId);

		TimeSet timeSetToChange = await _timeSetRepo.GetByIdAsync(timeIntervalToDel.TimeSetId);
		List<TimeInterval> intervalQuantityList = await _timeIntervalRepo.GetAllFromTimeSet(timeSetToChange.Id);
		bool isLastIntervalInTimeSet = intervalQuantityList.Count == 1;

    Stopwatch stopwatchToChange = await _stopwatchRepo.GetByIdAsync(timeIntervalToDel.StopwatchId);
		bool isFromActiveTimeSet = stopwatchToChange.Milliseconds == timeSetToChange.Milliseconds; // while highly unlikely, this check could be wrong

    _timeIntervalRepo.Delete(timeIntervalToDel);

    if (isFromActiveTimeSet)
		{
			if (isLastIntervalInTimeSet)
			{
        timeSetToChange.Milliseconds = 0;
        _timeSetRepo.Update(timeSetToChange);
				stopwatchToChange.Milliseconds = 0;
				_stopwatchRepo.Update(stopwatchToChange);
      }
			else
			{
        timeSetToChange.Milliseconds -= timeIntervalToDel.Milliseconds;
        _timeSetRepo.Update(timeSetToChange);
        stopwatchToChange.Milliseconds -= timeIntervalToDel.Milliseconds;
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
        timeSetToChange.Milliseconds -= timeIntervalToDel.Milliseconds;
        _timeSetRepo.Update(timeSetToChange);
      }
    }

    string timeSetTrMsg = $"{Math.Round(timeSetToChange.Milliseconds / 3600000, 2)} hours since last reset";
    await Clients.Caller.DelTimeInterval(stopwatchToChange.Id, timeSetToChange.Id, timeIntervalId, isFromActiveTimeSet, isLastIntervalInTimeSet, timeSetTrMsg, stopwatchToChange.Milliseconds);
  }



	private AppUser GetUser()
	{
		string myId = _contextAccessor.HttpContext.User.FindFirstValue("Id");
		return _appUserRepo.GetById(myId);
	}

	private static DateTime GetEasternTime()
	{
		return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time"));
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
	public double TimeSetMilli { get; set; }
	public string TimeSetHoursMsg { get; set; }
	public List<TimeIntervalDto> Intervals { get; set; }
}
