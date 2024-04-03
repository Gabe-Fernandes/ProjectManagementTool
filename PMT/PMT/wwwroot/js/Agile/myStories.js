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



  // Search filter & table pagination
  let foundSet = []; // create array for tr's
  let currentPage = 1;
  let resultsPerPage = 10;
  let numOfPages = 0;

  // 300 records 270ms?
  // 1000 records 6600ms - 7200ms
  // 300 records (no pagination) 21ms?
  function searchFilter() {
    console.time("searchFilter");
    foundSet = [];
    const filterString = ($("#filterInput").val()).toLowerCase();
    const showResolved = $("#showResolvedCheckbox").is(":checked");
    const color0 = $(".table-wrap").find("tbody:first").find("tr:first").css("background-color");
    const color1 = $(".table-wrap").css("background-color");
    let toggle = 0;
    const numOfRows = $(".sortStatus").length // storing this brought us from 6600-7200 to 6500-6800

    for (let i = 0; i < numOfRows; i++) { // iterate through each tr
      const trElement = $(`#myStoriesTR_${i}`);
      const status = $(`#myStoriesStatus_${i}`).html();
      const title = $(`#myStoriesTitle_${i}`).html().toLowerCase();

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
        continue;
      }
      trElement.addClass("hide");
    }

    paginateTable();
    console.timeEnd("searchFilter");
  }

  $("#filterInput").on("input", searchFilter);
  $("#showResolvedCheckbox").on("input", searchFilter);


  $("#firstPageBtn").on("click", () => {
    currentPage = 1;
    $("#currentPageInput").val(currentPage);
    paginateTable();
  });
  $("#prevPageBtn").on("click", () => {
    if (currentPage > 1) {
      currentPage--;
      $("#currentPageInput").val(currentPage);
      paginateTable();
    }
  });
  $("#nextPageBtn").on("click", () => {
    if (currentPage < numOfPages) {
      currentPage++;
      $("#currentPageInput").val(currentPage);
      paginateTable();
    }
  });
  $("#lastPageBtn").on("click", () => {
    currentPage = numOfPages;
    $("#currentPageInput").val(currentPage);
    paginateTable();
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
    paginateTable();
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
    paginateTable();
  });



  function paginateTable() {
    console.time("paginateTable");

    const foundSetSize = foundSet.length;
    const indexOfFirstShown = ((currentPage - 1) * resultsPerPage) + 1;
    let indexofLastShown = currentPage * resultsPerPage;

    // this could be true on the last page if it's not entirely full
    if (indexofLastShown > foundSetSize) { indexofLastShown = foundSetSize }

    // this can happen if a filter reduces the total pages and the current page becomes out of bounds - kick user to page one
    if (indexOfFirstShown > indexofLastShown) {
      currentPage = 1;
      $("#currentPageInput").val(1);
      return paginateTable();
    }
    numOfPages = Math.ceil(foundSetSize / resultsPerPage);
    let pages = [];
    let tempArr = [];
    let pageCount = 0;
    let resultCount = 0;

    // populate the pages data structure where every array is a page and every 2nd dimmensional element is a tr
    for (let i = 0; i < foundSetSize; i++) {
      if (resultCount === resultsPerPage) {
        pages.push(tempArr);
        tempArr = [];
        resultCount = 0;
        pageCount++;
      }
      resultCount++;
      tempArr.push(foundSet[i]);
    }
    pages.push(tempArr);

    console.log(pages);
    console.log(`num of pages: ${numOfPages}`)

    // show tr's from the current page and hide the rest
    for (let i = 0; i < pages.length; i++) {
      for (let j = 0; j < pages[i].length; j++) {
        if (i === currentPage - 1) {
          pages[i][j].removeClass("hide");
        }
        else {
          pages[i][j].addClass("hide");
        }
      }
    }

    const showingMsg = (foundSetSize === 0) ? "0 search results found" : `Showing ${indexOfFirstShown}-${indexofLastShown} of ${foundSetSize}`;
    $("#showingResultsLabel").html(showingMsg);
    console.timeEnd("paginateTable");
  }



  // init
  searchFilter();
  $("#resultsPerPageInput").val(resultsPerPage);
  $("#currentPageInput").val(currentPage);
});
