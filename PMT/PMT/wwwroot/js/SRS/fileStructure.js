$(function () {
  HighlightCurrentNavBtn($("#srsNavBtn"));

  const ctxMenu = $("#ctxMenu");

  function getDropPlacement(container, mouseY) {
    // an array of all draggable elements in the container that aren't currently being dragged
    const draggableElements = [...container.querySelectorAll(".draggable:not(.dragging)")];

    function findClosest(closest, child) {
      const box = child.getBoundingClientRect();
      const offset = mouseY - box.top - box.height / 2;
      if (closest.offset <= offset && offset < 0) {
        return { offset: offset, element: child }
      }
      else {
        return closest;
      }
    }

    return draggableElements.reduce(findClosest, { offset: Number.NEGATIVE_INFINITY }).element;
  }

  // Left and Right Click -------------------------------------------------------------

  function initializeFileStructure() {
    $(".container").on("dragover", (event) => {
      event.preventDefault();
      const dropPlacement = getDropPlacement($(".container")[0], event.clientY);
      if (dropPlacement == null) {
        $(".container").append($(".dragging"));
      }
      else {
        $(dropPlacement).before($(".dragging"));
      }
      determineDataTier($(".dragging"));
      setFileStructureIndentation($(".dragging"));
    });

    const fileStructureItems = $(".file-structure-item");

    for (let i = 0; i < fileStructureItems.length; i++) {
      setFileStructureEvents(fileStructureItems.eq(i));
      setFileStructureIndentation(fileStructureItems.eq(i));
    }

    $("#solutionDir").off("dragstart");
    $("#solutionDir").off("dragend");
  }
  // called once on page load
  initializeFileStructure();

  function determineDataTier(fileStructureItem) {
    let dataTier = "";

    // process for finding the tier

    fileStructureItem.attr("data-tier", dataTier);
  }

  function setFileStructureEvents(fileStructureItem) {
    // set click events
    fileStructureItem.on("click", (event) => {
      fileStructureItemLeftClickHandler(event);
    });
    fileStructureItem.on("contextmenu", (event) => {
      fileStructureItemRightClickHandler(event);
    });

    // set drag events  
    fileStructureItem.on("dragstart", () => {
      fileStructureItem.addClass("dragging");
      $(".file-structure-item").removeClass("file-structure-item-selected");
      fileStructureItem.addClass("file-structure-item-selected");
    });
    fileStructureItem.on("dragend", () => {
      fileStructureItem.removeClass("dragging");
    });
  }

  function setFileStructureIndentation(fileStructureItem) {
    // update indentation based on tier
    const tier = parseInt(fileStructureItem.attr("data-tier"));
    let offset = 2 * tier + 2;
    offset = offset.toString() + "vw";
    fileStructureItem.children("img").css("margin-left", offset);
  }

  function fileStructureItemLeftClickHandler(event) {
    $(".file-structure-item").removeClass("file-structure-item-selected");
    $(event.target).addClass("file-structure-item-selected");
  }
  function fileStructureItemRightClickHandler(event) {
    const target = $(event.target);

    // don't open regular ctx menu
    event.preventDefault();

    // adjust custom ctx menu contents for directories/files
    if (target.hasClass("dir")) {
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
    $(".file-structure-item").removeClass("file-structure-item-selected");
    target.addClass("file-structure-item-selected");
  }

  // Close Custom Ctx Menu -------------------------------------------------------------

  $("html").on("click", (event) => {
    const target = $(event.target);

    if (target.hasClass("has-submenu") === false) {
      ctxMenu.addClass("hide-menu");
    }

    if (target.hasClass("file-structure-item") === false &&
      target.hasClass("has-submenu") === false &&
      !target.is("#ctxMenuFolder") && !target.is("#ctxMenuFile")) {
      $(".file-structure-item").removeClass("file-structure-item-selected");
    }
  });
  $("html").on("contextmenu", (event) => {
    // hide custom when opening regular ctx menu
    if ($(event.target).hasClass("file-structure-item") === false) {
      ctxMenu.addClass("hide-menu");
    }
  });

  // alphabetical sorting algorithm -------------------------------------------------------------

  function sortFileStructureItem(fileStructureItem, type) {
    const parentDir = getParentDir(fileStructureItem);
    getDirContents(parentDir, type, fileStructureItem);

    // 
    fileStructureItem.addClass("temp");

    let dirContents = $(".temp");
    console.log(dirContents) // ------------------------------------------------------------------------------------------
    console.log("\n\n\n") // ------------------------------------------------------------------------------------------

    //dirContents = positionItemInCollection(dirContents, fileStructureItem.children("label")[0].innerHTML, fileStructureItem);

    // sort content
    let sortedItems = $();
    for (let i = 0; i < dirContents.length; i++) {
      sortedItems = positionItemInCollection(sortedItems, dirContents.eq(i).children("label")[0].innerHTML, dirContents.eq(i));
    }

    // move content
    for (let i = 0; i < sortedItems.length; i++) {
      //parentDir.after(sortedItems.eq(i));
    }
  }
  function positionItemInCollection(sortedItems, itemText, item) {
    console.log("NEW SORT") // ------------------------------------------------------------------------------------------
    console.log(sortedItems) // ------------------------------------------------------------------------------------------
    for (let i = 0; i < sortedItems.length; i++) {
      console.log(sortedItems.length) // ------------------------------------------------------------------------------------------
      // if the provided item is sorted before the item at index i, insert the item

      const testString = (sortedItems.length === 1) ? sortedItems[0].children("label")[0].innerText : sortedItems.eq(i).children("label")[0].innerText;

      const result = alphabeticallyFirst(itemText, testString);
      if (result === itemText) {
        sortedItems.splice(i, 0, item);
        return sortedItems;
      }
    }
    // if the condition is never met, item comes last
    console.log(item) // ------------------------------------------------------------------------------------------
    console.log(sortedItems) // ------------------------------------------------------------------------------------------
    sortedItems.push(item);
    console.log(sortedItems) // ------------------------------------------------------------------------------------------
    return sortedItems;
  }
  function alphabeticallyFirst(string1, string2) {
    for (let i = 0; i < string2.length; i++) {
      if (string1[i] === string2[i]) { continue; }
      if (string1[i] === undefined) { return string1; } // cases where chars match, but one string has more chars
      if (string2[i] === undefined) { return string2; }
      return (string1.toLowerCase().charCodeAt(i) < string2.toLowerCase().charCodeAt(i)) ? string1 : string2;
    }
    // if every char was a match
    return string2;
  }

  function getDirContents(dir, itemType, itemToExclude, getSubContents = true) {
    let dirContents = [];
    const dirDataTier = parseInt(dir.attr("data-tier"));

    let previousItem = dir;
    let nextItem = previousItem.next();
    let nextItemDataTier = parseInt(nextItem.attr("data-tier"));

    if (getSubContents) {
      while (dirDataTier < nextItemDataTier) {
        if (nextItem.hasClass(itemType) && nextItem.is(itemToExclude) === false) {
          //dirContents.push(nextItem);
          nextItem.addClass("temp");
        }

        previousItem = nextItem;
        nextItem = previousItem.next();
        nextItemDataTier = parseInt(nextItem.attr("data-tier"));
      }
    }
    else if (!getSubContents) {
      while (nextItemDataTier === dirDataTier + 1) {
        if (nextItem.hasClass(itemType)) {
          dirContents.push(nextItem);
        }

        previousItem = nextItem;
        nextItem = previousItem.next();
        nextItemDataTier = parseInt(nextItem.attr("data-tier"));
      }
    }
    return dirContents;
  }

  function getParentDir(fileStructureItem) {
    const itemDataTier = parseInt(fileStructureItem.attr("data-tier"));

    let prevItem = fileStructureItem.prev();
    let prevItemDataTier = parseInt(prevItem.attr("data-tier"));

    while (prevItemDataTier !== itemDataTier - 1) {
      prevItem = prevItem.prev();
      prevItemDataTier = parseInt(prevItem.attr("data-tier"));
    }
    return prevItem;
  }

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

  $("#ctxMenuFolder").on("click", () => {
    newItem("dir", "Icons/Folder.png", "(new folder)");
  });
  $("#ctxMenuFile").on("click", () => {
    newItem("file", "Icons/File.png", "(new file.extension)");
  });

  function newItem(type, src, name) {
    const target = $(".file-structure-item-selected");
    const previousTier = parseInt(target.attr("data-tier"));
    const nextTier = previousTier + 1;
    target.after(`
      <div tabindex="0" class="file-structure-item ${type} tier${nextTier} draggable" draggable="true" data-tier="${nextTier}">
        <img src="${src}">
        <label>${name}</label>
      </div>
    `);
    setFileStructureEvents(target.next());
    setFileStructureIndentation(target.next());
    target.removeClass("file-structure-item-selected");
    target.next().addClass("file-structure-item-selected");
    renameHandler();
  }

  $("#ctxMenuRename").on("click", () => {
    renameHandler();
  });

  function renameHandler() {
    const target = $(".file-structure-item-selected");
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
    $("#fileStructure").on("click.renameNameSpace", (event) => {
      if ($(event.target).is("input") === false) {
        confirmRename(target);
      }
    });
    $("html").on("contextmenu.renameNameSpace", () => {
      confirmRename(target);
    });
  }

  // consider a conf modal
  $("#ctxMenuDelete").on("click", () => {
    const target = $(".file-structure-item-selected");

    // delete single file
    if (target.hasClass("dir") === false) {
      target.remove();
      return;
    }

    // delete entire directory
    const dirDataTier = parseInt(target.attr("data-tier"));
    let previousItem = target;
    let nextItem = previousItem.next();
    let nextItemDataTier = parseInt(nextItem.attr("data-tier"));

    while (dirDataTier < nextItemDataTier) {
      if (!previousItem.is("#solutionDir")) {
        previousItem.remove();
      }
      previousItem = nextItem;
      nextItem = previousItem.next();
      nextItemDataTier = parseInt(nextItem.attr("data-tier"));
    }
    previousItem.remove();
  });

  function confirmRename(target) {
    const renameInput = target.children("input");
    $("#fileStructure").off("click.renameNameSpace");
    $("html").off("contextmenu.renameNameSpace");
    target.children("label")[0].innerHTML = renameInput.val();
    renameInput.remove();

    // alphabetically sort item
    sortFileStructureItem(target, "file"); // adjust this string literal argument later --------------------------------------------
  }

  // open/close Dir

  $(".dir-btn").on("click", (event) => {
    const dirBtn = $(event.target);
    if (dirBtn.hasClass("closed-dir-btn")) {
      dirBtn.removeClass("closed-dir-btn");
    }
    else {
      dirBtn.addClass("closed-dir-btn");
    }
  });

  // keyboard commands - handle this later
  $(".file-structure-item").on("keypress", (event) => {
    // delete key
    if (event.which === 46) {
      $("#ctxMenuDelete").trigger("click");
    }
  });

  $("#colorTest").on("mouseleave", () => {
    console.log($("#colorTest").val());
  });

});

  // add keyboard accessibility
    // delete key
    // arrow keys
  // rework drag&drop for dir
    // alphabetical sorting here too
  // alphabetical sorting on rename
  // open/close dir functionality
