// Get all checkboxes
const checkboxes = Array.from(document.querySelectorAll('.filter-checkbox input[type="checkbox"]'));
const container = document.querySelector('.mosaic-container');
const mosaics = Array.from(container.querySelectorAll('.mosaic'));
const directions = mosaics.map(() => ({
    x: (Math.random() > 0.5 ? 1 : -1) + Math.random() * 0.4,
    y: (Math.random() > 0.5 ? 1 : -1) + Math.random() * 0.4,
}));
const speedFactor = 0.5;
const becomingsBtn = document.getElementById('becomings-btn');
let isBecomingsBtnTriggered = false;
let isInitialUpdateTriggered = false;

// INITIAL SETUP
mosaics.forEach(mosaic => {
    mosaic.style.left = `${Math.random() * 80}vw`;
    mosaic.style.top = `${Math.random() * 80}vh`;
});

function updateMosaicEventListeners(addListeners = true) {
    mosaics.forEach(mosaic => {
        if (addListeners) {
            mosaic.addEventListener('mouseover', pauseMosaic);
            mosaic.addEventListener('mouseout', resumeMosaic);
        } else {
            mosaic.removeEventListener('mouseover', pauseMosaic);
            mosaic.removeEventListener('mouseout', resumeMosaic);
        }
    });
}

function pauseMosaic() {
    this.classList.add('paused');
    toggleMosaicText(true);
}

function resumeMosaic() {
    this.classList.remove('paused');
    toggleMosaicText(false);
}

function toggleMosaicText(show) {
    document.querySelectorAll('.mosaic a div').forEach(textElement => {
        textElement.style.opacity = show ? '1' : '0';
        textElement.style.display = show ? 'block' : 'none';
    });
}

function update() {
    mosaics.forEach((mosaic, index) => {
        if (mosaic.classList.contains('paused')) return;

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

// Update mosaic event listeners once when the page loads
if (!isInitialUpdateTriggered) {
    isInitialUpdateTriggered = true;
    updateMosaicEventListeners();
}

// BecomingButton click event listener
becomingsBtn.addEventListener('click', () => {
    isBecomingsBtnTriggered = !isBecomingsBtnTriggered;
    becomingsBtn.textContent = isBecomingsBtnTriggered ? 'Hide Becomings' : 'Show Becomings';

    updateMosaicEventListeners(!isBecomingsBtnTriggered);

    mosaics.forEach(mosaic => {
        mosaic.classList.toggle('paused', isBecomingsBtnTriggered);
    });

    toggleMosaicText(isBecomingsBtnTriggered);

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
        mosaic.style.position = "unset";
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

//Check cookies and apply css
function applyVisitedMosaics() {
    const visitedMosaics = getCookie('digital-thesis-mosaics');
    if (visitedMosaics) {
        const decodedMosaics = decodeURIComponent(visitedMosaics);
        const visitedList = JSON.parse(decodedMosaics);
        visitedList.forEach(id => {
            const element = document.getElementById(`mosaic-${id}`);
            if (element) {
                element.classList.add('visited');
            } else {
                console.log("Element not found for ID:", id);
            }
        });
    }
}

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

// Ensure the script runs on initial load
document.addEventListener('DOMContentLoaded', function () {
    applyVisitedMosaics();
});

// Ensure the script runs when the user navigates back to the page
window.addEventListener('pageshow', function (event) {
    if (event.persisted) {
        applyVisitedMosaics();
    }
});