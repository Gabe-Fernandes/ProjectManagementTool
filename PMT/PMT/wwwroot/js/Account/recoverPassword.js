$(function () {
  // Password Show Events
  $(".passHideBtn").on("click", (event) => {
    TogglePasswordShow($("#recoverPasswordPass"), $(".passHideBtn"), $(".passShowBtn"));
    TogglePasswordShow($("#recoverPasswordConfPass"), $(".passHideBtn"), $(".passShowBtn"));
    $(event.target).siblings("img").focus();
  });
  $(".passShowBtn").on("click", (event) => {
    TogglePasswordShow($("#recoverPasswordPass"), $(".passShowBtn"), $(".passHideBtn"));
    TogglePasswordShow($("#recoverPasswordConfPass"), $(".passShowBtn"), $(".passHideBtn"));
    $(event.target).siblings("img").focus();
  });

  // Validation Events
  $("#recoverPasswordPass").on("input", () => {
    LivePasswordValidation($("#recoverPasswordPass").val());
  });
});
