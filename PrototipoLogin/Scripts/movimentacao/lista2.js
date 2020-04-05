"use strict";


//Helper
var helper = {

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

var movimentacoes = {
    novo: function () {

        var url =  "/Movimentacoes/Create";
        $.ajax({
            url: url
            , datatype: "html"
            , type: "GET"
        }).done(function (data) {
            helper.openModal("Nova Movimentação", data, movimentacoes);
        });
    },

    novoMovimentacaoSaida: function () {
        var url = "/Movimentacoes/CreateSaida";
        $.ajax({
            url: url
            , datatype: "html"
            , type: "GET"
        }).done(function (data) {
            helper.openModal("Nova Movimentação", data, movimentacoes);
        });
    },

    novoMovimentacaoAhPagar: function () {
        var url = "/Contas/CreateAhPagar";
        $.ajax({
            url: url
            , datatype: "html"
            , type: "GET"
        }).done(function (data) {
            helper.openModal("Nova Movimentação", data, movimentacoes);
        });
    },

    novoMovimentacaoAhReceber: function () {
        var url = "/Contas/CreateAhReceber";
        $.ajax({
            url: url
            , datatype: "html"
            , type: "GET"
        }).done(function (data) {
            helper.openModal("Nova Movimentação", data, movimentacoes);
        });
    },

    salvar: function () {
        //  if (cliente.validar()) {

        //Nome
        var id = $("#Id").val();
        var tipo = $("#Tipo").val();
        var descricao = $("#Descricao").val();
        var dataCadastro = $("#DataCadastro").val();
        var valor = $("#Valor").val();

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
            url = tipo == "ENTRADA" ? "/Movimentacoes/Edit" : "/Movimentacoes/EditSaida";
        }

        $.ajax({
            url: url
            , type: "POST"
            , datatype: "json"
            , headers: headersadr
            , data: { Id: id, Descricao: descricao, DataCadastro: dataCadastro, Valor: valor, Tipo: tipo, __RequestVerificationToken: token }
            , beforeSend: function () {
                //  waitingDialog.show('Aguarde');
            }
            , complete: function () {
                //   waitingDialog.hide();
                // var mod = $(".modal fade show")
            }
        }).done(function (data) {
            if (data.Resultado == "Sucesso") {
                dataTable.ajax.reload();
                helper.message("Registro gravado com sucesso");
                helper.fecharModal();
            }
        });
        //   }
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
            , data: { Id: id, Descricao: descricao, DataCadastro: dataCadastro, Valor: valor, Tipo: tipo, FoiPagaOuRecebida: FoiPagaOuRecebida, __RequestVerificationToken: token }
            , beforeSend: function () {
                //  waitingDialog.show('Aguarde');
            }
            , complete: function () {
                //   waitingDialog.hide();
                // var mod = $(".modal fade show")
            }
        }).done(function (data) {
            if (data.Resultado == "Sucesso") {
                dataTable.ajax.reload();
                helper.message("Registro gravado com sucesso");
                helper.fecharModal();
            }
        });
        //   }
    },

    selecionarCliente: function (id, descricao, data, valor) {

        var url = "/Movimentacoes/Selecionar";
        $.ajax({
            url: url
            , datatype: "html"
            , type: "GET"
        }).done(function (data) {
            helper.openModal("Nova Movimentação", data, movimentacoes);
        });

        $("#Nome").val(nome);
        $("#IdCliente").val(id);
        $("#DataCadastro").val(data);
        $("#Valor").val(valor);
        $("#myModalCliente").modal("hide");
    },

    listar: function () {
        var div = $("#minhaTabela");
        var url = "/Movimentacoes/Listar";

        var descricao = $("#Descricao").val();
        var dataCadastro = $("#DataCadastro").val();
        var valor = $("#Valor").val();

        $.ajax({
            url: url
            , datatype: "html"
            , type: "GET"
            , data: { descricao: descricao, data: dataCadastro, valor: valor }
        }).done(function (data) {
            div.html("");
            div.html(data);

        });


    },
    validar: function () {
        $("#frmPedido").validate({
            rules: {
                Descricao: {
                    required: true
                }
            }
            , messages: {
                Descricao: {

                    required: "* O Campo CLIENTE é obrigatório"
                }
            }
            , highlight: function (element) {
                $(element).closest('.form-group').addClass('has-error');
            },
            unhighlight: function (element) {
                $(element).closest('.form-group').removeClass('has-error');
            },
            errorElement: 'span',
            errorClass: 'help-block',
            errorPlacement: function (error, element) {
                if (element.parent('.input-group').length) {
                    error.insertAfter(element.parent());
                } else {
                    error.insertAfter(element);
                }
            }
        });

        return $("#frmPedido").valid();

    },

    editar: function (id, descricao, dataMovimentacao, valor, tipo) {
        var url = "/Movimentacoes/Edit";

        $.ajax({
            url: url
            , datatype: "html"
            , type: "GET"
            , data: { id: id}
            , beforeSend: function () {
                //  waitingDialog.show('Aguarde');
            }
            , complete: function () {
                //    waitingDialog.hide();
            }
        }).done(function (data) {
            helper.openModal("Editar Movimentação", data, movimentacoes);
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
            helper.openModal("Editar Contas", data, movimentacoes);

            if ($("#ValorFoiPagaOuRecebida").val() == "True")
            {
                $("#FoiPagaOuRecebida")[0].checked = true;
            }
        });

    },

    excluirs: function (id) {
        var url = "/Movimentacoes/Excluirs";
        $.ajax({
            url: url
            , datatype: "html"
            , type: "GET"
            , data: { id: id }
            , beforeSend: function () {
                //  waitingDialog.show('Aguarde');
            }
            , complete: function () {
                //    waitingDialog.hide();
            }
        }).done(function (data) {
            helper.openModalExcluir("Excluir Movimentação", data, movimentacoes, id);
        });

    },

    excluir: function () {
        var id = $("#Id").val();

        var url = "/movimentacoes/excluir";
        $.ajax({
            url: url
            , datatype: "json"
            , type: "POST"
            , data: { id: id }
        }).done(function (data) {
            if (data.Resultado) {
              //  movimentacoes.listar();
                helper.fecharModalExclusao();
            } else {
                alert("Erro ao excluir registro, verifique os dados e tente novamente");
            }
        });


    },

    excluirsConta: function (id) {
        var url = "/Contas/Excluirs";

        $.ajax({
            url: url
            , datatype: "html"
            , type: "GET"
            , data: { id: id }
            , beforeSend: function () {
                //  waitingDialog.show('Aguarde');
            }
            , complete: function () {
                //    waitingDialog.hide();
            }
        }).done(function (data) {
            helper.openModalExcluir("Excluir Movimentação", data, movimentacoes);
        });

    },

    excluirConta: function () {
        var id = $("#Id").val();

        var url = "/Contas/excluir";
        $.ajax({
            url: url
            , datatype: "json"
            , type: "POST"
            , data: { id: id }
        }).done(function (data) {
            if (data.Resultado) {
           //     movimentacoes.listar();
                helper.fecharModalExclusao();
            } else {
                alert("Erro ao excluir registro, verifique os dados e tente novamente");
            }
        });
    },

    limpar: function () {
        $("#nome").val();
        $("#listarclientes").html("");
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
        var url = "/Movimentacoes/ConfirmacaoExcluirModal";
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
            helper.openModalExcluir("Excluir Movimentação", data, movimentacoes);
        });


    },

    Delete() {
        var ide = $("#identificador").val();
        var url = "/Movimentacoes/Delete";
        $.ajax({
            url: url,
            datatype: "html",
            type: "POST",
            data: { id: ide }
        }).done(function (data) {
            if (data.success) {
                dataTable.ajax.reload();
                helper.fecharModalExclusao();
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
                        dataTable.ajax.reload();

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
        movimentacoes.novo();
    });

    $("#btnNovaMovimentacaoDeSaida").bind('click', function () {
        movimentacoes.novoMovimentacaoSaida();
    });

    $("#btnNovaContaAhPagar").bind('click', function () {
        movimentacoes.novoMovimentacaoAhPagar();
    });

    $("#btnNovaContaAhReceber").bind('click', function () {
        movimentacoes.novoMovimentacaoAhReceber();
    });

    // Listar
    $("#buscaCliente").click(function () {
    //    movimentacoes.listar();
    });

    $("#limparCliente").click(function () {
        movimentacoes.limpar();
    });

    $("#moviEditar").click(function () {
        movimentacoes.editar();
    });

});