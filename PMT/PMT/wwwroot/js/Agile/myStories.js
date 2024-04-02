$(function () {
  HighlightCurrentNavBtn($("#agileNavBtn"));



  // modal events
  $(".del-btn").on("click", (event) => {
    $("#idToDelInput").val($(event.target).attr("data-idToDel"));
    ToggleModal($("#myStoriesContent"), $("#delStoryModal"), openModal);
  });
  $("#delCancelBtn").on("click", () => {
    ToggleModal($("#myStoriesContent"), $("#delStoryModal"), closeModal);
  });
  $("#delCloseBtn").on("click", () => {
    ToggleModal($("#myStoriesContent"), $("#delStoryModal"), closeModal);
  });



  // TH sorting events
  let thDueDateInOrder = true;
  let thPointsInOrder = true;
  let thTitleInOrder = true;
  let thStatusInOrder = true;

  $("#thDueDate").on("click", () => {
    thDueDateInOrder = thSortEvent("myStoriesTbody", thDueDateInOrder, "myStoriesTR", "sortDueDate", chronologicallyFirst);
  });
  $("#thPoints").on("click", () => {
    thPointsInOrder = thSortEvent("myStoriesTbody", thPointsInOrder, "myStoriesTR", "sortPoints", numericallyFirst);
  });
  $("#thTitle").on("click", () => {
    thTitleInOrder = thSortEvent("myStoriesTbody", thTitleInOrder, "myStoriesTR", "sortTitle", alphabeticallyFirst);
  });
  $("#thStatus").on("click", () => {
    thStatusInOrder = thSortEvent("myStoriesTbody", thStatusInOrder, "myStoriesTR", "sortStatus", alphabeticallyFirst);
  });



  // Search filter
  function searchFilter() {
    const filterString = ($("#filterInput").val()).toLowerCase();
    const showResolved = $("#showResolvedCheckbox").is(":checked");
    const color0 = $(".table-wrap").find("tbody:first").find("tr:first").css("background-color");
    const color1 = $(".table-wrap").css("background-color");
    let toggle = 0;
    let foundSet = []; // create array for tr's

    for (let i = 0; i < $(".sortStatus").length; i++) { // iterate through each tr
      const status = $(".sortStatus").eq(i).find("label").html();
      const title = $(".sortTitle").find("a").eq(i).html().toLowerCase();
      const trElement = $(`#myStoriesTR_${i}`);

      if (showResolved === false && status === "Resolved") {
        trElement.addClass("hide");
        continue;
      }

      if (title.includes(filterString)) {
        trElement.removeClass("hide");
        foundSet.push(trElement); // used in table pagination
        // force the striped pattern because hidden tr's disrupt this
        const colorToUse = (toggle === 0) ? color0 : color1;
        trElement.css("background-color", colorToUse);
        toggle = (toggle === 0) ? 1 : 0;
      }
      else {
        trElement.addClass("hide");
      }
    }
    paginateTable(foundSet);
  }

  $("#filterInput").on("input", searchFilter);
  $("#showResolvedCheckbox").on("input", searchFilter);



  // table pagination
  let currentPage = 1;
  let resultsPerPage = 10;
  let numOfPages = 0;
  let pages = [];

  $("#firstPageBtn").on("click", () => {
    currentPage = 1;
    $("#currentPageInput").val(currentPage);
  });
  $("#prevPageBtn").on("click", () => {
    if (currentPage > 1) {
      currentPage--;
      $("#currentPageInput").val(currentPage);
    }
  });
  $("#nextPageBtn").on("click", () => {
    if (currentPage < numOfPages) {
      currentPage++;
      $("#currentPageInput").val(currentPage);
    }
  });
  $("#lastPageBtn").on("click", () => {
    currentPage = numOfPages;
    $("#currentPageInput").val(currentPage);
  });



  $("#resultsPerPageInput").on("input", () => {
    // clear validation
    HideError("resultsPerPageInput", "resultsPerPageInputErr");

    // validate input is a positive integer
    const input = $("#resultsPerPageInput").val();

    for (let i = 0; i < input.length; i++) {
      if (NumericalChars.includes(input[i]) === false || input <= 0) {
        ShowError("resultsPerPageInput", "resultsPerPageInputErr", "Invalid entry");
        return;
      }
    }
    resultsPerPage = parseInt(input);
  });

  $("#currentPageInput").on("input", () => {
    // clear validation
    HideError("currentPageInput", "currentPageInputErr");

    // validate input is an integer between 1 and numOfPages
    const input = $("#currentPageInput").val();

    for (let i = 0; i < input.length; i++) {
      if (NumericalChars.includes(input[i]) === false || 1 > input || input > numOfPages) {
        ShowError("currentPageInput", "currentPageInputErr", "Invalid page number");
        return;
      }
    }
    currentPage = parseInt(input);
  });



  function paginateTable(foundSet) {
    numOfPages = Math.ceil(foundSet.length / resultsPerPage);

    for (let i = 0; i < numOfPages; i++) {
      for (let j = 0; j < resultsPerPage; j++) {

      }
    }

    $("#showingResultsLabel").html(`Showing ${0}-${0} of ${foundSet.length}`);
  }



  // init
  searchFilter();
  console.log("num of pages")
  console.log(numOfPages)
});
