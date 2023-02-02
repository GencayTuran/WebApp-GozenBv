let stockId = document.querySelectorAll(".stockId");
let repaired = document.querySelectorAll(".inputRepaired");
let deleted = document.querySelectorAll(".inputDeleted");
let damagedAmount = document.querySelectorAll(".damagedAmount")

let result = document.getElementById("completeDamagedResult");

let data = [];

function FillData(event) {
    for (let i = 0; i < stockId.length; i++) {
        let totalAmounts =  Number(repaired[i].value) + Number(deleted[i].value)
        if (damagedAmount[i].innerHTML != totalAmounts) {
            alert("given input not equal to total of repaired & deleted");
            data = [];
            event.preventDefault();
        }
            data.push([
                stockId[i].innerHTML,
                repaired[i].value == '' ? '0' : repaired[i].value, 
                deleted[i].value == '' ? '0' : deleted[i].value
            ]);
    }
    
    result.value = data;
}