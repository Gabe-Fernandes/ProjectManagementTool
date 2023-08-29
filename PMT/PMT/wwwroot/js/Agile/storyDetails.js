$(function () {
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
    $("#descriptionText").select();
    // copy text
    navigator.clipboard.writeText($("#descriptionText").val());
  });

  $("#delBtn").on("click", () => {
    ToggleModal($("#storyDetailsContent"), $("#delStoryModal"), openModal);
  });
  $("#delCancelBtn").on("click", () => {
    ToggleModal($("#storyDetailsContent"), $("#delStoryModal"), closeModal);
  });
  $("#delCloseBtn").on("click", () => {
    ToggleModal($("#storyDetailsContent"), $("#delStoryModal"), closeModal);
  });
});
