$(function () {
  HighlightCurrentNavBtn($("#agileNavBtn"));

  // modal events
  $(".del-btn").on("click", (event) => {
    $("#idToDelInput").val($(event.target).attr("data-idToDel"));
    ToggleModal($("#myStoriesContent"), $("#delStoryModal"), openModal);
  });
  $("#delCancelBtn").on("click", () => {
    ToggleModal($("#myStoriesContent"), $("#delStoryModal"), closeModal);
  });
  $("#delCloseBtn").on("click", () => {
    ToggleModal($("#myStoriesContent"), $("#delStoryModal"), closeModal);
  });

  // TH sorting events

  let thDueDateInOrder = true;
  let thTitleInOrder = true;
  let thStatusInOrder = true;

  $("#thDueDate").on("click", () => {
    thDueDateInOrder = thSortEvent("myStoriesTbody", thDueDateInOrder, "myStoriesTR", "sortDueDate", chronologicallyFirst);
  });
  $("#thTitle").on("click", () => {
    thTitleInOrder = thSortEvent("myStoriesTbody", thTitleInOrder, "myStoriesTR", "sortTitle", alphabeticallyFirst);
  });
  $("#thStatus").on("click", () => {
    thStatusInOrder = thSortEvent("myStoriesTbody", thStatusInOrder, "myStoriesTR", "sortStatus", alphabeticallyFirst);
  });
});
