$(function () {
  $(".show-nav-btn").on("click", () => {
    const btn = $(".show-nav-btn");

    if (btn.hasClass("point-left")) {
      // close nav
      btn.removeClass("point-left");
      btn.addClass("point-right");
      $("nav").addClass("hide-nav");
      $("#projectDashboardContent").css("margin-left", "0vw");
      $("#projectDashboardContent").css("width", "100vw");
    }
    else if (btn.hasClass("point-right")) {
      // open nav
      btn.removeClass("point-right");
      btn.addClass("point-left");
      $("nav").removeClass("hide-nav");
      $("#projectDashboardContent").css("margin-left", "15vw");
      $("#projectDashboardContent").css("width", "85vw");
    }
  });
});
