function showLoginOverlay() {
    var overlay = document.getElementById('overlay');
    overlay.style.display = "flex";
    document.querySelector('.mosaic-container').classList.add('blur-background');
    hideMosaics();

    // Retrieve username and password from local storage
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
    // Retrieve username and password from form
    var username = document.getElementById('username').value;
    var password = document.getElementById('password').value;

    // Save username and password to local storage if remember me is checked
    var rememberMeCheckbox = document.getElementById('rememberMe');
    if (rememberMeCheckbox.checked) {
        localStorage.setItem('rememberMeUsername', username);
        localStorage.setItem('rememberMePassword', password);
    }
}