using PMT.Data.Models;

namespace PMT.Services.ProjectMetrics;

public class BarGraphData
{
  public BarGraphData(Project proj, List<Story> stories, List<BugReport> bugReports)
  {
    DateTime normalizedStartDate = new(proj.StartDate.Date.Year, proj.StartDate.Date.Month, proj.StartDate.Date.Day, 0, 0, 0);
    TimeSpan duration = proj.DueDate - normalizedStartDate;
    DateTime currentDate = proj.StartDate;
    int highestWeight = 0;
    int sprintCount = 1;
    int numOfSundays = (proj.StartDate.DayOfWeek == DayOfWeek.Sunday) ? 0 : 1;
    List<int> tempWeightList = InitializeTempWeightList(proj.StartDate.DayOfWeek);
    List<string> tempDateList = [];
    List<List<int>> _completedIssueWeights = [];
    List<List<string>> _sprintDates = [];
    tempDateList.Add(proj.StartDate.ToString("M"));

    // iterate through every day in the project
    for (int i = 0; i < (duration.Days + 1); i++)
    {
      // check for start of new week
      if (currentDate.DayOfWeek == DayOfWeek.Sunday)
      {
        numOfSundays++;
      }

      // check for a new sprint
      if (numOfSundays == 3)
      {
        // set end date of completed sprint
        tempDateList.Add(currentDate.AddDays(-1).ToString("M"));

        // join these temp lists with our 2D data structures
        _completedIssueWeights.Add(tempWeightList);
        _sprintDates.Add(tempDateList);

        // reset temp lists and set start date of next sprint
        tempWeightList = [];
        tempDateList = [];
        tempDateList.Add(currentDate.ToString("M"));

        sprintCount++;
        numOfSundays = 1;
      }

      // store data point in CompletedIssueWeights
      int currentWeightOfDay = GetWeightForDay(currentDate, stories, bugReports);
      tempWeightList.Add(currentWeightOfDay);

      // check for highest weight to set weight scale
      highestWeight = (currentWeightOfDay > highestWeight) ? currentWeightOfDay : highestWeight;

      // check for last sprint - keeping (days + 1) because using the same exit condition is more intuitive
      if (i + 1 == duration.Days + 1)
      {
        // set end date of completed sprint
        tempDateList.Add(currentDate.ToString("M"));

        // fill rest of collection with 0's
        while (tempWeightList.Count < 14)
        {
          tempWeightList.Add(0);
        }

        // join these temp lists with our 2D data structures
        _completedIssueWeights.Add(tempWeightList);
        _sprintDates.Add(tempDateList);
      }

      // process next date
      currentDate = currentDate.AddDays(1);
    }

    WeightScale = (int)(highestWeight + highestWeight * 0.15);
    NumberOfSprints = sprintCount;
    SprintDates = _sprintDates;
    CompletedIssueWeights = _completedIssueWeights;
  }

  public int NumberOfSprints { get; set; }
  public int WeightScale { get; set; }
  public List<List<string>> SprintDates { get; set; }
  public List<List<int>> CompletedIssueWeights { get; set; }

  private static int GetWeightForDay(DateTime currentDate, List<Story> stories, List<BugReport> bugReports)
  {
    int weightForDay = 0;

    for (int i = 0; i < stories.Count; i++)
    {
      if (stories[i].DateResolved.Date == currentDate.Date)
      {
        weightForDay += DetermineIssueWeight(stories[i].Points);
      }
    }
    for (int i = 0; i < bugReports.Count; i++)
    {
      if (bugReports[i].DateResolved.Date == currentDate.Date)
      {
        weightForDay += DetermineIssueWeight(bugReports[i].Points);
      }
    }

    return weightForDay;
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

  private static List<int> InitializeTempWeightList(DayOfWeek dayOfWeek)
  {
    List<int> tempWeightList = [];
    int numOfZeros = GetNumOfZeroes(dayOfWeek);

    for (int i = 0; i < numOfZeros; i++)
    {
      tempWeightList.Add(0);
    }

    return tempWeightList;
  }

  private static int GetNumOfZeroes(DayOfWeek dayOfWeek)
  {
    switch (dayOfWeek)
    {
      case DayOfWeek.Sunday: return 0;
      case DayOfWeek.Monday: return 1;
      case DayOfWeek.Tuesday: return 2;
      case DayOfWeek.Wednesday: return 3;
      case DayOfWeek.Thursday: return 4;
      case DayOfWeek.Friday: return 5;
      case DayOfWeek.Saturday: return 6;
    }
    return 0;
  }
}
