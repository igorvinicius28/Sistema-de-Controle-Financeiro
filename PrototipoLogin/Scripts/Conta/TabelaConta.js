"use strict";

//Helper
var helperTabelaConta = {

    openModal: function (titulo, data, componente) {
        $("#modal-title").html("<h2>" + titulo + "</h2>");

        $("#modal-body").html(data);

        $("#btnSalvar").click(function () {
            movimentacoes.salvar();
        });

        $("#myModal").modal("show");

        AplicarMascara();
    }
    , message: function (mensagem) {
        $("#message").html(mensagem).addClass("alert alert-info").fadeOut(2000);
    }
    , fecharModal: function () {
        $("#myModal").modal("hide");
    }

    , fecharModalExclusao: function () {
        $("#myModal2").modal("hide");
    }

    , openModalExcluir: function (titulo, data, componente) {

        $("#modal-title2").html("<h2>" + titulo + "</h2>");

        $("#modal-body2").html(data);

        $("#Delete").click(function () {
            movimentacoes.salvar();
        });
        $("#myModal2").modal("show");

        AplicarMascara();
    }
};

var conta = {
   
    novoMovimentacaoAhPagar: function () {
        var url = "/Contas/CreateAhPagar";
        $.ajax({
            url: url
            , datatype: "html"
            , type: "GET"
        }).done(function (data) {
            helperTabelaConta.openModal("Nova Movimentação", data, movimentacoes);
        });
    },

    novoMovimentacaoAhReceber: function () {
        var url = "/Contas/CreateAhReceber";
        $.ajax({
            url: url
            , datatype: "html"
            , type: "GET"
        }).done(function (data) {
            helperTabelaConta.openModal("Nova Movimentação", data, movimentacoes);
        });
    },

    salvarConta: function () {
        //  if (cliente.validar()) {
        //Nome
        var id = $("#Id").val();
        var tipo = $("#Tipo").val();
        var descricao = $("#Descricao").val();
        var dataCadastro = $("#DataCadastro").val();
        var valor = $("#Valor").val();
        var FoiPagaOuRecebida = $("#FoiPagaOuRecebida")[0].checked;
        var dataDoPagamento = $("#DataDoPagamento").val();

        var token = $('input[name="__RequestVerificationToken"]').val();
        var tokenadr = $('form[action="/Movimentacoes/Create"] input[name="__RequestVerificationToken"]').val();
        var headers = {};
        var headersadr = {};
        headers['__RequestVerificationToken'] = token;
        headersadr['__RequestVerificationToken'] = tokenadr;

        //Gravar
        var url = movimentacoes.obtenhaUrl(tipo);

        var id = $("#Id").val();

        if (id !== "0") {
            url = "/Contas/Edit";
        }

        $.ajax({
            url: url
            , type: "POST"
            , datatype: "json"
            , headers: headersadr
            , data: { Id: id, Descricao: descricao, DataDoPagamento : dataDoPagamento,DataCadastro: dataCadastro, Valor: valor, Tipo: tipo, FoiPagaOuRecebida: FoiPagaOuRecebida, __RequestVerificationToken: token }
            , beforeSend: function () {
            }
            , complete: function () {

            }
        }).done(function (data) {
            if (data.Resultado == "Sucesso") {
                dataTableConta.ajax.reload();
                helperTabelaConta.message("Registro gravado com sucesso");
                helperTabelaConta.fecharModal();
            }
        });
    },

    editarConta: function (id, descricao, data, valor, tipo, foiPagaRecebida) {
        var url = "/Contas/Edit";

        $.ajax({
            url: url
            , datatype: "html"
            , type: "GET"
            , data: { id: id, Descricao: descricao, DataCadastro: data, Valor: valor, Tipo: tipo, FoiPagaOuRecebida: foiPagaRecebida }
            , beforeSend: function () {
                //  waitingDialog.show('Aguarde');
            }
            , complete: function () {
                //    waitingDialog.hide();
            }
        }).done(function (data) {
            helperTabelaConta.openModal("Editar Contas", data, movimentacoes);

            if ($("#ValorFoiPagaOuRecebida").val() == "True") {
                $("#FoiPagaOuRecebida")[0].checked = true;
            }
        });

    },

    obtenhaUrl: function (tipo) {

        if (tipo === "ENTRADA") {
            return "/Movimentacoes/CreateEntrada";
        }
        if (tipo === "SAIDA") {
            return "/Movimentacoes/CreateSaida";
        }
        if (tipo === "AHPAGAR") {
            return "/Contas/CreateAhPagar";
        }
        if (tipo === "AHRECEBER") {
            return "/Contas/CreateAhReceber";
        }
    },

    // IMIGRACAO

    DeleteModal: function (id) {
        var url = "/Contas/ConfirmacaoExcluirModal";
        $.ajax({
            url: url
            , datatype: "json"
            , type: "POST"
            , data: { id: id }
            , beforeSend: function () {
                //  waitingDialog.show('Aguarde');
            }
            , complete: function () {
                //    waitingDialog.hide();
            }
        }).done(function (data) {
            helperTabelaConta.openModalExcluir("Excluir Movimentação", data, movimentacoes);
        });
    },

    Delete() {
        var ide = $("#identificador").val();
        var url = "/Contas/Delete";
        $.ajax({
            url: url,
            datatype: "html",
            type: "POST",
            data: { id: ide }
        }).done(function (data) {
            if (data.success) {
                dataTableConta.ajax.reload();
                helperTabelaConta.fecharModalExclusao();
            } else {
                alert("Erro ao excluir registro, verifique os dados e tente novamente");
            }
        });
    },

} // FIM CONTEXTO

// LOAD

$(document).ready(function () {

    function PopupForm(url) {
        var formDiv = $('<div/>');
        $.get(url)
            .done(function (response) {
                formDiv.html(response);

                Popup = formDiv.dialog({
                    autoOpen: true,
                    resizable: false,
                    title: 'Fill Employee Details',
                    height: 500,
                    width: 700,
                    close: function () {
                        Popup.dialog('destroy').remove();
                    }
                });
            });
    }

    function SubmitForm(form) {
        $.validator.unobtrusive.parse(form);
        if ($(form).valid()) {
            $.ajax({
                type: "POST",
                url: form.action,
                data: $(form).serialize(),
                success: function (data) {
                    if (data.success) {
                        Popup.dialog('close');
                        dataTableConta.ajax.reload();

                        $.notify(data.message, {
                            globalPosition: "top center",
                            className: "success"
                        })
                    }
                }
            });
        }
        return false;
    }

    // Novo
    $("#btnNovoCliente").bind('click', function () {
        conta.novo();
    });

    $("#btnNovaMovimentacaoDeSaida").bind('click', function () {
        conta.novoMovimentacaoSaida();
    });

    $("#btnNovaContaAhPagar").bind('click', function () {
        conta.novoMovimentacaoAhPagar();
    });

    $("#btnNovaContaAhReceber").bind('click', function () {
        conta.novoMovimentacaoAhReceber();
    });

    // Listar
    $("#buscaCliente").click(function () {
        //    movimentacoes.listar();
    });

    $("#limparCliente").click(function () {
        conta.limpar();
    });

    $("#moviEditar").click(function () {
        conta.editar();
    });

});