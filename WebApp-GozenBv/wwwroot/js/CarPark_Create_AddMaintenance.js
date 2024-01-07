//Pass the maintenances into the provided string in the viewModel as JSON string
document.getElementById('btnSubmit').addEventListener('click', function () {
    var result = document.getElementById('objCarMaintenances');
    var rows = document.getElementById('maintenancesContainer').querySelectorAll('.maintenanceRow');
    var maintenances = [];
    rows.forEach(row => {
        var maintenance = new Object();
        maintenance.MaintenanceType = +row.querySelector('.maintenanceType').value;
        maintenance.MaintenanceKm = +row.querySelector('.maintenanceKm').value;
        maintenance.MaintenanceDate = row.querySelector('.maintenanceDate').value;
        maintenance.MaintenanceInfo = row.querySelector('.maintenanceInfo').value;
        maintenances.push(maintenance);
    });

    let jsonMaintenances = JSON.stringify(maintenances);
    result.value = jsonMaintenances;

    maintenances.forEach(maintenance => {
        if (maintenance.MaintenanceType == 0) {
            result.value = "";
            return;
        }
    })
});

//view or hide inputs when select changed
document.getElementById('selectType').addEventListener('change', function () {
    var selectedValue = this.value;

    // Get the parent container of the select element (maintenanceRow)
    var maintenanceRow = this.closest('.maintenanceRow');

    // Get the input elements in the same row
    var maintenanceKmInput = maintenanceRow.querySelector('.maintenanceKm');
    var maintenanceDateInput = maintenanceRow.querySelector('.maintenanceDate');
    var maintenanceInfoInput = maintenanceRow.querySelector('.maintenanceInfo');

    // Hide or show inputs based on the selected value
    switch (selectedValue) {
        case 1: //Date
            maintenanceDateInput.disabled = false;
            maintenanceKmInput.disabled = true;
            maintenanceKmInput.value = "";
            break;
        case 2: //Km
            maintenanceDateInput.disabled = true;
            maintenanceKmInput.disabled = false;
            maintenanceDateInput.value = "";
            break;
        case 3: //Other
            maintenanceDateInput.disabled = false;
            maintenanceKmInput.disabled = true;
            maintenanceDateInput.value = "";
            maintenanceKmInput.value = "";
            break;
        case 0:
            maintenanceDateInput.disabled = false;
            maintenanceKmInput.disabled = false;
            maintenanceInfoInput.disabled = false;
            maintenanceDateInput.value = "";
            maintenanceKmInput.value = "";
            maintenanceInfoInput.value = "";
            break;
    }
});

// Clone the first maintenance entry and append it to the container
document.getElementById('btnAddMaintenance').addEventListener('click', function () {
    var firstMaintenance = document.querySelector('.maintenanceRow');
    var clone = firstMaintenance.cloneNode(true);
    var newMaintenance = ClearInputs(clone);
    document.getElementById('maintenancesContainer').appendChild(newMaintenance);
});

function RemoveRow(obj) {
    var parent = document.getElementById('maintenancesContainer');
    var rowsCount = parent.childElementCount;

    if (rowsCount <= 1) {
        return;
    }

    var row = obj.closest('.maintenanceRow');
    parent.removeChild(row);
}

function ClearInputs(row) {
    row.querySelector('.maintenanceType').value = 0;
    row.querySelector('.maintenanceKm').value = "";
    row.querySelector('.maintenanceDate').value = "";
    row.querySelector('.maintenanceInfo').value = "";

    return row;
}

