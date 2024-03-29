$(function () {
  HighlightCurrentNavBtn($("#manageTeamNavBtn"));



  // Modal events
  $(".remove-app-user-btn").on("click", () => {
    ToggleModal($("#manageTeamContent"), $("#removeUserModal"), openModal);
  });
  $(".remove-user-close-btn").on("click", () => {
    ToggleModal($("#manageTeamContent"), $("#removeUserModal"), closeModal);
  });



  // TH sorting events
  let thFirstNameInOrder = true;
  let thLastNameInOrder = true;

  $("#thFirstName").on("click", () => {
    thFirstNameInOrder = thSortEvent("manageTeamTbody", thFirstNameInOrder, "manageTeamTR", "sortFirstName", alphabeticallyFirst);
  });
  $("#thLastName").on("click", () => {
    thLastNameInOrder = thSortEvent("manageTeamTbody", thLastNameInOrder, "manageTeamTR", "sortLastName", alphabeticallyFirst);
  });



  // Btn events
  $(".remove-app-user-btn").on("click", (event) => {
    const argument = $(event.target).attr("data-appUserId");
    const firstName = $(event.target).attr("data-first");
    const lastName = $(event.target).attr("data-last");
    $("#removeUserConf").attr("action", `/EMS/ManageTeamRemove?appUserIdToRemove=${argument}`);
    $("#removeUserMsg").html(`Remove ${firstName} ${lastName} from this project?`);
  });
  $(".confirm-btn").on("click", (event) => {
    const argument = $(event.target).attr("data-appUserId");
    $("#approveUserConf").attr("action", `/EMS/ManageTeamApprove?appUserIdToApprove=${argument}`);
    $("#approveUserConf").submit();
  });
  $(".deny-btn").on("click", (event) => {
    const argument = $(event.target).attr("data-appUserId");
    $("#removeUserConf").attr("action", `/EMS/ManageTeamRemove?appUserIdToRemove=${argument}`);
    $("#removeUserConf").submit();
  });



  // Search filter
  $("#manageTeamSearchName").on("input", () => {
    const filterString = ($("#manageTeamSearchName").val()).toLowerCase();
    const color0 = $(".table-wrap").find("tbody:first").find("tr:first").css("background-color");
    const color1 = $(".table-wrap").css("background-color");
    let toggle = 0;

    // set hide status for all <tr>
    for (let i = 0; i < $(".sortFirstName").length; i++) {
      const firstNameData = $(".sortFirstName").eq(i).html();
      const lastNameData = $(".sortLastName").eq(i).html();
      const fullName = (firstNameData + " " + lastNameData).toLowerCase();
      const trElement = $(`#manageTeamTR_${i}`);
      if (fullName.includes(filterString)) {
        trElement.removeClass("hide");
        // force the striped pattern because hidden tr's disrupt this
        const colorToUse = (toggle === 0) ? color0 : color1;
        trElement.css("background-color", colorToUse);
        toggle = (toggle === 0) ? 1 : 0;
      }
      else {
        trElement.addClass("hide");
      }
    }
  });
});
