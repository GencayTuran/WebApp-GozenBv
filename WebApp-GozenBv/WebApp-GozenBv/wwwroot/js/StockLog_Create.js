let btnAdd = document.getElementById('btnAdd');
let btnRemove = document.querySelector('.btnRemove');
let row = document.querySelector('#trStock');
let clone = row.cloneNode(true); //true means to clone the nested elements as well.

function AddNewRow {
    clone.ClassName = 'productRow';
    row.after(clone);
}

function RemoveRow{
    //remove selected button its tr head
    btnRemove.parentElement.parentElement.remove();
}

btnAdd.addEventListener('click', AddNewRow);
btnRemove.addEventListener('click', RemoveRow);


//get view data to controller
let selectedProducts = [];
//let countCells = document.querySelectorAll'productRow').length;
//let x = document.getElementById('SelectedProducts').rows[]



selectedProducts.forEach()



