$(function () {
    var local = window.localStorage;

    if (!testLC()) {
    $('#myHtml5Modal').modal({
        backdrop: 'static'
    });
    }

    $("#btnGoChrome").on('click', function () {
        window.open("https://www.google.com/chrome", "_self");
    });
    $("#btnGoFirefox").on('click', function () {
        window.open("https://www.mozilla.org/tr/firefox/new", "_self");
    });


    var isApproved = local.getItem("bolTerApproved");
    if (isApproved == null || isApproved == "false") {
        $('#myModal').modal({backdrop: 'static'});
    }

    $("#btnNotOk").on('click', function () {
        local.setItem("bolTerApproved", "false");
        window.open("https://www.google.com", "_self");
    });
    $("#btnOk").on('click', function () {
        local.setItem("bolTerApproved", "true");
        $('#myModal').modal("hide");
        
    });

    filterObj = getFilterObj();
    setFilterTags();
    LocalId = local.getItem("LocalId");
    if (LocalId == null) {
        $("#sortable").append(bosListText);
        
    }
    if (LocalId != null) getAndRenderBolumList();


    
    $("#cmbDV").on("click", "li", function () {
        var id = this.id;
        var txt = $('> a', this).text();
        if ($('> a > i', this).hasClass('fa-check')) {
            $('#b' + id).remove();
            $('> a > i', this).removeClass('fa-check');
            filterObj = setFilterObj(id, 'DV', txt, 'remove');
            removeSpace('DV');
        } else {
            createTag(id, txt, 'DV');
            $('> a > i', this).addClass('fa-check');
            filterObj = setFilterObj(id, 'DV', txt, 'insert');
        }
    });

    $("#cmbPuanTur").on("click", "li", function () {
        var id = this.id;
        var txt = $('> a', this).text();
        if ($('> a > i', this).hasClass('fa-check')) {
            $('#b' + id).remove();
            $('> a > i', this).removeClass('fa-check');
            filterObj = setFilterObj(id, 'PT', txt, 'remove');
            removeSpace('PT');
        } else {
            createTag(id, txt, 'PT');
            $('> a > i', this).addClass('fa-check');
            filterObj = setFilterObj(id, 'PT', txt, 'insert');
        }
    });
        
    $("#cmbSehir").on("click", "li", function () {
        var id = this.id;
        var txt = $('> a', this).text();
        if ($('> a > i', this).hasClass('fa-check')) {
            $('#b' + id).remove();
            $('> a > i', this).removeClass('fa-check');
            filterObj = setFilterObj(id, 'SH', txt, 'remove');
            removeSpace('SH');
        } else {
            createTag(id, txt, 'SH');
            $('> a > i', this).addClass('fa-check');
            filterObj = setFilterObj(id, 'SH', txt, 'insert');
        }
    });

    $('#pnlDV').on("click", "span i",function() {
        var myId = $(this).parent().parent().attr('id').substr(1);
        $(this).parent().parent().remove();
        $('#' + myId + ' > a > i').removeClass('fa-check');
        var txt = $('#' + myId + ' > a').text();
        filterObj = setFilterObj(myId, 'DV', txt, 'remove');
        removeSpace('DV');
    });

    $('#pnlSH').on("click", "span i", function () {
        var myId = $(this).parent().parent().attr('id').substr(1);
        $(this).parent().parent().remove();
        $('#' + myId + ' > a > i').removeClass('fa-check');
        var txt = $('#' + myId + ' > a').text();
        filterObj = setFilterObj(myId, 'SH', txt, 'remove');
        removeSpace('SH');

    });

    $('#pnlPT').on("click", "span i", function () {
        var myId = $(this).parent().parent().attr('id').substr(1);

        $(this).parent().parent().remove();
        $('#' + myId + ' > a > i').removeClass('fa-check');
        var txt = $('#' + myId + ' > a').text();
        filterObj = setFilterObj(myId, 'PT', txt, 'remove');
        removeSpace('PT');
    });

    function removeSpace(pnl) {
        $('#pnl' + pnl).html($('#pnl' + pnl).html().replace('&nbsp;', ''));
    }

    function getFilterObj() {       // filter obje ls'de yoksa default'u ekle ve return.
        
        if (local.getItem("filterObj") != null) {
            return JSON.parse(local.getItem("filterObj"));
        }
        else {
            local.setItem('filterObj', JSON.stringify(filterObj));
            return filterObj;
        }
    };
    
    function setFilterTags() {
        $.each(filterObj.DV, function (key,value) {
            createTag(value.Id, value.T, 'DV'); +

            $('#' + value.Id + ' > a > i').addClass('fa-check');
        });
        $.each(filterObj.PT, function (key, value) {
            createTag(value.Id, value.T,'PT');
            $('#' + value.Id + ' > a > i').addClass('fa-check');
        });
        $.each(filterObj.SH, function (key, value) {
            createTag(value.Id, value.T, 'SH');
            $('#' + value.Id + ' > a > i').addClass('fa-check');
        });

    }


    function createTag(id, txt,ObjType) {
        //var temp = "<button type='button' id='b" + id + "' class='btn btn-labeled btn-default'><span class='btn-label'><i class='fa fa-remove fa-fw'></i></span>" + txt + "</button>&nbsp;"
        var temp = "<span type='button' id='b" + id + "' class='btn btn-labeled btn-default'><span class='btn-label'><i class='fa fa-remove fa-fw'></i></span>" + txt + "</span>&nbsp;"
        $("#pnl"+ObjType).append(temp);
    }


    function setFilterObj(ObjId, ObjType, ObjText, action) {
        //var obj = JSON.parse(jsonStr);
        if (action == 'insert') filterObj[ObjType].push({ "Id": ObjId, "T": ObjText }); else {
            
            if (ObjType == "DV") filterObj.DV = $.grep(filterObj.DV, function (e) { return e.Id != ObjId });
            if (ObjType == "PT") filterObj.PT = $.grep(filterObj.PT, function (e) { return e.Id != ObjId });
            if (ObjType == "SH") filterObj.SH = $.grep(filterObj.SH, function (e) { return e.Id != ObjId });
            
        };

        local.setItem('filterObj', JSON.stringify(filterObj));
        return filterObj;
    };


    function getLocalId() {
        if (local.getItem("LocalId") != null)
            return local.getItem("LocalId");
        else
            return (getIdFromServer());
            
    };

    function testLC() {
        try {
            local.setItem("a", "a");
            local.removeItem("a");
            return true;
        } catch (e) {
            return false;
        }
    };



    function getIdFromServer() {
        var tmpID=0;
        $.ajax({
            url: '/Home/GetNewID',
            type: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            processData: false,
            data: myListStr, 
            success: function (res) {
                tmpID = res.newId;
                local.setItem("LocalId", tmpID);
                local.setItem("TerTarih",$.datepicker.formatDate('dd.mm.yy', new Date()));
                document.getElementById("btnKaydet").disabled = true;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Error: " + errorThrown + "-----" + XMLHttpRequest.statusText);
            }
        });
        
        return tmpID;
    }

    function updateMyList() {
        $.ajax({
            url: '/Home/UpdateTercihList',
            type: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            processData: false,
            data: JSON.stringify({ tJson: JSON.parse(myListStr), tId: LocalId }),
            beforeSend: function () { $("#loadingimg").show(); },
            success: function (res) {
                document.getElementById("btnKaydet").disabled = true;
                local.setItem("TerTarih", $.datepicker.formatDate('dd.mm.yy', new Date()));
                $("#spnTarih").text(local.getItem("TerTarih"));
            },
            complete: function () { $("#loadingimg").hide(); },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Eror: " + errorThrown + "-----" + XMLHttpRequest.statusText);
            }
        });

    }

    function getAndRenderBolumList() {
        $.ajax({
            url: '/Home/GetTercihListById',
            type: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            processData: false,
            data: JSON.stringify({ Id: LocalId }),
            beforeSend: function () { $("#loadingimg").show(); },
            success: function (res) {
                document.getElementById("btnKaydet").disabled = true;
                $("#sortable").html(Handlebars.compile($("#BolumListType").html())({ Data: res }));
                $("#numOfBolum").text(Object.keys(res).length);
                if (Object.keys(res).length == 0) $("#sortable").append(bosListText);
                $("#spnTarih").text(local.getItem("TerTarih"));
                sortKartOrder();
                
            },
            complete: function () { $("#loadingimg").hide(); },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Eror: " + errorThrown + "-----" + XMLHttpRequest.statusText);
            }
        });

    }

    $("#btnKaydet").on("click", function () {
        LocalId = getLocalId();
        if (!document.getElementById("btnKaydet").disabled)
        {
            updateMyList(myListStr);
        }
        
    });

    $("#txtAra").on("keyup", function (k) {
        if ($("#txtAra").val().length < 2) return true;
        if (k.keyCode == 13) {
            $("#btnAra").click();
        }
        return true;

    });

    //arama tarama
    $("#btnAra").on("click", function (event) {
        //regex control
        var regex = new RegExp("^[\a-zA-Z0-9çğüşiöçĞÜŞİÖÇ., ]*$");
        if (!$("#txtAra").val().match(regex)) { $("#txtAra").addClass("hataBorder");  return false; }
        //-
        filterObj.PG = 1;
        ssPage = 1;
        filterObj.TX = $("#txtAra").val();
        GetBolumList();
        event.preventDefault();
        $('#dvSearchAndFilter').switchClass("visible", "hidden", 400, "easeInOutQuad");
        $('#dvBolumByFilter').switchClass("hidden", "visible", 600, "easeInOutQuad");
        LastRight = "dvBolumByFilter";

    });
    $("#blmRight").on("click", function (event) {
        ssPage++;
        if (ssPage == 2) $("#blmLeft").removeClass("disabled");
        filterObj.PG = ssPage;
        filterObj.TX = $("#txtAra").val();
        GetBolumList();
        event.preventDefault();
        LastRight = "dvBolumByFilter";

    });
    $("#blmLeft").on("click", function (event) {
        ssPage--;
        if (ssPage < 2) $("#blmLeft").addClass("disabled");
        filterObj.PG = ssPage;
        filterObj.TX = $("#txtAra").val();
        GetBolumList();
        
        event.preventDefault();
        LastRight = "dvBolumByFilter";
        $("#blmRight").removeClass("disabled");
    });



    function GetBolumList() {
        //jQuery.support.cors = true;
        $.ajax({
            //cache: false,
            //async: false,
            url: '/Home/GetBolumList',
            type: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            processData: false,
            data: JSON.stringify(filterObj),            
            beforeSend: function (xhr) {
                $("#loadingimgR").show();
            },
            success: function (res) {
                $("#bolumTree").html(Handlebars.compile($("#BolumFilterType").html())({ Data: res }));
                myfilterResult = res;
                numberOfBolumReturned = res.length;
                if (numberOfBolumReturned < 12) $("#blmRight").addClass("disabled");
            },
            complete: function () { $("#loadingimgR").hide(); },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Eror: " + errorThrown + "-----" + XMLHttpRequest.statusText);
            }
        });
    }

    //sortable'ye bölüm eklemece
    $("#bolumTree").on("click", ".bolumAdd", function (k) {
        var myId = this.id;
        
        if (myListStr.indexOf(myId) > -1) return false;
        if ($("#numOfBolum").text() == "0") $("#sortable").empty();
        var bolumToAdd = $.grep(myfilterResult, function (e) { return e.tId == myId });
        bolumToAdd[0].Sira = $("#numOfBolum").text();
        var bolumHtml = Handlebars.compile($("#BolumListType").html())({ Data: bolumToAdd });
        
        if (parseInt($("#numOfBolum").text()) >= 30) {
            return false;
        } else {
            $("#sortable").append(bolumHtml);
            sortKartOrder();
            document.getElementById("btnKaydet").disabled = false;
        }
        k.preventDefault();
    });




    $('#btnFilterClose').on('click', function (event) {
            $('#dvBolumByFilter').switchClass("visible", "hidden", 400, "easeInOutQuad");
            $('#dvSearchAndFilter').switchClass("hidden", "visible", 600, "easeInOutQuad");
            LastRight = "dvSearchAndFilter";
            event.preventDefault();
    });
    $("#btnDetayClose").on('click', function () {
        $('#dvBolumDetay').switchClass("visible", "hidden", 400, "easeInOutQuad");
        $('#' + LastRight).switchClass("hidden", "visible", 600, "easeInOutQuad");
    });

    //ara regex control

    $('#txtAra').on('keypress',function (e) {
        var regex = new RegExp("^[\a-zA-Z0-9çğüşiöçĞÜŞİÖÇ., ]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            $("#txtAra").removeClass("hataBorder");
            return true;
        }

        e.preventDefault();
        return false;
    });





});



