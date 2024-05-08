// FILE-SCOPE VARIABLES
// Get the mosaic container
const container = document.querySelector('.mosaic-container');

// Get all mosaic elements
const mosaics = Array.from(container.querySelectorAll('.mosaic'));

// Create an array to store the direction of each mosaic
const directions = [];

// Define a speed factor (smaller values will make the animation slower)
const speedFactor = 0.5;

// Get all checkboxes
const checkboxes = Array.from(document.querySelectorAll('.filter-checkbox input[type="checkbox"]'));

// Select the button that will pause the mosaics
const becomingsBtn = document.getElementById('becomings-btn');
let isBecomingsBtnTriggered = false;
let isInitialUpdateTriggered = false;

//MOSAICS

// Set the initial position and direction of each mosaic
mosaics.forEach((mosaic) => {
    // Set the initial position of the mosaic
    mosaic.style.left = `${Math.random() * 80}vw`;
    mosaic.style.top = `${Math.random() * 80}vh`;

    // Set the initial direction of the mosaic
    directions.push({
        x: (Math.random() > 0.5 ? 1 : -1) + Math.random() * 0.5 - 0.1,
        y: (Math.random() > 0.5 ? 1 : -1) + Math.random() * 0.5 - 0.1
    });
});

function updateMosaicEventListeners() {
    mosaics.forEach((mosaic) => {
        // Remove existing event listeners
        mosaic.removeEventListener('mouseover', pauseMosaic);
        mosaic.removeEventListener('mouseout', resumeMosaic);

        // Add or remove event listeners based on the state of isBecomingsBtnTriggered
        if (!isBecomingsBtnTriggered) {
            mosaic.addEventListener('mouseover', pauseMosaic);
            mosaic.addEventListener('mouseout', resumeMosaic);
        }

        // Toggle the hover effect class based on the state of isBecomingsBtnTriggered
        if (isBecomingsBtnTriggered) {
            mosaic.classList.add('mosaic-becomingsbtn-effect');
            // Ensure the text within the mosaic is also made visible
            const textElements = mosaic.querySelectorAll('.mosaic a div');
            textElements.forEach(textElement => {
                textElement.style.opacity = '1';
                textElement.style.visibility = 'visible';
            });
        } else {
            mosaic.classList.remove('mosaic-becomingsbtn-effect');
            // Ensure the text within the mosaic is hidden
            const textElements = mosaic.querySelectorAll('.mosaic a div');
            textElements.forEach(textElement => {
                textElement.style.opacity = '0';
                textElement.style.visibility = 'hidden';
            });
        }
    });
}

function pauseMosaic() {
    this.classList.add('paused');
    // Ensure the text within the mosaic is made visible
    const textElements = this.querySelectorAll('.mosaic a div');
    textElements.forEach(textElement => {
        textElement.style.opacity = '1';
        textElement.style.visibility = 'visible';
    });
}

function resumeMosaic() {
    this.classList.remove('paused');
    // Ensure the text within the mosaic is hidden if the button is not pressed
    if (!isBecomingsBtnTriggered) {
        const textElements = this.querySelectorAll('.mosaic a div');
        textElements.forEach(textElement => {
            textElement.style.opacity = '0';
            textElement.style.visibility = 'hidden';
        });
    }
}

function update() {
    mosaics.forEach((mosaic, index) => {
        // Skip this mosaic if it's paused
        if (mosaic.classList.contains('paused')) {
            return;
        }

        // Get current position
        const rect = mosaic.getBoundingClientRect();

        // Check if the mosaic hit an edge and update direction
        if (rect.left < 0 || rect.right > window.innerWidth) {
            directions[index].x *= -1;
        }
        if (rect.top < 0 || rect.bottom > window.innerHeight) {
            directions[index].y *= -1;
        }

        // Update position
        mosaic.style.left = `${rect.left + directions[index].x * speedFactor}px`;
        mosaic.style.top = `${rect.top + directions[index].y * speedFactor}px`;
    });

    // Call update again on the next frame
    requestAnimationFrame(update);
}

// Start the update loop
update();

// Update mosaic event listeners once when the page loads
if (!isInitialUpdateTriggered) {
    isInitialUpdateTriggered = true;
    updateMosaicEventListeners();
}

// Add a click event listener to the button
becomingsBtn.addEventListener('click', function () {
    // Invert bool status of becomingsBtnTriggered
    isBecomingsBtnTriggered = !isBecomingsBtnTriggered;

    // Update the button text based on the new state
    if (isBecomingsBtnTriggered) {
        becomingsBtn.innerHTML = 'Hide Becomings';
    }
    else {
        becomingsBtn.innerHTML = 'Show Becomings';
    }

    // Update mosaic event listeners based on the new state
    updateMosaicEventListeners();

    if (isBecomingsBtnTriggered) {
        rearrangeMosaics();
    }

    // Pause all mosaics by adding the 'paused' class to each mosaic
    mosaics.forEach(mosaic => {
        mosaic.classList.add('paused');
    });

    if (this.classList.contains('resume')) {
        mosaics.forEach(mosaic => {
            mosaic.classList.remove('paused');
        });
        this.classList.remove('resume');
    } else {
        this.classList.add('resume');
    }
});

function rearrangeMosaics() {
    // Define the number of columns and rows for the grid
    const columns = 5; // Adjust based on your design
    const rows = Math.ceil(mosaics.length / columns);

    // Calculate the width and height of each mosaic
    const mosaicWidth = window.innerWidth / columns;
    const mosaicHeight = window.innerHeight / rows * 0.8;

    // Define padding or margin values
    const topPadding = 150; // Adjust this value as needed
    const leftPadding = 20; // Adjust this value as needed

    // Rearrange the mosaics
    mosaics.forEach((mosaic, index) => {
        const column = index % columns;
        const row = Math.floor(index / columns);

        // Calculate the position for the mosaic with padding
        const left = column * mosaicWidth + leftPadding;
        const top = row * mosaicHeight + topPadding;

        // Apply the position
        mosaic.style.left = `${left}px`;
        mosaic.style.top = `${top}px`;
    });
}

//FILTER
function openNav() {
    document.getElementById("mySidebar").style.width = "20rem";
    document.getElementById("main").style.marginLeft = "20rem";
    let btn = document.querySelector('.openbtn');
    btn.classList.add('hide');
}

function closeNav() {
    document.getElementById("mySidebar").style.width = "0";
    document.getElementById("main").style.marginLeft = "0";

    let btn = document.querySelector('.openbtn');
    btn.classList.remove('hide');
}

// Reset all checkboxes when the page loads
window.onload = function () {
    checkboxes.forEach((checkbox) => {
        checkbox.checked = false;
    });
};

// Add a click event listener to each checkbox
checkboxes.forEach((checkbox) => {
    checkbox.addEventListener('click', function () {
        // Get all selected tags
        const selectedTags = checkboxes.filter(checkbox => checkbox.checked).map(checkbox => checkbox.dataset.tag);

        // Filter mosaics
        mosaics.forEach((mosaic) => {
            const mosaicTags = mosaic.dataset.tags.split(',');
            if (selectedTags.length === 0 || selectedTags.some(tag => mosaicTags.includes(tag))) {
                // If no checkboxes are selected or the mosaic has at least one of the selected tags, make it fully visible
                mosaic.style.opacity = '1';
            } else {
                // Otherwise, make it almost transparent
                mosaic.style.opacity = '0.2';
            }
        });
    });
});
