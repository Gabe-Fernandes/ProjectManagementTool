namespace PMT.Services.TimeTracker;

public interface ITimeTrackerHub
{
	Task PrintStopwatch(int stopwatchId, string stopwatchName, bool clockIsRunning, DateTime clockRunningSince, List<TimeSetDto> timeSetDto);
	Task PrintTimeSet(int stopwatchId, int timeSetId, string timeSetHoursText);
	Task PrintTimeInterval(int stopwatchId, TimeIntervalDto timeIntervalDto);
	Task ClockOutTimeInterval(int stopwatchId, int timeIntervalId, string clockOut, double hours);
	Task PauseUpdate(int stopwatchId, double miliFromServer);
	Task DelStopwatch(int stopwatchId);
	Task PopulateEditIntervalModal(DateTime startDate, DateTime endDate);
	Task EditTimeInterval(int stopwatchId, int timeSetId, int timeIntervalId, bool isFromActiveTimeSet, double stopwatchMili, string timeSetMsg, TimeIntervalDto intervalDto);
	Task DelTimeInterval(int stopwatchId, int timeSetId, int timeIntervalId, bool isFromActiveTimeSet, bool isLastIntervalInTimeSet, string timeSetTrMsg, double stopwatchMili);
  Task TimeSetRefresh(int stopwatchId, int timeSetId, string timeSetText);
}
