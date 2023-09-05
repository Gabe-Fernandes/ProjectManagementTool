$(function () {
  // toggle password show ----------------------------------------------------------------------

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

  // Pagination Events ----------------------------------------------------------------------

  $("#nextBtn").on("click", () => {
    $("#page1").addClass("hide");
    $("#page2").removeClass("hide");
    $("#backBtn").removeClass("hide");
  });
  function backBtn() {
    $("#page2").addClass("hide");
    $("#page1").removeClass("hide");
    $("#backBtn").addClass("hide");
  }
  $("#backBtn").on("click", backBtn);

  // Validation Events ----------------------------------------------------------------------

  let passValidity = false;
  const charLimit = 40;
  const allInputNames = ["Email", "FirstName", "LastName", "Pass", "ConfPass"];
  let allInputIDs = [];
  let allInputFields = [];
  let allErrIDs = [];

  for (let i = 0; i < allInputNames.length; i++) {
    allInputIDs.push(`register${allInputNames[i]}`);
    allInputFields.push($(`#${allInputIDs[i]}`));
    allErrIDs.push(`${allInputIDs[i]}Err`);
  }

  $(".register-form").on("submit", (evt) => {
    RunCommonValidationTests(allInputFields, allErrIDs, charLimit);

    if (ValidEmail($("#registerEmail").val()) === false) {
      ShowError("registerEmail", "registerEmailErr", "invalid email");
    }
    CheckPasswordMatch($("#registerPass").val(), "registerConfPass", "registerConfPassErr");

    if ($(".err-input").length > 0 || !passValidity) { evt.preventDefault() }
    // if page1 has any errors, switch to that page
    if ($("#page1").find(".err-input").length > 0) { backBtn() }
  });

  // Real-Time Validation
  realTimeValidation(allInputIDs, allErrIDs, allInputFields, charLimit, registerValidations);

  function registerValidations(i) {
    if (allInputIDs[i] === "registerEmail") {
      if (ValidEmail(allInputFields[i].val()) === false) {
        ShowError(allInputIDs[i], allErrIDs[i], "invalid email");
        return true;
      }
    }
    else if (allInputIDs[i] === "registerPass") {
      if (LivePasswordValidation(allInputFields[i].val()) === false) {
        return true;
      }
    }
    else if (allInputIDs[i] === "registerConfPass") {
      if (CheckPasswordMatch($("#registerPass").val(), allInputIDs[i], allErrIDs[i]) === false) {
        return true;
      }
    }
  }

  $("#registerPass").on("input", () => {
    passValidity = LivePasswordValidation($("#registerPass").val());
  });
});
