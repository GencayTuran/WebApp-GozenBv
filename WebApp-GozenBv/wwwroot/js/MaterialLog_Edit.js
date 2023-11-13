document.addEventListener("DOMContentLoaded", function () {
    var checkboxes = document.querySelectorAll('[name$=".IsDamaged"]');

    checkboxes.forEach(function (checkbox) {
        var rowIndex = checkbox.dataset.rowIndex;
        var damagedAmountInput = document.querySelector('[name$=".DamagedAmount"][name*="' + rowIndex + '"]');
        var repairAmountInput = document.querySelector('[name$=".RepairAmount"][name*="' + rowIndex + '"]');
        var deleteAmountInput = document.querySelector('[name$=".DeleteAmount"][name*="' + rowIndex + '"]');

        // Initial check when the page is loaded
        if (!checkbox.checked) {
            damagedAmountInput.disabled = true;
            repairAmountInput.disabled = true;
            deleteAmountInput.disabled = true;
            damagedAmountInput.value = "";
            repairAmountInput.value = "";
            deleteAmountInput.value = "";
        }

        checkbox.addEventListener('change', function () {
            if (checkbox.checked) {
                damagedAmountInput.disabled = false;
                repairAmountInput.disabled = false;
                deleteAmountInput.disabled = false;
            } else {
                damagedAmountInput.disabled = true;
                repairAmountInput.disabled = true;
                deleteAmountInput.disabled = true;
                damagedAmountInput.value = "";
                repairAmountInput.value = "";
                deleteAmountInput.value = "";
            }
        });
    });
});