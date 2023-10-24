    
function ChangeDamaged(obj) {
    let materialAmount = obj.parentNode.parentNode.querySelector(".inputMaterialAmount");
    let inputDamaged = obj.parentNode.parentNode.querySelector(".inputDamagedAmount");
    let damagedTitle = document.getElementById("damagedTitle");
    let damaged = obj.checked;
    if (damaged) {
        inputDamaged.disabled = false;
        damagedTitle.style.opacity = 1;
        inputDamaged.value = materialAmount.value;
    }
    else {
        inputDamaged.value = "";
        inputDamaged.disabled = true;
        damagedTitle.style.opacity = 0.6;
    }

}

let result = document.getElementById("damagedMaterialResult");
function PassDamagedItems() {

    let inputReturnDamaged = document.querySelector("#isDamaged");
    let returnDamaged = false;

    let damagedMaterial = [];
    let rows = document.querySelectorAll(".materialLogItemRow")

    rows.forEach(row => {
        let itemDamaged = row.querySelector(".chkDamaged").checked;
        let materialLogItem = new Object();

        if (itemDamaged) {
            returnDamaged = true;
            let materialId = row.querySelector(".labelMaterialId").innerHTML;
            let damagedAmount = row.querySelector(".inputDamagedAmount").value;

            materialLogItem.materialId = +materialId;
            materialLogItem.damagedAmount = +damagedAmount;

            damagedMaterial.push(materialLogItem);
        }
    });
    let jsonDamagedMaterial = JSON.stringify(damagedMaterial);
    result.value = jsonDamagedMaterial;
    inputReturnDamaged.checked = returnDamaged;
}
