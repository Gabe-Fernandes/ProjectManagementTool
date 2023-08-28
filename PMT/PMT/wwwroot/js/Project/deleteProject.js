$(function () {
  // validate deletion confirmation
  $("form").on("submit", (event) => {
    if ($("#confInput").attr("placeholder") !== $("#confInput").val()) {
      $("#confInput").addClass("err-input");
      event.preventDefault();
    }
  });
  // clear error styles
  $("#confInput").on("input", () => {
    $("#confInput").removeClass("err-input");
  });
});
