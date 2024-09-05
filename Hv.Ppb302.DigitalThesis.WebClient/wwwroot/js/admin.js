document.addEventListener("DOMContentLoaded", () => {
    // Object to store the status of materials
    const materialsStatus = {};

    // Triggered from Files.cshtml with an onchange()
    function updateMaterialStatus(checkbox) {
        const index = checkbox.getAttribute("data-index");
        const name = checkbox.getAttribute("data-name");
        materialsStatus[name] = checkbox.checked;
        saveMaterialStatus();
    }

    function saveMaterialStatus() {
        const materialsDataInput = document.querySelector("#materialsData");
        const saveMaterialsForm = document.querySelector("#saveMaterialsForm");

        if (materialsDataInput && saveMaterialsForm) {
            materialsDataInput.value = JSON.stringify(materialsStatus);
            saveMaterialsForm.submit();
        } else {
            console.error("Cannot find element with id \"materialsData\" or \"saveMaterialsForm\"");
        }
    }

    function initializeFileInput() {
        const fileInput = document.querySelector("#files");
        const fileNameDisplay = document.querySelector("#files-upload-name");

        if (!fileInput || !fileNameDisplay) {
            console.error("Cannot find element with id \"files\" or \"files-upload-name\"");
            return;
        }

        fileInput.addEventListener("change", () => {
            fileNameDisplay.textContent = fileInput.files.length > 0 ? fileInput.files[0].name : "No file chosen";
        });
    }

    // Initialize the file input event listener
    initializeFileInput();
});
