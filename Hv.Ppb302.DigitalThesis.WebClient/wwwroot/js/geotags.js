document.addEventListener("DOMContentLoaded", () => {
    const tutorialOverlay = document.querySelector("#tutorial-overlay");
    const tutorialText = document.querySelector("#tutorial-text");
    const tutorialPrevBtn = document.querySelector("#tutorial-prev-btn");
    const tutorialNextBtn = document.querySelector("#tutorial-next-btn");
    const tutorialExitBtn = document.querySelector("#tutorial-exit-btn");
    const tutorialStepContainer = document.querySelector("#tutorial-step-container");
    const navbar = document.querySelector("#navbar");
    const geotags = ["link-geotag-1", "link-geotag-2", "link-geotag-3", "link-geotag-4", "link-geotag-5", "link-geotag-6"];
    const molarMosaics = ["mosaic-yellow-1", "mosaic-yellow-2", "mosaic-yellow-3", "link-molarmosaics"];
    const molecularMosaics = ["mosaic-blue-1", "mosaic-blue-2", "mosaic-blue-3", "mosaic-blue-4", "mosaic-blue-5", "link-molecularmosaics"];
    const kaleidoscope = document.querySelector("#link-kaleidoscope");
    const aboutNavPage = document.querySelector("#about-page-nav");
    const highlighter = document.querySelector("#highlighter");
    let currentStep = 1;

    const steps = [
        "Welcome to Fragile Mosaics of Teacher Becoming tutorial!",
        "When you want to come back to the map view, click on the Teacher Becoming-logo in the top left corner.",
        "To explore the various texts, click on any of the geotags.",
        "Read more about the composition of the map view and the thesis here.",
        "Set Molar Mosaics free and begin exploring them.",
        "Set Molecular Mosaics free and begin exploring them.",
        "Experience a one of a kind Kaleidoscoping mechanism.",
        "Each Mosaic page features an expandable filter menu in the bottom right corner."
    ];

    steps.forEach((step, index) => {
        const stepElement = document.createElement("div");
        stepElement.classList.add("tutorial-step");
        stepElement.id = `tutorial-step-${index + 1}`;
        stepElement.textContent = index + 1;
        tutorialStepContainer.appendChild(stepElement);
    });

    showStep();

    tutorialPrevBtn.addEventListener("click", function () {
        if (currentStep > 1) {
            currentStep--;
            showStep();
        } else {
            tutorialOverlay.style.display = "none";
        }
    });

    tutorialNextBtn.addEventListener("click", function () {
        if (currentStep < steps.length) {
            currentStep++;
            showStep();
        } else {
            tutorialOverlay.style.display = "none";
        }
    });

    tutorialExitBtn.addEventListener("click", function () {
        tutorialOverlay.style.display = "none";
    });

    function showStep() {
        tutorialText.innerText = steps[currentStep - 1];
        highlightCurrentStep();

        switch (currentStep) {
            case 1:
                tutorialPrevBtn.innerText = "Exit";
                navbar.style.zIndex = "1";
                break;
            case 2:
                tutorialPrevBtn.innerText = "Previous";
                navbar.style.zIndex = "10";
                applyZindexToArray(geotags, "0");
                break;
            case 3:
                navbar.style.zIndex = "1";
                applyZindexToArray(geotags, "10");
                applyZindexToArray(molarMosaics, "0");
                highlighter.style.display = "none";
                navbar.style.zIndex = "1";
                break;
            case 4:
                applyZindexToArray(molarMosaics, "0");
                applyZindexToArray(geotags, "0");
                highlighter.style.display = "block";
                navbar.style.zIndex = "10";
                applyHighlighter();
                break;
            case 5:
                applyZindexToArray(geotags, "0");
                applyZindexToArray(molarMosaics, "10");
                applyZindexToArray(molecularMosaics, "0");
                highlighter.style.display = "none";
                navbar.style.zIndex = "1";
                break;
            case 6:
                applyZindexToArray(molecularMosaics, "10");
                applyZindexToArray(molarMosaics, "0");
                kaleidoscope.style.zIndex = "0";
                break;
            case 7:
                tutorialNextBtn.innerText = "Next";
                kaleidoscope.style.zIndex = "10";
                kaleidoscope.style.position = "relative";
                applyZindexToArray(molecularMosaics, "0");
                applyZindexToArray(molarMosaics, "0");
                break;
            case 8:
                tutorialNextBtn.innerText = "Exit";
                applyZindexToArray(molecularMosaics, "10");
                applyZindexToArray(molarMosaics, "10");
                kaleidoscope.style.zIndex = "0";
                break;
            default:
                tutorialOverlay.style.display = "none";
                break;
        }
    }

    function applyHighlighter() {
        const aboutNavPageRect = aboutNavPage.getBoundingClientRect();
        // Set the highlighter's position and size
        highlighter.style.display = "block";
        highlighter.style.position = "absolute";
        highlighter.style.top = `${aboutNavPageRect.top - 5}px`;
        highlighter.style.left = `${aboutNavPageRect.left - 5}px`;
        highlighter.style.width = `${aboutNavPageRect.width + 10}px`;
        highlighter.style.height = `${aboutNavPageRect.height + 10}px`;
    }

    function highlightCurrentStep() {
        document.querySelectorAll(".tutorial-step").forEach((stepElement, index) => {
            if (index === currentStep - 1) {
                stepElement.classList.add("active");
            } else {
                stepElement.classList.remove("active");
            }
        });
    }

    function applyZindexToArray(array, zValue) {
        array.forEach(id => {
            const element = document.querySelector(`#${id}`);
            if (element) {
                element.style.zIndex = zValue;
            }
        });
    }
});
