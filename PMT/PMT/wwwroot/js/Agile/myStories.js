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
        // force the striped pattern because hidden tr's disrupt this
        const colorToUse = (toggle === 0) ? color0 : color1;
        trElement.css("background-color", colorToUse);
        toggle = (toggle === 0) ? 1 : 0;
      }
      else {
        trElement.addClass("hide");
      }
    }
  }

  $("#filterInput").on("input", searchFilter);
  $("#showResolvedCheckbox").on("input", searchFilter);
  searchFilter();
});
