let chkDamaged = document.querySelector("#chkDamaged");
let btnSubmit = document.querySelector("#btnSubmit");
let damagedView = document.querySelectorAll(".damagedView");
let labelAmount = document.querySelectorAll(".labelAmount");
let inputAmount = document.querySelectorAll(".inputAmount");

let result = document.getElementById("damagedStockResult");

ChangeDamagedView("none", inputAmount);

chkDamaged.addEventListener("change", () => {

    if (chkDamaged.checked) {
        ChangeDamagedView("block", inputAmount)
        ChangeDamagedView("none", labelAmount);
        ChangeDamagedView("block", damagedView);
        btnSubmit.addEventListener("click", PassDamagedItems);
    }
    else {
        ChangeDamagedView("none", inputAmount)
        ChangeDamagedView("block", labelAmount);
        ChangeDamagedView("none", damagedView);
        btnSubmit.removeEventListener("click", PassDamagedItems);
        result.value = "";
    }
});

function ChangeDamagedView(display, elementClass) {
    elementClass.forEach(element => {
        element.style.display = display;
    })
}

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
