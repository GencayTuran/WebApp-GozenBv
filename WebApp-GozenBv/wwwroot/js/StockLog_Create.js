const form = document.getElementById('form-id');
const parentNode = document.getElementById("rowCollection");
const row = document.querySelector(".productRow");
var result = document.getElementById("productsResult");

//single value input
var inputProduct = document.querySelector(".inputProduct");
var inputAmount = document.querySelector(".inputAmount");

//elements
let rows;
let inputProducts;
let inputAmounts;
let labelAlerts;

function GetElements()
{
    //collection value of inputs
    rows = document.querySelectorAll(".productRow");
    inputProducts = document.querySelectorAll(".inputProduct");
    inputAmounts = document.querySelectorAll(".inputAmount");
    labelAlerts = document.querySelectorAll(".labelAlert");
}

Init();

function Init() {
    inputProduct.value = "";
    SetMinValue();
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

    if (Succes()) {
        PassProducts();
    }
}

function PassProducts() {
    GetElements();
    var products = [];

    for (var i = 0; i < inputProducts.length; i++) {
        products.push([inputProducts[i].value, inputAmounts[i].value]); //id, amount
    }
    result.value = products;

}

function CheckQuantity()
{
    GetElements();
    check = true;
    alerts = [];

   for (let i = 0; i < rows.length; i++) {
    stockQtyAndNames.forEach(data => {
        if (inputProducts[i].value == data.stockId) {
            if (parseInt(inputAmounts[i].value) > data.quantity) {
                let message = (`Product: ${data.productNameBrand} amount is too high! Max Qty: ${data.quantity}`);
                alerts.push(message);
                labelAlerts[i].innerHTML = "*";
                check = false;
            }
            else{
                labelAlerts[i].innerHTML = "";
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
        if (inputProducts[i].value == "" || inputAmounts[i].value == "") {
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
    var clone = row.cloneNode(true);
    clone.querySelector(".inputProduct").value = "";
    clone.querySelector(".inputAmount").value = "";
    clone.style.backgroundColor = "transparent";
    clone.querySelector(".labelAlert").innerHTML = "";
    parentNode.appendChild(clone);
}

function RemoveRow(obj) {
    element = obj.parentNode.parentNode.parentNode;
    document.getElementById("rowCollection").removeChild(element);
}


function BothFalse() {
    return !CheckInputs() && !CheckQuantity();
}

function InputFalseOnly() {
    return !CheckInputs() && CheckQuantity();
}

function QtyFalseOnly() {
    return CheckInputs() && !CheckQuantity();
}

function Succes() {
    return CheckInputs() && CheckQuantity();
}

function SetMinValue() {   
    inputAmount.addEventListener("change", () => {
        if (inputAmount.value <= 0) {
            inputAmount.value = 1;
        }
    })
}
