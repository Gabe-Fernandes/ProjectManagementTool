namespace PMT.Services.TimeTracker;

public interface ITimeTrackerHub
{
	Task PrintStopwatch(int stopwatchId, string stopwatchName, bool clockIsRunning, List<TimeSetDto> timeSetDto);
	Task PrintTimeSet(int stopwatchId, int timeSetId, string timeSetHours);
	Task PrintTimeInterval(int stopwatchId, TimeIntervalDto timeIntervalDto);
	Task ClockOutTimeInterval(int stopwatchId, int timeIntervalId, string clockOut, double hours);
	Task DelStopwatch(int stopwatchId);
}
