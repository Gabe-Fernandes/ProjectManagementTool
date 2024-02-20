const SpecialChars = ["@", "_", "-", ".", "!", "#", "$", "%", "&", "*", "?"];
const PasswordSpecialChars = ["!", "@", "#", "$", "%", "&", "*"];
const NumericalChars = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];
const LowerCaseChars = ["a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"];
const UpperCaseChars = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
const AllValidChars = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "@", "_", "-", ".", "!", "#", "$", "%", "&", "*", "?"];
const hide = "hide";



// --------------------------------------  Process Validation  -------------------------------------- //



function processValidation(formId, nameSpace, allInputNames) {
  const charLimit = 3000; // make this dynamic later
  let allInputIDs = [];
  let allInputFields = [];
  let allErrIDs = [];

  for (let i = 0; i < allInputNames.length; i++) {
    allInputIDs.push(`${nameSpace}${allInputNames[i]}`);
    allInputFields.push($(`#${allInputIDs[i]}`));
    allErrIDs.push(`${allInputIDs[i]}Err`);
  }

  for (let i = 0; i < allInputFields.length; i++) {
    allInputFields[i].on("input", () => {
      HideError(allInputIDs[i], allErrIDs[i]);
    });
  }

  $(`#${formId}`).on("submit", (evt) => {
    RunCommonValidationTests(allInputFields, allErrIDs, charLimit);

    if ($(".err-input").length > 0) { evt.preventDefault() }
  });
}



// --------------------------------------  Error Handling  -------------------------------------- //



function ShowError(inputId, errorId, errorMessage) {
  $(`#${inputId}`).addClass("err-input");
  $(`#${errorId}`).removeClass(hide);
  $(`#${inputId}`).on("mouseover", () => {
    $(`#${errorId}`).removeClass(hide);
  }).on("mouseout", () => {
    $(`#${errorId}`).addClass(hide);
  });
  $(`#${errorId}`)[0].innerHTML = errorMessage;
}

function HideError(inputId, errorId) {
  $(`#${inputId}`).removeClass("err-input");
  $(`#${errorId}`).addClass(hide);
  $(`#${inputId}`).off("mouseover");
  $(`#${inputId}`).off("mouseout");
}



// -------------------------------------- Valid Char / Required Field / Char Limit  -------------------------------------- //



function AllCharsValid(inputFields, errIDs) {
  if (inputFields.length === 1) {
    inputFields = [inputFields];
    errIDs = [errIDs];
  }
  let inputValidity = true;
  for (let i = 0; i < inputFields.length; i++) {
    for (let j = 0; j < inputFields[i].val().length; j++) {
      if (inputFields[i].val()[j] === " ") { continue; } // skip spaces
      if (AllValidChars.includes(inputFields[i].val()[j]) === false) {
        inputValidity = false;
        ShowError(inputFields[i].attr("id"), errIDs[i], `"${inputFields[i].val()[j]}" is an invalid character`);
      }
    }
  }
  return inputValidity;
}

function ValidateRequiredInput(inputFields, errIDs) {
  if (inputFields.length === 1) {
    inputFields = [inputFields];
    errIDs = [errIDs];
  }
  let inputValidity = true;
  for (let i = 0; i < inputFields.length; i++) {
    if (inputFields[i].val() === "") {
      inputValidity = false;
      ShowError(inputFields[i].attr("id"), errIDs[i], "required field");
    }
  }
  return inputValidity;
}

function ValidCharLimit(inputFields, errIDs, limit) {
  if (inputFields.length === 1) {
    inputFields = [inputFields];
    errIDs = [errIDs];
  }
  let inputValidity = true;
  for (let i = 0; i < inputFields.length; i++) {
    if (inputFields[i].val().length > limit) {
      inputValidity = false;
      ShowError(inputFields[i].attr("id"), errIDs[i], `character limit of ${limit} exceeded`);
    }
  }
  return inputValidity;
}

function RunCommonValidationTests(inputFields, errIDs, charLimit) {
  let errorExists = false;

  if (AllCharsValid(inputFields, errIDs) === false) {
    errorExists = true;
  }
  if (ValidateRequiredInput(inputFields, errIDs) === false) {
    errorExists = true;
  }
  if (ValidCharLimit(inputFields, errIDs, charLimit) === false) {
    errorExists = true;
  }

  return errorExists;
}



// --------------------------------------  EMAIL  -------------------------------------- //



