$(document).ready(function () {
    showHideInputs($("#selectTipoPessoa").children("option:selected").val());
    adicionaEventoBtnRemover();
    $("#remover-telefone0").remove();

    $("#selectTipoPessoa").on("change", function () {
        $("#selectTipoPessoa option[value='']").remove();
        showHideInputs(this.value);
    });
});

$(document).ajaxComplete(function () {
    adicionaEventoBtnRemover();

    let indice = $(".telefones").length;

    if (indice > 2)
        $(`#remover-telefone${indice - 2}`).hide();
});

function adicionaEventoBtnRemover() {
    $(".btnRemover").on("click", function (event) {
        event.preventDefault();
        let name = this.id.split("-");
        $("#" + name[1]).remove();

        let indice = $(".telefones").length;

        if (indice >= 2)
            $(`#remover-telefone${indice - 1}`).show();
    });
}

function ajaxAdicionaTelefone(url, nomeLista, indice, divId) {
    $.ajax({
        url: url,
        data: { indice, nomeLista, divId },
        cache: false,
        success: function (html) { $("#telefonesDiv").append(html); },
        error: function (error) { console.log(error) }
    });
}

function showHideInputs(valorSelect) {
    let pessoaFisica = $(".pessoa-fisica");
    let pessoaJuridica = $(".pessoa-juridica");

    if (!valorSelect)
        return;

    $("#itensFormulario").removeClass("d-none");

    if (valorSelect == "PF") {
        pessoaFisica.show();
        pessoaJuridica.hide();
        return;
    }
    if (valorSelect == "PJ") {
        pessoaJuridica.show();
        pessoaFisica.hide();
        return;
    }
}