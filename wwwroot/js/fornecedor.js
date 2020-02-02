$(document).ready(function () {
    adicionaEventoBtnRemover();
    $("#remover-telefone0").remove();
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