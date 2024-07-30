function loadLoginComponent (url) {
    fetch(url)
        .then(response => response.text())
        .then(data => {
            document.getElementById("login-container").innerHTML = data;
        })
        .catch(error => console.error("Error fetching HTML", error));
}

document.onload = loadLoginComponent("../Assets/Components/Login/sign_in.html")