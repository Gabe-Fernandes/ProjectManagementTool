// =========================================================== Download File ===========================================================

function download(fileName, text) {
  let element = document.createElement("a");
  element.setAttribute("href", "data:text/plain;charset=utf-8," + encodeURIComponent(text));
  element.setAttribute("download", fileName);
  element.style.display = "none";
  document.body.appendChild(element);
  element.click();
  document.body.removeChild(element);
}

// =========================================================== Dropdown With Custom Option ===========================================================

function customDropdown(event) {
  const dropdown = $(event.target);
  const inputElement = dropdown.siblings(".custom-dropdown-input-wrap").find(".custom-dropdown-input:first");
  inputElement.val(dropdown.val());

  if (dropdown.val() === "custom") {
    dropdown.addClass("hide");
    inputElement.val("");
    inputElement.parent().removeClass("hide");
  }
}
$(".custom-dropdown").on("input", customDropdown);

// initialize dropdowns based on their inputs
loop_i: for (let i = 0; i < $(".custom-dropdown").length; i++) {
  const dropdown = $(".custom-dropdown").eq(i);
  const inputElement = dropdown.siblings(".custom-dropdown-input-wrap").find(".custom-dropdown-input:first");
  const options = dropdown.children();
  for (let j = 0; j < options.length; j++) {
    if (options.eq(j).val() === inputElement.val()) {
      options.eq(j).attr("selected", "selected");
      continue loop_i;
    }
  }
  // switch to custom mode if the value isn't in the dropdown
  dropdown.addClass("hide");
  inputElement.parent().removeClass("hide");
}

function customDropdownArrow(event) {
  const selectElement = $(event.target).parent().siblings(".custom-dropdown:first");
  selectElement.removeClass("hide");
  $(event.target).parent().addClass("hide");

  const option = selectElement.find("option:first");
  option.attr("selected", "selected");
  selectElement.val(option.val());

  const dropdownWrap = selectElement.siblings(".custom-dropdown-input-wrap:first");
  const inputId = dropdownWrap.children(".custom-dropdown-input:first").attr("id");
  const errSpanId = dropdownWrap.children(".err:first").attr("id");
  HideError(inputId, errSpanId);

  $(`#${inputId}`).val(option.val());
}
$(".custom-dropdown-arrow").on("click", customDropdownArrow);

// =========================================================== Modal ===========================================================

const openModal = "O";
const closeModal = "C";

function ToggleModal(main, modal, direction) {
  if (direction === openModal) {
    main.addClass("unclickable");
    modal.removeClass("fade");
  }
  else if (direction === closeModal) {
    main.removeClass("unclickable");
    modal.addClass("fade");
  }
}

function switchToMobileModal() {
  // if entering mobile mode
  if ($(window).width() <= 768) {
    for (let i = 0; i < $(".screen-tint").length; i++) {
      $(".screen-tint").eq(i).children().first().addClass("mobile-modal");
    }
  }
  // if exiting mobile mode
  else {
    $(".screen-tint").children().removeClass("mobile-modal");
  }
}

$(window).on("resize", switchToMobileModal);
switchToMobileModal();

// =========================================================== Toggle Password Show ===========================================================

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

// =========================================================== Accessibility ===========================================================

// Keyboard accessibility for btns that are <img> elements
// dynamically generated img btns need this event added on creation
function addKeyboardAccessibility(event) {
  if (event.which === 13) {
    $(event.target).trigger("click");
  }
}
$(`img[tabindex="0"]`).on("keypress", addKeyboardAccessibility);

// =========================================================== Misc. ===========================================================

function delay(time) {
  return new Promise(resolve => setTimeout(resolve, time));
}

// =========================================================== Pagination ===========================================================

// HTML requirement is to give each page a shared class(no underscores) and a 0 indexed id with that exact class name and an underscore in between ex: register_4.
// Also, every page should have the hide class except for the one showing on page load
function getIdIndex(textId) {
  for (let i = 0; i < textId.length; i++) {
    if (textId[i] === '_') {
      const idIndex = textId.substring(i + 1);
      return parseInt(idIndex);
    }
  }
}
function paginateClickHandler(direction, namespace) {
  const openPageId = $(`.${namespace}:not(.hide)`).attr("id");
  let idIndex = getIdIndex(openPageId);

  if (direction === "left") {
    idIndex--;
  }
  else if (direction === "right") {
    idIndex++;
  }

  const newId = namespace + "_" + idIndex;

  // if next page exists, hide all then show the page
  if ($(`#${newId}`).length > 0) {
    $(`.${namespace}`).addClass("hide");
    $(`#${newId}`).removeClass("hide");
  }
}
function paginate(namespace, leftBtnId, rightBtnId) {
  $(`#${leftBtnId}`).on("click", () => {
    paginateClickHandler("left", namespace);
  });
  $(`#${rightBtnId}`).on("click", () => {
    paginateClickHandler("right", namespace);
  });
}

// =========================================================== Read/Edit Swap ===========================================================

function switchFromReadToEdit(target) {
  target.parents(".toggle-read-edit").find(".read-data").addClass("fade-abs");
  target.parents(".toggle-read-edit").find(".edit-data").removeClass("fade-abs");
}

