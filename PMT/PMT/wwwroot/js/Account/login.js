$(function () {
  // helps ensure clean login process
  if ($("#cleanLoginForm").length > 0) { $("#cleanLoginForm").submit() }

  // toggle password show
  $(".passHideBtn").on("click", (event) => {
    TogglePasswordShow($("#loginPass"), $(".passHideBtn"), $(".passShowBtn"));
    $(event.target).siblings("img").focus();
  });
  $(".passShowBtn").on("click", (event) => {
    TogglePasswordShow($("#loginPass"), $(".passShowBtn"), $(".passHideBtn"));
    $(event.target).siblings("img").focus();
  });

  // demo modal
  $("#demoBtn").on("click", () => {
    ToggleModal($("#loginMain"), $("#demoModal"), openModal);
  });
  $("#demoCloseBtn").on("click", () => {
    ToggleModal($("#loginMain"), $("#demoModal"), closeModal);
  });

  // recover pass modal
  $("#recoverPassBtn").on("click", () => {
    ToggleModal($("#loginMain"), $("#recoverPassModal"), openModal);
  });
  $("#recoverPassCloseBtn").on("click", () => {
    ToggleModal($("#loginMain"), $("#recoverPassModal"), closeModal);
  });

  // resend email conf modal
  $("#resendEmailConfBtn").on("click", () => {
    ToggleModal($("#loginMain"), $("#resendEmailConfModal"), openModal);
  });
  $("#resendEmailConfCloseBtn").on("click", () => {
    ToggleModal($("#loginMain"), $("#resendEmailConfModal"), closeModal);
  });
});
