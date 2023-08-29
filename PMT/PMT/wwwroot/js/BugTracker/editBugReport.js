$(function () {
  HighlightCurrentNavBtn($("#bugTrackerNavBtn"));

  // initialize resolve wrap
  if ($("#statusInput").val() === "Resolved") {
    $("#confIcon").removeClass("hide");
    $("#resolvedBtn").text("Recall");
  }
  else if ($("#statusInput").val() === "Progressing") {
    $("#confIcon").addClass("hide");
    $("#resolvedBtn").text("Mark as resolved");
  }

  $("#resolvedBtn").on("click", () => {
    if ($("#confIcon").hasClass("hide")) {
      // mark as "Resolved""
      $("#confIcon").removeClass("hide");
      $("#resolvedBtn").text("Recall");
      $("#statusInput").val("Resolved");
    } else {
      // back to "In Progress"
      $("#confIcon").addClass("hide");
      $("#resolvedBtn").text("Mark as resolved");
      $("#statusInput").val("Progressing");
    }
  });

  $("#copyBtn").on("click", () => {
    // select text
    $("#solutionInput").select();
    // copy text
    navigator.clipboard.writeText($("#solutionInput").val());
  });

  $("#delBtn").on("click", () => {
    ToggleModal($("#bugDetailsContent"), $("#delBugReportModal"), openModal);
  });
  $("#delCancelBtn").on("click", () => {
    ToggleModal($("#bugDetailsContent"), $("#delBugReportModal"), closeModal);
  });
  $("#delCloseBtn").on("click", () => {
    ToggleModal($("#bugDetailsContent"), $("#delBugReportModal"), closeModal);
  });
});
