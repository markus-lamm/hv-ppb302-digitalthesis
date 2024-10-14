document.addEventListener("DOMContentLoaded", () => {
    //Checks the screen size and updates the display of the size guard element
    function checkScreenSize() {
        const sizeGuard = document.querySelector("#size-guard");
        if (!sizeGuard) {
            console.error("Cannot find element with id \"size-guard\"");
            return;
        }
        const sizeGuardTrigger = window.innerWidth < 1024 || window.innerHeight < 768;
        sizeGuard.style.display = sizeGuardTrigger ? "flex" : "none";
    }

    // Initial check on page load
    checkScreenSize();

    // Add resize event listener to check screen size on window resize
    window.addEventListener("resize", checkScreenSize);
});
