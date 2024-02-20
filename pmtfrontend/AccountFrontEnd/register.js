$(function () {
  // toggle password show
  $(".passHideBtn").on("click", ()=>{
    TogglePasswordShow($("#registerPass"), $(".passHideBtn"), $(".passShowBtn"));
    TogglePasswordShow($("#registerConfPass"), $(".passHideBtn"), $(".passShowBtn"));
  });
  $(".passShowBtn").on("click", ()=>{
    TogglePasswordShow($("#registerPass"), $(".passShowBtn"), $(".passHideBtn"));
    TogglePasswordShow($("#registerConfPass"), $(".passShowBtn"), $(".passHideBtn"));
  });

  $("#nextBtn").on("click", ()=> {
    $("#page1").addClass("hide");
    $("#page2").removeClass("hide");
    $("#backBtn").removeClass("hide");
  });
  $("#backBtn").on("click", ()=> {
    $("#page2").addClass("hide");
    $("#page1").removeClass("hide");
    $("#backBtn").addClass("hide");
  });
  
});
