$(function () {
  // helps ensure clean login process
  if ($("#cleanLoginForm").length > 0) { $("#cleanLoginForm").submit() }

  // check for failed login on page load -------------------------------------------------------------------------------------------
  const invalidCredentialErrMsg = "Email or password are incorrect";
  if ($(".failed-login").length === 1) {
    ShowError("loginEmail", "loginEmailErr", invalidCredentialErrMsg);
    ShowError("loginPass", "loginPassErr", invalidCredentialErrMsg);
  }

  $("#loginEmail").on("input", clearLoginErrosAfterLoginFail);
  $("#loginPassword").on("input", clearLoginErrosAfterLoginFail);
  function clearLoginErrosAfterLoginFail() {
    if ($("#loginPassErr")[0].innerHTML === invalidCredentialErrMsg) {
      HideError("loginEmail", "loginEmailErr");
      HideError("loginPass", "loginPassErr");
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

  // Modals --------------------------------------------------------------------------------------------------

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

  // validation --------------------------------------------------------------------------------------------------

  const charLimit = 40;
  const allInputNames = ["Email", "Pass", "ForgotPassEmail", "ResendEmailConfEmail"];
  let allInputIDs = [];
  let allInputFields = [];
  let allErrIDs = [];

  for (let i = 0; i < allInputNames.length; i++) {
    allInputIDs.push(`login${allInputNames[i]}`);
    allInputFields.push($(`#${allInputIDs[i]}`));
    allErrIDs.push(`${allInputIDs[i]}Err`);
  }

  $(".login-form").on("submit", (evt) => {
    const inputFields = [$("#loginEmail"), $("#loginPass")];
    const errIDs = ["loginEmailErr", "loginPassErr"];

    RunCommonValidationTests(inputFields, errIDs, charLimit);

    // test valid email
    if (ValidEmail($("#loginEmail").val()) === false) {
      ShowError("loginEmail", "loginEmailErr", "invalid email");
    }

    if ($(".err-input").length > 0) { evt.preventDefault() }
  });

  $("#recoverPassForm").on("submit", (evt) => {
    const inputFields = $("#loginForgotPassEmail");
    const errIDs = "loginForgotPassEmailErr";

    RunCommonValidationTests(inputFields, errIDs, charLimit);

    // test valid email
    if (ValidEmail($("#loginForgotPassEmail").val()) === false) {
      ShowError("loginForgotPassEmail", "loginForgotPassEmailErr", "invalid email");
    }

    if ($(".err-input").length > 0) { evt.preventDefault() }
  });

  $("#resendEmailConfForm").on("submit", (evt) => {
    const inputFields = $("#loginResendEmailConfEmail");
    const errIDs = "loginResendEmailConfEmailErr";

    RunCommonValidationTests(inputFields, errIDs, charLimit);

    // test valid email
    if (ValidEmail($("#loginResendEmailConfEmail").val()) === false) {
      ShowError("loginResendEmailConfEmail", "loginResendEmailConfEmailErr", "invalid email");
    }

    if ($(".err-input").length > 0) { evt.preventDefault() }
  });

  // Real-Time Validation
  realTimeValidation(allInputIDs, allErrIDs, allInputFields, charLimit, loginValidations);

  function loginValidations(i) {
    if (allInputIDs[i] === "loginEmail" || allInputIDs[i] === "loginForgotPassEmail" || allInputIDs[i] === "loginResendEmailConfEmail") {
      if (ValidEmail(allInputFields[i].val()) === false) {
        ShowError(allInputIDs[i], allErrIDs[i], "invalid email");
        return true; // errorExists === true
      }
    }
  }
});
