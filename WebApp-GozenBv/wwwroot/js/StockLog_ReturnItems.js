    
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

    let inputReturnDamaged = document.querySelector("#isDamaged");
    let returnDamaged = false;

    let damagedStock = [];
    let rows = document.querySelectorAll(".stockLogItemRow")

    rows.forEach(row => {
        let itemDamaged = row.querySelector(".chkDamaged").checked;
        let stockLogItem = new Object();

        if (itemDamaged) {
            returnDamaged = true;
            let stockId = row.querySelector(".labelStockId").innerHTML;
            let damagedAmount = row.querySelector(".inputDamagedAmount").value;

            stockLogItem.stockId = +stockId;
            stockLogItem.damagedAmount = +damagedAmount;

            damagedStock.push(stockLogItem);
        }
    });
    let jsonDamagedStock = JSON.stringify(damagedStock);
    result.value = jsonDamagedStock;
    inputReturnDamaged.checked = returnDamaged;
}
