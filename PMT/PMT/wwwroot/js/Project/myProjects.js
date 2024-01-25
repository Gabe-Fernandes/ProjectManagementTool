$(function () {
  // Modal Events
  $("#newProjBtn").on("click", () => {
    ToggleModal($("#myProjectsContent"), $("#newProjModal"), openModal);
  });
  $("#newProjCloseBtn").on("click", () => {
    ToggleModal($("#myProjectsContent"), $("#newProjModal"), closeModal);
  });
  $("#joinProjBtn").on("click", () => {
    ToggleModal($("#myProjectsContent"), $("#joinProjModal"), openModal);
  });
  $("#joinProjCloseBtn").on("click", () => {
    ToggleModal($("#myProjectsContent"), $("#joinProjModal"), closeModal);
  });
  $(".leave-proj-btn").on("click", () => {
    ToggleModal($("#myProjectsContent"), $("#leaveProjModal"), openModal);
  });
  $(".leave-proj-close-btn").on("click", () => {
    ToggleModal($("#myProjectsContent"), $("#leaveProjModal"), closeModal);
  });

  // Favorite Icon Events
  const emptyStarFilePath = "/icons/EmptyFavStar.png";
  const fullStarFilePath = "/icons/FavStar.png";

  $(".favStar").on("click", (event) => {
    // Turn favorite off if it's already favorited
    if ($(event.target).attr("src") === fullStarFilePath) {
      $(event.target).attr("src", emptyStarFilePath);
      return;
    }
    // Unfavorite all and favorite the selection
    $(".favStar").attr("src", emptyStarFilePath);
    $(event.target).attr("src", fullStarFilePath);
    // Package projId to send to server
    $("#defaultProjIdInput").val($(event.target).attr("data-id"));
  });

  // Btn events
  $(".copy-btn").on("click", (event) => {
    // select text
    const text = $(event.target).parent().select();
    // copy text
    navigator.clipboard.writeText(text.attr("data-joinCode"));
  });
  $(".leave-proj-btn").on("click", (event) => {
    const argument = $(event.target).attr("data-projId");
    $("#leaveProjForm").attr("action", `/Project/LeaveProject?projIdToLeave=${argument}`);
  });

  // Input events
  $("#joinCodeInput").on("input", () => {
    const argument = $("#joinCodeInput").val();
    $("#joinProjForm").attr("action", `/Project/JoinProject?joinCode=${argument}`);
    $("#joinCodeInputWrap").removeClass("err-input");
    $("#joinProjErrSpan").addClass("hide");
  });
});
