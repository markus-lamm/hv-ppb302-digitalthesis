// Object to store the status of materials
var materialsStatus = {};

function updateMaterialStatus(checkbox) {
    var index = checkbox.getAttribute('data-index');
    var name = checkbox.getAttribute('data-name');
    materialsStatus[name] = checkbox.checked;
}

function saveMaterialStatus() {
    // Convert the materialsStatus object to a JSON string and set it in the hidden input
    document.getElementById('materialsData').value = JSON.stringify(materialsStatus);

    // Submit the hidden form
    document.getElementById('saveMaterialsForm').submit();
}