// Checks if username is stored in session data, else go to login page
window.onload = function () {
    if (!sessionStorage.getItem("Username")){
        window.location.href = "./Pages/login.html"
    }
};