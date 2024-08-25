$(function () {
  HighlightCurrentNavBtn($("#srsNavBtn"));

  if ($("#receiveFileStructureData").val() !== undefined) {
    $("#fileStructure")[0].innerHTML = $("#receiveFileStructureData").val();
  }

  // jump to target on page load
  const jumpTarget = $("#jumpTargetForJs").attr("data-jump-target");
  if (jumpTarget !== "") {
    $(`${jumpTarget}`)[0].scrollIntoView(true);
  }

  // click events

  $(".fs-item").on("click", fileStructureItemLeftClickHandler);

  function fileStructureItemLeftClickHandler(event) {
    $(".fs-item").removeClass("fs-item-selected");
    if ($(event.target).is("img") === false) {
      $(event.target).addClass("fs-item-selected");
    }
  }

  $("#exportBackendScaffoldDataBtn").on("click", () => {
    const models = $("#exportBackendScaffoldDataBtn").attr("data-models");
    const props = $("#exportBackendScaffoldDataBtn").attr("data-props");
    const fileData = models + "###PMT###" + props;

    download("___PMT___DATA___FROM___WEBAPP___", fileData);
  });

  function removeFileExtension(fileName) {
    let output = "";
    for (let i = 0; i < fileName.length; i++) {
      if (fileName[i] !== '.') {
        output += fileName[i];
        continue;
      }
      break;
    }
    return output;
  }

  function extractScaffoldDataFromFileStructure() {
    let controllerNames = [];
    let viewNameCollections = [];

    const viewDirContainer = $("#rootContainer").find("label:contains('Views')").parent().siblings(".dir-container");

    const controllerDirs = viewDirContainer.find(".dir-content");

    for (let i = 0; i < controllerDirs.length; i++) {
      // extract controller names
      nextControllerName = controllerDirs.eq(i).find("label").html();
      controllerNames.push(nextControllerName);

      // extract view names
      const dirContainer = controllerDirs.eq(i).siblings(".dir-container");
      const viewNameFiles = dirContainer.find(".file");
      let viewNames = [];
      for (let j = 0; j < viewNameFiles.length; j++) {
        nextViewName = viewNameFiles.eq(j).find("label").html();
        viewNames.push(removeFileExtension(nextViewName));
      }
      viewNameCollections.push(viewNames);
    }

    console.log(controllerNames);
    console.log(viewNameCollections);
  }

  $("#exportFrontendScaffoldDataBtn").on("click", () => {
    extractScaffoldDataFromFileStructure();
    const fileData = "";
    download("___PMT___DATA___FROM___WEBAPP___", fileData);
  });

  $("#exportFrontendScaffoldDataSignalRBtn").on("click", () => {
    extractScaffoldDataFromFileStructure();
    const fileData = "";
    download("___PMT___DATA___FROM___WEBAPP___", fileData);
  });

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
