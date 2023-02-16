$('#inputKeuringDate').blur(function () {
    let keuringDate = new Date($('#inputKeuringDate').val());
    keuringDate.setFullYear(keuringDate.getFullYear() + 1);
    $('#inputDeadlineKeuring').val(formatDate(keuringDate));
});