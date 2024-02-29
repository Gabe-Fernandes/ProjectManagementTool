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
  function stopwatchEditConfHandler(event) {

  }
  function stopwatchEditDenyHandler(event) {

  }
  function stopWatchEditHandler(event) {

  }
  function stopWatchDelHandler(event) {
    ToggleModal($("#timeTrackerContent"), $("#delStopWatchModal"), openModal);
  }
  function expandHistoryHandler(event) {
    const stopwatch = $(event.target).parents(".stopwatch:first");
    const historyWrap = stopwatch.find(".history-wrap:first");
    const imgArrow = stopwatch.find(".history-arrow:first");
    const tableWrap = stopwatch.find(".table-wrap:first");

    if (imgArrow.hasClass("downward-arrow")) {
      // closing history
      imgArrow.removeClass("downward-arrow");
      stopwatch.css("height", "50vh");
      historyWrap.css("height", "5vh");
      tableWrap.css("height", "0vh");
    }
    else {
      // opening history
      imgArrow.addClass("downward-arrow");
      stopwatch.css("height", "70vh");
      historyWrap.css("height", "25vh");
      tableWrap.css("height", "20vh");
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
  $("#shiftEditBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#editShiftModal"), openModal);
    ToggleModal($("#timeTrackerContent"), $("#delShiftModal"), closeModal);
    // hide conf
    $("#delShiftImgWrap").removeClass("hide");
    $(".conf-wrap:first").addClass("hide");
  });
  $("#shiftDelBtn").on("click", () => {
    // show conf
    $("#delShiftImgWrap").addClass("hide");
    $(".conf-wrap:first").removeClass("hide");
  });
  $("#shiftDelConfBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#delShiftModal"), closeModal);
    // hide conf
    $("#delShiftImgWrap").removeClass("hide");
    $(".conf-wrap:first").addClass("hide");
  });
  $("#shiftDelDenyBtn").on("click", () => {
    // hide conf
    $("#delShiftImgWrap").removeClass("hide");
    $(".conf-wrap:first").addClass("hide");
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
