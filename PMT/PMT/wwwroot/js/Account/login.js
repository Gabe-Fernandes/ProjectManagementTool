$(function () {
  // helps ensure clean login process
  if ($("#cleanLoginForm").length > 0) { $("#cleanLoginForm").submit() }

  // check for failed login on page load
  const invalidCredentialErrMsg = "Email or password are incorrect";
  if ($(".failed-login").length === 1) {
    // Error ID html not written yet
    //ShowError("loginEmail", "loginEmailErr", invalidCredentialErrMsg);
    //ShowError("loginPassword", "loginPasswordErr", invalidCredentialErrMsg);
  }

  $("#loginEmail").on("input", clearLoginErrosAfterLoginFail);
  $("#loginPassword").on("input", clearLoginErrosAfterLoginFail);
  function clearLoginErrosAfterLoginFail() {
    if ($("#loginPasswordErr")[0].innerHTML === invalidCredentialErrMsg) {
      HideError("loginEmail", "loginEmailErr");
      HideError("loginPassword", "loginPasswordErr");
    }
  }

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
    ToggleModal($("#loginContent"), $("#demoModal"), openModal);
  });
  $("#demoCloseBtn").on("click", () => {
    ToggleModal($("#loginContent"), $("#demoModal"), closeModal);
  });

  // recover pass modal
  $("#recoverPassBtn").on("click", () => {
    ToggleModal($("#loginContent"), $("#recoverPassModal"), openModal);
  });
  $("#recoverPassCloseBtn").on("click", () => {
    ToggleModal($("#loginContent"), $("#recoverPassModal"), closeModal);
  });

  // resend email conf modal
  $("#resendEmailConfBtn").on("click", () => {
    ToggleModal($("#loginContent"), $("#resendEmailConfModal"), openModal);
  });
  $("#resendEmailConfCloseBtn").on("click", () => {
    ToggleModal($("#loginContent"), $("#resendEmailConfModal"), closeModal);
  });

  // open modal if showing confirmation onpageload
  if ($("#recoverPassForm").length === 0) {
    ToggleModal($("#loginContent"), $("#recoverPassModal"), openModal);
  }
  if ($("#resendEmailConfForm").length === 0) {
    ToggleModal($("#loginContent"), $("#resendEmailConfModal"), openModal);
  }
});
