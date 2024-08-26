// Mouseover mosaic name
document.addEventListener("DOMContentLoaded", function () {
    const mosaicNameAnchor = document.getElementById('mosaic-name');
    const mosaics = document.querySelectorAll('.mosaic');

    mosaics.forEach(mosaic => {
        mosaic.addEventListener('mouseover', function () {
            const mosaicName = mosaic.getAttribute('data-name');
            mosaicNameAnchor.innerText = mosaicName;
            let rect = mosaic.getBoundingClientRect();
            mosaicNameAnchor.style.display = 'flex';
            mosaicNameAnchor.style.position = 'absolute';
            mosaicNameAnchor.style.top = `${rect.top + window.scrollY}px`;
            mosaicNameAnchor.style.left = `${rect.left + window.scrollX}px`;
        });
        mosaic.addEventListener('mouseout', function () {
            mosaicNameAnchor.style.display = 'none';
        });
    });
});

// Mosaics placements within the kaleidoscope
document.addEventListener("DOMContentLoaded", function () {
    window.onload = function () {
        const kaleidoscopeImageContainer = document.getElementById('kaleidoscope-image-container');
        const mosaics = document.querySelectorAll('.mosaic');
        let maxAttempts = 100; // Maximum number of attempts to find a valid position
        let minRadius = 20; // Minimum radius for mosaics
        let bufferZone = 20; // Buffer zone to ensure circles do not touch
        let restart;
        let lastWidth = window.innerWidth;
        let lastHeight = window.innerHeight;
        const threshold = 100; // Pixel difference to trigger a resize
        const debounceDelay = 200; // Delay in milliseconds

        // Debounce function to delay the execution
        function debounce(func, delay) {
            let timeout;
            return function (...args) {
                clearTimeout(timeout);
                timeout = setTimeout(() => func.apply(this, args), delay);
            };
        }

        // Event listener to replace mosaics at resized window
        window.addEventListener('resize', debounce(function () {
            const currentWidth = window.innerWidth;
            const currentHeight = window.innerHeight;

            // Calculate the change in width and height
            const widthChange = Math.abs(currentWidth - lastWidth);
            const heightChange = Math.abs(currentHeight - lastHeight);

            // Check if the change exceeds the threshold
            if (widthChange > threshold || heightChange > threshold) {
                placeMosaics();
                // Update the last known dimensions
                lastWidth = currentWidth;
                lastHeight = currentHeight;
            }
        }, debounceDelay));

        placeMosaics();

        // Function to place mosaics
        function placeMosaics() {
            let occupiedPositions = []; // Array to store occupied positions

            mosaics.forEach(function (mosaics) {
                let isValidPosition = false;
                let attempts = 0;
                let mosaicRadius = mosaics.offsetWidth / 2;

                while (!isValidPosition && attempts < maxAttempts) {
                    // Generate random position for mosaic within the bounds of the kaleidoscope background
                    let maxDistance = Math.min(kaleidoscopeImageContainer.offsetWidth, kaleidoscopeImageContainer.offsetHeight) / 2 - mosaicRadius;
                    let randomDistance = Math.random() * maxDistance;
                    let randomAngle = Math.random() * 2 * Math.PI;
                    var randomX = Math.cos(randomAngle) * randomDistance;
                    var randomY = Math.sin(randomAngle) * randomDistance;

                    // Check if the new position collides with any existing mosaic
                    let collides = occupiedPositions.some(function (position) {
                        let distance = Math.sqrt(Math.pow(randomX - position.x, 2) + Math.pow(randomY - position.y, 2));
                        return distance < mosaicRadius + minRadius + bufferZone; // Include buffer zone
                    });

                    // If collision detected, reset position and try again; otherwise, mark position as valid
                    if (collides) {
                        randomX = 0; // Reset X position
                        randomY = 0; // Reset Y position
                        attempts++;
                    } else {
                        isValidPosition = true;
                        occupiedPositions.push({ x: randomX, y: randomY }); // Store the new position
                    }
                }

                // Set position of the small circle
                if (isValidPosition) {
                    mosaics.style.top = kaleidoscopeImageContainer.offsetHeight / 2 + randomY - mosaicRadius + 'px';
                    mosaics.style.left = kaleidoscopeImageContainer.offsetWidth / 2 + randomX - mosaicRadius + 'px';
                } else {
                    restart = true;
                }
            });
            if (restart) {
                if (bufferZone != 0) {
                    bufferZone = bufferZone - 5;
                }
                restart = false;
                placeMosaics();
            }
        }

    };
});

