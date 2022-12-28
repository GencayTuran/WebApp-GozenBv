const row = document.querySelector(".productRow");
const parentNode = document.getElementById("rowCollection");
var result = document.getElementById("productsResult");

let selectQty = document.getElementById("selectQty");
let selectChildren = selectQty.querySelectorAll("option");
let stockQty = [];
selectChildren.forEach(option => {
    stockQty.push([parseInt(option.value), parseInt(option.innerHTML)]); //stock, qty
})
console.log(stockQty);

function NewRow() {
    var clone = row.cloneNode(true);
    clone.querySelector(".inputProduct").value = "";
    parentNode.appendChild(clone);
}

function RemoveRow(obj) {
    element = obj.parentNode.parentNode.parentNode;
    document.getElementById("rowCollection").removeChild(element);
}

function PassProducts() {
    var products = [];
    var inputProducts = document.querySelectorAll(".inputProduct");
    var inputAmounts = document.querySelectorAll(".inputAmount");

    for (var i = 0; i < inputProducts.length; i++) {
        products.push([inputProducts[i].value, inputAmounts[i].value]); //id, amount
    }

    result.value = products;
}
