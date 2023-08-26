$(function () {

const openModal = "O";
const closeModal = "C";

function ToggleModal(main, modal, direction) {
  if (direction === openModal) {
    main.addClass("unclickable");
    modal.removeClass("hide");
  }
  else if (direction === closeModal) {
    main.removeClass("unclickable");
    modal.addClass("hide");
  }
}

function TogglePasswordShow(passEle, eleToHide, eleToShow) {
  eleToHide.addClass("hide");
  eleToShow.removeClass("hide");
  if (passEle.attr("type") === "password") {
    passEle.attr("type", "text");
  }
  else {
    passEle.attr("type", "password");
  }
}

function HighlightCurrentNavBtn(btnToHighlight) {
  $(".nav-btn").removeClass("nav-highlight");
  btnToHighlight.addClass("nav-highlight");
}

function thSortEvent(tbody, directionFlag, rowNamespace, tdSortClass, sortingFunction) {
  const rowCount = $(`#${tbody}`).children().length;

  // sort content
  let sortedRows = [];
  for (let i = 0; i < rowCount; i++) {
    const row = $(`#${rowNamespace}_${i}`)
    const rowText = row.children(`.${tdSortClass}`).html();
    const rowObj = { row: row, rowText: rowText };
    sortedRows = positionRowInCollection(sortedRows, rowObj, sortingFunction);
  }

  // move content
  const appendDirection = directionFlag ? "append" : "prepend";
  for (let i = 0; i < rowCount; i++) {
    $(`#${tbody}`)[appendDirection](sortedRows[i].row);
  }
  return !directionFlag;
}
function positionRowInCollection(sortedRows, rowObj, sortingFunction) {
  for (let i = 0; i < sortedRows.length; i++) {
    // if the provided row is sorted before the row at index i, insert the rowObj
    const result = sortingFunction(rowObj.rowText, sortedRows[i].rowText);
    if (result === rowObj.rowText) {
      sortedRows.splice(i, 0, rowObj);
      return sortedRows;
    }
  }
  // if the condition is never met, rowObj comes last
  sortedRows.push(rowObj);
  return sortedRows;
}
function alphabeticallyFirst(string1, string2) {
  for (let i = 0; i < string2.length; i++) {
    if (string1[i] === string2[i]) { continue; }
    if (string1[i] === undefined) { return string1; } // cases where chars match, but one string has more chars
    if (string2[i] === undefined) { return string2; }
    return (string1.toLowerCase().charCodeAt(i) < string2.toLowerCase().charCodeAt(i)) ? string1 : string2;
  }
  // if every char was a match
  return string2;
}
function numericallyFirst(num1, num2) {
  const numToSort = parseFloat(num1);
  const numFromArr = parseFloat(num2);
  return (numToSort < numFromArr) ? numToSort.toString() : numFromArr.toString();
}
function chronologicallyFirst(date1, date2) {
  const year1 = parseInt(date1[8] + date1[9] + date1[10] + date1[11]);
  const year2 = parseInt(date2[8] + date2[9] + date2[10] + date2[11]);
  if (year1 !== year2) {
    return (year1 < year2) ? date1 : date2;
  }

  const month1 = abbreviatedMonthToInt(date1[0] + date1[1] + date1[2]);
  const month2 = abbreviatedMonthToInt(date2[0] + date2[1] + date2[2]);
  if (month1 !== month2) {
    return (month1 < month2) ? date1 : date2;
  }

  const day1 = parseInt(date1[4] + date1[5]);
  const day2 = parseInt(date2[4] + date2[5]);
  return (day1 < day2) ? date1 : date2;
}
function abbreviatedMonthToInt(abbreviatedMonth) {
  switch (abbreviatedMonth) {
    case "Jan": return 1;
    case "Feb": return 2;
    case "Mar": return 3;
    case "Apr": return 4;
    case "May": return 5;
    case "Jun": return 6;
    case "Jul": return 7;
    case "Aug": return 8;
    case "Sep": return 9;
    case "Oct": return 10;
    case "Nov": return 11;
    case "Dec": return 12;
  }
}

// Move mobile nav btns from navbar to mobile navbar

function moveNavBtns() {
  // if entering mobile mode
  if ($(window).width() <= 768) {
    $("#mobileNavContainer").append($("nav").children(".nav-btn"));
    $("#mobileNavContainer").append($("nav").children(".profile-link"));
    $("#mobileNavContainer").append($("nav").children(".logout-form"));
  }
  // if exiting mobile mode
  else {
    $("nav").append($("#mobileNavContainer").children());
  }
}

// execute once on each page load
moveNavBtns();

$(window).on("resize", moveNavBtns);

// Mobile Nav Menu Toggle
$("#mobileNavBtn").on("click", () => {
  if ($("#mobileNavContainer").hasClass("slide-mobile-nav")) {
    $("#mobileNavContainer").removeClass("slide-mobile-nav");
  }
  else {
    $("#mobileNavContainer").addClass("slide-mobile-nav");
  }
});

// Keyboard accessibility for btns that are <img> elements
// dynamically generated img btns currently don't get this event

$(`img[tabindex="0"]`).on("keypress", (event) => {
  if (event.which === 13) {
    $(event.target).trigger("click");
  }
});

// swap between read and edit fields
function switchFromReadToEdit(target) {
  target.parents(".toggle-read-edit").find(".read-data").addClass("hide");
  target.parents(".toggle-read-edit").find(".edit-data").removeClass("hide");
}

$(".edit-btn").on("click", (event) => {
  switchFromReadToEdit($(event.target));
  $(event.target).addClass("hide");
});

// ------------------------------------------------------------ solution specific ------------------------------------------------------------

// side nav
$(".show-nav-btn").on("click", () => {
  const btn = $(".show-nav-btn");

  if (btn.hasClass("point-left")) {
    // close nav
    btn.removeClass("point-left");
    btn.addClass("point-right");
    $("nav").addClass("hide-nav");
    $(".content-container").css("margin-left", "0vw");
    $(".content-container").css("width", "100vw");
  }
  else if (btn.hasClass("point-right")) {
    // open nav
    btn.removeClass("point-right");
    btn.addClass("point-left");
    $("nav").removeClass("hide-nav");
    $(".content-container").css("margin-left", "15vw");
    $(".content-container").css("width", "85vw");
  }
});

});
