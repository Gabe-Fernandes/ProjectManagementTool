using PMT.Data.Models;

namespace PMT.Services.ProjectMetrics;

public class BurnDownChartData
{
  public BurnDownChartData(Project proj, List<Story> stories, List<BugReport> bugReports)
  {
    _stories = stories;
    _bugReports = bugReports;
    TotalPointsForProject = GetTotalPointsForProj();

    List<List<string>> _burnDownChartValues = [];
    List<string> tempList = [];

    // add header
    _burnDownChartValues.Add(["Day", "Ideal Burn", "Actual Burn"]);

    DateTime currentDate = new(proj.StartDate.Date.Year, proj.StartDate.Date.Month, proj.StartDate.Date.Day, 0, 0, 0);
    TimeSpan duration = proj.DueDate - currentDate; // currentDate here is a normalized projStartDate
    List<string> idealBurn = GetIdealBurn(duration.Days + 1);
    TimeSpan projStartToNow = DateTime.Now - proj.StartDate;
    List<string> actualBurn = GetActualBurn(duration.Days + 1, currentDate);

    for (int i = 0; i < (duration.Days + 1); i++)
    {
      // populate tempList
      tempList.Add($"{currentDate.Month}/{currentDate.Day}");
      tempList.Add(idealBurn[i]);
      string actualValue = (projStartToNow.Days + 1 > i) ? actualBurn[i] : null;
      tempList.Add(actualValue);
      // add and reset tempList
      _burnDownChartValues.Add(tempList);
      tempList = [];
      // increment currentDate
      currentDate = currentDate.AddDays(1);
    }

    BurnDownChartValues = _burnDownChartValues;
  }



  public List<List<string>> BurnDownChartValues { get; set; }

  private static List<Story> _stories;
  private static List<BugReport> _bugReports;
  private static double TotalPointsForProject;



  private static List<string> GetIdealBurn(int numberOfDaysInProj)
  {
    List<string> idealBurn = [];
    double idealIncrement = TotalPointsForProject / (numberOfDaysInProj - 1);
    double currentIdealDataPoint = TotalPointsForProject;

    for (int i = 0; i < numberOfDaysInProj; i++)
    {
      idealBurn.Add(currentIdealDataPoint.ToString());
      currentIdealDataPoint -= idealIncrement;

      // if the ideal increment can't be taken off without reaching a negative number, set the data point to 0
      if (currentIdealDataPoint < idealIncrement)
      { 
        currentIdealDataPoint = 0;
      }
    }

    return idealBurn;
  }

  private static List<string> GetActualBurn(int numberOfDaysInProj, DateTime startDate)
  {
    List<string> actualBurn = [];
    double currentActualDataPoint = TotalPointsForProject;

    for (int i = 0; i < numberOfDaysInProj; i++)
    {
      actualBurn.Add(currentActualDataPoint.ToString());
      currentActualDataPoint = TotalPointsForProject - GetPointsCompletedToDate(startDate.AddDays(i));
    }

    return actualBurn;
  }

  private static int GetPointsCompletedToDate(DateTime date)
  {
    int pointsCompleted = 0;

    for (int i = 0; i < _stories.Count; i++)
    {
      if (_stories[i].Status == Str.Resolved && _stories[i].DateResolved < date)
      {
        pointsCompleted += DetermineIssueWeight(_stories[i].Points);
      }
    }

    for (int i = 0; i < _bugReports.Count; i++)
    {
      if (_bugReports[i].Status == Str.Resolved && _bugReports[i].DateResolved < date)
      {
        pointsCompleted += DetermineIssueWeight(_bugReports[i].Points);
      }
    }

    return pointsCompleted;
  }

  private static int GetTotalPointsForProj()
  {
    int totalPoints = 0;

    for (int i = 0; i < _stories.Count; i++)
    {
      totalPoints += DetermineIssueWeight(_stories[i].Points);
    }

    for (int i = 0; i < _bugReports.Count; i++)
    {
      totalPoints += DetermineIssueWeight(_bugReports[i].Points);
    }

    return totalPoints;
  }

  private static int DetermineIssueWeight(int fibNum)
  {
    return fibNum switch
    {
      1 => 1,
      2 => 2,
      3 => 3,
      5 => 4,
      8 => 5,
      13 => 6,
      _ => 0
    };
  }
}
