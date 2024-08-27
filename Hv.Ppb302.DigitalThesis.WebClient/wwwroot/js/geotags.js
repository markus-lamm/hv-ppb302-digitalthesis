document.addEventListener('DOMContentLoaded', () => {
    const tutorialOverlay = document.getElementById('tutorial-overlay');
    const tutorialBox = document.getElementById('tutorial-box');
    const tutorialText = document.getElementById('tutorial-text');
    const tutorialPrevBtn = document.getElementById('tutorial-prev-btn');
    const tutorialNextBtn = document.getElementById('tutorial-next-btn');
    const tutorialExitBtn = document.getElementById('tutorial-exit-btn');
    const tutorialStepContainer = document.getElementById('tutorial-step-container');
    const navbar = document.getElementById('navbar');
    const geotags = ["link-geotag-1", "link-geotag-2", "link-geotag-3", "link-geotag-4", "link-geotag-5", "link-geotag-6"];
    const molarMosaics = ["mosaic-yellow-1", "mosaic-yellow-2", "mosaic-yellow-3"];
    const molecularMosaics = ["mosaic-blue-1", "mosaic-blue-2", "mosaic-blue-3", "mosaic-blue-4", "mosaic-blue-5"];
    const kaleidoscope = document.getElementById('link-kaleidoscope');
    let currentStep = 1;

    const steps = [
        "Welcome to Fragile Mosaics of Teacher Becoming tutorial!",
        "When you want to come back to the map view, click on the Teacher Becoming-logo in the top left corner.",
        "To explore the various texts, click on any of the geotags.",
        "Set Molar Mosaics free and begin exploring them.",
        "Set Molecular Mosaics free and begin exploring them.",
        "Experience a one of a kind Kaleido-scoping mechanism.",
        "On the respective Mosaics pages, you will find an expandable filter menu in the bottom right corner."
    ];

    steps.forEach((step, index) => {
        const stepElement = document.createElement('div');
        stepElement.classList.add('tutorial-step');
        stepElement.id = `tutorial-step-${index + 1}`;
        stepElement.textContent = index + 1;
        tutorialStepContainer.appendChild(stepElement);
    });

    showStep();

    tutorialPrevBtn.addEventListener('click', function () {
        if (currentStep > 1) {
            currentStep--;
            showStep();
        } else {
            tutorialOverlay.style.display = "none";
        }
    });

    tutorialNextBtn.addEventListener('click', function () {
        if (currentStep < steps.length) {
            currentStep++;
            showStep();
        } else {
            tutorialOverlay.style.display = "none";
        }
    });

    tutorialExitBtn.addEventListener('click', function () {
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
                break;
            case 4:
                applyZindexToArray(geotags, "0");
                applyZindexToArray(molarMosaics, "10");
                applyZindexToArray(molecularMosaics, "0");
                break;
            case 5:
                applyZindexToArray(molecularMosaics, "10");
                applyZindexToArray(molarMosaics, "0");
                kaleidoscope.style.zIndex = "0";
                break;
            case 6:
                tutorialNextBtn.innerText = "Next"
                kaleidoscope.style.zIndex = "10";
                applyZindexToArray(molecularMosaics, "0");
                applyZindexToArray(molarMosaics, "0");
                break;
            case 7:
                tutorialNextBtn.innerText = "Exit"
                applyZindexToArray(molecularMosaics, "10");
                applyZindexToArray(molarMosaics, "10");
                kaleidoscope.style.zIndex = "0";
                break;
            default:
                tutorialOverlay.style.display = "none";
                break;
        }
    }

    function highlightCurrentStep() {
        document.querySelectorAll('.tutorial-step').forEach((stepElement, index) => {
            if (index === currentStep - 1) {
                stepElement.classList.add('active');
            } else {
                stepElement.classList.remove('active');
            }
        });
    }

    function applyZindexToArray(array, zValue) {
        array.forEach(id => {
            let element = document.getElementById(id);
            if (element) {
                element.style.zIndex = zValue;
            }
        });
    }
});
