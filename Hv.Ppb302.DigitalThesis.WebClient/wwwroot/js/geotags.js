document.addEventListener('DOMContentLoaded', () => {
    const tutorialOverlay = document.getElementById('tutorial-overlay');
    const tutorialBox = document.getElementById('tutorial-box');
    const tutorialText = document.getElementById('tutorial-text');
    const tutorialPrevBtn = document.getElementById('tutorial-prev-btn');
    const tutorialNextBtn = document.getElementById('tutorial-next-btn');
    const tutorialExitBtn = document.getElementById('tutorial-exit-btn');
    const tutorialPrevStep = document.getElementById('tutorial-prev-step');
    const tutorialCurrentStep = document.getElementById('tutorial-current-step');
    const tutorialNextStep = document.getElementById('tutorial-next-step');
    const navbar = document.getElementById('navbar');
    const geotags = ["link-geotag-1", "link-geotag-2", "link-geotag-3", "link-geotag-4", "link-geotag-5", "link-geotag-6"];
    const molarMosaics = ["mosaic-yellow-1", "mosaic-yellow-2", "mosaic-yellow-3"];
    const molecularMosaics = ["mosaic-blue-1", "mosaic-blue-2", "mosaic-blue-3", "mosaic-blue-4", "mosaic-blue-5"];
    const kaleidoscope = document.getElementById('link-kaleidoscope');
    let currentStep = 1;

    showStep();

    tutorialPrevBtn.addEventListener('click', function () {
        currentStep--;
        showStep();
    });

    tutorialNextBtn.addEventListener('click', function () {
        currentStep++;
        showStep();
    });

    tutorialExitBtn.addEventListener('click', function () {
        tutorialOverlay.style.display = "none";
    });

    function showStep() {

        switch (currentStep) {
            case 1: {
                tutorialText.innerText = "Welcome to Fragile Mosaics of Teacher Becoming tutorial!"
                tutorialPrevBtn.innerText = "Exit"
                tutorialCurrentStep.innerText = currentStep;
                tutorialPrevStep.style.backgroundColor = "transparent";
                tutorialBox.style.top = "50%";
                tutorialBox.style.left = "50%";
                navbar.style.zIndex = "1";
                break;
            }
            case 2: {
                tutorialText.innerText = "When you want to come back to the map view, click on the Teacher Becoming-logo in the top left corner."
                tutorialPrevBtn.innerText = "Previous"
                tutorialCurrentStep.innerText = currentStep;
                tutorialPrevStep.style.display = "";
                tutorialPrevStep.style.backgroundColor = "darkred";
                tutorialBox.style.top = "40%";
                tutorialBox.style.left = "30%";
                navbar.style.zIndex = "10";
                applyZindexToArray(geotags, "0");
                break;
            }
            case 3: {
                tutorialText.innerText = "To explore the various texts, click on any of the geotags."
                tutorialCurrentStep.innerText = currentStep;
                tutorialPrevStep.style.backgroundColor = "cornflowerblue";
                tutorialBox.style.top = "40%";
                tutorialBox.style.left = "70%";
                navbar.style.zIndex = "1";
                applyZindexToArray(geotags, "10");
                applyZindexToArray(molarMosaics, "0");
                break;
            }
            case 4: {
                tutorialText.innerText = "Set Molar Mosaics free and begin exploring them."
                tutorialCurrentStep.innerText = currentStep;
                tutorialBox.style.top = "60%";
                tutorialBox.style.left = "30%";
                applyZindexToArray(geotags, "0");
                applyZindexToArray(molarMosaics, "10");
                applyZindexToArray(molecularMosaics, "0");
                break;
            }
            case 5: {
                tutorialText.innerText = "Set Molecular Mosaics free and begin exploring them."
                tutorialCurrentStep.innerText = currentStep;
                tutorialNextStep.style.backgroundColor = "cornflowerblue";
                tutorialBox.style.top = "60%";
                tutorialBox.style.left = "40%";
                applyZindexToArray(molecularMosaics, "10");
                applyZindexToArray(molarMosaics, "0");
                kaleidoscope.style.zIndex = "0";
                break;
            }
            case 6: {
                tutorialText.innerText = "Experience a one of a kind Kaleido-scoping mechanism"
                tutorialCurrentStep.innerText = currentStep;
                tutorialNextStep.style.display = "";
                tutorialNextStep.style.backgroundColor = "darkred";
                tutorialNextBtn.innerText = "Next"
                tutorialBox.style.top = "40%";
                tutorialBox.style.left = "50%";
                kaleidoscope.style.zIndex = "10";
                applyZindexToArray(molecularMosaics, "0");
                applyZindexToArray(molarMosaics, "0");
                break;
            }
            case 7: {
                tutorialText.innerText = "On the respective Mosaics pages, you will find an expandable filter menu in the bottom right corner."
                tutorialCurrentStep.innerText = currentStep;
                tutorialNextStep.style.backgroundColor = "transparent";
                tutorialNextBtn.innerText = "Exit"
                tutorialBox.style.top = "50%";
                tutorialBox.style.left = "30%";
                applyZindexToArray(molecularMosaics, "10");
                applyZindexToArray(molarMosaics, "10");
                kaleidoscope.style.zIndex = "0";
                break;
            }
            default: {
                tutorialOverlay.style.display = "none";
                break;
            }
        }
    }

    function applyZindexToArray (array, zValue) {
        array.forEach(id => {
            let element = document.getElementById(id);
            if (element) {
                element.style.zIndex = zValue;
            }
        });
    }
});
