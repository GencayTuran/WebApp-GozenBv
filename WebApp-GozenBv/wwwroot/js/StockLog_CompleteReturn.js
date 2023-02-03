function ChangeDamaged(obj) {
    let inputDamaged = obj.parentNode.parentNode.querySelector(".inputAmount")
    let damaged = obj.checked;
    if (damaged) {
        inputDamaged.disabled = false;
    }
    else {
        inputDamaged.disabled = true;
    }

    ChangeTitleOpacity();
}

function ChangeTitleOpacity() {
    let damaged = false;
    let damagedTitle = document.getElementById("damagedTitle");
    let checks = document.querySelectorAll(".chkDamaged");
    checks.forEach(check => {
        if (check.checked) {
            damaged = true;
        }
    })

    if (damaged)
    {
        damagedTitle.style.opacity = 1;
    } else {
        damagedTitle.style.opacity = 0.6;
    }
}




let result = document.getElementById("damagedStockResult");
function PassDamagedItems() {
    
    let damagedStock = [];
    let rows = document.querySelectorAll(".stockLogItemRow")

    rows.forEach(row => {
        let isDamaged = row.querySelector(".chkDamaged").checked;

        if (isDamaged) {
            let stockId = row.querySelector(".labelStockId").innerHTML;
            let damagedAmount = row.querySelector(".inputAmount").value;

            damagedStock.push([stockId, damagedAmount]);
        }
    });

    result.value = damagedStock;
}
