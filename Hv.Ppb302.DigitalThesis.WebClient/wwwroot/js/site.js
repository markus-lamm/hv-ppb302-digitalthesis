// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Login modal
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
function showLoginOverlay() {
    var overlay = document.getElementById('overlay');
    overlay.style.display = "flex";
    document.querySelector('.mosaic-container').classList.add('blur-background');
    hideMosaics();

    // Hämta användarnamn och lösenord från lokal lagring (om tillgängligt) och fyll i formuläret
    var storedUsername = localStorage.getItem('rememberMeUsername');
    var storedPassword = localStorage.getItem('rememberMePassword');

    if (storedUsername && storedPassword) {
        document.getElementById('username').value = storedUsername;
        document.getElementById('password').value = storedPassword;
    }
}

function hideLoginOverlay() {
    var overlay = document.getElementById('overlay');
    overlay.style.display = "none";
    document.querySelector('.mosaic-container').classList.remove('blur-background');
    showMosaics();
}

function validateLoginForm() {
    // Hämta användarnamn och lösenord från formuläret
    var username = document.getElementById('username').value;
    var password = document.getElementById('password').value;

    // Sparar användarnamn och lösenord i lokal lagring om "Remember me" är markerad
    var rememberMeCheckbox = document.getElementById('rememberMe');
    if (rememberMeCheckbox.checked) {
        localStorage.setItem('rememberMeUsername', username);
        localStorage.setItem('rememberMePassword', password);
    }
}

//Filter sidebar

function openNav() {
    document.getElementById("mySidebar").style.width = "250px";
    document.getElementById("main").style.marginLeft = "250px";
    let btn = document.querySelector('.openbtn');
    btn.classList.add('hide');
}

/* Set the width of the sidebar to 0 and the left margin of the page content to 0 */
function closeNav() {
    document.getElementById("mySidebar").style.width = "0";
    document.getElementById("main").style.marginLeft = "0";

    let btn = document.querySelector('.openbtn');
    btn.classList.remove('hide');
}

//Caleidoscoping

function caleidoscoping(e) {
    const caleidoscopes = document.querySelectorAll(".mosaic-float .mosaic");
    let filter = e.target.dataset.filter;
    caleidoscopes.forEach(caleidoscope => {

        if (caleidoscope.classList.contains(filter)) {
            caleidoscope.classList.remove('hidden');
        }
        else if (filter == "clear") {
            caleidoscope.classList.remove('hidden');
        }
        else {
            caleidoscope.classList.add('hidden');
        }
    });
};



// Klickhanteringslogik för mosaic-objekten
$(document).ready(function () {
    $(".mosaic").click(function () {
        var mosaicId = $(this).attr("id");
        window.location.href = "/Home/Mosaics?mosaicId=" + mosaicId;
    });
});


var playButton = document.getElementById('play-button');
var audioPlayer = document.getElementById('audio-player');


playButton.addEventListener('click', function () {
    playButton.classList.add('clicked');
    setTimeout(function () {
        playButton.classList.remove('clicked');
    }, 200);
    audioPlayer.play();
});
