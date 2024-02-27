$(function () {
  HighlightCurrentNavBtn($("#projNavBtn"));
  const dashboardColor1 = "#003870";
  const dashboardColor2 = "#2d80d2";

  // ================================================ Bar Graph ================================================

  let currentSprintIndex;
  let numberOfSprints;
  let weightScale;
  let completedIssueWeights;
  let sprintDates;
  let sprintEndPoints;

  const bars = $(".bar");
  const days = $(".weekday");

  function setBarHeight(currentSprintIndex) {
    for (let i = 0; i < bars.length; i++) {
      const issueWeight = completedIssueWeights[currentSprintIndex][i];
      const date = sprintDates[currentSprintIndex][i];
      bars.eq(i).attr("data-tooltip", `${issueWeight} pts`);
      days.eq(i).attr("data-tooltip", days.eq(i).attr("data-day") + ` ${date}`);
      const heightPercentage = 100 * issueWeight / weightScale;
      bars.eq(i).css("height", `${heightPercentage}%`);
    }
  }

  $(".bar, .weekday").on("mouseover", (event) => {
    if ($(event.target).attr("data-tooltip") !== "0 pts" && $(event.target).attr("data-tooltip").includes("empty") === false) {
      $("#prodBarToolTip").removeClass("hide");
      $("#prodBarToolTip").html($(event.target).attr("data-tooltip"));
      $(event.target).append($("#prodBarToolTip"));
    }
  });
  $(".bar, .weekday").on("mouseout", () => {
    $("#prodBarToolTip").addClass("hide");
  });

  $("#prodLeftPage").on("click", () => {
    if (currentSprintIndex > 0) {
      currentSprintIndex--;
    }
    // render sprint
    setBarHeight(currentSprintIndex);
    $("#sprintIndexLabel").html(`Sprint ${currentSprintIndex + 1} of ${numberOfSprints}`);
    $("#sprintDateLabel").html(`${sprintEndPoints[currentSprintIndex][0]} - ${sprintEndPoints[currentSprintIndex][1]}`);
  });
  $("#prodRightPage").on("click", () => {
    if (currentSprintIndex < numberOfSprints - 1) {
      currentSprintIndex++;
    }
    // render sprint
    setBarHeight(currentSprintIndex);
    $("#sprintIndexLabel").html(`Sprint ${currentSprintIndex + 1} of ${numberOfSprints}`);
    $("#sprintDateLabel").html(`${sprintEndPoints[currentSprintIndex][0]} - ${sprintEndPoints[currentSprintIndex][1]}`);
  });

  // ================================================ Burn Down Chart ================================================

  var burnDownChartData;

  google.charts.load('current', { 'packages': ['corechart'] });

  function drawChart() {
    let options = {
      curveType: 'function',
      animation: 'startup',
      animation: { duration: '5s' },
      legend: { position: 'top' },
      vAxis: {
        viewWindow: {
          min: 0
        }
      },
      series: {
        0: { color: dashboardColor1 },
        1: { color: dashboardColor2 }
      }
    };

    let chart = new google.visualization.LineChart(document.getElementById('curveChart'));

    chart.draw(burnDownChartData, options);
  }

  // consider putting this in site.js
  function ResizeSensor(element, callback) {
    let expand = document.createElement('div');
    expand.style.position = "absolute";
    expand.style.left = "0px";
    expand.style.top = "0px";
    expand.style.right = "0px";
    expand.style.bottom = "0px";
    expand.style.overflow = "hidden";
    expand.style.visibility = "hidden";

    let expandChild = document.createElement('div');
    expandChild.style.position = "absolute";
    expandChild.style.left = "0px";
    expandChild.style.top = "0px";
    expandChild.style.width = "10000000px";
    expandChild.style.height = "10000000px";
    expand.appendChild(expandChild);

    let shrink = document.createElement('div');
    shrink.style.position = "absolute";
    shrink.style.left = "0px";
    shrink.style.top = "0px";
    shrink.style.right = "0px";
    shrink.style.bottom = "0px";
    shrink.style.overflow = "hidden";
    shrink.style.visibility = "hidden";

    let shrinkChild = document.createElement('div');
    shrinkChild.style.position = "absolute";
    shrinkChild.style.left = "0px";
    shrinkChild.style.top = "0px";
    shrinkChild.style.width = "200%";
    shrinkChild.style.height = "200%";
    shrink.appendChild(shrinkChild);

    element.appendChild(expand);
    element.appendChild(shrink);

    function setScroll() {
      expand.scrollLeft = 10000000;
      expand.scrollTop = 10000000;

      shrink.scrollLeft = 10000000;
      shrink.scrollTop = 10000000;
    };
    setScroll();

    let size = element.getBoundingClientRect();

    let currentWidth = size.width;
    let currentHeight = size.height;

    let onScroll = function () {
      let size = element.getBoundingClientRect();
      let newWidth = size.width;
      let newHeight = size.height;

      if (newWidth != currentWidth || newHeight != currentHeight) {
        currentWidth = newWidth;
        currentHeight = newHeight;

        callback();
      }

      setScroll();
    };

    expand.addEventListener('scroll', onScroll);
    shrink.addEventListener('scroll', onScroll);
  };

  // ================================================ Pie Chart ================================================

  // data from server
  let totalBugReportWeight;
  let totalStoryWeight;

  async function setPieChart() {
    await delay(600); // wait a moment before playing the pie chart animation

    const pieWeight = parseInt(totalStoryWeight) + parseInt(totalBugReportWeight);
    const storyDeg = parseInt(360 * totalStoryWeight / pieWeight);

    let color1StartDeg = 0;
    const color1FinalDeg = storyDeg;
    let color2StartDeg = storyDeg;
    const color2FinalDeg = 360;


    while (color1StartDeg != color1FinalDeg || color2StartDeg != color2FinalDeg) {
      await delay(0.3);
      if (color1StartDeg != color1FinalDeg) { color1StartDeg++ }
      if (color2StartDeg != color2FinalDeg) { color2StartDeg++ }

      $(".pie").css("background-image", `repeating-conic-gradient(
      ${dashboardColor1} 0deg ${color1StartDeg}deg,
      ${dashboardColor2} ${color1FinalDeg}deg ${color2StartDeg}deg)`);
    }

    $(".pie-color:nth(0)").css("background-color", dashboardColor1);
    $(".pie-color:nth(1)").css("background-color", dashboardColor2);
    $(".pie-key:first").children().removeClass("hide");
  }

  // ================================================ Page Load ================================================

  // Create and start signalR connection
  var razorToJs = new signalR.HubConnectionBuilder().withUrl("/projectDash").build();
  function razorToJsSuccess() { console.log("razorToJs success"); pageLoad() }
  function failure() { console.log("failure") }
  razorToJs.start().then(razorToJsSuccess, failure);
  razorToJs.onclose(async () => await razorToJs.start());

  async function pageLoad() {
    const projId = $("#projIdForJs").val();
    razorToJs.send("PackagePieChart", projId);
    razorToJs.send("PackageBarGraph", projId);
    razorToJs.send("PackageBurnDownChart", projId);
    await delay(600);
  }

  function reformatBurnDownChartData(data) {
    // turn the strings at indeces 1 and 2 into integers (skipping the header with i = 1)
    for (let i = 1; i < data.length; i++) {
      data[i][1] = parseInt(data[i][1]);
      data[i][2] = parseInt(data[i][2]);
    }
    return data;
  }

  razorToJs.on("ReceivePieChartData", dataFromServer => {
    totalBugReportWeight = dataFromServer.totalBugReportWeight;
    totalStoryWeight = dataFromServer.totalStoryWeight;
    setPieChart();
  });
  razorToJs.on("ReceiveBarGraphData", dataFromServer => {
    currentSprintIndex = dataFromServer.currentSprintIndex;
    numberOfSprints = dataFromServer.numberOfSprints;
    weightScale = dataFromServer.weightScale;
    completedIssueWeights = dataFromServer.completedIssueWeights;
    sprintDates = dataFromServer.sprintDates;
    sprintEndPoints = dataFromServer.sprintEndPoints;
    setBarHeight(currentSprintIndex);
  });
  razorToJs.on("ReceiveBurnDownChartData", dataFromServer => {
    const burnDownChartValues = reformatBurnDownChartData(dataFromServer.burnDownChartValues);
    burnDownChartData = google.visualization.arrayToDataTable(burnDownChartValues);

    google.charts.setOnLoadCallback(drawChart);

    new ResizeSensor($('.graph-container:first')[0], function () {
      drawChart();
    });
  });
});
