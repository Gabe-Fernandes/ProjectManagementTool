using Microsoft.AspNetCore.SignalR;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;
using System.Security.Claims;

namespace PMT.Services.ProjectMetrics;

public class RazorToJsHub(IProjectRepo projectRepo,
  IStoryRepo storyRepo,
  IBugReportRepo bugReportRepo,
  IAppUserRepo appUserRepo,
  IHttpContextAccessor contextAccessor) : Hub<IRazorToJsHub>
{
  private readonly IProjectRepo _projectRepo = projectRepo;
  private readonly IStoryRepo _storyRepo = storyRepo;
  private readonly IBugReportRepo _bugReportRepo = bugReportRepo;
  private readonly IAppUserRepo _appUserRepo = appUserRepo;
  private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

  public async Task PackagePieChart()
  {
    int projId = GetUser().CurrentProjId;

    var stories = await _storyRepo.GetAllUnresolvedStoriesWithSearchFilterAsync(projId, string.Empty);
    var bugReports = await _bugReportRepo.GetAllUnresolvedReportsAsync(projId, string.Empty);
    PieChartData pieChartData = new(stories.ToList(), bugReports.ToList());
    await Clients.Caller.ReceivePieChartData(pieChartData);
  }

  public async Task PackageBarGraph()
  {
    int projId = GetUser().CurrentProjId;
    var project = await _projectRepo.GetByIdAsync(projId);

    var resolvedStories = await _storyRepo.GetAllResolved(projId);
    var resolvedBugReports = await _bugReportRepo.GetAllResolved(projId);

    BarGraphData barGraphData = new(project, resolvedStories.ToList(), resolvedBugReports.ToList());
    await Clients.Caller.ReceiveBarGraphData(barGraphData);
  }

  public async Task PackageBurnDownChart()
  {
    int projId = GetUser().CurrentProjId;
    var project = await _projectRepo.GetByIdAsync(projId);

    var stories = await _storyRepo.GetAllWithSearchFilterAsync(projId, string.Empty);
    var bugReports = await _bugReportRepo.GetAllAsync(projId, string.Empty);

    BurnDownChartData burnDownChartData = new(project, stories.ToList(), bugReports.ToList());
    await Clients.Caller.ReceiveBurnDownChartData(burnDownChartData);
  }

  private AppUser GetUser()
  {
    string myId = _contextAccessor.HttpContext.User.FindFirstValue("Id");
    return _appUserRepo.GetById(myId);
  }
}