$(".edit-btn").on("click", (event) => {
  switchFromReadToEdit($(event.target));
  $(event.target).addClass("fade-abs");
});

// =========================================================== Navigation Menu ===========================================================

function HighlightCurrentNavBtn(btnToHighlight) {
  $(".nav-btn").removeClass("nav-highlight");
  btnToHighlight.addClass("nav-highlight");
}

window.setTimeout(() => {
  $(".preload").removeClass("preload");
}, 250);

// side nav
function toggleNav() {
  if (sessionStorage.getItem("navState") === "opened") {
    closeNav();
  }
  else if (sessionStorage.getItem("navState") === "closed") {
    openNav();
  }
}

function openNav() {
  $(".show-nav-btn").removeClass("point-right");
  $(".show-nav-btn").addClass("point-left");
  $("nav").removeClass("hide-nav");
  $(".content-container").css("margin-left", "14vw");
  $(".content-container").css("width", "84%");
  sessionStorage.setItem("navState", "opened");
}

function closeNav() {
  $(".show-nav-btn").removeClass("point-left");
  $(".show-nav-btn").addClass("point-right");
  $("nav").addClass("hide-nav");
  $(".content-container").css("margin-left", "0vw");
  $(".content-container").css("width", "100%");
  sessionStorage.setItem("navState", "closed");
}

// set nav state if it's undefined (first time on a page with the nav or new tab/window)
if (sessionStorage.getItem("navState") == undefined && $(".show-nav-btn:first").length > 0) {
  const initState = $(".show-nav-btn:first").hasClass("point-left") ? "opened" : "closed";
  sessionStorage.setItem("navState", initState);
}

// close nav on pageload if that's the current setting
if (sessionStorage.getItem("navState") === "closed") {
  closeNav();
}

$(".show-nav-btn").on("click", toggleNav);

// --- Mobile Nav ---
function switchToMobileNav() {
  // if entering mobile mode
  if ($(window).width() <= 768) {
    $("nav").removeClass(navClass);
    $("nav").addClass("mobile-nav");
    if ($("nav").hasClass("hide-nav")) {
      $("nav").removeClass("hide-nav");
      $("nav").addClass("nav-was-hidden");
    }

    $(".content-container").addClass("was-content-container");
    $(".content-container").removeClass("content-container");

    $(".show-nav-btn").addClass("hide");
    $(".mobile-nav").css("transition", "left 0.25s");
    $(".mobile-nav").css("height", "10vh");

    $("nav").find("img").not("#mobileNavBtn").addClass("squish-nav-item");
    $("nav").find("label").addClass("squish-nav-item");
    $("nav").find("a").addClass("squish-nav-item");
    $(".nav-item").css("transition", "opacity 0s");
    $(".nav-item").addClass("squish-nav-item");
  }
  // if exiting mobile mode
  else {
    $("nav").addClass(navClass);
    $("nav").removeClass("mobile-nav");
    if ($("nav").hasClass("nav-was-hidden")) {
      $("nav").removeClass("nav-was-hidden");
      $("nav").addClass("hide-nav");
    }

    $(".was-content-container").addClass("content-container");
    $(".was-content-container").removeClass("was-content-container");

    $(".show-nav-btn").removeClass("hide");
    $(".side-nav").css("height", "100vh");

    $("nav").find("img").not("#mobileNavBtn").removeClass("squish-nav-item");
    $("nav").find("label").removeClass("squish-nav-item");
    $("nav").find("a").removeClass("squish-nav-item");
    $(".nav-item").css("transition", "opacity 0.3s");
    $(".nav-item").removeClass("squish-nav-item");
  }
}

$(window).on("resize", switchToMobileNav);
const navClass = $("nav").hasClass("top-nav") ? "top-nav" : "side-nav";
switchToMobileNav();

// Mobile Nav Menu Toggle
$(".mobile-nav-btn").on("click", () => {
  const action = $(".mobile-nav").attr("style").includes("height: 50vh;") ? "close" : "open";

  if (action === "open") {
    $(".mobile-nav").css("transition", "height 0.3s");
    $(".mobile-nav").css("height", "50vh");
    $(".nav-item").css("transition", "opacity 0.3s");
    $(".nav-item").removeClass("squish-nav-item");
    $("nav").find("img").not("#mobileNavBtn").not(".show-nav-btn").removeClass("squish-nav-item");
    $("nav").find("label").removeClass("squish-nav-item");
    $("nav").find("a").removeClass("squish-nav-item");
  }
  else if (action === "close") {
    $(".mobile-nav").css("transition", "height 0.3s");
    $(".mobile-nav").css("height", "10vh");
    $(".nav-item").css("transition", "opacity 0s");
    $(".nav-item").addClass("squish-nav-item");
    $("nav").find("img").not("#mobileNavBtn").not(".show-nav-btn").addClass("squish-nav-item");
    $("nav").find("label").addClass("squish-nav-item");
    $("nav").find("a").addClass("squish-nav-item");
  }
});

// =========================================================== Table Features ===========================================================

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
// Only works for dates formatted as ToString("MMM dd, yyyy")
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
