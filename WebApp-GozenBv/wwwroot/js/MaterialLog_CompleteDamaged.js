let materialId = document.querySelectorAll(".materialId");
let repaired = document.querySelectorAll(".inputRepaired");
let deleted = document.querySelectorAll(".inputDeleted");
let damagedAmount = document.querySelectorAll(".damagedAmount")

let result = document.getElementById("completeDamagedResult");


function FillData(event) {
    let data = [];
    for (let i = 0; i < materialId.length; i++) {
        let totalAmounts = Number(repaired[i].value) + Number(deleted[i].value)
        if (damagedAmount[i].innerHTML != totalAmounts) {
            alert("given input not equal to total of repaired & deleted");
            event.preventDefault();
        }
        let damagedMaterial = new Object();
        damagedMaterial.materialId = +materialId[i].innerHTML;
        damagedMaterial.repairedAmount = repaired[i].value == '' ? 0 : +repaired[i].value;
        damagedMaterial.deletedAmount = deleted[i].value == '' ? 0 : +deleted[i].value
        data.push(damagedMaterial);
    }
  
    let jsonData = JSON.stringify(data);
    result.value = jsonData;
}