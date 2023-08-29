$(function () {
  HighlightCurrentNavBtn($("#bugTrackerNavBtn"));

  // Modal Events
  $("#delCancelBtn").on("click", () => {
    ToggleModal($("#bugTrackingContent"), $("#delBugReportModal"), closeModal);
  });
  $("#delCloseBtn").on("click", () => {
    ToggleModal($("#bugTrackingContent"), $("#delBugReportModal"), closeModal);
  });
  $(".del-btn").on("click", (event) => {
    ToggleModal($("#bugTrackingContent"), $("#delBugReportModal"), openModal);
    const idToDel = $(event.target).attr("data-bugReportId");
    $("#bugReportIdToDel").val(idToDel);
  });

  // TH sorting events

  let thDueDateInOrder = true;
  let thPriorityInOrder = true;
  let thDescriptionInOrder = true;
  let thStatusInOrder = true;

  $("#thDueDate").on("click", () => {
    thDueDateInOrder = thSortEvent("bugTrackingTbody", thDueDateInOrder, "bugTrackingTR", "sortDueDate", chronologicallyFirst);
  });
  $("#thPriority").on("click", () => {
    thPriorityInOrder = thSortEvent("bugTrackingTbody", thPriorityInOrder, "bugTrackingTR", "sortPriority", alphabeticallyFirst);
  });
  $("#thDescription").on("click", () => {
    thDescriptionInOrder = thSortEvent("bugTrackingTbody", thDescriptionInOrder, "bugTrackingTR", "sortDescription", alphabeticallyFirst);
  });
  $("#thStatus").on("click", () => {
    thStatusInOrder = thSortEvent("bugTrackingTbody", thStatusInOrder, "bugTrackingTR", "sortStatus", alphabeticallyFirst);
  });

  // Package search filter data
  $("#filterBtn").on("click", () => {
    let href = $("#filterBtn").attr("href");
    href += "&filterString=" + $("#filterInput").val();
    $("#filterBtn").attr("href", href);
  });
});
