
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


//For materialOrder
const div = document.getElementById("files-div");
const sorting = Sortable.create(div, {
    onEnd: function (evt) {
        const newOrder = getSortedOrder();
        var orderJson = JSON.stringify(newOrder);

        document.getElementById('file-orders').value = orderJson;
        document.getElementById('order-form').submit();
    }
});

function getSortedOrder() {
    var order = [];
    document.querySelectorAll('.file-item-container').forEach(function (item, index) {
        var name = item.getAttribute('data-name'); 
        order.push({ name: name, order: index + 1 });
    });
    return order;
}
    

    

