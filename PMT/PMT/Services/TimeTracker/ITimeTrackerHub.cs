namespace PMT.Services.TimeTracker;

public interface ITimeTrackerHub
{
	Task PrintStopwatch(int stopwatchId, string stopwatchName, List<TimeSetDto> timeSetDto);
	Task ApplyStopwatchId(int stopwatchId);
	Task DelStopwatch(int stopwatchId);
}
