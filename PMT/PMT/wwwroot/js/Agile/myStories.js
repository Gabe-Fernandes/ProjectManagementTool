$(function () {
  // Show Resolved Checkbox Event
  $("#showResolvedCheckbox").on("input", () => {
    $("#showResolvedForm").trigger("submit");
  });

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
});
