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
  $("#filterInput").on("input", () => {
    const filterString = ($("#filterInput").val()).toLowerCase();


    // set hide status for all <tr>
    for (let i = 0; i < $(".sortDescription").length; i++) {
      const title = $(".sortDescription").eq(i).html().toLowerCase();
      const trElement = $(`#bugTrackingTR_${i}`);
      if (title.includes(filterString)) {
        trElement.removeClass("hide");
      }
      else {
        trElement.addClass("hide");
      }
    }
  });

  function checkBoxFilter() {
    if ($("#showResolvedCheckbox").is(":checked")) {
      $("tr").removeClass("hide");
      return;
    }

    // set hide status for all <tr>
    for (let i = 0; i < $(".sortStatus").length; i++) {
      const status = $(".sortStatus").eq(i).find("label").html();
      const trElement = $(`#bugTrackingTR_${i}`);
      if (status !== "Resolved") {
        trElement.removeClass("hide");
      }
      else {
        trElement.addClass("hide");
      }
    }
  }

  $("#showResolvedCheckbox").on("input", checkBoxFilter);
  checkBoxFilter();
});
