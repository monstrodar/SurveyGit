function PreencherTabela(dados) {
    var txt = '<thead>\
            <tr>\
                <th>#ID</th>\
                <th>Título</th>\
                <th>Início</th>\
                <th>Fim</th>\
                <th>Guid</th>\
                <th>...</th>\
            </tr>\
        </thead >\
        <tbody>';
    $.each(dados, function () {
        txt += '<tr><td>' + this.Id + '</td><td>' + this.Nome + '</td><td>' + FormatarData(this.Inicio) +
            '</td><td>' + FormatarData(this.Fim) + '</td><td>' + this.Guid + '</td><td>\
                <a role="button" class="btn btn-warning" href="javascript:Alterar(' + this.Id + ')">Alterar</a>\
                <a role="button" class="btn btn-danger" href="javascript:Excluir(' + this.Id + ')">Excluir</a>\
                </td></tr>';
    });
    txt += '</tbody>';
    $("#tableQuestionarios").html(txt);
};

function ObterQuestionarios() {
    $("#divLoading").show(300);
    $.getJSON("/Questionario/ObterPorUsuario/" + getCookie("token", 0), function (data) {
        PreencherTabela(data);
    });
    $("#divLoading").hide(300);
};

$(document).ready(function () {
    ObterQuestionarios();
});

$("#btnPesquisar").click(function () {
    if ($("#txtPalavraChave").val() == "") {
        ObterQuestionarios();
    }
    else {
        $("#divLoading").show(300);
        $.ajax({
            type: 'POST',
            url: '/Questionario/ObterPorPalavraChave',
            data: { Palavra: $("#txtPalavraChave").val(), idUsuario: getCookie("token", 0) },
            success: function (result) {
                if (result != null && result.length > 0) {
                    PreencherTabela(result);
                }
                else {
                    bootbox.alert("Nenhum questionário encontrado.");
                }
                $("#divLoading").hide(300);
            },
            error: function (XMLHttpRequest, txtStatus, errorThrown) {
                alert("Status: " + txtStatus); alert("Error: " + errorThrown);
                $("#divLoading").hide(300);
            }
        });
    }
});

$("#btnConfirmar").click(function () {
    var msg = "";
    var id = $("#txtId").val();
    var titulo = $("#txtTitulo").val();
    var inicio = $("#txtDataInicio").val();
    var fim = $("#txtDataFim").val();
    var feedback = $("#txtFeedback").val();
    var guid = $("#txtGuid").val();

    if (titulo == "") {
        msg += "Por favor, informe um título para o questionário.<br />";
    }
    if (inicio == "") {
        msg += "Por favor, informe a data para início do questionário.<br />";
    }
    if (fim == "") {
        msg += "Por favor, informe a data para fechamento do questionário.<br />";
    }
    if (guid == "") {
        msg += "Por favor, informe a Guid (URL) para o questionário.<br />";
    }
    if (msg.length > 0) {
        Mensagem("divAlertaNovoQuestionario", msg);
    }
    else {
        $("#divLoading").show(300);
        $.ajax({
            type: 'POST',
            url: '/Questionario/Gravar',
            data: { Id: id, Nome: titulo, Inicio: inicio, Fim: fim, MsgFeedBack: feedback, Guid: guid },
            success: function (result) {
                $("#divLoading").hide(300);
                if (result.length > 0) {
                    Mensagem("divAlertaNovoQuestionario", result);
                }
                else {
                    LimparFormulario();
                    $.fancybox.close();
                    ObterQuestionarios();
                }
            },
            error: function (XMLHttpRequest, txtStatus, errorThrown) {
                alert("Status: " + txtStatus); alert("Error: " + errorThrown);
                $("#divLoading").hide(300);
            }
        });
    }
});

function Alterar(id) {
    $("#divLoading").show(300);
    $.ajax({
        type: 'POST',
        url: '/Questionario/Obter',
        data: { Id: id, IdUsuario: getCookie("token", 0) },
        success: function (result) {
            if (Object.keys(result).length > 0) {
                $("#divLoading").hide(300);
                $.fancybox.open({
                    src: '#formQuestionario',
                    type: 'inline'
                });
                $("#txtId").val(result.Id);
                $("#txtTitulo").val(result.Nome);
                $("#txtDataInicio").val(FormatarDataIso(result.Inicio));
                $("#txtDataFim").val(FormatarDataIso(result.Fim));
                $("#txtFeedback").val(result.MsgFeedback);
                $("#txtGuid").val(result.Guid);
            }
            else {
                $("#divLoading").hide(300);
            }
        },
        error: function (XMLHttpRequest, txtStatus, errorThrown) {
            alert("Status: " + txtStatus); alert("Error: " + errorThrown);
            $("#divLoading").hide(300);
        }
    });
};

function Excluir(id) {
    bootbox.confirm({
        message: "Confirma a exclusão deste registro?",
        buttons: {
            confirm: {
                label: 'Sim',
                className: 'btn-success'
            },
            cancel: {
                label: 'Não',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    type: 'POST',
                    url: '/Questionario/Excluir',
                    data: { Id: id },
                    success: function (result) {
                        if (result == "") {
                            ObterQuestionarios();
                        }
                        else {
                            Mensagem("divAlerta", result);
                        }
                    },
                    error: function (XMLHttpRequest, txtStatus, errorThrown) {
                        alert("Status: " + txtStatus); alert("Error: " + errorThrown);
                        $("#divLoading").hide(300);
                    }
                });
            }
        }
    });
};

function LimparFormulario() {
    $("#txtId").val("0");
    $("input[type='text']").val("");
    $("input[type='date']").val("0000-00-00");
    $("textarea").val("");
}
