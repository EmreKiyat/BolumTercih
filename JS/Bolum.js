$(document).ready(function (e) {
    $('.search-panel .dropdown-menu').find('a').click(function (e) {
        e.preventDefault();
        var param = $(this).attr("href").replace("#", "");
        var concept = $(this).text();
        $('.search-panel span#search_concept').text(concept);
        $('.input-group #search_param').val(param);
    });
});

$(function () {

    $("#txtAra").on("keypress", function (e) {
        var regex = new RegExp("^[\a-zA-Z0-9çğüşiöçĞÜŞİÖÇ., ]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            $("#txtAra").removeClass("hataBorder");
            return true;
        }
        var code = e.keyCode || e.which;
        if (code == 13) { getBolumListBySearchText(1); }
        e.preventDefault();
        return false;
    });

    $('#btnAra').on('click', function () {
        getBolumListBySearchText(1);
    });
    
    function getBolumListBySearchText(PG) {
        var txtAra = $('#txtAra').val();
        ssPage = PG;
        $.ajax({
            url: '/Bolum/GetBolumListBySearchText',
            type: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            processData: false,
            data: JSON.stringify({ TX: txtAra, PG: PG }),
            beforeSend: function () { $("#loadingimg").show(); },
            success: function (res) {

                $("#bolumTX").html(Handlebars.compile($("#BolumTXListType").html())({ Data: res }));
                if (res.length > 0) {
                    $("#tHead").removeClass("hidden");
                    $("#blmRight").removeClass("hidden");
                    $("#blmLeft").removeClass("hidden");

                } else {
                    $("#tHead").addClass("hidden");
                    $("#blmRight").addClass("hidden");
                    $("#blmLeft").addClass("hidden");
                }

            },
            complete: function () { $("#loadingimg").hide(); },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Eror: " + errorThrown + "-----" + XMLHttpRequest.statusText);
            }
        });

    }


    $("#blmRight").on("click", function (event) {
        
        ssPage++;
        location.href = location.href + ssPage.toString();
        if (ssPage == 2) $("#blmLeft").removeClass("disabled");
        getBolumListBySearchText(ssPage);
        event.preventDefault();

    });
    $("#blmLeft").on("click", function (event) {
        ssPage--;
        if (ssPage < 2) $("#blmLeft").addClass("disabled");
        
        getBolumListBySearchText(ssPage);

        event.preventDefault();
        $("#blmRight").removeClass("disabled");
    });


    
});