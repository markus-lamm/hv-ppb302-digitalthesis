﻿//Mosaics
function hideMosaics() {
    var mosaics = document.querySelectorAll('.mosaic-container .mosaic-float .mosaic');
    mosaics.forEach(function (mosaic) {
        mosaic.style.display = "none";
    });
}

function showMosaics() {
    var mosaics = document.querySelectorAll('.mosaic-container .mosaic-float .mosaic');
    mosaics.forEach(function (mosaic) {
        mosaic.style.display = "";
    });
}

$(document).ready(function () {
    $(".mosaic").click(function () {
        var mosaicId = $(this).attr("id");
        window.location.href = "/Home/Mosaics?mosaicId=" + mosaicId;
    });
});

//Filter sidebar
function openNav() {
    document.getElementById("mySidebar").style.width = "250px";
    document.getElementById("main").style.marginLeft = "250px";
    let btn = document.querySelector('.openbtn');
    btn.classList.add('hide');
}

function closeNav() {
    document.getElementById("mySidebar").style.width = "0";
    document.getElementById("main").style.marginLeft = "0";

    let btn = document.querySelector('.openbtn');
    btn.classList.remove('hide');
}

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
});

function update() {
    mosaics.forEach((mosaic, index) => {
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
        mosaic.style.left = `${rect.left + directions[index].x}px`;
        mosaic.style.top = `${rect.top + directions[index].y}px`;
    });

    // Call update again on the next frame
    requestAnimationFrame(update);
}

// Start the update loop
update();