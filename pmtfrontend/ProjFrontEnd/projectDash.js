$(function () {
  const scoreScale = 100;
  const scores = [15, 14, 74, 89, 14, 68, 42, 73, 82, 14, 58, 69, 80, 48];

  const bars = $(".bar");

  function setBarHeight() {
    for (let i = 0; i < bars.length; i++) {
      const heightPercentage = scores[i]/scoreScale * 100;
      bars.eq(i).css("height", `${heightPercentage}%`);
    }
  }

  // consider putting this in site.js
  function delay(time) {
    return new Promise(resolve => setTimeout(resolve, time));
  }

  async function setBurnDown() {

  }

  async function setPieChart() {
    let color1StartDeg = 0;
    const color1FinalDeg = 200;
    let color2StartDeg = 200;
    const color2FinalDeg = 300;

    while (color1StartDeg != color1FinalDeg || color2StartDeg != color2FinalDeg) {
      await delay(0.3);
      if (color1StartDeg != color1FinalDeg) { color1StartDeg++ }
      if (color2StartDeg != color2FinalDeg) { color2StartDeg++ }

      $(".pie").css("background-image", `repeating-conic-gradient(
        #17993c 0deg ${color1StartDeg}deg,
        #FDDA16 ${color1StartDeg}deg ${color2StartDeg}deg,
        #781d1d ${color2StartDeg}deg 360deg)`);
    }
    
    $(".pie-color:nth(0)").css("background-color", "#17993c");
    $(".pie-color:nth(1)").css("background-color", "#FDDA16");
    $(".pie-color:nth(2)").css("background-color", "#781d1d");
    $(".pie-key:first").children().removeClass("hide");
  }

  $("#prodSprintBtn").on("click", ()=> {
    $(".pagination-wrap").css("opacity", "100%");
    $("#timeLabel").text("Time: 2 weeks");
    setBarHeight();
  });
  $("#prodEpicBtn").on("click", ()=> {
    $(".pagination-wrap").css("opacity", "100%");
    $("#timeLabel").text("Time: entire epic");
    setBarHeight();
  });
  $("#prodInitiativeBtn").on("click", ()=> {
    $(".pagination-wrap").css("opacity", "100%");
    $("#timeLabel").text("Time: entire initiative");
    setBarHeight();
  });
  $("#prodAllBtn").on("click", ()=> {
    $(".pagination-wrap").css("opacity", "0%");
    $("#timeLabel").text("Time: entire project");
    setBarHeight();
  });
  $("#leftPage").on("click", ()=> {
    setBarHeight();
  });
  $("#rightPage").on("click", ()=> {
    setBarHeight();
  });
  $("#pieSprintBtn").on("click", ()=> {
    setPieChart();
  });
  $("#pieEpicBtn").on("click", ()=> {
    setPieChart();
  });
  $("#pieInitiativeBtn").on("click", ()=> {
    setPieChart();
  });
  $("#pieAllBtn").on("click", ()=> {
    setPieChart();
  });

  function nameThisLater(maxPoints) {
    for (let i = maxPoints; i > 0; i--) {
      //console.log(i);
      i -= 141;
    }
  }
  nameThisLater(2000);

  google.charts.load('current', {'packages':['corechart']});
  google.charts.setOnLoadCallback(drawChart);

  function drawChart() {
    var data = google.visualization.arrayToDataTable([
      ['Day', 'Ideal Burn', 'Actual Burn'],
      ['S',  2000, 2000],
      ['M',  1858, 1858],
      ['T',  1716, 1616],
      ['W',  1575, 1475],
      ['R',  1428, 1528],
      ['F',  1285, 1385],
      ['S',  1142, 1242],
      ['S',  999, 999],
      ['M',  713, 613],
      ['T',  570, 670],
      ['W',  427, 327],
      ['R',  284, 184],
      ['F',  1, 1],
      ['S',  1, 1]
    ]);
    
    let options = {
      curveType: 'function',
      animation: 'startup',
      animation: { duration: '5s' },
      legend: { position: 'top' }
    };

    let chart = new google.visualization.LineChart(document.getElementById('curveChart'));

    chart.draw(data, options);
  }

  // consider putting this in site.js
  function ResizeSensor(element, callback)
  {
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

    function setScroll()
    {
      expand.scrollLeft = 10000000;
      expand.scrollTop = 10000000;

      shrink.scrollLeft = 10000000;
      shrink.scrollTop = 10000000;
    };
    setScroll();

    let size = element.getBoundingClientRect();

    let currentWidth = size.width;
    let currentHeight = size.height;

    let onScroll = function()
    {
      let size = element.getBoundingClientRect();
      let newWidth = size.width;
      let newHeight = size.height;

      if(newWidth != currentWidth || newHeight != currentHeight)
      {
        currentWidth = newWidth;
        currentHeight = newHeight;

        callback();
      }

      setScroll();
    };

    expand.addEventListener('scroll', onScroll);
    shrink.addEventListener('scroll', onScroll);
  };

  new ResizeSensor($('.graph-container:first')[0], function(){ 
    drawChart();
  });

  setPieChart();
  console.log("hello world");
});
