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
  $(".expand-history-btn").on("click", expandHistoryHandler);
  $(".more-table-options-btn").on("click", moreTableOptionsHandler);
  $(".start-pause-btn").on("click", startPauseHandler);
  $(".reset-btn").on("click", resetHandler);
  $("#shiftEditBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#editShiftModal"), openModal);
  });
  $("#shiftDelBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#editShiftModal"), openModal);
  });
  $("#shiftDelConfBtn").on("click", () => {

  });
  $("#shiftDelDenyBtn").on("click", () => {

  });
}); 
