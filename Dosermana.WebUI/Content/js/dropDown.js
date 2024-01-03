function toggleDropdown() {
    var dropdownMenu = document.getElementById("dropdownMenu");
    dropdownMenu.style.display = dropdownMenu.style.display === "none" ? "block" : "none";
}

document.addEventListener("click", function (event) {
    var dropdownMenu = document.getElementById("dropdownMenu");
    var targetElement = event.target;

    if (!dropdownMenu.contains(targetElement)) {
        dropdownMenu.style.display = "none";
    }
});