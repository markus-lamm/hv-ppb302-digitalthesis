document.addEventListener("DOMContentLoaded", function () {
    const radios = document.querySelectorAll('.custom-radio');
    var bigImage = document.getElementById('biggi');
    var smallCircles = document.querySelectorAll('.smallCircle');

    radios.forEach(radio => {
        radio.addEventListener('change', function () {
            // Get the current rotation value (if any)
            const currentRotation = parseInt(bigImage.style.transform.replace('rotate(', '').replace('deg)', ''), 10) || 0;
            const currentRotationsm = parseInt(smallCircles[0].style.transform.replace('rotate(', '').replace('deg)', ''), 10) || 0;
            // Add 90 degrees to the current rotation
            const newRotation = currentRotation + 90;
            const newRotationsm = currentRotationsm + 90;
            // Apply the new rotation
            smallCircles.forEach((smallCircle, index) => {
                smallCircle.style.transform = `rotate(${newRotationsm}deg)`;
            });
            bigImage.style.transform = `rotate(${newRotation}deg)`;

            const imagefilter = ["filterbigimage", "filterbigimage2", "filterbigimage3", "filterbigimage4"]

            const appliedFilter = Array.from(bigImage.classList).find(className => imagefilter.includes(className));

            const availableFilters = imagefilter.filter(className => className !== appliedFilter);

            const randomFilter = availableFilters[Math.floor(Math.random() * availableFilters.length)];

            bigImage.classList.remove(...imagefilter);

            bigImage.classList.add(randomFilter);
        });
    });
});

// Add event listeners to radio buttons
document.querySelectorAll('.custom-radio').forEach(function (radio) {
    radio.addEventListener('change', function () {
        var selectedTag = this.getAttribute('data-tag');
        var images = document.querySelectorAll('.smallCircle');
        images.forEach(function (image) {
            var tags = image.getAttribute('data-tags').split(',');
            if (tags.includes(selectedTag)) {
                image.style.opacity = 1; // Set full opacity for matching tags
            } else {
                image.style.opacity = 0.5; // Set lower opacity for non-matching tags
            }
        });
    });
});

document.addEventListener("DOMContentLoaded", function () {
    window.onload = function () {
        var roulette = document.querySelector('.roulette');
        var rouletteimage = document.querySelector('.rouletteimage');
        var bigImage = document.getElementById('bigImage');
        var smallCircles = document.querySelectorAll('.smallCircle');

        var bigImageWidth = bigImage.offsetWidth;
        var bigImageHeight = bigImage.offsetHeight;

        var maxAttempts = 100; // Maximum number of attempts to find a valid position
        var minRadius = 20; // Minimum radius for small circles
        var bufferZone = 20; // Buffer zone to ensure circles do not touch
        var restart;

        // Function to place small circles
        function placeSmallCircles() {
            var occupiedPositions = []; // Array to store occupied positions
            
            smallCircles.forEach(function (smallCircle) {
                var isValidPosition = false;
                var attempts = 0;
                var smallCircleRadius = smallCircle.offsetWidth / 2;

                while (!isValidPosition && attempts < maxAttempts) {
                    // Generate random position for small circle within the bounds of the larger circle
                    var maxDistance = Math.min(bigImageWidth, bigImageHeight) / 2 - smallCircleRadius;
                    var randomDistance = Math.random() * maxDistance;
                    var randomAngle = Math.random() * 2 * Math.PI;
                    var randomX = Math.cos(randomAngle) * randomDistance;
                    var randomY = Math.sin(randomAngle) * randomDistance;

                    // Check if the new position collides with any existing small circle
                    var collides = occupiedPositions.some(function (position) {
                        var distance = Math.sqrt(Math.pow(randomX - position.x, 2) + Math.pow(randomY - position.y, 2));
                        return distance < smallCircleRadius + minRadius + bufferZone; // Include buffer zone
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
                    smallCircle.style.top = bigImageHeight / 2 + randomY - smallCircleRadius + 'px';
                    smallCircle.style.left = bigImageWidth / 2 + randomX - smallCircleRadius + 'px';
                } else {
                    restart = true;
                }
            });
        }

        // Initial placement of small circles
        placeSmallCircles();
        if (restart) {
            var bufferZone = 0;
            smallCircles.forEach(function (circle) {
                circle.style.width = 3 + 'rem';
                circle.style.height = 3 + 'rem';
            });
            placeSmallCircles();
        }
    };
});
