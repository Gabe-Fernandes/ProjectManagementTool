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
  }

  timeTrackerHub.on("PrintStopwatch", (stopwatchId, stopwatchName, clockIsRunning, clockRunningSince, timeSetDto) => {
    renderStopwatch(stopwatchId, stopwatchName, clockIsRunning, clockRunningSince, timeSetDto);
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

  function renderStopwatch(stopwatchId, stopwatchName, clockIsRunning, clockRunningSince, timeSetDto) {
    const stopwatchEditConfBtn = $(`<img tabindex="0" src="/icons/confirm.png" class="stopwatch-edit-conf-btn fade">`);
    const stopwatchEditDenyBtn = $(`<img tabindex="0" src="/icons/deny.png" class="stopwatch-edit-deny-btn fade">`);
    const stopwatchEditBtn = $(`<img tabindex="0" src="/Icons/Edit.png" class="stopwatch-edit-btn edit-btn" />`);
    const stopwatchDelBtn = $(`<img tabindex="0" src="/Icons/Delete.png" class="stopwatch-del-btn" />`);
    const expandHistoryBtn = $(`
          <div class="history-title btn text-btn">
					  <img tabindex="0" src="/icons/ContextMenuArrow.png" class="history-arrow">
					  <label>History</label>
				  </div>`);
    const moreTableOptionsBtn = $(`<td><img tabindex="0" src="/Icons/Plus.png" class="more-table-options-btn" /></td>`);
    const startPauseMode = (clockIsRunning) ? "pause" : "start";
    const startBtnText = (clockIsRunning) ? "Pause" : "Start"; // language and mode kept separate to not rely on English user-facing text
    const startBtn = $(`<button type="button" class="btn start-pause-btn" data-mode="${startPauseMode}">Start</button>`);
    startBtn.html(startBtnText);
    const resetBtn = $(`<button type="button" class="btn reset-btn">Reset</button>`);

    const stopwatch = $(`
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
				  <span class="time" data-startDateTime="-1"></span>
			  </div>

			  <div class="btn-wrap">

			  </div>
	    </section>
    `);
    $("#stopwatchContainer").append(stopwatch);

    // time intervals
    if (timeSetDto != undefined) {
      const tbody = stopwatch.find("tbody:first");
      for (let i = 0; i < timeSetDto.length; i++) {
        const timeSetTr = $(`<tr id="timeSet_${timeSetDto[i].id}" data-id="${timeSetDto[i].id}"><td colspan="5">${timeSetDto[i].timeSetHoursMsg}</td></tr>`);
        for (let j = 0; j < timeSetDto[i].intervals.length; j++) {
          const optionsBtn = $(`<td><img tabindex="0" src="/Icons/Plus.png" class="more-table-options-btn" data-shiftId="${timeSetDto[i].intervals[j].id}" /></td>`);
          const newTr = $(`<tr id="timeInterval_${timeSetDto[i].intervals[j].id}">
          <td>${timeSetDto[i].intervals[j].startDate}</td>
          <td>${timeSetDto[i].intervals[j].clockIn}</td>
          <td>${timeSetDto[i].intervals[j].clockOut}</td>
          <td>${timeSetDto[i].intervals[j].timeElapsed}</td>
        </tr>`);
          newTr.append(optionsBtn);
          optionsBtn.on("click", moreTableOptionsHandler);
          tbody.append(newTr);
        }
        tbody.append(timeSetTr);

        // set mili from server
        const timer = stopwatch.find(".time:first");
        timer.attr("data-mili", timeSetDto[i].timeSetMili);
        if (clockIsRunning) {
          timer.attr("data-startDateTime", Date.parse(clockRunningSince));
        }
      }
    }

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
    stopwatch.find(".left:first").append(stopwatchEditConfBtn);
    stopwatch.find(".left:first").append(stopwatchEditDenyBtn);
    stopwatch.find(".right:first").append(stopwatchEditBtn);
    stopwatch.find(".right:first").append(stopwatchDelBtn);
    stopwatch.find(".history-wrap:first").prepend(expandHistoryBtn);
    stopwatch.find(".btn-wrap:first").append(startBtn);
    stopwatch.find(".btn-wrap:first").append(resetBtn);
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
    const btn = $(event.target);
    const stopwatch = btn.parents(".stopwatch:first");
    const stopwatchId = stopwatch.attr("data-id");
    const timeSetTr = stopwatch.find("tbody:first").children("tr:last[data-id]");
    const timeSetId = timeSetTr.attr("data-id");
    const timer = stopwatch.find(".time:first");
    let timeSetTrToUpdate = stopwatch.find("tbody:first").find("tr:last");

    if (btn.attr("data-mode") === "start") {
      btn.html("Pause");
      btn.attr("data-mode", "pause");
      timer.attr("data-startDateTime", Date.now);
      timeTrackerHub.send("StartBtn", projId, parseInt(stopwatchId), parseInt(timeSetId));
    }
    else if (btn.attr("data-mode") === "pause") {
      btn.html("Start");
      btn.attr("data-mode", "start");
      const timeIntervalId = timeSetTr.prev().find("img").attr("data-shiftId");
      timer.attr("data-startDateTime", "-1");
      timeTrackerHub.send("PauseBtn", parseInt(timeIntervalId), false);
    }
    timeTrackerHub.send("TimeSetRefresh", parseInt(stopwatchId), parseInt(timeSetTrToUpdate.attr("data-id")));
  }
  function resetHandler(event) {
    const stopwatch = $(event.target).parents(".stopwatch:first")
    const stopwatchId = stopwatch.attr("data-id");
    const timeIntervalId = stopwatch.find("tbody:first").children("tr:last").prev().find(".more-table-options-btn:first").attr("data-shiftId");
    const btn = $(event.target).siblings("start-pause-btn");
    const timer = stopwatch.find(".time:first");

    btn.html("Start");
    btn.attr("data-mode", "start");
    timer.attr("data-startDateTime", "-1");

    if (timeIntervalId != undefined) {
      timeTrackerHub.send("ResetBtn", projId, parseInt(stopwatchId), parseInt(timeIntervalId));
    }
  }

  // ============================================================================= btn events =============================================================================

  $("#newStopwatchBtn").on("click", () => {
    renderStopwatch(-1, "New Stopwatch", false, undefined); // once saved to the db, the id will be passed to the client and look for the stopwatch with id "-1"
    timeTrackerHub.send("CreateStopwatch", projId);
  });

  // del stopwatch -----------------------------
  $("#delStopwatchYesBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#delStopWatchModal"), closeModal);
    const stopwatchId = $("#delStopWatchModal").attr("data-stopwatchId");
    timeTrackerHub.send("DelStopWatch", parseInt(stopwatchId));
  });
  $("#delStopwatchNoBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#delStopWatchModal"), closeModal);
  });

  // update shift -----------------------------
  $("#shiftEditBtn").on("click", () => {
    //ToggleModal($("#timeTrackerContent"), $("#editShiftModal"), openModal);          Modal won't open until content is ready
    ToggleModal($("#timeTrackerContent"), $("#delShiftModal"), closeModal);
    // fade conf
    $("#delShiftImgWrap").removeClass("fade-abs");
    $(".conf-wrap:first").addClass("fade-abs");
  });
  $("#updateShiftBtn").on("click", () => {
    // validate input
    const shiftId = $("#delShiftModal").attr("data-shiftId");
    const shiftData = {
      startDate: $("#editStartDate").val(),
      endDate: $("#editEndDate").val(),
    };
    if (shiftData.endDate > shiftData.startDate) {
      timeTrackerHub.send("EditTimeInterval", parseInt(shiftId), shiftData);
    }
    else {
      // error handle ========================================================================================================
    }
    ToggleModal($("#timeTrackerContent"), $("#editShiftModal"), closeModal);
  });

  // del shift -----------------------------
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
    timeTrackerHub.send("DelTimeInterval", parseInt(shiftId));
  });
  $("#shiftDelDenyBtn").on("click", () => {
    // fade conf
    $("#delShiftImgWrap").removeClass("fade-abs");
    $(".conf-wrap:first").addClass("fade-abs");
  });

  // ============================================================================= receive from server =============================================================================

  timeTrackerHub.on("PrintTimeInterval", (stopwatchId, timeIntervalDto) => {
    const stopwatch = $("#stopwatchContainer").children(`section[data-id|=${stopwatchId}]`);
    const tbody = stopwatch.find("tbody:first");

    const optionsBtn = $(`<td><img tabindex="0" src="/Icons/Plus.png" class="more-table-options-btn" data-shiftId="${timeIntervalDto.id}" /></td>`);
    const newTr = $(`<tr id="timeInterval_${timeIntervalDto.id}">
        <td>${timeIntervalDto.startDate}</td>
        <td>${timeIntervalDto.clockIn}</td>
        <td>${timeIntervalDto.clockOut}</td>
        <td>${timeIntervalDto.timeElapsed}</td>
      </tr>`);
    newTr.append(optionsBtn);
    optionsBtn.on("click", moreTableOptionsHandler);
    const lastTr = tbody.children("tr:last");
    newTr.insertBefore(lastTr);
  });

  timeTrackerHub.on("ClockOutTimeInterval", (stopwatchId, timeIntervalId, clockOut, hours) => {
    const stopwatch = $("#stopwatchContainer").children(`section[data-id|=${stopwatchId}]`);
    const moreOptionsIconTd = stopwatch.find(`#timeInterval_${timeIntervalId}`).children("td:last");
    moreOptionsIconTd.prev().prev().html(clockOut);
    //moreOptionsIconTd.prev().html(Math.round(((hours / 3600000) + Number.EPSILON * 100) / 100));
    moreOptionsIconTd.prev().html(hours);
  });

  timeTrackerHub.on("PrintTimeSet", (stopwatchId, timeSetId, timeSetHoursMsg) => {
    // apply stopwatchId
    let stopwatch = $("#stopwatchContainer").children("section[data-id|='-1']");

    if (stopwatch.length === 1) {
      stopwatch.attr("data-id", stopwatchId);
    }
    else if (stopwatch.length === 0) {
      stopwatch = $("#stopwatchContainer").children(`section[data-id|=${stopwatchId}]`);
    }

    const tbody = stopwatch.find("tbody");
    const timeSetTr = $(`<tr id="timeSet_${timeSetId}" data-id="${timeSetId}"><td colspan="5">${timeSetHoursMsg}</td></tr>`);
    tbody.append(timeSetTr);

    // set miliseconds from server
    stopwatch.find(".time:first").attr("data-mili", "0");
  });

  timeTrackerHub.on("PauseUpdate", (stopwatchId, miliFromServer) => {
    const stopwatch = $("#stopwatchContainer").children(`section[data-id|=${stopwatchId}]`);
    stopwatch.find(".time:first").attr("data-mili", miliFromServer);
  });

  timeTrackerHub.on("DelStopwatch", stopwatchId => {
    const stopwatch = $("#stopwatchContainer").children(`section[data-id|=${stopwatchId}]`);
    stopwatch.remove();
  });

  timeTrackerHub.on("DelTimeInterval", (stopwatchId, timeIntervalId, changeTimeSetMsg) => {
    const stopwatch = $("#stopwatchContainer").children(`section[data-id|=${stopwatchId}]`);
    const timeInterval = stopwatch.find(`#timeInterval_${timeIntervalId}`);
    if (changeTimeSetMsg) {
      timeInterval.parent().children().last().children("td").html("no recorded times yet");
    }
    timeInterval.remove();

    // get a better refresh later //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    $("#stopwatchContainer").empty();
    timeTrackerHub.send("GetStopwatches", projId);
  });

  timeTrackerHub.on("TimeSetRefresh", (stopwatchId, timeSetId, timeSetText) => {
    const stopwatch = $("#stopwatchContainer").children(`section[data-id|=${stopwatchId}]`);
    stopwatch.find(`#timeSet_${timeSetId}`).html(`<td colspan="5">${timeSetText} hours since last reset</td>`);
  });

  // ============================================================================= clock =============================================================================

  function convertSecondsToTimeFormat(seconds) {
    let hours = 0;
    let minutes = 0;

    while (seconds >= 3600) {
      hours++;
      seconds -= 3600;
    }
    while (seconds >= 60) {
      minutes++
      seconds -= 60;
    }

    if (minutes.toString().length == 1) { minutes = "0" + minutes }
    if (seconds.toString().length == 1) { seconds = "0" + seconds }

    return `${hours}:${minutes}:${seconds}`;
  }

  setInterval(function () {
    const timers = $(".time"); // check every tick to account for deletions

    for (let i = 0; i < timers.length; i++) {
      let startDate = timers.eq(i).attr("data-startDateTime");
      if (startDate === "-1") { // if timer is paused
        startDate = Date.now();
      }
      const miliFromServer = timers.eq(i).attr("data-mili");
      const delta = (Date.now() - startDate) + parseInt(miliFromServer); // milliseconds
      const totalSeconds = Math.floor(delta / 1000);
      const timeFormat = convertSecondsToTimeFormat(totalSeconds);
      timers.eq(i).html(timeFormat);
    }
  }, 1000); // execute every second (not perfect accuracy, but the dela computation accounts for this)
});
