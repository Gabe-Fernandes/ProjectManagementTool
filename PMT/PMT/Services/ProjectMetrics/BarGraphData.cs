using PMT.Data.Models;

namespace PMT.Services.ProjectMetrics;

public class BarGraphData
{
  public BarGraphData(Project proj)
  {
    NumberOfSprints = GetNumberOfSprints(proj.StartDate, proj.DueDate);
    SprintDates = [["1/2", "1/5"], ["1/6", "1/12"], ["1/13", "1/19"]];
  }

  public int NumberOfSprints { get; set; }

  public int WeightScale { get; set; }

  public List<List<string>> SprintDates { get; set; }

  public List<List<int>> CompletedIssueWeights { get; set; }

  private static int GetNumberOfSprints(DateTime startDate, DateTime dueDate)
  {
    int numberOfSprints = 0;
    TimeSpan duration = dueDate - startDate;

    DateTime firstSunday = FindFirstSundayInProject(duration.Days, startDate);



    return numberOfSprints;
  }

  private static DateTime FindFirstSundayInProject(int numberOfDays, DateTime currentDate)
  {
    for (int i = 0; i < numberOfDays; i++)
    {
      if (currentDate.DayOfWeek == DayOfWeek.Sunday)
      {
        return currentDate;
      }
      currentDate = currentDate.AddDays(1);
    }

    // Handle a timespan with no Sunday's
    return DateTime.Now; // currently not handled
  }
}
