const row = document.querySelector(".productRow");
const parentNode = document.getElementById("rowCollection");
var result = document.getElementById("productsResult");
var inputProduct = document.querySelector(".inputProduct");
var inputAmount = document.querySelector(".inputAmount");

Init();

//let stockQty = document.getElementById("selectQty");
//let selectChildren = stockQty.querySelectorAll("option");
//let stockQtyData = [];
//selectChildren.forEach(option => {
//    stockQtyData.push([parseInt(option.value), parseInt(option.innerHTML)]); //stock, qty
//})

inputProduct.addEventListener("change", () => {
    if (inputProduct.value != 0) {
        inputAmount.stockid = inputProduct.value;
    }
});

function Init(){
    inputProduct.value = "";
}

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

    //CheckQuantity(inputProducts, inputAmounts, inputProducts.length);

    for (var i = 0; i < inputProducts.length; i++) {
        products.push([inputProducts[i].value, inputAmounts[i].value]); //id, amount
    }

    result.value = products;
}

//function CheckQuantity(stock, amount, length)
//{
//    let maxQuantityData = [];
//    stock.forEach(s => {
//       stockQtyData.forEach(qtyData => {
//        if (s.value == qtyData[0]) {
//            //if amount[i] > qtyData[1] --> set max limit or validation?
//        }
//       })
//    })

//}
