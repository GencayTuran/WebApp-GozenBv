﻿const parentNode = document.getElementById("rowCollection");
const row = document.querySelector(".productRow");
var result = document.getElementById("productsResult");

//single value input
// var inputSelectedProduct = document.querySelector(".inputSelectedProduct");
var selectStock = document.querySelector(".selectStock");
var inputAmount = document.querySelector(".inputAmount");

//elements
let rows;
let inputStocks;
let inputAmounts;
let labelAlerts;

function GetElements()
{
    //collection value of inputs
    // inputProducts = document.querySelectorAll(".inputSelectedProduct");
    rows = document.querySelectorAll(".productRow");
    selectStocks = document.querySelectorAll(".selectStock");
    inputAmounts = document.querySelectorAll(".inputAmount");
    chkUsed = document.querySelectorAll(".chkUsed");
    labelAlerts = document.querySelectorAll(".labelAlert");
}

Init();

function Init() {
    selectStock.value = "";
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
    
    for (var i = 0; i < selectStocks.length; i++) {
        let selectedItem = new Object();
        isUsed = chkUsed[i].checked ? true : false;

        selectedItem.stockId = +selectStocks[i].value;
        selectedItem.amount = +inputAmounts[i].value;
        selectedItem.used = isUsed;
        products.push(selectedItem)
    }

    let jsonStock = JSON.stringify(products);

    result.value = jsonStock;

}

function CheckQuantity()
{
    GetElements();
    check = true;
    alerts = [];

   for (let i = 0; i < rows.length; i++) {
    stockQtyAndNames.forEach(data => {
        if (selectStocks[i] == data.stockId) {
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
    let lightRed = "rgba(255,0,0,0.5)";
    check = true;

    for (let i = 0; i < rows.length; i++) {
        if (selectStocks[i].value == "" || inputAmounts[i].value == "") {
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
    $(row.querySelector(".selectStock")).select2("destroy");

    var clone = row.cloneNode(true);
    $(clone.querySelector(".selectStock")).select2({width: '100%'});
    clone.querySelector(".selectStock").value = "";
    clone.querySelector(".inputAmount").value = "";
    clone.querySelector(".chkUsed").checked = false;
    clone.style.backgroundColor = "transparent";
    clone.querySelector(".labelAlert").innerHTML = "";
    parentNode.appendChild(clone);

    $(row.querySelector(".selectStock")).select2({width: '100%'});
}

function RemoveRow(obj) {
    element = obj.parentNode.parentNode.parentNode;
    document.getElementById("rowCollection").removeChild(element);
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
