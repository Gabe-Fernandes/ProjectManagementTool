namespace PMT.Services;

public class Str
{
    // Issue Statuses
    public const string Unaddressed = "Unaddressed";
    public const string InProgress = "In Progress";
    public const string Resolved = "Resolved";

    // Controllers and action methods
    public const string Account = "Account";

    public const string Agile = "Agile";
    public const string MyStories = "MyStories";
    public const string StoryDetails = "StoryDetails";
    public const string UpdateStoryDetails = "UpdateStoryDetails";
    public const string DeleteStory = "DeleteStory";
    public const string NewStory = "NewStory";
    public const string AgileOutline = "AgileOutline";
    public const string Timeline = "Timeline";

    public const string BugTracker = "BugTracker";
    public const string BugTracking = "BugTracking";
    public const string CreateBugReport = "CreateBugReport";
    public const string EditBugReport = "EditBugReport";
    public const string DeleteBugReport = "DeleteBugReport";

    public const string Project = "Project";
    public const string MyProjects = "MyProjects";
    public const string NewProject = "NewProject";
    public const string DeleteProject = "DeleteProject";
    public const string ProjectDash = "ProjectDash";

    public const string SRS = "SRS";
    public const string TechStack = "TechStack";
    public const string FileStructure = "FileStructure";
    public const string ColorPalette = "ColorPalette";
    public const string ModelsAndValidation = "ModelsAndValidation";
    public const string InitializeSRS = "InitializeSRS";
  
  // ViewData
  public const string Stories = "Stories";
    public const string Projects = "Projects";
    public const string BugReports = "BugReports";

  // Array storage in strings
  public const string delimiter = "_____&_____";

  public static List<string> ExtractData(string data)
  {
    if (string.IsNullOrEmpty(data)) { return new List<string>(); }

    List<string> extractedList = new();
    string extractedElement = "";

    for (int i = 0; i < data.Length; i++)
    {
      if (CheckForDelimiter(data, i))
      {
        i += delimiter.Length - 1;
        extractedList.Add(extractedElement);
        extractedElement = "";
      }
      else
      {
        extractedElement += data[i];
      }
    }

    return extractedList;
  }

  private static bool CheckForDelimiter(string data, int index)
  {
    for (int i = 0; i < delimiter.Length; i++)
    {
      if (delimiter[i] != data[index]) { return false; }
      index++;
    }
    return true;
  }
}
