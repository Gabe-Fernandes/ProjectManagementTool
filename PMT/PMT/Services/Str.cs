namespace PMT.Services;

public class Str
{
  // Issue Statuses
  public const string Unaddressed = "Unaddressed";
  public const string InProgress = "Progressing";
  public const string Resolved = "Resolved";

  // Controllers and action methods
  public const string Account = "Account";
  public const string Login = "Login";
  public const string Logout = "Logout";
  public const string Register = "Register";
	public const string RecoverPassword = "RecoverPassword";
	public const string ConfirmEmail = "ConfirmEmail";
	public const string ForgotPassword = "ForgotPassword";
	public const string ResendEmailConf = "ResendEmailConf";

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
  public const string SetDefaultProjId = "SetDefaultProjId";

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

  // Identity Framework
  public const string AppEmail = "gabe-portfolioapp@outlook.com";
  public const string Cookie = "Cookie";
  public const string recovery_email_sent = "recovery_email_sent";
  public const string conf_email_sent = "conf_email_sent";
  public const string CleanLogin = "CleanLogin";
  public const string failed_login_attempt = "failed_login_attempt";
	
	// Array storage in strings
	public const string delimiter = "_____&_____";

  public static List<string> ExtractData(string data)
  {
    if (string.IsNullOrEmpty(data)) { return new List<string>(); }

    List<string> extractedList = new();
    string extractedElement = "";

    for (int i = 0; i < data.Length; i++)
    {
      if (CheckForDelimiter(data, i, delimiter))
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

  private static bool CheckForDelimiter(string data, int index, string delimiter)
  {
    for (int i = 0; i < delimiter.Length; i++)
    {
      if (delimiter[i] != data[index]) { return false; }
      index++;
    }
    return true;
  }

  // Solution specific string processing for ModelPlanning
  public const string newModelDelimiter = "M____&_____";
  public const string newPropDelimiter = "P____&_____";
  
  public static List<List<string>> ExtractProperties(string data)
  {
    if (string.IsNullOrEmpty(data)) { return new List<List<string>>(); }

    List<List<string>> extractedList = new();
    int currentModelIndex = -1;
    string extractedElement = "";

    for (int i = 0; i < data.Length; i++)
    {
      if (CheckForDelimiter(data, i, delimiter))
      {
        i += delimiter.Length - 1;
        extractedList[currentModelIndex].Add(extractedElement);
        extractedElement = "";
      }
      else if (CheckForDelimiter(data, i, newModelDelimiter))
      {
        i += delimiter.Length - 1;
        List<string> modelContainer = new();
        currentModelIndex++;
        extractedList.Add(modelContainer);
      }
      else
      {
        extractedElement += data[i];
      }
    }

    return extractedList;
  }

  public static List<List<List<string>>> ExtractValidations(string data)
  {
    if (string.IsNullOrEmpty(data)) { return new List<List<List<string>>>(); }

    List<List<List<string>>> extractedList = new();
    int currentModelIndex = -1;
    int currentPropIndex = -1;
    string extractedElement = "";

    for (int i = 0; i < data.Length; i++)
    {
      if (CheckForDelimiter(data, i, delimiter))
      {
        i += delimiter.Length - 1;
        extractedList[currentModelIndex][currentPropIndex].Add(extractedElement);
        extractedElement = "";
      }
      else if (CheckForDelimiter(data, i, newModelDelimiter))
      {
        i += newModelDelimiter.Length - 1;
        List<List<string>> modelContainer = new();
        currentModelIndex++;
        // reset the property count
        currentPropIndex = -1;
        extractedList.Add(modelContainer);
      }
      else if (CheckForDelimiter(data, i, newPropDelimiter))
      {
        i += newPropDelimiter.Length - 1;
        List<string> valiContainer = new();
        currentPropIndex++;
        extractedList[currentModelIndex].Add(valiContainer);
      }
      else
      {
        extractedElement += data[i];
      }
    }

    return extractedList;
  }

  // For some reason, sanitized html was missing ";" at the end of inline styles
  // and "vw;" at the end of the first margin:left style
  public static string ReformatSanitizedHtml(string html)
  {
    for (int i = 0; i < html.Length; i++)
    {
      if (html[i] == '\n')
      {
        html = html.Insert(i, "\r");
        i++;
      }
      if (i < html.Length-13 && html.Substring(i, 13) == "margin-left: ")
      {
        // first case is unique
        if (html[i+13] == '0')
        {
          html = html.Insert(i + 14, "vw;");
          continue;
        }
        html = html.Insert(FindVWInsertIndex(html, i+13), ";");
      }
    }
    return html;
  }

  private static int FindVWInsertIndex(string html, int index)
  {
    for (int i = index; i < html.Length; i++)
    {
      if (html[i] == 'v')
      {
        return i +2;
      }
    }
    return -1;
  }
}
