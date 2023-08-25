$(function () {
  function switchFromReadToEdit(target) {
    target.parents(".toggle-read-edit").find(".read-data").addClass("hide");
    target.parents(".toggle-read-edit").find(".edit-data").removeClass("hide");
  }

  $(".edit-btn").on("click", (event) => {
    switchFromReadToEdit($(event.target));
    $(event.target).addClass("hide");
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
