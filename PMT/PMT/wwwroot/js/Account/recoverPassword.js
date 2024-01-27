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

  const charLimit = 40;
  const allInputNames = ["Pass", "ConfPass"];
  let allInputIDs = [];
  let allInputFields = [];
  let allErrIDs = [];

  for (let i = 0; i < allInputNames.length; i++) {
    allInputIDs.push(`recoverPassword${allInputNames[i]}`);
    allInputFields.push($(`#${allInputIDs[i]}`));
    allErrIDs.push(`${allInputIDs[i]}Err`);
  }

  $(".recover-password-form").on("submit", (evt) => {
    RunCommonValidationTests(allInputFields, allErrIDs, charLimit);

    CheckPasswordMatch($("#recoverPasswordPass").val(), "recoverPasswordConfPass", "recoverPasswordConfPassErr");

    if ($(".err-input").length > 0) { evt.preventDefault() }
  });

  // Real-Time Validation
  realTimeValidation(allInputIDs, allErrIDs, allInputFields, charLimit, registerValidations);

  function registerValidations(i) {
    if (allInputIDs[i] === "recoverPasswordPass") {
      if (LivePasswordValidation(allInputFields[i].val()) === false) {
        return true;
      }
      if ($("#recoverPasswordConfPass").val() !== "" &&
        CheckPasswordMatch($("#recoverPasswordConfPass").val(), allInputIDs[i], allErrIDs[i]) === false) {
        return true;
      }
    }
    else if (allInputIDs[i] === "recoverPasswordConfPass") {
      if (CheckPasswordMatch($("#recoverPasswordPass").val(), allInputIDs[i], allErrIDs[i]) === false) {
        return true;
      }
    }
  }
});
