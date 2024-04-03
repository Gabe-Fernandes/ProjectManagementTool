$(function () {
  HighlightCurrentNavBtn($("#bugTrackerNavBtn"));



  // Modal Events
  $("#delCancelBtn").on("click", () => {
    ToggleModal($("#bugTrackingContent"), $("#delBugReportModal"), closeModal);
  });
  $("#delCloseBtn").on("click", () => {
    ToggleModal($("#bugTrackingContent"), $("#delBugReportModal"), closeModal);
  });
  $(".del-btn").on("click", (event) => {
    ToggleModal($("#bugTrackingContent"), $("#delBugReportModal"), openModal);
    const idToDel = $(event.target).attr("data-bugReportId");
    $("#bugReportIdToDel").val(idToDel);
  });



  // TH sorting events
  let thDueDateInOrder = true;
  let thPriorityInOrder = true;
  let thPointsInOrder = true;
  let thDescriptionInOrder = true;
  let thStatusInOrder = true;

  $("#thDueDate").on("click", () => {
    thDueDateInOrder = thSortEvent("bugTrackingTbody", thDueDateInOrder, "bugTrackingTR", "sortDueDate", chronologicallyFirst);
  });
  $("#thPriority").on("click", () => {
    thPriorityInOrder = thSortEvent("bugTrackingTbody", thPriorityInOrder, "bugTrackingTR", "sortPriority", alphabeticallyFirst);
  });
  $("#thPoints").on("click", () => {
    thPointsInOrder = thSortEvent("bugTrackingTbody", thPointsInOrder, "bugTrackingTR", "sortPoints", numericallyFirst);
  });
  $("#thDescription").on("click", () => {
    thDescriptionInOrder = thSortEvent("bugTrackingTbody", thDescriptionInOrder, "bugTrackingTR", "sortDescription", alphabeticallyFirst);
  });
  $("#thStatus").on("click", () => {
    thStatusInOrder = thSortEvent("bugTrackingTbody", thStatusInOrder, "bugTrackingTR", "sortStatus", alphabeticallyFirst);
  });



  // Search filter & table pagination
  let foundSet = []; // create array for tr's
  let currentPage = 1;
  let resultsPerPage = 10;
  let numOfPages = 0;

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
      const trElement = $(`#bugTrackingTR_${i}`);
      const status = $(`#bugTrackingStatus_${i}`).html();
      const description = $(`#bugTrackingDesc_${i}`).html().toLowerCase();

      if (showResolved === false && status === "Resolved") {
        trElement.addClass("hide");
        continue;
      }

      if (description.includes(filterString)) {
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



  //// Search filter
  //function searchFilter() {
  //  const filterString = ($("#filterInput").val()).toLowerCase();
  //  const showResolved = $("#showResolvedCheckbox").is(":checked");
  //  const color0 = $(".table-wrap").find("tbody:first").find("tr:first").css("background-color");
  //  const color1 = $(".table-wrap").css("background-color");
  //  let toggle = 0;

  //  for (let i = 0; i < $(".sortStatus").length; i++) { // iterate through each tr
  //    const status = $(".sortStatus").eq(i).find("label").html();
  //    const description = $(".sortDescription").find("a").eq(i).html().toLowerCase();
  //    const trElement = $(`#bugTrackingTR_${i}`);

  //    if (showResolved === false && status === "Resolved") {
  //      trElement.addClass("hide");
  //      continue;
  //    }

  //    if (description.includes(filterString)) {
  //      trElement.removeClass("hide");
  //      // force the striped pattern because hidden tr's disrupt this
  //      const colorToUse = (toggle === 0) ? color0 : color1;
  //      trElement.css("background-color", colorToUse);
  //      toggle = (toggle === 0) ? 1 : 0;
  //    }
  //    else {
  //      trElement.addClass("hide");
  //    }
  //  }
  //}

  //$("#filterInput").on("input", searchFilter);
  //$("#showResolvedCheckbox").on("input", searchFilter);
  //searchFilter();
});
