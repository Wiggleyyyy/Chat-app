// Checks if username is stored in session data, else go to login page
window.onload = function () {
    if (window.location.href == "/chat-app/src/Pages/login.html"){
        return;
    }
    if (!sessionStorage.getItem("Username")){        
        window.location.href = "Pages/login.html";
    }
};

function signUp(){
    const usernameInput = document.getElementById("username_input").value;
    const emailInput = document.getElementById("email_input").value;
    const passwordInput = document.getElementById("password_input").value;
    const firstnameInput = document.getElementById("firstname_input").value;
    const lastnameInput = document.getElementById("lastname_input").value;
    const countryInput = document.getElementById("country_input").value;
    const birthdateInput = document.getElementById("birthdate_input").value;

    if(usernameInput && emailInput && passwordInput && firstnameInput && lastnameInput && countryInput && birthdateInput){

    }else{
        alert("Error, missing required fields!");
    }

}


function signIn(){
    const usernameIntext = document.getElementById("username_input").value;
    const passwordIntext = document.getElementById("password_input").value;

    if (usernameIntext && passwordIntext){

    }
    else {
        alert("Error, missing required fields!");
    }
}