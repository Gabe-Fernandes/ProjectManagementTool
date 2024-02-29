$(function () { 
  // Modal Events
  $("#delStopWatchCloseBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#delStopWatchModal"), closeModal);
  });
  $("#delShiftCloseBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#delShiftModal"), closeModal);
  });
  $("#editShiftCloseBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#editShiftModal"), closeModal);
  });

  // btn events
  function backToReadMode(event) {
    const stopwatch = $(event.target).parents(".stopwatch:first");

    stopwatch.find(".read-data").removeClass("fade-abs");
    stopwatch.find(".stopwatch-edit-btn:first").removeClass("fade-abs");

    stopwatch.find(".edit-data").addClass("fade-abs");
    stopwatch.find("label:first").addClass("fade");
    stopwatch.find(".stopwatch-edit-conf-btn:first").addClass("fade");
    stopwatch.find(".stopwatch-edit-deny-btn:first").addClass("fade");
  }
  function stopwatchEditConfHandler(event) { // CRUD signalR call should be added here
    backToReadMode(event);
  }
  function stopwatchEditDenyHandler(event) { // if no code gets added here, remove this extra step and use backToReadMode as the callback function
    backToReadMode(event);
  }
  function stopWatchEditHandler(event) {
    const btnBar = $(event.target).parents(".btn-bar:first");
    btnBar.find(".stopwatch-edit-conf-btn:first").removeClass("fade");
    btnBar.find(".stopwatch-edit-deny-btn:first").removeClass("fade");
    btnBar.find("label:first").removeClass("fade");

    const stopwatchNameWrap = btnBar.parent().find(".name:first");
    const stopwatchName = stopwatchNameWrap.find("h2").html();
    stopwatchNameWrap.find("input").val(stopwatchName);
  }
  function stopWatchDelHandler(event) {
    ToggleModal($("#timeTrackerContent"), $("#delStopWatchModal"), openModal);
  }
  function expandHistoryHandler(event) {
    const stopwatch = $(event.target).parents(".stopwatch:first");
    const historyWrap = stopwatch.find(".history-wrap:first");
    const imgArrow = stopwatch.find(".history-arrow:first");
    const tableWrap = stopwatch.find(".table-wrap:first");
    const timerImgWrap = stopwatch.find(".timer-img-wrap:first");

    if (imgArrow.hasClass("downward-arrow")) {
      // closing history
      imgArrow.removeClass("downward-arrow");
      stopwatch.css("height", "50vh");
      historyWrap.css("height", "5vh");
      tableWrap.css("height", "0vh");
      timerImgWrap.css("height", "24vh");
    }
    else {
      // opening history
      imgArrow.addClass("downward-arrow");
      stopwatch.css("height", "70vh");
      historyWrap.css("height", "37vh");
      tableWrap.css("height", "32vh");
      timerImgWrap.css("height", "12vh");
    }
  }
  function moreTableOptionsHandler(event) {
    ToggleModal($("#timeTrackerContent"), $("#delShiftModal"), openModal);
  }
  function startPauseHandler(event) {

  }
  function resetHandler(event) {

  }

  $("#newStopwatchBtn").on("click", () => {

  });
  $(".stopwatch-edit-conf-btn").on("click", stopwatchEditConfHandler);
  $(".stopwatch-edit-deny-btn").on("click", stopwatchEditDenyHandler);
  $(".stopwatch-edit-btn").on("click", stopWatchEditHandler);
  $(".stopwatch-del-btn").on("click", stopWatchDelHandler);
  $(".history-title:first").on("click", expandHistoryHandler);
  $(".more-table-options-btn").on("click", moreTableOptionsHandler);
  $(".start-pause-btn").on("click", startPauseHandler);
  $(".reset-btn").on("click", resetHandler);
  $("#delStopwatchNoBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#delStopWatchModal"), closeModal);
  });
  $("#shiftEditBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#editShiftModal"), openModal);
    ToggleModal($("#timeTrackerContent"), $("#delShiftModal"), closeModal);
    // fade conf
    $("#delShiftImgWrap").removeClass("fade-abs");
    $(".conf-wrap:first").addClass("fade-abs");
  });
  $("#shiftDelBtn").on("click", () => {
    // show conf
    $("#delShiftImgWrap").addClass("fade-abs");
    $(".conf-wrap:first").removeClass("fade-abs");
  });
  $("#shiftDelConfBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#delShiftModal"), closeModal);
    // fade conf
    $("#delShiftImgWrap").removeClass("fade-abs");
    $(".conf-wrap:first").addClass("fade-abs");
  });
  $("#shiftDelDenyBtn").on("click", () => {
    // fade conf
    $("#delShiftImgWrap").removeClass("fade-abs");
    $(".conf-wrap:first").addClass("fade-abs");
  });

  // Create and start signalR connection
  var timeTrackerHub = new signalR.HubConnectionBuilder().withUrl("/timeTracker").build();
  function timeTrackerHubSuccess() { console.log("timeTrackerHub success"); pageLoad() }
  function failure() { console.log("failure") }
  timeTrackerHub.start().then(timeTrackerHubSuccess, failure);
  timeTrackerHub.onclose(async () => await timeTrackerHub.start());

  function pageLoad() {
    //const projId = $("#projIdForJs").val();
  }
}); 
