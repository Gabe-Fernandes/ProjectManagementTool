$(function () {
  // toggle password show
  $("#passHideBtn").on("click", ()=>{
    TogglePasswordShow($("#loginPass"), $("#passHideBtn"), $("#passShowBtn"));
  });
  $("#passShowBtn").on("click", ()=>{
    TogglePasswordShow($("#loginPass"), $("#passShowBtn"), $("#passHideBtn"));
  });

  // demo modal
  $("#demoBtn").on("click", ()=>{
    ToggleModal($("#loginMain"), $("#demoModal"), openModal);
  });
  $("#demoCloseBtn").on("click", ()=>{
    ToggleModal($("#loginMain"), $("#demoModal"), closeModal);
  });

  // recover pass modal
  $("#recoverPassBtn").on("click", ()=>{
    ToggleModal($("#loginMain"), $("#recoverPassModal"), openModal);
  });
  $("#recoverPassCloseBtn").on("click", ()=>{
    ToggleModal($("#loginMain"), $("#recoverPassModal"), closeModal);
  });

  // resend email conf modal
  $("#resendEmailConfBtn").on("click", ()=>{
    ToggleModal($("#loginMain"), $("#resendEmailConfModal"), openModal);
  });
  $("#resendEmailConfCloseBtn").on("click", ()=>{
    ToggleModal($("#loginMain"), $("#resendEmailConfModal"), closeModal);
  });
});
