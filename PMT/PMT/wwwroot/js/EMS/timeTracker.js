$(function () { 
  let projId = "";

  // ============================================================================= init page =============================================================================

  // Create and start signalR connection
  var timeTrackerHub = new signalR.HubConnectionBuilder().withUrl("/timeTracker").build();
  function timeTrackerHubSuccess() { console.log("timeTrackerHub success"); pageLoad() }
  function failure() { console.log("failure") }
  timeTrackerHub.start().then(timeTrackerHubSuccess, failure);
  timeTrackerHub.onclose(async () => await timeTrackerHub.start());

  function pageLoad() {
    projId = parseInt($("#projIdForJs").val());
    timeTrackerHub.send("GetStopwatches", projId);

    // there might not be any static elements like this to give events to =================================================================================
    $(".stopwatch-edit-conf-btn").on("click", stopwatchEditConfHandler);
    $(".stopwatch-edit-deny-btn").on("click", backToReadMode);
    $(".stopwatch-edit-btn").on("click", stopWatchEditHandler);
    $(".stopwatch-del-btn").on("click", stopWatchDelHandler);
    $(".history-title:first").on("click", expandHistoryHandler);
    $(".more-table-options-btn").on("click", moreTableOptionsHandler);
    $(".start-pause-btn").on("click", startPauseHandler);
    $(".reset-btn").on("click", resetHandler);
  }

  timeTrackerHub.on("PrintStopwatches", stopwatches => {
    for (let i = 0; i < stopwatches.length; i++) {
      renderStopwatch(stopwatches[i].name, stopwatches[i].id);
    }
  });

  // ============================================================================= Modal Events =============================================================================

  $("#delStopWatchCloseBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#delStopWatchModal"), closeModal);
  });
  $("#delShiftCloseBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#delShiftModal"), closeModal);
  });
  $("#editShiftCloseBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#editShiftModal"), closeModal);
  });

  // ============================================================================= helper functions =============================================================================

  function getStopwatchNameAndId(event) {
    const stopwatch = $(event.target).parents(".stopwatch:first");
    const stopwatchId = stopwatch.attr("data-id");
    const stopwatchNameWrap = stopwatch.find(".name:first");
    const stopwatchName = stopwatchNameWrap.find("h2").html();
    return [stopwatchId, stopwatchName];
  }

  function renderStopwatch(stopwatchName, stopwatchId) {
    const stopwatchEditConfBtn = $(`<img tabindex="0" src="/icons/confirm.png" class="stopwatch-edit-conf-btn fade">`);
    const stopwatchEditDenyBtn = $(`<img tabindex="0" src="/icons/deny.png" class="stopwatch-edit-deny-btn fade">`);
    const stopwatchEditBtn = $(`<img tabindex="0" src="/Icons/Edit.png" class="stopwatch-edit-btn edit-btn" />`);
    const stopwatchDelBtn = $(`<img tabindex="0" src="/Icons/Delete.png" class="stopwatch-del-btn" />`);
    const expandHistoryBtn = $(`
          <div class="history-title">
					  <img tabindex="0" src="/icons/ContextMenuArrow.png" class="history-arrow">
					  <button class="btn text-btn">History</button>
				  </div>`);
    const moreTableOptionsBtn = $(`<td><img tabindex="0" src="/Icons/Plus.png" class="more-table-options-btn" /></td>`);
    const startBtn = $(`<button type="button" class="btn start-pause-btn">Start</button>`);
    const resetBtn = $(`<button type="button" class="btn reset-btn">Reset</button>`);

    const stopWatch = $(`
      <section class="stopwatch toggle-read-edit" data-id="${stopwatchId}">

			  <div class="btn-bar">
				  <div class="left">
					  <label class="fade">Save changes?</label>

				  </div>
				  <div class="right">

				  </div>
			  </div>

			  <div class="name">
				  <h2 class="read-data">${stopwatchName}</h2>
				  <input class="edit-data fade-abs" type="text" placeholder="stopwatch name" />
			  </div>

			  <div class="history-wrap">

				  <div class="table-wrap">
					  <table>
						  <thead>
							  <tr>
								  <th class="sortable-th" id="">Date</th>
								  <th class="sortable-th" id="">Clock In</th>
								  <th class="sortable-th" id="">Clock Out</th>
								  <th class="sortable-th" id="">Hours</th>
								  <th></th>
							  </tr>
						  </thead>
						  <tbody id="">
						  </tbody>
					  </table>
				  </div>

			  </div>

			  <div class="timer-img-wrap">
				  <img src="/Images/hourglass.gif" />
			  </div>

			  <div class="time-wrap">
				  <span class="time">00:00:00</span>
			  </div>

			  <div class="btn-wrap">

			  </div>
	    </section>
    `);

    // set events
    stopwatchEditConfBtn.on("click", stopwatchEditConfHandler);
    stopwatchEditDenyBtn.on("click", backToReadMode);
    stopwatchEditBtn.on("click", stopWatchEditHandler);
    stopwatchDelBtn.on("click", stopWatchDelHandler);
    expandHistoryBtn.on("click", expandHistoryHandler);
    moreTableOptionsBtn.on("click", moreTableOptionsHandler);
    startBtn.on("click", startPauseHandler);
    resetBtn.on("click", resetHandler);

    // append elements that have events
    stopWatch.find(".left:first").append(stopwatchEditConfBtn);
    stopWatch.find(".left:first").append(stopwatchEditDenyBtn);
    stopWatch.find(".right:first").append(stopwatchEditBtn);
    stopWatch.find(".right:first").append(stopwatchDelBtn);
    stopWatch.find(".history-wrap:first").prepend(expandHistoryBtn);
    stopWatch.find(".btn-wrap:first").append(startBtn);
    stopWatch.find(".btn-wrap:first").append(resetBtn);

    $("#stopwatchContainer").append(stopWatch);
  }

  // ============================================================================= btn handlers =============================================================================

  function backToReadMode(event) {
    const stopwatch = $(event.target).parents(".stopwatch:first");

    stopwatch.find(".read-data:first").removeClass("fade-abs");
    stopwatch.find(".stopwatch-edit-btn:first").removeClass("fade-abs");

    stopwatch.find(".edit-data").addClass("fade-abs");
    stopwatch.find("label:first").addClass("fade");
    stopwatch.find(".stopwatch-edit-conf-btn:first").addClass("fade");
    stopwatch.find(".stopwatch-edit-deny-btn:first").addClass("fade");
  }
  function stopwatchEditConfHandler(event) {
    const stopwatch = $(event.target).parents(".stopwatch:first");
    const stopwatchName = stopwatch.find(".edit-data").val();

    if (stopwatchName !== "") {
      const stopwatchId = parseInt(stopwatch.attr("data-id"));
      timeTrackerHub.send("EditStopWatch", stopwatchId, stopwatchName);
      stopwatch.find(".read-data:first").html(stopwatchName);
      backToReadMode(event);
    }
    else {
      // error handle ========================================================================================================
    }
  }
  function stopWatchEditHandler(event) {
    // enter edit mode
    const stopwatch = $(event.target).parents(".stopwatch:first");

    stopwatch.find(".stopwatch-edit-conf-btn:first").removeClass("fade");
    stopwatch.find(".stopwatch-edit-deny-btn:first").removeClass("fade");
    stopwatch.find("label:first").removeClass("fade");

    const stopwatchNameWrap = stopwatch.find(".name:first");
    const stopwatchName = stopwatchNameWrap.find("h2").html();
    stopwatch.find(".read-data:first").addClass("fade-abs");
    stopwatch.find(".edit-data").removeClass("fade-abs");
    stopwatch.find(".edit-data").val(stopwatchName);
  }
  function stopWatchDelHandler(event) {
    const idAndName = getStopwatchNameAndId(event);
    $("#delStopWatchModal").find("h3:first").html(`Are you sure you want to delete the ${idAndName[1]} stopwatch?`);
    $("#delStopWatchModal").attr("data-stopwatchId", idAndName[0]);
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
    const shiftId = $(event.target).attr("data-shiftId");
    $("#delShiftModal").attr("data-shiftId", shiftId);
    ToggleModal($("#timeTrackerContent"), $("#delShiftModal"), openModal);
  }
  function startPauseHandler(event) {

  }
  function resetHandler(event) {

  }

  // ============================================================================= btn events =============================================================================

  $("#newStopwatchBtn").on("click", () => {
    renderStopwatch("New Stopwatch", -1); // once saved to the db, the id will be passed to the client and look for the stopwatch with id "-1"
    timeTrackerHub.send("CreateStopwatch", projId);
  });

  // del stopwatch
  $("#delStopwatchYesBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#delStopWatchModal"), closeModal);
    const stopwatchId = $("#delStopWatchModal").attr("data-stopwatchId");
    timeTrackerHub.send("DelStopWatch", parseInt(stopwatchId));
  });
  $("#delStopwatchNoBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#delStopWatchModal"), closeModal);
  });

  // update shift
  $("#shiftEditBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#editShiftModal"), openModal);
    ToggleModal($("#timeTrackerContent"), $("#delShiftModal"), closeModal);
    // fade conf
    $("#delShiftImgWrap").removeClass("fade-abs");
    $(".conf-wrap:first").addClass("fade-abs");
  });
  $("#updateShiftBtn").on("click", () => {
    // validate input
    const shiftId = $("#delShiftModal").attr("data-shiftId");
    const shiftData = {
      clockIn: $("#editShiftClockIn").val(),
      clockOut: $("#editShiftClockOut").val(),
      date: $("#editShiftDate").val()
    };
    timeTrackerHub.send("EditShift", shiftId, shiftData);
    ToggleModal($("#timeTrackerContent"), $("#editShiftModal"), closeModal);
  });

  // del shift
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

    const shiftId = $("#delShiftModal").attr("data-shiftId");
    timeTrackerHub.send("DelShift", shiftId);
  });
  $("#shiftDelDenyBtn").on("click", () => {
    // fade conf
    $("#delShiftImgWrap").removeClass("fade-abs");
    $(".conf-wrap:first").addClass("fade-abs");
  });

  // ============================================================================= receive from server =============================================================================

  timeTrackerHub.on("ApplyStopwatchId", stopwatchId => {
    const stopwatch = $("#stopwatchContainer").children("section[data-id|='-1']");
    stopwatch.attr("data-id", stopwatchId);
  });
  timeTrackerHub.on("DelStopwatch", stopwatchId => {
    const stopwatch = $("#stopwatchContainer").children(`section[data-id|=${stopwatchId}]`);
    stopwatch.remove();
  });
});
