window.onload = function() {
    var user_profile = document.getElementById("self_username");
    if (sessionStorage.getItem("username")) {
        user_profile.innerHTML = sessionStorage.getItem("username");
    } else {
        user_profile.innerHTML = "ERR 404";
        showErrorToast("Couldnt get username");
    }
}

function copyTag() {
    navigator.clipboard.writeText(sessionStorage.getItem("user_tag"));
    showSuccessToast("Copied");
}