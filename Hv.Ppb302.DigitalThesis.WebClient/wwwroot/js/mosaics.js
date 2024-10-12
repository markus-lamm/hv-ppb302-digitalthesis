document.addEventListener("DOMContentLoaded", () => {
    // File-scope variables
    const container = document.querySelector(".mosaic-container");
    const mosaics = Array.from(container.querySelectorAll(".mosaic"));

    // SIDEBAR
    const sidebarContainer = document.querySelector("#mosaics-sidebar-container");
    const sidebarBtn = document.querySelector(".mosaics-sidebar-btn");
    const sidebar = document.querySelector("#mosaics-sidebar");
    let isRotated = false;
    let isSidebarOpen = false;

    if (sidebarBtn && sidebarContainer) {
        sidebarBtn.addEventListener("click", toggleSidebar);
    } else {
        console.error("Cannot find elements with ids \"mosaics-sidebar-btn\" or \"mosaics-sidebar-container\"");
    }

    //Toggles the sidebar open/minimized state.
    function toggleSidebar() {
        isRotated = !isRotated;
        isSidebarOpen = !isSidebarOpen;
        sidebarBtn.classList.toggle("rotate");
        sidebarContainer.classList.toggle("show");
        sidebar.classList.toggle("overflowenabled");
    }

    // Get all checkboxes
    const checkboxes = Array.from(document.querySelectorAll('.filter-checkbox input[type="checkbox"]'));
    const directions = mosaics.map(() => ({
        x: (Math.random() > 0.5 ? 1 : -1) + Math.random() * 0.4,
        y: (Math.random() > 0.5 ? 1 : -1) + Math.random() * 0.4
    }));
    const speedFactor = 0.5;
    const becomingsBtn = document.querySelector("#becomings-btn");
    let isBecomingsBtnTriggered = false;

    if (checkboxes.length > 0) {
        checkboxes.forEach((checkbox) => {
            checkbox.addEventListener("click", filterMosaics);
        });
    } else {
        console.error("Cannot find elements with class \"filter-checkbox\"");
    }

    // Filters mosaics based on selected checkboxes
    function filterMosaics() {
        const selectedTags = checkboxes.filter(checkbox => checkbox.checked).map(checkbox => checkbox.dataset.tag);

        mosaics.forEach((mosaic) => {
            const mosaicTags = mosaic.dataset.tags.split(",");
            if (selectedTags.length === 0 || selectedTags.some(tag => mosaicTags.includes(tag))) {
                mosaic.style.opacity = "1";
            } else {
                mosaic.style.opacity = "0.2";
            }
        });
    }

    // Reset all checkboxes when the page loads
    window.onload = () => {
        checkboxes.forEach((checkbox) => {
            checkbox.checked = false;
        });
        if (becomingsBtn) {
            becomingsBtn.checked = false;
        }
    };

    if (becomingsBtn) {
        becomingsBtn.addEventListener("change", toggleBecomings);
    } else {
        console.error("Cannot find element with id \"becomings-btn\"");
    }

    // Toggles the becomings button state and updates mosaic event listeners
    function toggleBecomings() {
        isBecomingsBtnTriggered = !isBecomingsBtnTriggered;
        updateMosaicEventListeners(!isBecomingsBtnTriggered);

        mosaics.forEach(mosaic => {
            mosaic.classList.toggle("paused", isBecomingsBtnTriggered);
        });
    }

    // MOSAIC SETUP
    mosaics.forEach(mosaic => {
        mosaic.style.left = `${Math.random() * 80}vw`;
        mosaic.style.top = `${Math.random() * 80}vh`;
    });

    // Updates mosaic event listeners based on the becomings button state
    function updateMosaicEventListeners(addListeners = true) {
        mosaics.forEach(mosaic => {
            const mosaicContent = mosaic.querySelector(".mosaic-content");
            const floatContainer = document.querySelector(".mosaic-float");

            if (addListeners) {
                mosaic.classList.add("mosaic-position");
                mosaicContent.classList.remove("active");
                container.style.overflow = "hidden";
                container.style.position = "fixed";
                mosaic.addEventListener("mouseenter", pauseMosaic);
                mosaic.addEventListener("mouseleave", resumeMosaic);
                floatContainer.style.marginInline = "0";
            } else {
                mosaic.removeEventListener("mouseenter", pauseMosaic);
                mosaic.removeEventListener("mouseleave", resumeMosaic);
                mosaic.addEventListener("mouseenter", (event) => {
                    toggleMosaicText(true);
                    event.currentTarget.style.zIndex = "100";
                });

                mosaic.addEventListener("mouseleave", (event) => {
                    toggleMosaicText(false);
                    event.currentTarget.style.zIndex = "0";
                });
                floatContainer.style.marginInline = "10rem";
                mosaicContent.classList.add("active");
                container.style.overflow = "unset";
                container.style.position = "unset";
                mosaic.classList.remove("mosaic-position");
            }
        });
    }

    function pauseMosaic() {
        this.classList.add("paused");
        toggleMosaicText(true);
    }

    function resumeMosaic() {
        this.classList.remove("paused");
        toggleMosaicText(false);
    }

    function toggleMosaicText(show) {
        document.querySelectorAll(".mosaic a div").forEach(textElement => {
            textElement.style.opacity = show ? "1" : "0";
            textElement.style.display = show ? "block" : "none";
        });
    }

    function update() {
        mosaics.forEach((mosaic, index) => {
            if (mosaic.classList.contains("paused")) return;

            const rect = mosaic.getBoundingClientRect();

            if (rect.left < 0 || rect.right > window.innerWidth) {
                directions[index].x *= -1;
            }
            if (rect.top < 0 || rect.bottom > window.innerHeight) {
                directions[index].y *= -1;
            }

            mosaic.style.left = `${rect.left + directions[index].x * speedFactor}px`;
            mosaic.style.top = `${rect.top + directions[index].y * speedFactor}px`;
        });

        requestAnimationFrame(update);
    }

    // Start the update loop
    update();

    // COOKIES
    //Check cookies and apply css
    function applyVisitedMosaics() {
        const visitedMosaics = getCookie("digital-thesis-mosaics");
        if (visitedMosaics) {
            const decodedMosaics = decodeURIComponent(visitedMosaics);
            const visitedList = JSON.parse(decodedMosaics);
            visitedList.forEach(id => {
                const element = document.querySelector(`#mosaic-${id}`);
                if (element) {
                    const elementImg = element.querySelector(".mosaicImg");
                    if (elementImg) {
                        elementImg.classList.add("visited");
                    }
                }
                else {
                    console.log("Element not found for ID:", id);
                }
            });
        }
    }

    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        return parts.pop().split(";").shift();
    }

    // Ensure the script runs on initial load
    applyVisitedMosaics();
    updateMosaicEventListeners();
});
