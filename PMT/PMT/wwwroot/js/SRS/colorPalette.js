$(function () {
  HighlightCurrentNavBtn($("#srsNavBtn"));

  // color wrap btn events
  function delBtnEvent(event) {
    $("#movableYesNoWrap").removeClass("hide");
    $(event.target).parent(".color-wrap").append($("#movableYesNoWrap"));
  }
  function copyBtnEvent(event) {
    const colorTextInput = $(event.target).siblings("input[type=text]");
    colorTextInput.select();
    navigator.clipboard.writeText(colorTextInput.val());
  }

  // relate color input and color text
  function setColorVal(event) {
    const colorTextVal = $(event.target).val();
    const colorInput = $(event.target).siblings("input[type=color]");
    colorInput.val(colorTextVal);
  }
  function setColorTextVal(event) {
    const colorVal = $(event.target).val();
    const colorTextInput = $(event.target).siblings("input[type=text]");
    colorTextInput.val(colorVal);
  }

  // add color btn
  $("#addColorBtn").on("click", () => {
    const colorContainer = $(`
    <div class="color-container">
      <div class="color-wrap">
        <input type="color">
        <input type="text">
        <img class="copy-btn" src="/icons/copy.png">
        <img class="del-btn" src="/icons/Delete.png">
      </div>
    </div>
    `);
    colorContainer.insertBefore($(".btn-wrap"));
    // setup events for new color wrap
    colorContainer.find(".copy-btn").on("click", copyBtnEvent);
    colorContainer.find(".del-btn").on("click", delBtnEvent);
    colorContainer.find("input[type=text]").on("input", setColorVal);
    colorContainer.find("input[type=color]").on("input", setColorTextVal);
  });

  // yes-no mini modal for deletion
  $("#confirmBtn").on("click", (event) => {
    const container = $(event.target).parents(".color-container");
    $("#colorPaletteContent").append($("#movableYesNoWrap"));
    $("#movableYesNoWrap").addClass("hide");
    container.remove();
  });

  $("#denyBtn").on("click", () => {
    $("#movableYesNoWrap").addClass("hide");
  });

  // setup onload events
  $(".del-btn").on("click", delBtnEvent);
  $(".copy-btn").on("click", copyBtnEvent);
  $("input[type=text]").on("input", setColorVal);
  $("input[type=color]").on("input", setColorTextVal);

  // store data to send to server
  $("form").on("submit", () => {
    let dataString = "";
    for (let i = 0; i < $("input[type='color']").length; i++) {
      dataString += $("input[type='color']").eq(i).val() + delimiter;
    }
    $("#colorData").val(dataString);
  });
});
