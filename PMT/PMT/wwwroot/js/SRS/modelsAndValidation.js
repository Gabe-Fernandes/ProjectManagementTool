$(function () {
  HighlightCurrentNavBtn($("#srsNavBtn"));

  // model wrap events
  function showPropsBtn(event) {
    const arrowImg = $(event.target)
    // close content
    if (arrowImg.hasClass("rotate-expansion-arrow")) {
      arrowImg.removeClass("rotate-expansion-arrow");
      arrowImg.parent().siblings(".property-container").addClass("hide");
      arrowImg.parent().siblings(".validation-container").addClass("hide");
      const wrap = (arrowImg.parent().hasClass("model-content")) ? arrowImg.parents(".model-wrap:first") : arrowImg.parents(".property-wrap:first");
      wrap.find("img, input").attr("tabindex", "-1");
      arrowImg.parent().children("img, input").attr("tabindex", "0");
    }
    // open content
    else {
      arrowImg.addClass("rotate-expansion-arrow");
      arrowImg.parent().siblings(".property-container").removeClass("hide");
      arrowImg.parent().siblings(".validation-container").removeClass("hide");
      const wrap = (arrowImg.parent().hasClass("model-content")) ? arrowImg.parents(".model-wrap:first") : arrowImg.parents(".property-wrap:first");
      wrap.find("img, input").attr("tabindex", "0");
    }
  }
  function delBtn(event) {
    $("#movableYesNoWrap").removeClass("hide");
    $("#movableYesNoWrap").find("img").attr("tabindex", "0");
    $("#movableYesNoWrap").insertAfter($(event.target));
  }
  function addValidationBtn(event) {
    const validation = $(`
    <div class="validation-wrap">
      <div class="validation-content">
        <input type="text" placeholder="validation">
        <img tabindex="0" class="del-btn" src="/icons/Delete.png">
      </div>
    </div>
    `);
    $(event.target).parent(".property-content").siblings(".validation-container").append(validation);
    validation.find(".del-btn").on("click", delBtn);
    validation.find(".del-btn").on("keypress", addKeyboardAccessibility);
    // expand validations on creation
    $(event.target).siblings(".show-props-btn").addClass("rotate-expansion-arrow");
    validation.parent(".validation-container").removeClass("hide");
  }
  function addPropertyBtn(event) {
    const property = $(`
    <div class="property-wrap">
      <div class="property-content">
        <img tabindex="0" class="show-props-btn" src="/icons/ContextMenuArrow.png">
        <input type="text" placeholder="type">
        <input type="text" placeholder="property name">
        <img tabindex="0" class="del-btn" src="/icons/Delete.png">
        <img tabindex="0" class="add-validation-btn" src="/icons/Plus.png">
        <label>add validation</label>
      </div>

      <div class="validation-container hide">

      </div>
    </div>
    `);
    $(event.target).parent(".model-content").siblings(".property-container").append(property);
    property.find(".show-props-btn").on("click", showPropsBtn);
    property.find(".show-props-btn").on("keypress", addKeyboardAccessibility);
    property.find(".del-btn").on("click", delBtn);
    property.find(".del-btn").on("keypress", addKeyboardAccessibility);
    property.find(".add-validation-btn").on("click", addValidationBtn);
    property.find(".add-validation-btn").on("keypress", addKeyboardAccessibility);
    // expand properties on creation
    $(event.target).siblings(".show-props-btn").addClass("rotate-expansion-arrow");
    property.parent(".property-container").removeClass("hide");
  }

  // add model btn
  $("#addModelBtn").on("click", () => {
    const model = $(`
    <div class="model-wrap">
      <div class="model-content">
        <img tabindex="0" class="show-props-btn" src="/icons/ContextMenuArrow.png">
        <input type="text" placeholder="model name">
        <img tabindex="0" class="del-btn" src="/icons/Delete.png">
        <img tabindex="0" class="add-property-btn" src="/icons/Plus.png">
        <label>add property</label>
      </div>

      <div class="property-container hide">

      </div>
    </div>
    `);
    model.insertBefore(".btn-wrap");
    model.find(".show-props-btn").on("click", showPropsBtn);
    model.find(".show-props-btn").on("keypress", addKeyboardAccessibility);
    model.find(".del-btn").on("click", delBtn);
    model.find(".del-btn").on("keypress", addKeyboardAccessibility);
    model.find(".add-property-btn").on("click", addPropertyBtn);
    model.find(".add-property-btn").on("keypress", addKeyboardAccessibility);
  });

  // yes-no mini modal for deletion
  $("#confirmBtn").on("click", (event) => {
    let wrapToRemove = $();
    if ($(event.target).parents(".validation-wrap").length > 0) {
      wrapToRemove = $(event.target).parents(".validation-wrap");
    }
    else if ($(event.target).parents(".property-wrap").length > 0) {
      wrapToRemove = $(event.target).parents(".property-wrap");
    }
    else if ($(event.target).parents(".model-wrap").length > 0) {
      wrapToRemove = $(event.target).parents(".model-wrap");
    }
    $("#modelsAndValidationContent").append($("#movableYesNoWrap"));
    $("#movableYesNoWrap").addClass("hide");
    $("#movableYesNoWrap").find("img").removeAttr("tabindex");
    wrapToRemove.remove();
  });
  $("#denyBtn").on("click", () => {
    $("#movableYesNoWrap").addClass("hide");
    $("#movableYesNoWrap").find("img").removeAttr("tabindex");
  });

  // setup onload events
  $(".show-props-btn").on("click", showPropsBtn);
  $(".del-btn").on("click", delBtn);
  $(".add-validation-btn").on("click", addValidationBtn);
  $(".add-property-btn").on("click", addPropertyBtn);
  $("#movableYesNoWrap").find("img").on("keypress", addKeyboardAccessibility);

  // store data to send to server
  $("#modelsAndValidationContent").on("submit", () => {
    let modelDataString = "";
    let propDataString = "";
    let valiDataString = "";

    for (let i = 0; i < $("input[placeholder='model name']").length; i++) {
      modelDataString += $("input[placeholder='model name']").eq(i).val() + delimiter;
      propDataString += newModelDelimiter;
      valiDataString += newModelDelimiter;
      const relatedProps = $("input[placeholder='model name']").eq(i).parents(".model-wrap").find("input[placeholder='type']");
      for (let j = 0; j < relatedProps.length; j++) {
        propDataString += relatedProps.eq(j).val() + delimiter + relatedProps.eq(j).siblings("input[placeholder='property name']").val() + delimiter;
        valiDataString += newPropDelimiter;
        const relatedValis = relatedProps.eq(j).parents(".property-wrap").find("input[placeholder='validation']");
        for (let k = 0; k < relatedValis.length; k++) {
          valiDataString += relatedValis.eq(k).val() + delimiter;
        }
      }
    }

    $("#modelData").val(modelDataString);
    $("#propData").val(propDataString);
    $("#validationData").val(valiDataString);
  });
});
