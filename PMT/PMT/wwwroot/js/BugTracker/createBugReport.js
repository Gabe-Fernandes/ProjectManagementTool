$(function () {
  HighlightCurrentNavBtn($("#bugTrackerNavBtn"));

  const inputs = ["Description", "RecreateIssue", "DueDate"];

  processValidation("createBugReportForm", "createBugReport", inputs);
});
