$(document).ready(function () {//runs on every page loaded
    OnPageLoad();
});
function OnPageLoad() {
    FormatUIControls();
}
function FormatUIControls() {
    //alert("FormatUIControls call");
    $(".table").addClass("table-hover table-striped");
    $(".table-simple").addClass("table");

    $('.multiselect').multiselect({//multiselect with maxheight
        disableIfEmpty: true, maxHeight: 280, enableFiltering: false, includeSelectAllOption: true, nonSelectedText: 'Seleccionar...', nSelectedText: ' seleccionados', selectAllText: "Seleccionar todos"
    });
    $('.multiselect-small').multiselect({
        disableIfEmpty: true, nonSelectedText: 'Seleccionar...', nSelectedText: ' seleccionados', selectAllText: "Seleccionar todos"
    });
    $('.multiselect-large').multiselect({//multiselect with maxheight
        disableIfEmpty: true, maxHeight: 500, enableFiltering: true, includeSelectAllOption: true, nonSelectedText: 'Seleccionar...', nSelectedText: ' seleccionados', selectAllText: "Seleccionar todos"
    });
    $(".datepicker").datepicker({
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true,
        //currentText: "01/01/2007",
        //showMonthAfterYear: true,
        //yearRange: "2000:2012",
        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"]
    });
    
    $('.file-input').bootstrapFileInput();



}
function FocusOnLoad() {
    $(".focus-on-load").focus();
}

//funtion for all jquery id calls (this solves issue for not finding the control id when the id contains an special character like '.' as when a form contains the inputs for an ASP .NET MVC object (ex: id="User.UserId"))
//example, instead of: $("mycontrol").val = "hello";
//use: $(jFormatId("mycontrol")).val = "hello";
function jFormatId(myid) {
    return "#" + myid.replace(/(:|\.|\[|\]|,)/g, "\\$1");
}

// Creates and display an alert message with the proper formatting
function AlertErrorMessage(textToDisplay) {
    // remove previous alert messages that may exist already, we just want to display a new alert
    $('.alert-dismissible').remove();

    // creating alert
    var alert = '<div class="alert alert-danger alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong><a @*TODO:put error detail (maybe info debug)*@ class="alert-link"></a></strong>' + textToDisplay + '</div >'
    $(alert).insertBefore('.page-header');
}
