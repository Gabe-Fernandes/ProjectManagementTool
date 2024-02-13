$(function () {
  $(".passHideBtn").on("click", ()=>{
    TogglePasswordShow($("#registerPass"), $(".passHideBtn"), $(".passShowBtn"));
    TogglePasswordShow($("#registerConfPass"), $(".passHideBtn"), $(".passShowBtn"));
  });
  $(".passShowBtn").on("click", ()=>{
    TogglePasswordShow($("#registerPass"), $(".passShowBtn"), $(".passHideBtn"));
    TogglePasswordShow($("#registerConfPass"), $(".passShowBtn"), $(".passHideBtn"));
  });
});
