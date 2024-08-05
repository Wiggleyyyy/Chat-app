const baseurl = "https://localhost:7280";

function User(first_name, last_name, country, date_of_birth, username, email, public_key, created_at, user_tag) {
    this.user_id = null;
    this.first_name = first_name;
    this.last_name = last_name;
    this.country = country;
    this.date_of_birth = date_of_birth;
    this.username = username;
    this.email = email;
    this.encrypted_password = null;
    this.public_key = public_key;
    this.created_at = created_at;
    this.user_tag = user_tag;
}

async function generateRandomKey() {
    // #region oldScript
    /*
    const key = await window.crypto.subtle.generateKey({
        name: "AES-GCM",
        length: 256, 
    },
    true,
    ["encrypt", "decrypt"]
    );
    
    const exportedKey = await window.crypto.subtle.exportKey("raw", key);
    const keyArray = new Uint8Array(exportedKey);
    return arrayBufferToBase64(keyArray.buffer);

    var encrypted = CryptoJS.AES.encrypt(
    )    
    */
   // #endregion
    const chars = "0123456789abcdef";
    let key;

    for (let i = 0; i < 13; i++) {
        key += chars[Math.random(0, chars.length + 1)];
    }
    return key;
}

function stringToArrayBuffer(string) {
    const encoder = new TextEncoder();
    return encoder.encode(string);
}

function arrayBufferToBase64(buffer) {
    let binary = '';
    const bytes = new Uint8Array(buffer);
    for (let i = 0; i < bytes.byteLength; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return btoa(binary);
}

async function encryptPassword(input, keyString) {
    const keyMaterial = await crypto.subtle.importKey(
        'raw',
        stringToArrayBuffer(keyString),
        'PBKDF2',
        false,
        ['deriveKey']
    );

    const salt = crypto.getRandomValues(new Uint8Array(16));
    const iv = crypto.getRandomValues(new Uint8Array(12));

// #region derivedKey
    const derivedKey = await crypto.subtle.deriveKey(
        {
            name: 'PBKDF2',
            salt: salt,
            iterations: 100000,
            hash: 'SHA-256'
        },
        keyMaterial,
        {
            name: 'AES-GCM',
            length: 256
        },
        true,
        ['encrypt']
    );
// #endregion

    const encrypted = await crypto.subtle.encrypt(
        {
            
            name: 'AES-GCM',
            iv: iv
        },
        derivedKey,
        stringToArrayBuffer(input)
    );

    // Combine salt, iv, and encrypted data into a single array
    const combinedBuffer = new Uint8Array(salt.byteLength + iv.byteLength + encrypted.byteLength);
    combinedBuffer.set(salt, 0);
    combinedBuffer.set(iv, salt.byteLength);
    combinedBuffer.set(new Uint8Array(encrypted), salt.byteLength + iv.byteLength);

    return arrayBufferToBase64(combinedBuffer.buffer);
}

async function decryptPassword() {
    const decrypted = await crypto.subtle.decrypt(
        {
            name: 'AES-GCM',
            iv: iv
        }
    )
}

async function signUp() {
    // Check for public key and private key
    if (!localStorage.getItem("private_key")) {
        const privateKey = await generateRandomKey();
        localStorage.setItem("private_key", privateKey);
    }
    if (!localStorage.getItem("public_key")) {
        const publicKey = await generateRandomKey();
        localStorage.setItem("public_key", publicKey);
    }

    const usernameInput = document.getElementById("username_input").value;
    const emailInput = document.getElementById("email_input").value;
    const passwordInput = document.getElementById("password_input").value;
    const repeatedPasswordInput = document.getElementById("repeatedPassword_input").value;
    const firstnameInput = document.getElementById("firstname_input").value;
    const lastnameInput = document.getElementById("lastname_input").value;
    const countryInput = document.getElementById("country_input").value;
    const birthdateInput = document.getElementById("birthdate_input").value;

    if (passwordInput !== repeatedPasswordInput) {
        alert("Passwords do not match.");
        return;
    }

    if (usernameInput && emailInput && passwordInput && repeatedPasswordInput && firstnameInput && lastnameInput && countryInput && birthdateInput) {
        const now = new Date();
        const formattedDate = `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}-${String(now.getDate()).padStart(2, '0')} ${String(now.getHours()).padStart(2, '0')}:${String(now.getMinutes()).padStart(2, '0')}:${String(now.getSeconds()).padStart(2, '0')}`;

        const user = new User(
            firstnameInput,
            lastnameInput,
            countryInput,
            birthdateInput,
            usernameInput,
            emailInput,
            localStorage.getItem("public_key"),
            formattedDate,
            `@${usernameInput}`
        );

        user.encrypted_password = await encryptPassword(passwordInput, localStorage.getItem("public_key"));

        try {
            const apiUrl = `${baseurl}/Users`;

            console.log(user);

            const response = await fetch(apiUrl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(user),
            });

            if (response.status === 200) {
                // Success
                console.log("Created account, and logged in");
                sessionStorage.setItem("user_tag", user.user_tag);
                window.location.href = "/chat-app/src/index.html";
            } else if (response.status === 400) {
                // Bad request
                alert("Error creating account.");
                throw new Error("Bad request.");
            } else if (response.status === 404) {
                // Not found
                alert("Error creating account");
                throw new Error("API not found.");
            } else {
                console.log("Response: " + response.status);
            }
        } catch (error) {
            console.error("There was a problem creating account: ", error);
        }

    } else {
        alert("Error, missing required fields!");
    }
}

function signIn() {
    const usernameInput = document.getElementById("username_input").value;
    const passwordInput = document.getElementById("password_input").value;
    
    const user = new User();
    if (usernameInput.includes("@")){
        user.email = usernameInput;
    }
    else {
        user.username = usernameInput;
    }
    user.encrypted_password = encryptPassword(passwordInput, localStorage.getItem("private_key"));

    if (usernameInput && passwordInput) {
        // Implement sign-in logic here
        const apiURL = `${baseurl}/Users`;

        try {
            const response = await fetch(apiURL,{
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify()
            }); 
        }
    } else {
        alert("Error, missing required fields!");
    }
}
