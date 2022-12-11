const row = document.querySelector(".productRow");
const parentNode = document.getElementById("rowCollection");
var result = document.getElementById("productsResult");

function NewRow() {
    var clone = row.cloneNode(true);
    clone.firstChild.nextSibling.children[0].firstChild.nextElementSibling.value = "";
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
