document.addEventListener("DOMContentLoaded", function () {
    const radios = document.querySelectorAll('.custom-radio');
    var kaleidoscopeContainerItem = document.getElementById('kaleidoscope-container-item');
    var mosaics = document.querySelectorAll('.mosaic');

    radios.forEach(radio => {
        radio.addEventListener('change', function () {
            // Get the current rotation value (if any)
            const currentRotation = parseInt(kaleidoscopeContainerItem.style.transform.replace('rotate(', '').replace('deg)', ''), 10) || 0;
            const currentRotationsm = parseInt(mosaics[0].style.transform.replace('rotate(', '').replace('deg)', ''), 10) || 0;
            // Add 90 degrees to the current rotation
            const newRotation = currentRotation + 90;
            const newRotationsm = currentRotationsm + 90;
            // Apply the new rotation
            mosaics.forEach((mosaic, index) => {
                mosaic.style.transform = `rotate(${newRotationsm}deg)`;
            });
            kaleidoscopeContainerItem.style.transform = `rotate(${newRotation}deg)`;

            const imagefilter = ["kaleidoscope-filter-1", "kaleidoscope-filter-2", "kaleidoscope-filter-3", "kaleidoscope-filter-4"]

            const appliedFilter = Array.from(kaleidoscopeContainerItem.classList).find(className => imagefilter.includes(className));

            const availableFilters = imagefilter.filter(className => className !== appliedFilter);

            const randomFilter = availableFilters[Math.floor(Math.random() * availableFilters.length)];

            kaleidoscopeContainerItem.classList.remove(...imagefilter);

            kaleidoscopeContainerItem.classList.add(randomFilter);
        });
    });
});

// Dictionary to store the tags for each assemblage and a separate value
var assemblageTagsDictionary = {};

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

function assignAssemblageTags(tag) {
    // If the assemblage is not already assigned, assign a random value
    if (!assemblageTagsDictionary[tag]) {
        // Generate multiple random values and select the least similar one
        var randomValue = getLeastSimilarValue(Object.values(assemblageTagsDictionary));
        assemblageTagsDictionary[tag] = randomValue;
    }
    return assemblageTagsDictionary[tag];
}

// Add event listeners to radio buttons
document.querySelectorAll('.custom-radio').forEach(function (radio) {
    radio.addEventListener('change', function () {
        var selectedTag = this.getAttribute('data-tag');
        var images = document.querySelectorAll('.mosaic');
        images.forEach(function (image) {
            var tags = image.getAttribute('data-tags').split(':');
            var kaleidoscopeTags = tags[0].split(',');
            var assemblageTag = tags[1].split(',');

            // Reset the hue value
            image.style.filter = 'hue-rotate(0deg)';

            if (selectedTag === 'Assemblages') {
                image.style.opacity = 1;
                image.classList.remove('mosaic-highlight-effect');
                assemblageTag.forEach(function (tag) {
                    var randomValue = assignAssemblageTags(tag);
                    image.style.filter = 'hue-rotate(' + randomValue + 'deg)';
                });
            }
            else if (selectedTag === 'Experiment') {
                // Create a random value between 1 and 3
                const randomValue = 3;
                var random = Math.floor(Math.random() * randomValue) + 1;

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

var kaleidoscopeImageItem = document.getElementById('kaleidoscope-image-item');
if (kaleidoscopeImageItem) {
    document.addEventListener("DOMContentLoaded", function () {
        window.onload = function () {
            var kaleidoscopeContainer = document.querySelector('.kaleidoscope-container');
            var kaleidoscopeImage = document.querySelector('.kaleidoscope-image');
            var kaleidoscopeImageItem = document.getElementById('kaleidoscope-image-item');
            var mosaics = document.querySelectorAll('.mosaic');

            var kaleidoscopeImageItemWidth = kaleidoscopeImageItem.offsetWidth;
            var kaleidoscopeImageItemHeight = kaleidoscopeImageItem.offsetHeight;

            var maxAttempts = 100; // Maximum number of attempts to find a valid position
            var minRadius = 20; // Minimum radius for mosaics
            var bufferZone = 20; // Buffer zone to ensure circles do not touch
            var restart;

            // Function to place mosaics
            function placeMosaics() {
                var occupiedPositions = []; // Array to store occupied positions

                mosaics.forEach(function (mosaics) {
                    var isValidPosition = false;
                    var attempts = 0;
                    var mosaicRadius = mosaics.offsetWidth / 2;

                    while (!isValidPosition && attempts < maxAttempts) {
                        // Generate random position for mosaic within the bounds of the kaleidoscope background
                        var maxDistance = Math.min(kaleidoscopeImageItemWidth, kaleidoscopeImageItemHeight) / 2 - mosaicRadius;
                        var randomDistance = Math.random() * maxDistance;
                        var randomAngle = Math.random() * 2 * Math.PI;
                        var randomX = Math.cos(randomAngle) * randomDistance;
                        var randomY = Math.sin(randomAngle) * randomDistance;

                        // Check if the new position collides with any existing mosaic
                        var collides = occupiedPositions.some(function (position) {
                            var distance = Math.sqrt(Math.pow(randomX - position.x, 2) + Math.pow(randomY - position.y, 2));
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
                        mosaics.style.top = kaleidoscopeImageItemHeight / 2 + randomY - mosaicRadius + 'px';
                        mosaics.style.left = kaleidoscopeImageItemWidth / 2 + randomX - mosaicRadius + 'px';
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

            // Initial placement of mosaics
            placeMosaics();
            //if (restart) {
            //    var bufferZone = 0;
            //    restart = false;
            //    placeSmallCircles();
            //}
        };
    });
}


