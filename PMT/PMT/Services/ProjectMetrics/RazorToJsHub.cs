using Microsoft.AspNetCore.SignalR;
using PMT.Data.RepoInterfaces;

namespace PMT.Services.ProjectMetrics;

public class RazorToJsHub(IProjectRepo projectRepo,
  IStoryRepo storyRepo,
  IBugReportRepo bugReportRepo) : Hub<IRazorToJsHub>
{
  private readonly IProjectRepo _projectRepo = projectRepo;
  private readonly IStoryRepo _storyRepo = storyRepo;
  private readonly IBugReportRepo _bugReportRepo = bugReportRepo;

  public async Task PackagePieChart(string projIdAsString)
  {
    int projId = int.Parse(projIdAsString);
    var stories = await _storyRepo.GetAllUnresolvedStoriesWithSearchFilterAsync(projId, string.Empty);
    var bugReports = await _bugReportRepo.GetAllUnresolvedReportsAsync(projId, string.Empty);
    PieChartData pieChartData = new(stories.ToList(), bugReports.ToList());
    await Clients.Caller.ReceivePieChartData(pieChartData);
  }

  public async Task PackageBarGraph(string projIdAsString)
  {
    int projId = int.Parse(projIdAsString);
    var project = await _projectRepo.GetByIdAsync(projId);

    var resolvedStories = await _storyRepo.GetAllResolved(projId);
    var resolvedBugReports = await _bugReportRepo.GetAllResolved(projId);

    BarGraphData barGraphData = new(project, resolvedStories.ToList(), resolvedBugReports.ToList());
    await Clients.Caller.ReceiveBarGraphData(barGraphData);
  }
}
