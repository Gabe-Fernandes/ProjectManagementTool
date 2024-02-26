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
  let thPointsInOrder = true;
  let thDescriptionInOrder = true;
  let thStatusInOrder = true;

  $("#thDueDate").on("click", () => {
    thDueDateInOrder = thSortEvent("bugTrackingTbody", thDueDateInOrder, "bugTrackingTR", "sortDueDate", chronologicallyFirst);
  });
  $("#thPriority").on("click", () => {
    thPriorityInOrder = thSortEvent("bugTrackingTbody", thPriorityInOrder, "bugTrackingTR", "sortPriority", alphabeticallyFirst);
  });
  $("#thPoints").on("click", () => {
    thPointsInOrder = thSortEvent("bugTrackingTbody", thPointsInOrder, "bugTrackingTR", "sortPoints", numericallyFirst);
  });
  $("#thDescription").on("click", () => {
    thDescriptionInOrder = thSortEvent("bugTrackingTbody", thDescriptionInOrder, "bugTrackingTR", "sortDescription", alphabeticallyFirst);
  });
  $("#thStatus").on("click", () => {
    thStatusInOrder = thSortEvent("bugTrackingTbody", thStatusInOrder, "bugTrackingTR", "sortStatus", alphabeticallyFirst);
  });



  // Search filter
  function searchFilter() {
    const filterString = ($("#filterInput").val()).toLowerCase();
    const showResolved = $("#showResolvedCheckbox").is(":checked");

    for (let i = 0; i < $(".sortStatus").length; i++) { // iterate through each tr
      const status = $(".sortStatus").eq(i).find("label").html();
      const description = $(".sortDescription").find("a").eq(i).html().toLowerCase();
      const trElement = $(`#bugTrackingTR_${i}`);

      if (showResolved === false && status === "Resolved") {
        trElement.addClass("hide");
        continue;
      }

      if (description.includes(filterString)) {
        trElement.removeClass("hide");
      }
      else {
        trElement.addClass("hide");
      }
    }
  }

  $("#filterInput").on("input", searchFilter);
  $("#showResolvedCheckbox").on("input", searchFilter);
  searchFilter();
});
