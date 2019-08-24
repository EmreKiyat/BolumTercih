$(function () {
    

    $('#sortable').sortable({
        cursor: "move",
        start: function (event, ui) {
            var start_pos = ui.item.index();
            ui.item.data('start_pos', start_pos);
            ui.item.find('.bolumKartLeft').html('?');
        },
        change: function (event, ui) {
            var start_pos = ui.item.data('start_pos');
            var index = ui.placeholder.index();
            if (start_pos < index) {
                ui.item.find('.bolumKartLeft').html(index);
            } else {
                ui.item.find('.bolumKartLeft').html(index+1);
            }
            
        },
        update: function (event, ui) {
            if ($('#sortable').sortable('toArray').length > 30) $('#sortable').sortable('cancel'); else {
                sortKartOrder();
                
            }
        },
        stop: function( event, ui ) {document.getElementById("btnKaydet").disabled = false;}
    });






    function moveUp(item) {
        var prev = item.prev();
        if (prev.length == 0)
            return;
        prev.css('z-index', 999).css('position', 'relative').animate({ top: item.height() }, 250);
        item.css('z-index', 1000).css('position', 'relative').animate({ top: '-' + prev.height() }, 300, function () {
            prev.css('z-index', '').css('top', '').css('position', '');
            item.css('z-index', '').css('top', '').css('position', '');
            item.insertBefore(prev);
            sortKartOrder();
        });
    }
    function moveDown(item) {
        var next = item.next();
        if (next.length == 0)
            return;
        next.css('z-index', 999).css('position', 'relative').animate({ top: '-' + item.height() }, 250);
        item.css('z-index', 1000).css('position', 'relative').animate({ top: next.height() }, 300, function () {
            next.css('z-index', '').css('top', '').css('position', '');
            item.css('z-index', '').css('top', '').css('position', '');
            item.insertAfter(next);
            sortKartOrder();
        });
    }


    $('#sortable').on("click", '.fa-caret-up', function () {
        var btn = $(this);
        moveUp(btn.parents('.bolumKart'));
        document.getElementById("btnKaydet").disabled = false;
    });

    $('#sortable').on("click", ".fa-caret-down", function () {
        var btn = $(this);
        moveDown(btn.parents('.bolumKart'));
        document.getElementById("btnKaydet").disabled = false;
    });

    $("#sortable").on("mouseenter", ".bolumKart", function () {
        $('> .bolumKartRight', this).stop().fadeTo('slow', 1);
    });


    $("#sortable").on("mouseleave", ".bolumKart", function () {
        $('> .bolumKartRight', this).stop().fadeTo('slow', 0);
    });


    $("#sortable").on("click", ".bolumKartMid", function () {
        bolumDetayGoster(this);
    });
    function bolumDetayGoster(obj) {
        $('#dvBolumDetay').switchClass("hidden", "visible", 600, "easeInOutQuad");
        $('#dvBolumByFilter').switchClass("visible", "hidden", 400, "easeInOutQuad");
        $('#dvSearchAndFilter').switchClass("visible", "hidden", 400, "easeInOutQuad");
        $("#dtBK").html($('> h5', obj).attr("title"));
        $("#dtBL").html($('> h5', obj).text());
        $("#dtFK small").html($('> span', obj).attr("title"));
        $("#dtUN small").html($('> span', obj).text());
        $("#dtDV").html($('> p > span:eq(1)', obj).text());
        $("#dtSU").html($('> p > span:eq(5)', obj).text());
        $("#dtPT").html($('> p > span:eq(0)', obj).text());
        $("#dtptKN").html($('> p > span:eq(6)', obj).text());
        $("#dtBS").html($('> p > span:eq(2)', obj).text());
        $("#dtEK").html($('> p > span:eq(3)', obj).text());
        //Koşulları al
        getBolumKosul($(obj).parent().attr("id"));
        
    }

    function getBolumKosul(BolumId) {
        
        $.ajax({
            url: '/Home/GetBolumKosul',
            type: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            processData: false,
            data: JSON.stringify({ Id: BolumId }),
            success: function (res) {
                
                $("#dvKosullar").html(Handlebars.compile($("#KosulType").html())({ Data: res }));
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Error: " + errorThrown + "-----" + XMLHttpRequest.statusText);
            }
        });


    }

    $("#sortable").on("click", ".fa-remove", function (e) {
        $(this).parent().parent().parent().fadeOutAndRemove('slow');
        e.preventDefault();
    });

    jQuery.fn.fadeOutAndRemove = function (speed) {
        $(this).fadeOut(speed, function () {
            $(this).remove();
            sortKartOrder();

            if ($("#numOfBolum").text() == "0") {
                $("#sortable").append(bosListText);
                
            }
            document.getElementById("btnKaydet").disabled = false;
        })
    }


    $("#btnYaz").on("click", function () {
       

        var tableHead = "<table class='table table-hover table-striped table-condensed' style='font-size:10px;'>"
        tableHead+="<thead class='TableHeadColor'><tr><th>#</th><th>Bölüm</th><th>Devlet/Vakıf</th><th>Puan Tür</th><th>Süre</th><th>Kontenjan</th><th>Başarı Sıra</th><th>EK Puan</th><th>Koşullar</th></tr></thead><tbody >"
        var tableClose = "</table>"
        var mRow = "";
        $("#sortable .bolumKart").each(function () {
            var sira = $(this).find(".bolumKartLeft").text();
            var bAd = $(this).find("h5").text();
            var bKod = $(this).find("h5").attr("title");
            var bUni = $(this).find(".lblUniv").text();
            var bFak = $(this).find(".lblUniv").attr("title");
            var bPT = $(">div>p>span[title='Puan Türü']",this).text();
            var bDV = $(">div>p>span[title='Devlet-Vakıf']",this).text();
            var bSR = $(">div>p>span[title='Sıra(2014)']", this).text();
            var bEK = $(">div>p>span[title='En Küçük Puan(2014)']", this).text();
            var bKosul = $(">div>p>span[title='Koşullar']", this).text();
            var bSure = $(">div>p>span:nth-child(6)", this).text().substring(3, 2);
            var bKnt = $(">div>p>span:nth-child(7)", this).text();

            mRow += "<tr style='border-bottom:2px solid'><td><strong>"+sira+"</strong></td><td><span style='color:#008080'>" + bKod + "</span>&nbsp;&nbsp;&nbsp;<span><strong>" + bAd + "</strong></span><br /><span class='mic-info'><small>" + bUni + "</small></span></td>"
            mRow += "<td>"+bDV+"</td><td>"+bPT+"</td><td>"+bSure+"</td><td>"+bKnt+"</td><td>"+bSR+"</td><td>"+bEK+"</td><td>"+bKosul+"</td></tr>"

        });


        var divContents = tableHead + mRow + tableClose;
        var printWindow = window.open('', '', 'height=500,width=1000');
        printWindow.document.write('<html><head><title>BölümTercih.com</title>');
        printWindow.document.write('</head><body ><h3>Tercih Listem</h3>');
        printWindow.document.write(divContents);
        printWindow.document.write('</body></html>');
        printWindow.document.close();
        printWindow.print();
    });



});
