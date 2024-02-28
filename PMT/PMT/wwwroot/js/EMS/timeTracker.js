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
    const imgArrow = $(event.target).parent(".img-btn-wrap:first").find(".history-arrow:first");
    if (imgArrow.hasClass("downward-arrow")) {
      imgArrow.removeClass("downward-arrow");
    }
    else {
      imgArrow.addClass("downward-arrow");
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
  $(".history-wrap").on("click", expandHistoryHandler);
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
