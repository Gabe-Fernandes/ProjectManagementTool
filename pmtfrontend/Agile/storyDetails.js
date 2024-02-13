$(function () {
  $("#resolvedBtn").on("click", () => {
    if ($("#confIcon").hasClass("hide")) {
      $("#confIcon").removeClass("hide");
      $("#resolvedBtn").text("undo");
    } else {
      $("#confIcon").addClass("hide");
      $("#resolvedBtn").text("Mark as resolved");
    }
  });



  // move this to site.js
  function switchFromReadToEdit(target) {
    target.parents(".toggle-read-edit").find(".read-data").addClass("hide");
    target.parents(".toggle-read-edit").find(".edit-data").removeClass("hide");
  }

  $(".edit-btn").on("click", (event)=> {
    switchFromReadToEdit($(event.target));
    $(event.target).addClass("hide");
  });

  $("#copyBtn").on("click", ()=> {
    // select text
    $("#descriptionText").select();
    // copy text
    navigator.clipboard.writeText($("#descriptionText").val());
  });

  $("#delBtn").on("click", ()=> {
    ToggleModal($("#storyDetailsContent"), $("#delStoryModal"), openModal);
  });
  $("#delCancelBtn").on("click", ()=>{
    ToggleModal($("#storyDetailsContent"), $("#delStoryModal"), closeModal);
  });
  $("#delCloseBtn").on("click", ()=>{
    ToggleModal($("#storyDetailsContent"), $("#delStoryModal"), closeModal);
  });
});
