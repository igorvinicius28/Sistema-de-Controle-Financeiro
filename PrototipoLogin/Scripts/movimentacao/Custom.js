/*------------------------------------------------------
    Author : www.webthemez.com
    License: Commons Attribution 3.0
    http://creativecommons.org/licenses/by/3.0/
---------------------------------------------------------  */

(function ($) {

    "use strict";

    var helper = {
        openModal: function (titulo, data, componente) {

            $("#modal-title").val(titulo);

            $("#modal-body").html(data);

            $("#btnSalvar").click(function () {
                componente.Salvar();
            });

            $("#myModal").modal("show");
        }
    };
}(jQuery));



function AplicarMascara() {

    $(".data").mask("99/99/9999");
    $(".dinheiro").maskMoney({ showSymbol: true, symbol: "R$", decimal: ",", thousands: "." });
    //$(".date").datepicker({
    //    dateFormat: 'dd/mm/yy',
    //    dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado', 'Domingo'],
    //    dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
    //    dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
    //    monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
    //    monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
    //    constrainInput: true,
    //    buttonImageOnly: true
    //});

}