function ValidEmail(email) {
  let atSign = 0; //The index where @ is present
  let dot = 0; //The index where . is present

  email = email.toLowerCase();
  for (let i = 0; i < email.length; i++) { //Check email prefix
    if (email[i] === "@") { atSign = i; break }

    if (AllValidChars.includes(email[i]) === false) { return false }

    if (SpecialChars.includes(email[i])) {
      if (i === 0) { return false } //Special chars can't be first
      if (SpecialChars.includes(email[i + 1])) { return false } //Special chars can't be consecutive
    }
  }
  if (atSign === 0) { return false } //@ Must be present and not first
  if (SpecialChars.includes(email[atSign - 1])) { return false } //char before @ can't be a special char


  for (let j = email.length - 1; j > atSign; j--) { //Check top-level domain
    if (email[j] === ".") {
      if (j > email.length - 3) { return false } //Dot must have at least 2 alphabetical chars after it
      dot = j; break;
    }
    if (LowerCaseChars.includes(email[j]) === false) { return false }
  }
  if (dot === 0) { return false } //Dot must be present
  if (dot === atSign + 1) { return false } //There must be a domain between @ and dot


  for (let k = dot - 1; k > atSign; k--) { //Check email domain
    if (AllValidChars.includes(email[k]) === false) { return false }
    if (email[k] === "@") { return false } //There can only be one @

    if (SpecialChars.includes(email[k])) {
      if (k === atSign + 1) { return false } //Special chars can't be right after @
      if (k === dot - 1) { return false } //Special chars can't be right before dot
      if (SpecialChars.includes(email[k - 1])) { return false } //Special chars can't be consecutive
    }
  }

  return true;
}



// --------------------------------------  Password  -------------------------------------- //



function IncludesCharTypeResult(password, charSet) {
  for (let i = 0; i < password.length; i++) {
    if (charSet.includes(password[i]) === true) {
      return "valid";
    }
  }
  return "invalid";
}

function LivePasswordValidation(password) {
  $("#charMinConfirmation")[0].className = (password.length < 8) ? "invalid" : "valid";
  $("#charLowerConfirmation")[0].className = IncludesCharTypeResult(password, LowerCaseChars);
  $("#charUpperConfirmation")[0].className = IncludesCharTypeResult(password, UpperCaseChars);
  $("#charNumberConfirmation")[0].className = IncludesCharTypeResult(password, NumericalChars);
  $("#charSpecialConfirmation")[0].className = IncludesCharTypeResult(password, PasswordSpecialChars);

  return ($(".invalid").length === 0);
}

function CheckPasswordMatch(password, repeatPassID, errID) {
  if ($(`#${repeatPassID}`).val() === password) {
    return true;
  }
  ShowError(repeatPassID, errID, "passwords must match");
  return false;
}



// --------------------------------------  Phone Number  -------------------------------------- //



function PhoneNumberFormatting(phoneInput) {
  let previousInputLength = 0;
  phoneInput.on("input", () => {
    let input = phoneInput.val();

    // allows backspace/deleting
    if (input.length < previousInputLength) {
      previousInputLength = input.length;
      return;
    }

    // normalize input to make it consistent to work with
    input = input.replace(/-/g, "");

    // define output
    let output = input;
    if (input.length >= 3) {
      output = input.slice(0, 3) + "-" + input.slice(3, 6);
    }
    if (input.length >= 6) {
      output = input.slice(0, 3) + "-" + input.slice(3, 6) + "-" + input.slice(6, 10);
    }
    previousInputLength = output.length;

    // print
    phoneInput.val(output);
  });
}

function ValidatePhoneNumber(inputID, errID) {
  const phoneNum = $(`#${inputID}`).val();
  for (let i = 0; i < phoneNum.length; i++) {
    if (i === 3 || i === 7) { continue; } // skip "-"
    if (NumericalChars.includes(phoneNum[i]) === false ||
      phoneNum.length != 12) {
      ShowError(inputID, errID, "invalid phone number");
      return false;
    }
  }
  return true;
}



// --------------------------------------  Dates -------------------------------------- //



function DateIsPastDate(inputID, errID) {
  const inputDate = new Date($(`#${inputID}`).val());
  const today = new Date(Date.now());
  if (inputDate > today) {
    ShowError(inputID, errID, "DOB can't be a future date");
    return false;
  }
  return true;
}

function startDateBeforeEnd(startId, endId, errId) {
  const startDate = new Date($(`#${startId}`).val());
  const endDate = new Date($(`#${endId}`).val());
  if (endDate < startDate) {
    ShowError(endId, errId, "Must be due after start date");
    return false;
  }
  return true;
}



// --------------------------------------  Postal Code -------------------------------------- //



function ValidatePostalCode(inputID, errID) {
  const postalCode = $(`#${inputID}`).val();
  for (let i = 0; i < postalCode.length; i++) {
    if (NumericalChars.includes(postalCode[i]) === false ||
      postalCode.length != 5) {
      ShowError(inputID, errID, "invalid postal code");
      return false;
    }
  }
  return true;
}



// --------------------------------------  Real Time Validation -------------------------------------- //



// Real-Time Validation

function realTimeValidation(allInputIDs, allErrIDs, allInputFields, charLimit, specificTestsFunction) {
  for (let i = 0; i < allInputIDs.length; i++) {
    allInputFields[i].on("input", () => {
      let errorExists = false;

      if (ValidCharLimit(allInputFields[i], allErrIDs[i], charLimit) === false) {
        errorExists = true;
      }
      if (AllCharsValid(allInputFields[i], allErrIDs[i]) === false) {
        errorExists = true;
      }

      if (specificTestsFunction !== undefined) {
        if (specificTestsFunction(i) === true) { errorExists = true; }
      }

      if (errorExists === false) {
        HideError(allInputIDs[i], allErrIDs[i]);
      }
    });
  }
}
