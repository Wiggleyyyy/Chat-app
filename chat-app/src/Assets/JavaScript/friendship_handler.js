const baseurl = "https://localhost:7280"

async function sendFriendRequest() {
    const friend_usertag_input = document.getElementById("friend_usertag_input").value;
    console.log("david er til mænd");
    if (!friend_usertag_input) {
        showErrorToast("Need to enter a username.");
        return;
    }
    showSuccessToast("Sent friend request.");
    //api call to send friend request
}