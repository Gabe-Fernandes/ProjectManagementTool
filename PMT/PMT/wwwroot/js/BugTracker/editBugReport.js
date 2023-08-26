$(function () {
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
