    function passProduct(key){
        console.log(key)
    }

    let select = document.getElementById('selectProduct');
    select.addEventListener('change', () =>{
        console.log(select.value)
        select.value = "Selecteer producten"
    }) 