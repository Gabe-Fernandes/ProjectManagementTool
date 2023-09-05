$(function () {
  HighlightCurrentNavBtn($("#srsNavBtn"));
  $("#fileStructure")[0].innerHTML = $("#receiveFileStructureData").val();
  const ctxMenu = $("#ctxMenu");

  // click events -------------------------------------------------------------

  $(".fs-item").on("click", fileStructureItemLeftClickHandler);
  $(".fs-item").on("contextmenu", fileStructureItemRightClickHandler);

  function fileStructureItemLeftClickHandler(event) {
    $(".fs-item").removeClass("fs-item-selected");
    if ($(event.target).is("img") === false) {
      $(event.target).addClass("fs-item-selected");
    }
  }

  function fileStructureItemRightClickHandler(event) {
    const target = $(event.target);

    // don't open regular ctx menu
    event.preventDefault();

    // adjust custom ctx menu contents for directories/files
    if (target.hasClass("dir-content")) {
      $("#ctxMenu").css("height", "15vh");
      $("#ctxMenuNew").removeClass("hide");
    }
    else {
      $("#ctxMenu").css("height", "10vh");
      $("#ctxMenuNew").addClass("hide");
    }

    // position and reveal custom ctx menu
    ctxMenu.css("left", event.pageX);
    ctxMenu.css("top", event.pageY);
    ctxMenu.removeClass("hide-menu");

    // select item on right click as well
    $(".fs-item").removeClass("fs-item-selected");
    target.addClass("fs-item-selected");
  }

  // Close Custom Ctx Menu -------------------------------------------------------------

  $("html").on("click", (event) => {
    const target = $(event.target);

    if (target.hasClass("has-submenu") === false) {
      ctxMenu.addClass("hide-menu");
    }

    if (target.hasClass("fs-item") === false &&
      target.hasClass("has-submenu") === false &&
      !target.is("#ctxMenuFolder") && !target.is("#ctxMenuFile")) {
      $(".fs-item").removeClass("fs-item-selected");
    }
  });
  $("html").on("contextmenu", (event) => {
    // hide custom when opening regular ctx menu
    if ($(event.target).hasClass("fs-item") === false) {
      ctxMenu.addClass("hide-menu");
    }
  });

  // ctx menu actions -------------------------------------------------------------

  $("#ctxMenuNew").on("mouseenter", () => {
    $("#subMenu").removeClass("hide-menu");
  });
  $("#subMenu").on("mouseenter", () => {
    $("#subMenu").removeClass("hide-menu");
  });
  $("#ctxMenuNew").on("mouseleave", () => {
    $("#subMenu").addClass("hide-menu");
  });
  $("#subMenu").on("mouseleave", () => {
    $("#subMenu").addClass("hide-menu");
  });

  // Renaming ------------------------------------------------------------------

  $("#ctxMenuRename").on("click", renameHandler);

  function renameHandler() {
    const target = $(".fs-item-selected");
    const originalName = target.children("label")[0].innerHTML;
    target.children("label")[0].innerHTML = "";

    target.append(`<input value="${originalName}" />`);
    target.children("input").select();

    target.children("input").on("keypress", (event) => {
      // enter key
      if (event.which === 13) {
        confirmRename(target);
      }
    });
    $("html").on("click.renameNameSpace", (event) => {
      if ($(event.target).is("input") === false &&
        $(event.target).is("#ctxMenuFolder") === false &&
        $(event.target).is("#ctxMenuFile") === false &&
        $(event.target).is("#ctxMenuRename") === false) {
        confirmRename(target);
      }
    });
    $("html").on("contextmenu.renameNameSpace", () => {
      confirmRename(target);
    });
  }

  function confirmRename(target) {
    const renameInput = target.children("input");
    $("html").off("click.renameNameSpace");
    $("html").off("contextmenu.renameNameSpace");
    target.children("label")[0].innerHTML = renameInput.val();
    renameInput.remove();
    sortContainer(target.parents(".dir-container:first"));
  }

  // Sorting Algorithm ---------------------------------------------------------------

  function sortContainer(dirContainer) {
    const directSubDirs = dirContainer.children(".dir");
    const directSubFiles = dirContainer.children(".file");

    let sortedDirs = [];
    let sortedFiles = [];

    // sort content
    for (let i = 0; i < directSubDirs.length; i++) {
      const dir = directSubDirs.eq(i);
      const dirText = dir.find("label:first")[0].innerHTML;
      const dirObj = { fsItem: dir, text: dirText };
      sortedDirs = positionInCollection(sortedDirs, dirObj);
    }
    for (let i = 0; i < directSubFiles.length; i++) {
      const file = directSubFiles.eq(i);
      const fileText = file.find("label:first")[0].innerHTML;
      const fileObj = { fsItem: file, text: fileText };
      sortedFiles = positionInCollection(sortedFiles, fileObj);
    }

    // move content & set margin
    for (let i = 0; i < sortedDirs.length; i++) {
      dirContainer.append(sortedDirs[i].fsItem);
    }
    for (let i = 0; i < sortedFiles.length; i++) {
      dirContainer.append(sortedFiles[i].fsItem);
    }
    updateIndentation();
  }

  function positionInCollection(collection, objToPosition) {
    for (let i = 0; i < collection.length; i++) {
      // if the provided item is sorted before the item at index i, insert the obj
      const winningResult = alphabeticallyFirst(objToPosition.text, collection[i].text);
      if (winningResult === objToPosition.text) {
        collection.splice(i, 0, objToPosition);
        return collection;
      }
    }
    // if the condition is never met, objToPosition comes last
    collection.push(objToPosition);
    return collection;
  }

  function alphabeticallyFirst(string1, string2) {
    for (let i = 0; i < string2.length; i++) {
      if (string1[i] === string2[i]) { continue; }
      if (string1[i] === undefined) { return string1; } // cases where chars match, but one string has more chars
      if (string2[i] === undefined) { return string2; }
      return (string1.toLowerCase().charCodeAt(i) < string2.toLowerCase().charCodeAt(i)) ? string1 : string2;
    }
    // if every char was a match (add error handling here later)
    return string2;
  }

  // deletion ----------------------------------------------------------------------
  $("#ctxMenuDelete").on("click", () => {
    const target = $(".fs-item-selected");
    const itemToDelete = (target.hasClass("file")) ? target : target.parent(".dir:first");

    // can't delete root
    if (itemToDelete.hasClass("root") === false) {
      itemToDelete.remove();
    } else {
      // if target is root, empty its container
      $("#rootContainer").children().remove();
    }
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

  // new fsItem ----------------------------------------------------------------------

  $("#ctxMenuFolder").on("click", newDir);
  $("#ctxMenuFile").on("click", newFile);

  function newDir() {
    const target = $(".fs-item-selected");
    const dirContainer = target.siblings(".dir-container");
    const newDir = `
      <div tabindex="0" class="fs-item dir" draggable="true">
        <div class="dir-content fs-item">
          <img src="/Icons/FileDirectoryArrow.png" class="dir-btn">
          <img src="/icons/Folder.png">
          <label>(new folder)</label>
        </div>

        <div class="dir-container">

        </div>
      </div>
    `;
    dirContainer.append(newDir);
    const dirBtn = dirContainer.children(":last").find(".dir-btn:first");
    dirBtn.on("click", toggleDirArrowDirection);

    target.removeClass("fs-item-selected");
    dirContainer.children(":last").children(".dir-content:first").addClass("fs-item-selected");
    renameHandler();
  }

  function newFile() {
    const target = $(".fs-item-selected");
    const dirContainer = target.siblings(".dir-container");
    const newFile = `
      <div tabindex="0" class="fs-item file" draggable="true">
        <img src="/icons/File.png">
        <label>(new file.extension)</label>
      </div>
    `;
    dirContainer.append(newFile);

    target.removeClass("fs-item-selected");
    dirContainer.children(":last").addClass("fs-item-selected");
    renameHandler();
  }

  // drag & drop ----------------------------------------------------------------------

  let itemToMove;

  const dragStart = (event) => itemToMove = $(event.target);
  const dragOver = (event) => event.preventDefault();
  const dragEnter = (event) => $(event.target).addClass("drag-highlight");
  const dragLeave = (event) => $(event.target).removeClass("drag-highlight");

  function itemDrop(event) {
    $(event.target).removeClass("drag-highlight");
    const dirContainer = $(event.target).parents(".dir").children(".dir-container:first");
    if (dirContainer.is(itemToMove.find(".dir-container")) === false) {
      dirContainer.append(itemToMove);
    }
    sortContainer(dirContainer);
  }

  // initialize drag events
  $(".file").on("dragstart", dragStart);
  $(".file").on("dragover", dragOver);
  $(".file").on("dragenter", dragEnter);
  $(".file").on("dragleave", dragLeave);
  $(".file").on("drop", itemDrop);
  $(".dir").on("dragstart", dragStart);
  $(".dir").on("dragover", dragOver);
  $(".dir").on("dragenter", dragEnter);
  $(".dir").on("dragleave", dragLeave);
  $(".dir").on("drop", itemDrop);

  // Indentation ----------------------------------------------------------------------

  function updateIndentation() {
    const dirBtns = $(".dir-btn");
    const fileImgs = $(".file").children("img");

    for (let i = 0; i < dirBtns.length; i++) {
      const scale = dirBtns.eq(i).parents(".dir-container").length;
      const indentationSize = 2;
      dirBtns.eq(i).css("margin-left", `${scale * indentationSize}vw`);
    }
    for (let i = 0; i < fileImgs.length; i++) {
      const scale = fileImgs.eq(i).parents(".dir-container").length;
      const indentationSize = 2;
      fileImgs.eq(i).css("margin-left", `${scale * indentationSize}vw`);
    }
  }
  // initialize indentation on pageload
  updateIndentation();

  // form submission
  $("form").on("submit", () => {
    $("#fileStructureDataInput").val($("#fileStructure")[0].innerHTML);
  });
});
