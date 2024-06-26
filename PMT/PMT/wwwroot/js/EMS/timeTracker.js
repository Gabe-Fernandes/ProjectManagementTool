$(function () { 

  let isVero = false;

  // ============================================================================= init page =============================================================================
  
  // Create and start signalR connection
  var timeTrackerHub = new signalR.HubConnectionBuilder().withUrl("/timeTracker").build();
  function timeTrackerHubSuccess() { console.log("timeTrackerHub success"); pageLoad() }
  function failure() { console.log("failure") }
  timeTrackerHub.start().then(timeTrackerHubSuccess, failure);
  timeTrackerHub.onclose(async () => await timeTrackerHub.start());

  function pageLoad() {
    if (timeTrackerHub.state !== "Connected") { connectionLost(); return; }
    timeTrackerHub.send("GetStopwatches");
  }

  function connectionLost() {
    alert("Connection lost - please refresh the page.");
  }

  timeTrackerHub.on("PrintStopwatch", (stopwatchId, stopwatchName, clockIsRunning, clockRunningSince, timeSetDto, isVeroFromServer) => {
    renderStopwatch(stopwatchId, stopwatchName, clockIsRunning, clockRunningSince, timeSetDto);
    isVero = isVeroFromServer;
    const stopwatch = $("#stopwatchContainer").children(`section[data-id|=${stopwatchId}]`);
    const tableWrap = stopwatch.find(".table-wrap:first");
    tableWrap.scrollTop(tableWrap.prop("scrollHeight"));
  });

  // ============================================================================= Modal Events =============================================================================

  $("#delStopWatchCloseBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#delStopWatchModal"), closeModal);
  });
  $("#delShiftCloseBtn").on("click", () => {
    // fade conf
    $("#delShiftImgWrap").removeClass("fade-abs");
    $(".conf-wrap:first").addClass("fade-abs");
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
    const startPauseMode = (clockIsRunning) ? "pause" : "start";
    const startBtnText = (clockIsRunning) ? "Pause" : "Start"; // language and mode kept separate to not rely on English user-facing text
    const startBtn = $(`<button type="button" class="btn start-pause-btn" data-mode="${startPauseMode}">Start</button>`);
    startBtn.html(startBtnText);
    const resetBtn = $(`<button type="button" class="btn reset-btn">Reset</button>`);
    const customBtn = $(`<button type="button" class="btn">Custom</button>`);

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
				  <h2 id="nameH2_${stopwatchId}" class="read-data">${stopwatchName}</h2>
          <div class="input-validation-wrap fade-abs">
				    <input id="nameInput_${stopwatchId}" class="edit-data" type="text" placeholder="stopwatch name" />
            <span id="nameInput_${stopwatchId}Err" class="err hide"></span>
          </div>
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
          let optionsBtn;
          if (timeSetDto[i].intervals[j].clockOut === "") { // if this interval is in progress, the extra options should be disabled
            optionsBtn = $(`<td class="disable-on-start"><img tabindex="0" src="/Icons/Plus.png" class="more-table-options-btn disable-on-start" data-shiftId="${timeSetDto[i].intervals[j].id}" /></td>`);
          }
          else {
            optionsBtn = $(`<td><img tabindex="0" src="/Icons/Plus.png" class="more-table-options-btn" data-shiftId="${timeSetDto[i].intervals[j].id}" /></td>`);
          }

          const newTr = $(`<tr id="timeInterval_${timeSetDto[i].intervals[j].id}">
          <td>${timeSetDto[i].intervals[j].startDate}</td>
          <td>${timeSetDto[i].intervals[j].clockIn}</td>
          <td>${timeSetDto[i].intervals[j].clockOut}</td>
          <td>${timeSetDto[i].intervals[j].timeElapsed}</td>
        </tr>`);
          newTr.append(optionsBtn);
          optionsBtn.on("click", moreTableOptionsHandler);
          optionsBtn.on("keypress", addKeyboardAccessibility);
          tbody.append(newTr);
        }
        tbody.append(timeSetTr);

        // set milli from server
        const timer = stopwatch.find(".time:first");
        timer.attr("data-milli", timeSetDto[i].timeSetMilli);
        if (clockIsRunning) {
          timer.attr("data-startDateTime", Date.parse(clockRunningSince));
          stopwatch.addClass("stopwatch-on");
        }
      }
    }

    // set events
    stopwatchEditConfBtn.on("click", stopwatchEditConfHandler);
    stopwatchEditConfBtn.on("keypress", addKeyboardAccessibility);
    stopwatchEditDenyBtn.on("click", backToReadMode);
    stopwatchEditDenyBtn.on("keypress", addKeyboardAccessibility);
    stopwatchEditBtn.on("click", stopWatchEditHandler);
    stopwatchEditBtn.on("keypress", addKeyboardAccessibility);
    stopwatchDelBtn.on("click", stopWatchDelHandler);
    stopwatchDelBtn.on("keypress", addKeyboardAccessibility);
    expandHistoryBtn.on("click", expandHistoryHandler);
    expandHistoryBtn.on("keypress", addKeyboardAccessibility);
    startBtn.on("click", startPauseHandler);
    resetBtn.on("click", resetHandler);
    customBtn.on("click", customHandler);
    // confirm name change with enter key    
    $(`#nameInput_${stopwatchId}`).on("keypress", (event) => {
      if (event.which === 13) {
        stopwatchEditConfHandler(event);
      }
    });
    $(`#nameInput_${stopwatchId}`).on("input", () => {
      HideError(`nameInput_${stopwatchId}`, `nameInput_${stopwatchId}Err`);
    });

    // append elements that have events
    stopwatch.find(".left:first").append(stopwatchEditConfBtn);
    stopwatch.find(".left:first").append(stopwatchEditDenyBtn);
    stopwatch.find(".right:first").append(stopwatchEditBtn);
    stopwatch.find(".right:first").append(stopwatchDelBtn);
    stopwatch.find(".history-wrap:first").prepend(expandHistoryBtn);
    stopwatch.find(".btn-wrap:first").append(startBtn);
    stopwatch.find(".btn-wrap:first").append(resetBtn);
    stopwatch.find(".btn-wrap:first").append(customBtn);
  }

  // ============================================================================= btn handlers =============================================================================

  function backToReadMode(event) {
    const stopwatch = $(event.target).parents(".stopwatch:first");
    const stopwatchId = parseInt(stopwatch.attr("data-id"));
    HideError(`nameInput_${stopwatchId}`, `nameInput_${stopwatchId}Err`);

    $(`#nameH2_${stopwatchId}`).removeClass("fade-abs");
    stopwatch.find(".stopwatch-edit-btn:first").removeClass("fade-abs");

    $(`#nameInput_${stopwatchId}`).parent().addClass("fade-abs");
    stopwatch.find("label:first").addClass("fade");
    stopwatch.find(".stopwatch-edit-conf-btn:first").addClass("fade");
    stopwatch.find(".stopwatch-edit-deny-btn:first").addClass("fade");
  }
  function stopwatchEditConfHandler(event) {
    const stopwatch = $(event.target).parents(".stopwatch:first");
    const stopwatchId = parseInt(stopwatch.attr("data-id"));
    const stopwatchName = $(`#nameInput_${stopwatchId}`).val();

    if (stopwatchName !== "") {
      if (timeTrackerHub.state !== "Connected") { connectionLost(); return; }
      timeTrackerHub.send("EditStopWatch", stopwatchId, stopwatchName);
      $(`#nameH2_${stopwatchId}`).html(stopwatchName);
      backToReadMode(event);
    }
    else {
      ShowError(`nameInput_${stopwatchId}`, `nameInput_${stopwatchId}Err`, "required");
    }
  }
  function stopWatchEditHandler(event) {
    const stopwatch = $(event.target).parents(".stopwatch:first");
    const stopwatchId = parseInt(stopwatch.attr("data-id"));

    stopwatch.find(".stopwatch-edit-conf-btn:first").removeClass("fade"); // enter edit mode
    stopwatch.find(".stopwatch-edit-deny-btn:first").removeClass("fade");
    stopwatch.find("label:first").removeClass("fade");

    const stopwatchName = $(`#nameH2_${stopwatchId}`).html();
    $(`#nameH2_${stopwatchId}`).addClass("fade-abs");
    $(`#nameInput_${stopwatchId}`).parent().removeClass("fade-abs");
    $(`#nameInput_${stopwatchId}`).val(stopwatchName);
    $(`#nameInput_${stopwatchId}`).select();
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
    if ($(event.target).hasClass("disable-on-start")) { return }
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

    if (btn.attr("data-mode") === "start") { // press start btn
      btn.html("Pause");
      btn.attr("data-mode", "pause");
      timer.attr("data-startDateTime", Date.now());
      if (timeTrackerHub.state !== "Connected") { connectionLost(); return; }
      timeTrackerHub.send("StartBtn", parseInt(stopwatchId), parseInt(timeSetId));
    }
    else if (btn.attr("data-mode") === "pause") { // press pause btn
      btn.html("Start");
      btn.attr("data-mode", "start");
      const timeIntervalId = timeSetTr.prev().find("img").attr("data-shiftId");
      timer.attr("data-startDateTime", "-1");
      if (timeTrackerHub.state !== "Connected") { connectionLost(); return; }
      timeTrackerHub.send("PauseBtn", parseInt(timeIntervalId), false, true); // isReset, clockWasStopped
    }
    if (timeTrackerHub.state !== "Connected") { connectionLost(); return; }
    timeTrackerHub.send("TimeSetRefresh", parseInt(stopwatchId), parseInt(timeSetTrToUpdate.attr("data-id")));
  }
  function resetHandler(event) {
    if (isVero) { return; }
    const stopwatch = $(event.target).parents(".stopwatch:first");
    const stopwatchId = stopwatch.attr("data-id");
    const timeIntervalId = stopwatch.find("tbody:first").children("tr:last").prev().find(".more-table-options-btn:first").attr("data-shiftId");
    const startPausebtn = $(event.target).siblings(".start-pause-btn");
    const timer = stopwatch.find(".time:first");

    const clockWasStopped = timer.attr("data-startDateTime") !== "-1"; // if this attr is NOT -1, it means the clock was running when reset was pressed

    startPausebtn.html("Start");
    startPausebtn.attr("data-mode", "start");
    timer.attr("data-startDateTime", "-1");

    if (timeIntervalId != undefined) {
      if (timeTrackerHub.state !== "Connected") { connectionLost(); return; }
      timeTrackerHub.send("ResetBtn", parseInt(stopwatchId), parseInt(timeIntervalId), clockWasStopped);
    }
  }
  function customHandler(event) {
    // set modal text
    $("#editShiftTitle").html("New Time Interval");
    $("#updateShiftBtn").html("Create");
    ToggleModal($("#timeTrackerContent"), $("#editShiftModal"), openModal);
    // package reference data
    const stopwatch = $(event.target).parents(".stopwatch:first");
    const stopwatchId = stopwatch.attr("data-id");

    $("#editShiftModal").attr("data-stopwatchId", stopwatchId);
  }

  // ============================================================================= btn events =============================================================================

  $("#newStopwatchBtn").on("click", () => {
    renderStopwatch(-1, "New Stopwatch", false, undefined); // once saved to the db, the id will be passed to the client and look for the stopwatch with id "-1"
    if (timeTrackerHub.state !== "Connected") { connectionLost(); return; }
    timeTrackerHub.send("CreateStopwatch");
  });

  // del stopwatch -----------------------------
  $("#delStopwatchYesBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#delStopWatchModal"), closeModal);
    const stopwatchId = $("#delStopWatchModal").attr("data-stopwatchId");
    if (timeTrackerHub.state !== "Connected") { connectionLost(); return; }
    timeTrackerHub.send("DelStopWatch", parseInt(stopwatchId));
  });
  $("#delStopwatchNoBtn").on("click", () => {
    ToggleModal($("#timeTrackerContent"), $("#delStopWatchModal"), closeModal);
  });

  // update shift -----------------------------
  $("#shiftEditBtn").on("click", () => {
    // populate modal with existing data
    const shiftId = $("#delShiftModal").attr("data-shiftId");
    if (timeTrackerHub.state !== "Connected") { connectionLost(); return; }
    timeTrackerHub.send("GetDatesToEdit", parseInt(shiftId));

    // set modal text
    $("#editShiftTitle").html("Edit Time Interval");
    $("#updateShiftBtn").html("Update");

    ToggleModal($("#timeTrackerContent"), $("#editShiftModal"), openModal);
    ToggleModal($("#timeTrackerContent"), $("#delShiftModal"), closeModal);
    // fade conf
    $("#delShiftImgWrap").removeClass("fade-abs");
    $(".conf-wrap:first").addClass("fade-abs");
  });
  $("#updateShiftBtn").on("click", () => {
    // validate input
    const shiftId = $("#delShiftModal").attr("data-shiftId");
    const stopwatchId = $("#editShiftModal").attr("data-stopwatchId");
    const shiftData = {
      startDate: $("#editStartDate").val(),
      endDate: $("#editEndDate").val(),
    };
    if (shiftData.endDate > shiftData.startDate) {
      // if a stopwatchId is present, a time interval is being created, not edited
      if (stopwatchId != undefined) {
        $("#editShiftModal").removeAttr("data-stopwatchId");
        if (timeTrackerHub.state !== "Connected") { connectionLost(); return; }
        timeTrackerHub.send("CustomTimeInterval", parseInt(stopwatchId), shiftData);
      }
      else {
        if (timeTrackerHub.state !== "Connected") { connectionLost(); return; }
        timeTrackerHub.send("EditTimeInterval", parseInt(shiftId), shiftData);
      }
      ToggleModal($("#timeTrackerContent"), $("#editShiftModal"), closeModal);
    }
    else {
      ShowError("editStartDate", "editStartDateErr", "start date/time must come first");
      ShowError("editEndDate", "editEndDateErr", "start date/time must come first");
    }
  });
  $("#editShiftModal").find("input").on("input", () => {
    HideError("editStartDate", "editStartDateErr");
    HideError("editEndDate", "editEndDateErr");
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
    if (timeTrackerHub.state !== "Connected") { connectionLost(); return; }
    timeTrackerHub.send("DelTimeInterval", parseInt(shiftId));
  });
  $("#shiftDelDenyBtn").on("click", () => {
    // fade conf
    $("#delShiftImgWrap").removeClass("fade-abs");
    $(".conf-wrap:first").addClass("fade-abs");
  });

  // ============================================================================= receive from server =============================================================================

  timeTrackerHub.on("PrintTimeInterval", (stopwatchId, timeIntervalDto, timeSetId, timeSetMsg, stopwatchMilli) => {
    const stopwatch = $("#stopwatchContainer").children(`section[data-id|=${stopwatchId}]`);
    const tbody = stopwatch.find("tbody:first");
    const timer = stopwatch.find(".time:first");
    const timeSetTr = stopwatch.find(`#timeSet_${timeSetId}`);

    const optionsBtn = $(`<td class="disable-on-start"><img tabindex="0" src="/Icons/Plus.png" class="more-table-options-btn disable-on-start" data-shiftId="${timeIntervalDto.id}" /></td>`);
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

    // start btn made new interval
    if (timeIntervalDto.clockOut === "") {
      stopwatch.addClass("stopwatch-on");
    }
    else { // new custom interval
      optionsBtn.removeClass("disable-on-start");
      optionsBtn.children("img").removeClass("disable-on-start");
      timer.attr("data-milli", stopwatchMilli);
      timeSetTr.children("td").html(timeSetMsg);
    }
  });

  timeTrackerHub.on("ClockOutTimeInterval", (stopwatchId, timeIntervalId, clockOut, hours) => {
    const stopwatch = $("#stopwatchContainer").children(`section[data-id|=${stopwatchId}]`);
    const moreOptionsIconTd = stopwatch.find(`#timeInterval_${timeIntervalId}`).children("td:last");
    moreOptionsIconTd.prev().prev().html(clockOut);
    moreOptionsIconTd.prev().html(hours);

    stopwatch.removeClass("stopwatch-on");

    // make more options btn usable again
    moreOptionsIconTd.removeClass("disable-on-start");
    moreOptionsIconTd.children("img").removeClass("disable-on-start");
  });

  timeTrackerHub.on("PrintTimeSet", (stopwatchId, timeSetId, timeSetHoursMsg) => {
    // apply stopwatchId
    let stopwatch = $("#stopwatchContainer").children("section[data-id|='-1']");

    if (stopwatch.length === 1) {
      stopwatch.attr("data-id", stopwatchId);
      $("#nameInput_-1").attr("id", `nameInput_${stopwatchId}`);
      $("#nameInput_-1Err").attr("id", `nameInput_${stopwatchId}Err`);
      $("#nameH2_-1").attr("id", `nameH2_${stopwatchId}`);
    }
    else if (stopwatch.length === 0) {
      stopwatch = $("#stopwatchContainer").children(`section[data-id|=${stopwatchId}]`);
    }

    const tbody = stopwatch.find("tbody");
    const timeSetTr = $(`<tr id="timeSet_${timeSetId}" data-id="${timeSetId}"><td colspan="5">${timeSetHoursMsg}</td></tr>`);
    tbody.append(timeSetTr);

    // set milliseconds from server
    stopwatch.find(".time:first").attr("data-milli", "0");
  });

  timeTrackerHub.on("PauseUpdate", (stopwatchId, milliFromServer) => {
    const stopwatch = $("#stopwatchContainer").children(`section[data-id|=${stopwatchId}]`);
    stopwatch.find(".time:first").attr("data-milli", milliFromServer);
  });

  timeTrackerHub.on("DelStopwatch", stopwatchId => {
    const stopwatch = $("#stopwatchContainer").children(`section[data-id|=${stopwatchId}]`);
    stopwatch.remove();
  });

  timeTrackerHub.on("PopulateEditIntervalModal", (startDate, endDate) => {
    $("#editStartDate").val(startDate.substring(0,16));
    $("#editEndDate").val(endDate.substring(0,16));
  });

  timeTrackerHub.on("EditTimeInterval", (stopwatchId, timeSetId, timeIntervalId, isFromActiveTimeSet, stopwatchMilli, timeSetMsg, intervalDto) => {
    const stopwatch = $("#stopwatchContainer").children(`section[data-id|=${stopwatchId}]`);
    const timeSetTr = stopwatch.find(`#timeSet_${timeSetId}`);
    const timeIntervalTr = stopwatch.find(`#timeInterval_${timeIntervalId}`);
    const timer = stopwatch.find(".time:first");

    // update timeSet
    timeSetTr.children("td").html(timeSetMsg);

    // update interval
    const firstTimeSetTd = timeIntervalTr.children("td:first");
    firstTimeSetTd.html(intervalDto.startDate);
    firstTimeSetTd.nextAll().eq(0).html(intervalDto.clockIn);
    firstTimeSetTd.nextAll().eq(1).html(intervalDto.clockOut);
    firstTimeSetTd.nextAll().eq(2).html(intervalDto.timeElapsed);

    // update stopwatch timer
    if (isFromActiveTimeSet) {
      timer.attr("data-milli", stopwatchMilli);
    }
  });

  timeTrackerHub.on("DelTimeInterval", (stopwatchId, timeSetId, timeIntervalId, isFromActiveTimeSet, isLastIntervalInTimeSet, timeSetTrMsg, stopwatchMilli) => {
    const stopwatch = $("#stopwatchContainer").children(`section[data-id|=${stopwatchId}]`);
    const timeSetTr = stopwatch.find(`#timeSet_${timeSetId}`);
    const timeIntervalTr = stopwatch.find(`#timeInterval_${timeIntervalId}`);
    const timer = stopwatch.find(".time:first");

    if (isFromActiveTimeSet) {
      if (isLastIntervalInTimeSet) {
        timeSetTr.children("td").html("no records in this time set yet");
        timer.attr("data-milli", stopwatchMilli);
      }
      else {
        timeSetTr.children("td").html(timeSetTrMsg);
        timer.attr("data-milli", stopwatchMilli);
      }
    }
    else {
      if (isLastIntervalInTimeSet) {
        timeSetTr.remove();
      }
      else {
        timeSetTr.children("td").html(timeSetTrMsg);
      }
    }

    timeIntervalTr.remove();
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
      const milliFromServer = timers.eq(i).attr("data-milli");
      const delta = (Date.now() - startDate) + parseInt(milliFromServer); // milliseconds
      const totalSeconds = Math.floor(delta / 1000);
      const timeFormat = convertSecondsToTimeFormat(totalSeconds);
      timers.eq(i).html(timeFormat);
    }
  }, 1000); // execute every second (not perfect accuracy, but the dela computation accounts for this)
});
