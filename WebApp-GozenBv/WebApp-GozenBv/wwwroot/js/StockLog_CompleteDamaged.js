let stockId = document.querySelectorAll(".stockId");
let repaired = document.querySelectorAll(".inputRepaired");
let deleted = document.querySelectorAll(".inputDeleted");
let damaged = document.querySelectorAll(".inputDamaged")

let result = document.getElementById("completeDamagedResult");

let data = [];

function FillData() {
    for (let i = 0; i < stockId.length; i++) {
            data.push([
                stockId[i].innerHTML,
                repaired[i].value == '' ? '0' : repaired[i].value, 
                deleted[i].value == '' ? '0' : deleted[i].value
            ]);
    }
    
    result.value = data;
}