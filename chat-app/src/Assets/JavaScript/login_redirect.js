// Checks if username is stored in session data, else go to login page
window.onload = function () {
    if (window.location.href == "/chat-app/src/Pages/login.html"){
        return;
    }
    if (!sessionStorage.getItem("user_tag")){        
        window.location.href = "Pages/login.html";
    }
};