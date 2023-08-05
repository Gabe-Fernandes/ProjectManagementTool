$(function () {
  // toggle password show
  $(".passHideBtn").on("click", (event) => {
    TogglePasswordShow($("#registerPass"), $(".passHideBtn"), $(".passShowBtn"));
    TogglePasswordShow($("#registerConfPass"), $(".passHideBtn"), $(".passShowBtn"));
    $(event.target).siblings("img").focus();
  });
  $(".passShowBtn").on("click", (event) => {
    TogglePasswordShow($("#registerPass"), $(".passShowBtn"), $(".passHideBtn"));
    TogglePasswordShow($("#registerConfPass"), $(".passShowBtn"), $(".passHideBtn"));
    $(event.target).siblings("img").focus();
  });

  // Pagination Events
  $("#nextBtn").on("click", () => {
    $("#page1").addClass("hide");
    $("#page2").removeClass("hide");
    $("#backBtn").removeClass("hide");
  });
  $("#backBtn").on("click", () => {
    $("#page2").addClass("hide");
    $("#page1").removeClass("hide");
    $("#backBtn").addClass("hide");
  });

  // Validation Events
  $("#registerPass").on("input", () => {
    LivePasswordValidation($("#registerPass").val());
  });
});
