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
        const mosaicContent = mosaic.querySelector('.mosaic-content');
        const floatcontainer = document.querySelector('.mosaic-float');

        if (addListeners) {
            mosaic.classList.add('mosaic-position');
            mosaicContent.classList.remove('active');
            container.style.overflow = 'hidden';
            container.style.position = 'fixed';
            mosaic.addEventListener('mouseenter', pauseMosaic);
            mosaic.addEventListener('mouseleave', resumeMosaic);
            floatcontainer.style.marginInline = '0';
        } else {
            mosaic.removeEventListener('mouseenter', pauseMosaic);
            mosaic.removeEventListener('mouseleave', resumeMosaic);
            mosaic.addEventListener('mouseenter', (event) => {
                toggleMosaicText(true);
                event.currentTarget.style.zIndex = '100';
            });

            mosaic.addEventListener('mouseleave', (event) => {
                toggleMosaicText(false);
                event.currentTarget.style.zIndex = '0';
            });
            floatcontainer.style.marginInline = '10rem';
            mosaicContent.classList.add('active');
            container.style.overflow = 'unset';
            container.style.position = 'unset';
            mosaic.classList.remove('mosaic-position');
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
becomingsBtn.addEventListener('change', () => {
    const becomingsText = becomingsBtn.querySelector('.becomings-text');
    isBecomingsBtnTriggered = !isBecomingsBtnTriggered;
    //becomingsText.textContent = isBecomingsBtnTriggered ? 'Hide Becomings' : 'Show Becomings';

    updateMosaicEventListeners(!isBecomingsBtnTriggered);

    mosaics.forEach(mosaic => {
        mosaic.classList.toggle('paused', isBecomingsBtnTriggered);
    });


});

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
    becomingsBtn.checked = false;
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
            const elementen = document.getElementById(`mosaic-${id}`);
            const element = elementen.querySelector('.mosaicImg');
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