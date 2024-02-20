$(function () {
  // Modal Events
  $("#newProjBtn").on("click", ()=> {
    ToggleModal($("#myProjectsContent"), $("#newProjModal"), openModal);
  });
  $("#newProjCloseBtn").on("click", ()=> {
    ToggleModal($("#myProjectsContent"), $("#newProjModal"), closeModal);
  });

  // Favorite Icon Events
  const emptyStarFilePath = "/icons/EmptyFavStar.png";
  const fullStarFilePath = "/icons/FavStar.png";

  $(".favStar").on("click", (event)=> {
    // Turn favorite off if it's already favorited
    if ($(event.target).attr("src") === fullStarFilePath) {
      $(event.target).attr("src", emptyStarFilePath);
      return;
    }
    // Unfavorite all and favorite the selection
    $(".favStar").attr("src", emptyStarFilePath);
    $(event.target).attr("src", fullStarFilePath);
  });
});
