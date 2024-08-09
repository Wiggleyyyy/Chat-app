var modal = document.getElementById("modal_popup");

function show_modal() {
    modal.style.display = "block";
}

function hide_modal() {
    modal.style.display = "none";
}

window.onclick = function(event) {
    if (event.target === modal) {
        modal.style.display = "none";
    }
}