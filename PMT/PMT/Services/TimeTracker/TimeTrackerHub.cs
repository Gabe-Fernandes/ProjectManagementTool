using Microsoft.AspNetCore.SignalR;
using PMT.Data.RepoInterfaces;

namespace PMT.Services.TimeTracker;

public class TimeTrackerHub(IStopwatchRepo stopwatchRepo,
	IShiftRepo shiftRepo) : Hub<ITimeTrackerHub>
{
	private readonly IStopwatchRepo _stopwatchRepo = stopwatchRepo;
	private readonly IShiftRepo _shiftRepo = shiftRepo;




}
