// Object to store the status of materials
let materialsStatus = {};

function updateMaterialStatus(checkbox) {
    const index = checkbox.getAttribute('data-index');
    const name = checkbox.getAttribute('data-name');
    materialsStatus[name] = checkbox.checked;
    saveMaterialStatus();
}

function saveMaterialStatus() {
    // Convert the materialsStatus object to a JSON string and set it in the hidden input
    document.getElementById('materialsData').value = JSON.stringify(materialsStatus);

    // Submit the hidden form
    document.getElementById('saveMaterialsForm').submit();
}

document.addEventListener('DOMContentLoaded', function () {
    const fileInput = document.getElementById('files');
    const fileNameDisplay = document.getElementById('files-upload-name');

    if (!fileInput || !fileNameDisplay) {
        console.error("Cannot find element with id \"files\" or \"files-upload-name\"")
    }
    else {
        fileInput.addEventListener('change', function () {
            if (fileInput.files.length > 0) {
                fileNameDisplay.textContent = fileInput.files[0].name;
            } else {
                fileNameDisplay.textContent = 'No file chosen';
            }
        });
    }
});
