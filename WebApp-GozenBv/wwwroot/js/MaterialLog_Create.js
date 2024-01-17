const parentNode = document.getElementById("rowCollection");
const row = document.querySelector(".productRow");
var result = document.getElementById("productsResult");

//single value input
// var inputSelectedProduct = document.querySelector(".inputSelectedProduct");
var selectMaterial = document.querySelector(".selectMaterial");
var inputAmount = document.querySelector(".inputAmount");

//elements
let rows;
let inputMaterials;
let inputAmounts;
let labelAlerts;

function GetElements()
{
    //collection value of inputs
    // inputProducts = document.querySelectorAll(".inputSelectedProduct");
    rows = document.querySelectorAll(".productRow");
    selectMaterials = document.querySelectorAll(".selectMaterial");
    inputAmounts = document.querySelectorAll(".inputAmount");
    chkUsed = document.querySelectorAll(".chkUsed");
    labelAlerts = document.querySelectorAll(".labelAlert");
}

Init();

function Init() {
    selectMaterial.value = "";
    // SetMinValue();
}

let alerts = [];
function ValidateForm(event) {    
    
    if (BothFalse())
    {
        alerts.push("Not all inputs have a value!");       
        
        let strAlerts = "";
        alerts.forEach(alert => {
            strAlerts += alert + "\n";
        });
        window.alert(strAlerts);

        event.preventDefault();
    }
    if (QtyFalseOnly()) {
        let strAlerts = "";
        alerts.forEach(alert => {
            strAlerts += alert + "\n";
        });
        window.alert(strAlerts);

        event.preventDefault();
    }
    if (InputFalseOnly()) {
        window.alert("Not all inputs have a value!");
        event.preventDefault();
    }
    if (Success()) {
            PassProducts();
    }
}

function PassProducts() {
    GetElements();
    let products = [];
    let isUsed;
    
    for (var i = 0; i < selectMaterials.length; i++) {
        let selectedItem = new Object();
        isUsed = chkUsed[i].checked ? true : false;

        selectedItem.materialId = +selectMaterials[i].value;
        selectedItem.amount = +inputAmounts[i].value;
        selectedItem.used = isUsed;
        products.push(selectedItem)
    }

    let jsonMaterial = JSON.stringify(products);

    result.value = jsonMaterial;

}

//TODO: do you still need to check in the view for qty?
function CheckQuantity()
{
    GetElements();
    check = true;
    alerts = [];

   for (let i = 0; i < rows.length; i++) {
    materialQtyAndNames.forEach(data => {
        if (selectMaterials[i] == data.materialId) {
            if (chkUsed[i].checked) {
                if (parseInt(inputAmounts[i].value) > data.quantityUsed) {
                    let message = (`Product: ${data.productNameCode} amount is too high! Max Used Qty: ${data.quantityUsed}`);
                    alerts.push(message);
                    labelAlerts[i].innerHTML = "*";
                    check = false;
                }
                else{
                    labelAlerts[i].innerHTML = "";
                }
            }
            else{
                if (parseInt(inputAmounts[i].value) > data.quantity) {
                    let message = (`Product: ${data.productNameCode} amount is too high! Max Qty: ${data.quantity}`);
                    alerts.push(message);
                    labelAlerts[i].innerHTML = "*";
                    check = false;
                }
                else{
                    labelAlerts[i].innerHTML = "";
                }
            }
        }
    })
    }
    return check;
}

function CheckInputs() {
    GetElements();
    let lightRed = "rgba(255,0,0,0.8)";
    check = true;

    for (let i = 0; i < rows.length; i++) {
        if (selectMaterials[i].value == "" || inputAmounts[i].value == "") {
            rows[i].style.backgroundColor = lightRed;
            check = false;
        }
        else{
            rows[i].style.backgroundColor = "transparent";
        }
    }

    return check;
}

function NewRow() {
    //$(row.querySelector(".selectMaterial")).select2("destroy");

    var clone = row.cloneNode(true);
    //$(clone.querySelector(".selectMaterial")).select2({width: '100%'});
    clone.querySelector(".selectMaterial").value = "";
    clone.querySelector(".inputAmount").value = "";
    clone.querySelector(".chkUsed").checked = false;
    clone.style.backgroundColor = "transparent";
    clone.querySelector(".labelAlert").innerHTML = "";
    parentNode.appendChild(clone);

    //$(row.querySelector(".selectMaterial")).select2({width: '100%'});
}

function RemoveRow(obj) {
    GetElements();
    if (rows.length != 1) {
        element = obj.parentNode.parentNode.parentNode;
        document.getElementById("rowCollection").removeChild(element);
    }
}


//return bools
function BothFalse() {
    return !CheckInputs() && !CheckQuantity();
}
function InputFalseOnly() {
    return !CheckInputs() && CheckQuantity();
}
function QtyFalseOnly() {
    return CheckInputs() && !CheckQuantity();
}
function Success() {
    return CheckInputs() && CheckQuantity();
}

// function SetMinValue() {   
//     inputAmount.addEventListener("change", () => {
//         if (inputAmount.value <= 0) {
//             inputAmount.value = 1;
//         }
//     })
// }
