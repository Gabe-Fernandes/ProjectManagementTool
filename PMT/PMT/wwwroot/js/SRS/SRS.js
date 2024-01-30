$(function () {
  HighlightCurrentNavBtn($("#srsNavBtn"));

  if ($("#receiveFileStructureData").val() !== undefined) {
    $("#fileStructure")[0].innerHTML = $("#receiveFileStructureData").val();
  }

  // jump to target on page load
  const jumpTarget = $("#jumpTargetForJs").attr("data-jump-target");
  $(`${jumpTarget}`)[0].scrollIntoView(true);

  // click events

  $(".fs-item").on("click", fileStructureItemLeftClickHandler);

  function fileStructureItemLeftClickHandler(event) {
    $(".fs-item").removeClass("fs-item-selected");
    if ($(event.target).is("img") === false) {
      $(event.target).addClass("fs-item-selected");
    }
  }

  // open/close Dir (arrow animation and expand/collapse) ----------------------------

  $(".dir-btn").on("click", toggleDirArrowDirection);

  function toggleDirArrowDirection(event) {
    const dirBtn = $(event.target);
    const dirContainer = dirBtn.parent(".dir-content").siblings(".dir-container");

    if (dirBtn.hasClass("closed-dir-btn")) {
      // expand
      dirBtn.removeClass("closed-dir-btn");
      dirContainer.removeClass("hide");
    }
    else {
      // collapse
      dirBtn.addClass("closed-dir-btn");
      dirContainer.addClass("hide");
    }
  }
});
