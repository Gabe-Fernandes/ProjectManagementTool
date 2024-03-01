using PMT.Data.Models;

namespace PMT.Services.TimeTracker;

public interface ITimeTrackerHub
{
	Task PrintStopwatches(List<Stopwatch> stopwatches);
	Task ApplyStopwatchId(int stopwatchId);
}
