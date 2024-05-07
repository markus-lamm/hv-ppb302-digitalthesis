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

//MOSAICS

// Get the mosaic container
const container = document.querySelector('.mosaic-container');

// Get all mosaic elements
const mosaics = Array.from(container.querySelectorAll('.mosaic'));

// Create an array to store the direction of each mosaic
const directions = [];

// Set the initial position and direction of each mosaic
mosaics.forEach((mosaic) => {
    // Set the initial position of the mosaic
    mosaic.style.left = `${Math.random() * 100}vw`;
    mosaic.style.top = `${Math.random() * 100}vh`;

    // Set the initial direction of the mosaic
    directions.push({
        x: (Math.random() > 0.5 ? 1 : -1) + Math.random() * 0.5 - 0.1,
        y: (Math.random() > 0.5 ? 1 : -1) + Math.random() * 0.5 - 0.1
    });

    // Pause the animation when the mouse is over the mosaic
    mosaic.addEventListener('mouseover', function () {
        this.classList.add('paused');
    });
    mosaic.addEventListener('mouseout', function () {
        this.classList.remove('paused');
    });
});

// Define a speed factor (smaller values will make the animation slower)
const speedFactor = 0.5;

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

// Get all checkboxes
const checkboxes = Array.from(document.querySelectorAll('.filter-checkbox input[type="checkbox"]'));

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