// Radio buttons for kaleidoscope filters
document.querySelectorAll('.custom-radio').forEach(function (radio) {
    radio.addEventListener('change', function () {
        let selectedTag = this.getAttribute('data-tag');
        const images = document.querySelectorAll('.mosaic');
        images.forEach(function (image) {
            let tags = image.getAttribute('data-tags').split(':');
            let kaleidoscopeTags = tags[0].split(',');
            let assemblageTag = tags[1].split(',');

            // Reset the hue value
            image.style.filter = 'hue-rotate(0deg)';

            if (selectedTag === 'Assemblages') {
                image.style.opacity = 1;
                image.classList.remove('mosaic-highlight-effect');
                assemblageTag.forEach(function (tag) {
                    let randomValue = assignAssemblageTags(tag);
                    image.style.filter = 'hue-rotate(' + randomValue + 'deg)';
                });
            }
            else if (selectedTag === 'Experiment') {
                // Create a random value between 1 and 3
                let randomValue = 3;
                let random = Math.floor(Math.random() * randomValue) + 1;

                // If the random value is 1, set the opacity to 1 and add the highlight effect class
                if (random === 1) {
                    image.style.opacity = 1;
                    image.classList.add('mosaic-highlight-effect');
                } else {
                    image.style.opacity = 0.4;
                    image.classList.remove('mosaic-highlight-effect');
                }
            }
            else if (kaleidoscopeTags.includes(selectedTag)) {
                image.style.opacity = 1; // Set full opacity for matching tags
                image.classList.add('mosaic-highlight-effect'); // Add the highlight effect class
            } else {
                image.style.opacity = 0.4; // Set lower opacity for non-matching tags
                image.classList.remove('mosaic-highlight-effect'); // Remove the highlight effect class
            }
        });
    });
});

// Dictionary to store the tags for each assemblage and a separate value
let assemblageTagsDictionary = {};

function assignAssemblageTags(tag) {
    // If the assemblage is not already assigned, assign a random value
    if (!assemblageTagsDictionary[tag]) {
        // Generate multiple random values and select the least similar one
        let randomValue = getLeastSimilarValue(Object.values(assemblageTagsDictionary));
        assemblageTagsDictionary[tag] = randomValue;
    }
    return assemblageTagsDictionary[tag];
}

function getLeastSimilarValue(existingValues, count = 5) {
    let candidates = [];
    for (let i = 0; i < count; i++) {
        let randomValue = Math.floor(Math.random() * 361);
        // Check if the new value is sufficiently different from all existing ones
        let isUnique = true;
        for (let value of existingValues) {
            if (Math.abs(randomValue - value) <= 20) { // Threshold of 20 degrees for similarity
                isUnique = false;
                break;
            }
        }
        if (isUnique) {
            candidates.push(randomValue);
        }
    }
    // Return the first candidate that passes the uniqueness check
    return candidates[0];
}

// Kaleidoscope image and mosaics rotation
document.addEventListener("DOMContentLoaded", function () {
    const radios = document.querySelectorAll('.custom-radio');
    const kaleidoscopeContainer = document.getElementById('kaleidoscope-container');
    const mosaics = document.querySelectorAll('.mosaic');

    radios.forEach(radio => {
        radio.addEventListener('change', function () {
            let ksContainerRotation = Math.floor(Math.random() * 180) - 90;
            kaleidoscopeContainer.style.transform = `rotate(${ksContainerRotation}deg)`;

            const ksObject1 = document.getElementById('kaleidoscope-image-object-1');
            const ksObject2 = document.getElementById('kaleidoscope-image-object-2');
            const ksObject3 = document.getElementById('kaleidoscope-image-object-3');
            const ksObject4 = document.getElementById('kaleidoscope-image-object-4');
            const ksObject5 = document.getElementById('kaleidoscope-image-object-5');
            const ksObject6 = document.getElementById('kaleidoscope-image-object-6');
            const ksObject7 = document.getElementById('kaleidoscope-image-object-7');

            const elementsToRotate = [
                ksObject1,
                ksObject3,
                ksObject5,
                ksObject7
            ];
            const elementsToRotateInverted = [
                ksObject2,
                ksObject4,
                ksObject6
            ];

            // Rotate all uneven elements clockwise and all even elements counter-clockwise
            elementsToRotate.forEach(element => {
                let rotation = Math.floor(Math.random() * 180);
                element.style.transform = `rotate(${rotation}deg)`;
                element.style.opacity = 0.5;
            });
            elementsToRotateInverted.forEach(element => {
                let rotation = Math.floor(Math.random() * 180 * -1);
                element.style.transform = `rotate(${rotation}deg)`;
                element.style.opacity = 0.5;
            });

            // Randomly select one element from each array to have an opacity of 1
            let randomIndex = Math.floor(Math.random() * elementsToRotate.length);
            elementsToRotate[randomIndex].style.opacity = 1;
            let randomIndexInverted = Math.floor(Math.random() * elementsToRotateInverted.length);
            elementsToRotateInverted[randomIndexInverted].style.opacity = 1;

            mosaics.forEach((mosaic) => {
                let rotation = Math.floor(Math.random() * 180) - 90;
                mosaic.style.transform = `rotate(${rotation}deg)`;
            });

            // Apply an image filter to change the appereance of the kaleidoscope images
            const imagefilter = ["kaleidoscope-filter-1", "kaleidoscope-filter-2", "kaleidoscope-filter-3", "kaleidoscope-filter-4"]
            let appliedFilter = Array.from(kaleidoscopeContainer.classList).find(className => imagefilter.includes(className));
            let availableFilters = imagefilter.filter(className => className !== appliedFilter);
            let randomFilter = availableFilters[Math.floor(Math.random() * availableFilters.length)];
            kaleidoscopeContainer.classList.remove(...imagefilter);
            kaleidoscopeContainer.classList.add(randomFilter);
        });
    });
});
