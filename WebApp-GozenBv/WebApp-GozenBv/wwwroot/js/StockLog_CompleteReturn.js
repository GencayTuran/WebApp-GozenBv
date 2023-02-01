let result = document.getElementById("damagedStockResult");

function PassDamagedItems() {

    let damagedStock = [];
    let rows = document.querySelectorAll(".stockLogItemRow")

    rows.forEach(row => {
        let damagedProduct = row.querySelector(".checkRow").checked;

        if (damagedProduct) {
            let stockId = row.querySelector(".labelStockId").innerHTML;
            let stockAmount = row.querySelector(".inputAmount").value;

            damagedStock.push([stockId, stockAmount]);
        }
    });

    result.value = damagedStock;
}
