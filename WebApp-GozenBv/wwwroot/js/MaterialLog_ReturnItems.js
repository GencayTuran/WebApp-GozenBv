document.addEventListener("DOMContentLoaded", function () {
    // Function to disable and clear inputs based on the state of the "Damaged" checkbox
    function handleDamagedCheckboxChange() {
        var checkboxes = document.querySelectorAll('.chkDamaged');
        checkboxes.forEach(function (checkbox) {
            var row = checkbox.closest('tr');
            var damagedAmountInput = row.querySelector('.inputDamagedAmount');
            var repairAmountInput = row.querySelector('.inputRepairAmount');
            var deleteAmountInput = row.querySelector('.inputDeleteAmount');

            if (!checkbox.checked) {
                damagedAmountInput.value = '';
                repairAmountInput.value = '';
                deleteAmountInput.value = '';
                damagedAmountInput.disabled = true;
                repairAmountInput.disabled = true;
                deleteAmountInput.disabled = true;
            } else {
                damagedAmountInput.disabled = false;
                repairAmountInput.disabled = false;
                deleteAmountInput.disabled = false;
            }
        });
    }

    // Check the state of the "Damaged" checkbox on page load
    handleDamagedCheckboxChange();

    // Bind the function to the change event of the "Damaged" checkboxes
    var checkboxes = document.querySelectorAll('.chkDamaged');
    checkboxes.forEach(function (checkbox) {
        checkbox.addEventListener('change', handleDamagedCheckboxChange);
    });

    // Function to set "isDamaged" to true if any "Damaged" checkbox is checked
    function setDamaged() {
        var isDamagedCheckbox = document.getElementById('isDamaged');
        isDamagedCheckbox.checked = Array.from(checkboxes).some(function (checkbox) {
            return checkbox.checked;
        });
    }

    // Bind the function to the form submission
    var form = document.querySelector('form');
    form.addEventListener('submit', function () {
        setDamaged();
    });
});