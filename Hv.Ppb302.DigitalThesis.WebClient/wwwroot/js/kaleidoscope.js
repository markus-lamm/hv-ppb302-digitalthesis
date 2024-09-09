document.addEventListener("DOMContentLoaded", () => {
    // File-scope variables
    const kaleiImgContainer = document.querySelector("#kaleidoscope-image-container");
    const kaleiModalOpen = document.querySelector("#kalei-modal-open");
    const mosaics = document.querySelectorAll(".mosaic");

    // KALEIDOSCOPE MODAL
    const kaleiModal = document.querySelector("#kalei-modal");
    const kaleiModalClose = document.querySelector(".kalei-modal-close");
    const tagmenuBtn = document.querySelector("#tagmenu-btn");

    function showModal() {
        kaleiModal.style.display = "block";
    }

    function hideModal() {
        kaleiModal.style.display = "none";
    }

    function resetModal() {
        kaleiModal.style.display = "block";
        editor.value = tagmenuBtn.getAttribute("data-content");
    }

    // Display the intro modal conditionally
    if (!sessionStorage.getItem("kaleidoscopeModalShown")) {
        showModal();
        sessionStorage.setItem("kaleidoscopeModalShown", "true");
    }

    // Open the modal
    kaleiModalOpen.addEventListener("click", showModal);

    // Close the modal
    kaleiModalClose.addEventListener("click", hideModal);

    // Reopen the modal with the kaleidoscope intro content
    tagmenuBtn.addEventListener("click", resetModal);

    // Close the modal when the user clicks outside it
    window.addEventListener("click", function (event) {
        if (event.target === kaleiModal) {
            hideModal();
        }
    });

    // MOSAIC NAME HOVER
    const mosaicNameAnchor = document.querySelector("#mosaic-name");

    // Function to show the mosaic name anchor
    function showMosaicName(event) {
        if (event.target.classList.contains("mosaic")) {
            const mosaic = event.target;
            const mosaicName = mosaic.getAttribute("data-name");
            const mosaicBecoming = mosaic.getAttribute("data-becomings").split(",");
            const rect = mosaic.getBoundingClientRect();

            mosaicNameAnchor.innerHTML = `<h3>${mosaicName}</h3>${mosaicBecoming.join("<br>")}`;
            mosaicNameAnchor.style.display = "flex";
            mosaicNameAnchor.style.top = `${rect.top + window.scrollY}px`;
            mosaicNameAnchor.style.left = `${rect.left + rect.width + window.scrollX}px`;
        }
    }

    // Function to hide the mosaic name anchor
    function hideMosaicName(event) {
        if (event.target.classList.contains("mosaic")) {
            mosaicNameAnchor.style.display = "none";
        }
    }

    // Attach event listeners to show and hide the mosaic name anchor
    kaleiImgContainer.addEventListener("mouseover", showMosaicName);
    kaleiImgContainer.addEventListener("mouseout", hideMosaicName);

    // MOSAIC PLACEMENT
    const maxAttempts = 100; // Maximum number of attempts to find a valid position
    const minRadius = 20; // Minimum radius for mosaics
    let bufferZone = 20; // Buffer zone to ensure circles do not touch
    let restart;
    let lastWidth = window.innerWidth;
    let lastHeight = window.innerHeight;
    const threshold = 100; // Pixel difference to trigger a resize

    // Function to place mosaics
    function placeMosaics() {
        const occupiedPositions = []; // Array to store occupied positions
        const containerWidth = kaleiImgContainer.offsetWidth;
        const containerHeight = kaleiImgContainer.offsetHeight;
        const maxDistance = Math.min(containerWidth, containerHeight) / 2;

        mosaics.forEach(mosaic => {
            let isValidPosition = false;
            let attempts = 0;
            const mosaicRadius = mosaic.offsetWidth / 2;

            while (!isValidPosition && attempts < maxAttempts) {
                const randomDistance = Math.random() * (maxDistance - mosaicRadius);
                const randomAngle = Math.random() * 2 * Math.PI;
                const randomX = Math.cos(randomAngle) * randomDistance;
                const randomY = Math.sin(randomAngle) * randomDistance;

                // Check if the new position collides with any existing mosaic
                const collides = occupiedPositions.some(position => {
                    const distance = Math.hypot(randomX - position.x, randomY - position.y);
                    return distance < mosaicRadius + minRadius + bufferZone; // Include buffer zone
                });

                if (!collides) {
                    isValidPosition = true;
                    occupiedPositions.push({ x: randomX, y: randomY }); // Store the new position
                    mosaic.style.top = `${containerHeight / 2 + randomY - mosaicRadius}px`;
                    mosaic.style.left = `${containerWidth / 2 + randomX - mosaicRadius}px`;
                } else {
                    attempts++;
                }
            }

            if (!isValidPosition) {
                restart = true;
            }
        });

        if (restart) {
            if (bufferZone > 0) {
                bufferZone -= 5;
            }
            restart = false;
            placeMosaics();
        }
    }

    // Handle window resize with requestAnimationFrame
    function handleResize() {
        const currentWidth = window.innerWidth;
        const currentHeight = window.innerHeight;

        if (Math.abs(currentWidth - lastWidth) > threshold || Math.abs(currentHeight - lastHeight) > threshold) {
            placeMosaics();
            lastWidth = currentWidth;
            lastHeight = currentHeight;
        }
    }

    window.addEventListener("resize", () => {
        requestAnimationFrame(handleResize);
    });

    // Initial placement of mosaics
    requestAnimationFrame(placeMosaics);

    // KALEIDOSCOPE TAGS RADIO BUTTONS
    document.querySelectorAll(".custom-radio").forEach(radio => {
        radio.addEventListener("change", function () {
            // Move the kalei-modal-open div when a radio button is pressed
            const parentContainer = this.closest(".kaleiitem-container");
            if (parentContainer && kaleiModalOpen) {
                parentContainer.appendChild(kaleiModalOpen);
                kaleiModalOpen.style.display = "flex"; // Ensure the div is visible
            }

            const selectedTagId = this.getAttribute("data-tag");
            const selectedTagContent = this.getAttribute("data-content");
            editor.value = selectedTagContent;

            document.querySelectorAll(".mosaic").forEach(image => {
                const [kaleidoscopeTags, assemblageTags] = image.getAttribute("data-tags").split(":").map(tags => tags.split(","));

                // Reset the hue value and pointer events
                image.style.filter = "hue-rotate(0deg)";
                image.style.pointerEvents = "auto";

                if (selectedTagId === "f2d2a02b-73bb-42e4-8774-2102ef9c3102") { // Assemblages
                    image.style.opacity = 1;
                    image.classList.remove("mosaic-highlight-effect");
                    assemblageTags.forEach(tag => {
                        const randomValue = assignAssemblageTags(tag);
                        image.style.filter = `hue-rotate(${randomValue}deg)`;
                    });
                } else if (selectedTagId === "1ac2b7b1-c3bf-4fc3-a5fb-37c88eeb1e97") { // Experiment
                    const random = Math.floor(Math.random() * 3) + 1;
                    if (random === 1) {
                        image.style.opacity = 1;
                        image.classList.add("mosaic-highlight-effect");
                    } else {
                        image.style.pointerEvents = "none";
                        image.style.opacity = 0.4;
                        image.classList.remove("mosaic-highlight-effect");
                    }
                } else if (kaleidoscopeTags.includes(selectedTagId)) {
                    image.style.opacity = 1;
                    image.classList.add("mosaic-highlight-effect");
                } else {
                    image.style.pointerEvents = "none";
                    image.style.opacity = 0.4;
                    image.classList.remove("mosaic-highlight-effect");
                }
            });
        });
    });

    // Dictionary to store the tags for each assemblage and a separate value
    const assemblageTagsDictionary = new Map();

    function assignAssemblageTags(tag) {
        // If the assemblage is not already assigned, assign a random value
        if (!assemblageTagsDictionary.has(tag)) {
            const randomValue = getLeastSimilarValue([...assemblageTagsDictionary.values()]);
            assemblageTagsDictionary.set(tag, randomValue);
        }
        return assemblageTagsDictionary.get(tag);
    }

    function getLeastSimilarValue(existingValues, count = 5) {
        const candidates = Array.from({ length: count }, () => Math.floor(Math.random() * 361));
        const threshold = 20; // Threshold of 20 degrees for similarity

        for (const candidate of candidates) {
            if (existingValues.every(value => Math.abs(candidate - value) > threshold)) {
                return candidate;
            }
        }

        // If no unique candidate is found, return the first candidate
        return candidates[0];
    }

    // OBJECT ROTATIONS
    const radios = document.querySelectorAll(".custom-radio");
    const kaleidoscopeContainer = document.querySelector("#kaleidoscope-container");
    const ksObjects = [
        document.querySelector("#kaleidoscope-image-object-1"),
        document.querySelector("#kaleidoscope-image-object-2"),
        document.querySelector("#kaleidoscope-image-object-3"),
        document.querySelector("#kaleidoscope-image-object-4"),
        document.querySelector("#kaleidoscope-image-object-5"),
        document.querySelector("#kaleidoscope-image-object-6"),
        document.querySelector("#kaleidoscope-image-object-7")
    ];
    const elementsToRotate = [ksObjects[0], ksObjects[2], ksObjects[4], ksObjects[6]];
    const elementsToRotateInverted = [ksObjects[1], ksObjects[3], ksObjects[5]];

    // Add event listeners for rotation on each radio button
    radios.forEach(radio => {
        radio.addEventListener("change", () => {
            const ksContainerRotation = Math.floor(Math.random() * 180) - 90;
            kaleidoscopeContainer.style.transform = `rotate(${ksContainerRotation}deg)`;

            rotateElements(elementsToRotate, 180, 0.5);
            rotateElements(elementsToRotateInverted, -180, 0.5);

            // Randomly select one element from each array to have an opacity of 1
            setRandomOpacity(elementsToRotate, 1);
            setRandomOpacity(elementsToRotateInverted, 1);

            mosaics.forEach(mosaic => {
                const rotation = Math.floor(Math.random() * 180) - 90;
                mosaic.style.transform = `rotate(${rotation}deg)`;
            });

            applyRandomFilter(kaleidoscopeContainer, [
                "kaleidoscope-filter-1", "kaleidoscope-filter-2", "kaleidoscope-filter-3", "kaleidoscope-filter-4"
            ]);
        });
    });

    function rotateElements(elements, maxRotation, opacity) {
        elements.forEach(element => {
            const rotation = Math.floor(Math.random() * maxRotation);
            element.style.transform = `rotate(${rotation}deg)`;
            element.style.opacity = opacity;
        });
    }

    function setRandomOpacity(elements, opacity) {
        const randomIndex = Math.floor(Math.random() * elements.length);
        elements[randomIndex].style.opacity = opacity;
    }

    function applyRandomFilter(container, filters) {
        const appliedFilter = Array.from(container.classList).find(className => filters.includes(className));
        const availableFilters = filters.filter(className => className !== appliedFilter);
        const randomFilter = availableFilters[Math.floor(Math.random() * availableFilters.length)];
        container.classList.remove(...filters);
        container.classList.add(randomFilter);
    }
});
