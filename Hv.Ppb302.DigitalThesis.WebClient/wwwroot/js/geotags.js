﻿
document.addEventListener('DOMContentLoaded', () => {
    showStep(currentStep);
});

let currentStep = 0;

function showStep(step) {
    const overlayBoxes = document.querySelectorAll('.overlay-box');
    const steps = document.querySelectorAll('.step');

    overlayBoxes.forEach(box => box.style.display = 'none');
    steps.forEach(s => s.classList.remove('active'));

    const cancelButton = document.querySelector('.cancel-btn');

    if (step < overlayBoxes.length) {
        overlayBoxes[step].style.display = 'block';
        steps[step].classList.add('active');

        const buttonContainer = document.querySelector('.button-container');
        const activeBox = overlayBoxes[step];
        activeBox.appendChild(buttonContainer);
        activeBox.appendChild(cancelButton);

        document.querySelector('.back-button').disabled = step === 0;
        document.querySelector('.next-button').disabled = step === overlayBoxes.length;
    } else {
        if (document.getElementById('tutorial-overlay')) {
            document.getElementById('tutorial-overlay').style.display = "none";
        }
    }

    if (cancelButton) {
        cancelButton.addEventListener('click', () => {
            document.getElementById('tutorial-overlay').style.display = "none";
        });
    }

    const elementLogo = document.getElementById("navbar");
    if (currentStep === 1) {
        elementLogo.style.zIndex = 1001

    } else {
        elementLogo.style.zIndex = 1000;
    }

    const geotagIDs = ["link-geotag-1", "link-geotag-2", "link-geotag-3", "link-geotag-6"];
    if (geotagIDs) {
        geotagIDs.forEach(id => {
            const element = document.getElementById(id);
            if (element) {
                if (currentStep === 2) {
                    element.style.zIndex = 1001;
                } else {
                    element.style.zIndex = 1000;
                }
            }
        });
    }

    const yellowMosaicIDs = ["yellow-mosaic1", "yellow-mosaic2", "yellow-mosaic3"];
    if (yellowMosaicIDs) {
        yellowMosaicIDs.forEach(id => {
            const element = document.getElementById(id); 3
            if (element) {
                if (currentStep === 3 || currentStep === 6) {
                    element.style.zIndex = 1001;
                } else {
                    element.style.zIndex = 1000;
                }
            }
        });
    }

    const blueMosaicIDs = ["blue-mosaic1", "blue-mosaic2", "blue-mosaic3", "blue-mosaic4", "blue-mosaic5"];
    if (blueMosaicIDs) {
        blueMosaicIDs.forEach(id => {
            const element = document.getElementById(id);
            if (element) {
                if (currentStep === 4 || currentStep === 6) {
                    element.style.zIndex = 1001;
                } else {
                    element.style.zIndex = 1000;
                }
            }
        });
    }

    const element = document.getElementById("link-kaleidoscope");
    const whiteImage = document.getElementById("geotag-white");
    const blackImage = document.getElementById("geotag-black");
    if (element && whiteImage && blackImage) {
        if (currentStep === 5) {
            element.style.zIndex = 1001;
            whiteImage.style.display = 'none';
            blackImage.style.display = '';
        } else {
            element.style.zIndex = 1000;
            blackImage.style.display = 'none';
            whiteImage.style.display = '';
        }
    }

}

function nextStep() {
    if (currentStep < document.querySelectorAll('.overlay-box').length) {
        currentStep++;
        showStep(currentStep);
    }
}

function prevStep() {
    if (currentStep > 0) {
        currentStep--;
        showStep(currentStep);
    }
}
