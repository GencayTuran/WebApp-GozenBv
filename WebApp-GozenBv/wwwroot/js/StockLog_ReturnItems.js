
function ChangeDamaged(obj) {
    let stockAmount = obj.parentNode.parentNode.querySelector(".inputStockAmount");
    let inputDamaged = obj.parentNode.parentNode.querySelector(".inputDamagedAmount");
    let damagedTitle = document.getElementById("damagedTitle");
    let damaged = obj.checked;
    if (damaged) {
        inputDamaged.disabled = false;
        damagedTitle.style.opacity = 1;
        inputDamaged.value = stockAmount.value;
    }
    else {
        inputDamaged.value = "";
        inputDamaged.disabled = true;
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
            let damagedAmount = row.querySelector(".inputDamagedAmount").value;

            damagedStock.push([stockId, damagedAmount]);
        }
    });

    result.value = damagedStock;
}
