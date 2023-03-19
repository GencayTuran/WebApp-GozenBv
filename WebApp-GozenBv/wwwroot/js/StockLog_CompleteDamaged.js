let stockId = document.querySelectorAll(".stockId");
let repaired = document.querySelectorAll(".inputRepaired");
let deleted = document.querySelectorAll(".inputDeleted");
let damagedAmount = document.querySelectorAll(".damagedAmount")

let result = document.getElementById("completeDamagedResult");


function FillData(event) {
    for (let i = 0; i < stockId.length; i++) {
        let totalAmounts = Number(repaired[i].value) + Number(deleted[i].value)
        if (damagedAmount[i].innerHTML != totalAmounts) {
            alert("given input not equal to total of repaired & deleted");
            event.preventDefault();
        }
        let data = [];
        let damagedStock = new Object();
        damagedStock.stockId = +stockId[i].innerHTML;
        damagedStock.repairedAmount = repaired[i].value == '' ? 0 : +repaired[i].value;
        damagedStock.deletedAmount = deleted[i].value == '' ? 0 : +deleted[i].value
        data.push(damagedStock);
    }

    let jsonData = JSON.stringify(data);
    result.value = jsonData;
}