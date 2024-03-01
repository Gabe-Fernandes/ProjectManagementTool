using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;
using System.Security.Claims;

namespace PMT.Services.TimeTracker;

public class TimeTrackerHub(IStopwatchRepo stopwatchRepo,
	IShiftRepo shiftRepo,
	IHttpContextAccessor contextAccessor,
	IAppUserRepo appUserRepo) : Hub<ITimeTrackerHub>
{
	private readonly IStopwatchRepo _stopwatchRepo = stopwatchRepo;
	private readonly IShiftRepo _shiftRepo = shiftRepo;
	private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
	private readonly IAppUserRepo _appUserRepo = appUserRepo;



	public async Task GetStopwatches(int projId)
	{
		AppUser appuser = GetUser();

		List<Stopwatch> stopwatches = await _stopwatchRepo.GetAllFromUser(appuser.Id, projId);

		await Clients.Caller.PrintStopwatches(stopwatches);
	}

	public async Task CreateStopwatch(int projId)
	{
		AppUser appuser = GetUser();

		Stopwatch newStopwatch = new()
		{
			AppUserId = appuser.Id,
			ProjId = projId,
			Name = "New Stopwatch"
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

	public async Task DelStopWatch(string stopwatchIdAsString)
	{
		int stopwatchId = int.Parse(stopwatchIdAsString);

		List<Shift> relatedShifts = await _shiftRepo.GetAllFromStopwatch(stopwatchId);
		for (int i = 0; i < relatedShifts.Count; i++)
		{
			_shiftRepo.Delete(relatedShifts[i]);
		}

		Stopwatch stopwatchToDel = await _stopwatchRepo.GetByIdAsync(stopwatchId);
		_stopwatchRepo.Delete(stopwatchToDel);

		// callback function deletes html
	}



	public async Task CreateShift(string projIdAsString, string stopwatchIdAsString)
	{
		int projId = int.Parse(projIdAsString);
		int stopwatchId = int.Parse(stopwatchIdAsString);
		AppUser appuser = GetUser();

		Shift newShift = new()
		{
			ProjId = projId,
			AppUserId = appuser.Id,
			StopwatchId = stopwatchId,
			ClockIn = DateTime.UtcNow, // ==================================================
			ClockOut = DateTime.UtcNow, // ==================================================
			Date = DateTime.UtcNow, // ==================================================
			Hours = 0
		};
		_shiftRepo.Add(newShift);

		// render new shift
	}

	public async Task EditShift(string shiftIdAsString, object shiftFromClient)
	{
		int shiftId = int.Parse(shiftIdAsString);
		Shift shiftToEdit = await _shiftRepo.GetByIdAsync(shiftId);
		
		string shiftFromClientAsString = shiftFromClient.ToString();
		Shift clientObj = JsonConvert.DeserializeObject<Shift>(shiftFromClientAsString);

		if (clientObj.ClockOut > clientObj.ClockIn)
		{
			shiftToEdit.ClockIn = clientObj.ClockIn;
			shiftToEdit.ClockOut = clientObj.ClockOut;
			shiftToEdit.Hours = (int)(clientObj.ClockOut - clientObj.ClockIn).TotalHours;
			shiftToEdit.Date = clientObj.Date;
			_shiftRepo.Update(shiftToEdit);
		}

		// render shift changes
	}

	public async Task DelShift(string shiftIdAsString)
	{
		int shiftId = int.Parse(shiftIdAsString);

		Shift shiftToDel = await _shiftRepo.GetByIdAsync(shiftId);
		_shiftRepo.Delete(shiftToDel);

		// callback function deletes html
	}



	private AppUser GetUser()
	{
		string myId = _contextAccessor.HttpContext.User.FindFirstValue("Id");
		return _appUserRepo.GetById(myId);
	}
}
