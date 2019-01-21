function ProductKO() {
    var self = this;
    self.Success = ko.observable(false);
    self.Error = ko.observable(false);
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");

    self.savebtn = ko.observable(true);
    self.updatebtn = ko.observable(false);

    self.insert = ko.observable(false);
    self.index = ko.observable(true);

    self.id = ko.observable(0);
    self.productname = ko.observable("");
    self.productcode = ko.observable("");
    self.productcategoryid = ko.observable("");
    self.productshortsedcription = ko.observable("");
    self.productdescription = ko.observable("");
    self.isactive = ko.observable(false);
    self.isfeatured = ko.observable(false);
    self.productprice = ko.observable("");
    self.pagetitle = ko.observable("");
    self.producturl = ko.observable("");
    self.productrank = ko.observable("");
    self.metakeywords = ko.observable("");
    self.metadescription = ko.observable("");
    self.videourl = ko.observable("");
    self.productattribute = ko.observableArray();
    self.Catgory = ko.observableArray();
    self.Catgorys = ko.observableArray();
    self.Stocks = ko.observableArray();
    self.Statuses = ko.observableArray();
    self.each = ko.observable("");

    self.imagename = ko.observable();
    self.edit = ko.observable(false);

    self.products = ko.observableArray();
    self.totalrecord = ko.observable(0);

    self.tb1 = ko.observable(true);
    self.tb2 = ko.observable(false);
    self.tb3 = ko.observable(false);
    self.tb4 = ko.observable(false);

    //paging
    self.currentPage = ko.observable();
    self.pageSize = ko.observable(50);
    self.currentPageIndex = ko.observable(1);
    self.sortType = "ascending";
    self.currentColumn = ko.observable("");
    self.iconType = ko.observable("");

    //select multiselect dropdown
    self.multiselectdropdown = ko.observable(false);
    self.multiattribtevalue = ko.observableArray();

    self.entrsv = function (d, e) {
        if (e.keyCode == 13) {
            if (self.updatebtn() == true) {
                self.Update(d);
            }
            else {
                self.Save(d);
            }
        }
    }

    self.clear = function () {
        self.id(0);
        self.productname("");
        self.productcode("");
        self.productprice("");
        self.pagetitle("");
        self.productrank(1);
        self.producturl("");
        self.metakeywords("");
        self.videourl("");
        self.metadescription("");
        self.productcategoryid("");
        self.productshortsedcription("");
        self.isactive(false);
        self.isfeatured(false);
        $('#fine-uploader-manual-trigger div ul li').remove();
        $('.multislect_drop ul li').remove();
        $('.multislect_drop ul').append("<li style=\"list-style:none;\"><input type=\"checkbox\" data-bind=\"checked:IsChecked,event:{change: $root.openmultiselect }\"><span data-bind=\"text:attributename\"></span></li>");
        $("#col div").remove();


    }

    self.Tab1 = function () {
        $('#txtproductname').focus();
        self.tb1(true);
        self.tb2(false);
        self.tb3(false);
        self.tb4(false);
        $('#tb1').addClass('active');
        $('#tb2').removeClass('active');
        $('#tb3').removeClass('active');
        $('#tb4').removeClass('active');
        self.Error(false);
        self.Success(false);
    }

    self.Tab2 = function () {
        self.tb1(false);
        self.tb2(true);
        self.tb3(false);
        self.tb4(false);
        $('#tb2').addClass('active');
        $('#tb1').removeClass('active');
        $('#tb3').removeClass('active');
        $('#tb4').removeClass('active');

    }

    self.Tab3 = function () {
        self.tb1(false);
        self.tb2(false);
        self.tb3(true);
        self.tb4(false);
        $('#tb3').addClass('active');
        $('#tb2').removeClass('active');
        $('#tb1').removeClass('active');
        $('#tb4').removeClass('active');

    }

    self.Tab4 = function () {
        self.tb1(false);
        self.tb2(false);
        self.tb4(true);
        self.tb3(false);
        $('#tb4').addClass('active');
        $('#tb2').removeClass('active');
        $('#tb1').removeClass('active');
        $('#tb3').removeClass('active');

    }

    self.getselectedpages = function (products) {
        self.id(products.id);
        self.productname(products.productname);
        self.productcode(products.productcode);
        self.productprice(products.productprice);
        self.pagetitle(products.pagetitle);
        self.productrank(products.productrank);
        self.producturl(products.producturl);
        self.metakeywords(products.metakeywords);
        self.metadescription(products.metadescription);
        self.productcategoryid(products.productcategoryid);
        self.productshortsedcription(products.productshortsedcription);
        self.isactive(products.isactive);
        self.isfeatured(products.isfeatured);
        self.videourl(products.videourl);
        self.Error(false);
        self.Success(false);
        self.index(false);
        self.insert(true);
        self.updatebtn(true);
        self.savebtn(false);
        getattributes(self.id());
        self.edit(true);
        getimages(self.id());


        $('#txtproductname').focus();
    }
    function getimages(id) {


        $.ajax({

            type: "GET",
            url: '/Product/GetImages/' + id,
            Contenttype: "application/JSON",

            success: function (data) {
                var html1 = "";
                for (var i = 0; i < data.length; i++) {
                    var html = "";

                    html = "<li class=\"qq-file-id-1\" qq-file-id=\"1\"><div class=\"qq-progress-bar-container-selector qq-hide\"><div role=\"progressbar\" aria-valuenow=\"0\" aria-valuemin=\"0\" aria-valuemax=\"100\" class=\"qq-progress-bar-selector qq-progress-bar\"></div></div><span class=\"qq-upload-spinner-selector qq-upload-spinner qq-hide\"></span>"
                        + "<img class=\"qq-thumbnail-selector\" qq-max-size=\"100\" qq-server-scale=\"\" id=\"imgproduct\" src=\"" + data[i].imagename + "\">"
                        + "<span class=\"qq-upload-file-selector qq-upload-file qq-editable\" title=\"amitab.png\">" + data[i].imagetitle + "</span>"
                        + "<span class=\"qq-edit-filename-icon-selector qq-edit-filename-icon qq-editable\" aria-label=\"Edit filename\" onclick=\"EditShow(this.id);\" id=\"edit" + data[i].id + "\"></span>"
                        + "<input class=\"qq-edit-filename-selector qq-edit-filename\" tabindex=\"0\" type=\"text\">"
                        + "<span class=\"qq-upload-size-selector qq-upload-size qq-hide\"></span>"

                        + "<button type=\"button\" class=\"qq-btn qq-upload-cancel-selector qq-upload-cancel\" id=\"" + data[i].id + "\" onclick=\"CancelRecord(this.id);\">Cancel</button>"
                        + "<span role=\"Status\" class=\"qq-upload-Status-text-selector qq-upload-Status-text\"></span>"
                        + "</li>";
                    html1 = html1 + html;
                }

                $('#fine-uploader-manual-trigger ul').append(html1);



            }

        });
    }

    // self.producturl = self.productname(data);
    //$.ajax({
    //    type: "POST",
    //    url: '/Product/',
    //    ContentType: "application/Json",
    //    success : function (data){

    //        $('#loader').hide();
    //    }
    //});

    self.Update = function (data) {

        var imagename = "";
        var imagetitle = "";
        for (var i = 0; i < $('#fine-uploader-manual-trigger ul li').length; i++) {

            imagename = imagename + "$" + $('#fine-uploader-manual-trigger ul li:eq(' + i + ') img').attr('src');
            imagetitle = imagetitle + "$" + $('#fine-uploader-manual-trigger ul li:eq(' + i + ') span:eq(1)').text();
        }
        var attributeids = ""; attributesidvalue = "", attributeandvalues = "", attrbutesfinals = "";

        for (var i = 0; i < self.productattribute().length; i++) {
            if (self.productattribute()[i].IsChecked == true) {
                attributeids = self.productattribute()[i].attributeid;

                for (var j = 2; j < $('#prd' + self.productattribute()[i].attributeid + ' span div ul li').length; j++) {
                    if (j == 2) {

                        attributesidvalue = ';';
                    }

                    if ($('#prd' + self.productattribute()[i].attributeid + ' span div ul li:eq(' + j + ') a').find("input[type='checkbox']:checked").val() != undefined) {
                        attributesidvalue = attributesidvalue + "," + $('#prd' + self.productattribute()[i].attributeid + ' span div ul li:eq(' + j + ') a').find("input[type='checkbox']:checked").val();
                    }
                }
                attributeandvalues = attributeids + attributesidvalue;

                attrbutesfinals = attrbutesfinals + ':' + attributeandvalues;

            }
        }
        data.imagename(imagename);
        jQuery.fn.CKEditorValFor = function (element_id) {
            return CKEDITOR.instances[element_id].getData();
            //var ckval = CKEDITOR.instances[element_id].getData();
        }
        data.productdescription = $().CKEditorValFor('txtproductdesc');
        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax({
                url: '/Product/Update/?attrbutesfinals=' + attrbutesfinals + "&imagetitle=" + imagetitle,
                type: "POST",
                dataType: 'json',
                data: ko.toJSON(data),
                contentType: "application/json",
                success: function (data) {
                    self.Error(false);
                    self.MessageSuccess("Record has been Updated Successfully");
                    self.Success(true);
                    self.insert(false);
                    self.index(true);
                    GetData();
                    self.clear();

                }

            });
        }
        else {
            self.Error(true);
            self.MessageError("Check validation Missing");

        }
    }

    self.deletepopup = function (products) {
        $("#myModal").modal('show');

        $('#DeleteID').val(products.id);

    }

    self.deleterec = function () {
        id = $('#DeleteID').val(),
            $.ajax({
                type: "POST",
                url: '/Product/Deleteupdate/' + id,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#myModal").modal('hide');
                    gettotalrecord();
                    GetData();
                    self.Error(false);
                    self.Success(true);
                    self.MessageSuccess("Recored Deleted Successfully");
                }
            });

    }


    self.Save = function (data) {

        var imagename = "";
        var imagetitle = "";
        for (var i = 0; i < $('#fine-uploader-manual-trigger ul li').length; i++) {
            imagename = imagename + "$" + $('#fine-uploader-manual-trigger ul li:eq(' + i + ') img').attr('src');
            imagetitle = imagetitle + "$" + $('#fine-uploader-manual-trigger ul li:eq(' + i + ') span:eq(1)').text();
        }

        var attributeids = ""; attributesidvalue = "", attributeandvalues = "", attrbutesfinals = "";

        for (var i = 0; i < self.productattribute().length; i++) {
            if (self.productattribute()[i].IsChecked == true) {
                attributeids = self.productattribute()[i].attributeid;
                for (var j = 2; j < $('#prd' + self.productattribute()[i].attributeid + ' span div ul li').length; j++) {
                    if (j == 2) {
                        attributesidvalue = ';';
                    }
                    if ($('#prd' + self.productattribute()[i].attributeid + ' span div ul li:eq(' + j + ') a').find("input[type='checkbox']:checked").val() != undefined) {
                        attributesidvalue = attributesidvalue + "," + $('#prd' + self.productattribute()[i].attributeid + ' span div ul li:eq(' + j + ') a').find("input[type='checkbox']:checked").val();
                    }
                }
                attributeandvalues = attributeids + attributesidvalue;
                attrbutesfinals = attrbutesfinals + ':' + attributeandvalues;
            }
        }
        data.imagename(imagename);
        jQuery.fn.CKEditorValFor = function (element_id) {
            return CKEDITOR.instances[element_id].GetData();
        };
        data.productdescription = $().CKEditorValFor('txtproductdesc');
        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax({
                type: "POST",
                url: '/Product/Create?attrbutesfinals=' + attrbutesfinals + "&imagetitle=" + imagetitle,
                dataType: 'json',
                data: ko.toJSON(data),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    self.insert(false);
                    self.index(true);
                    self.MessageSuccess("Record has been Saved Successfully..");
                    self.Success(true);
                    self.Error(false);
                    getid();
                    gettotalrecord();
                    GetData();

                }
            });
        }
        else {
            self.Error(true);
        }
    };

    function getnames() {
        $.ajax({
            type: "GET",
            url: '/Product/getnames',
            Contenttype: "application/JSON",
            success: function (data) {
                self.Catgory(data);
                self.Catgorys(data);
            }
        });
    }

    self.ddlchangeprocat = function () {
        var pagesize = document.getElementById("pageSizeSelector").value;
        var Filname = document.getElementById("FilterbyName").value;
        var oldpageno = self.currentPageIndex();
        var productcatid = document.getElementById("ddlcategory").value;
        var sort = "order by productrank asc";
        var Status = document.getElementById("ddlstatus").value;
        var StockqtyFrom = document.getElementById("frmstk").value;
        var StockqtyTo = document.getElementById("tostk").value;
        self.currentPageIndex(1);

        $.ajax({
            type: "GET",
            url: '/Product/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&productname=" + Filname + "&sort=" + sort + "&productcategoryid=" + productcatid + "&StockqtyFrom=" + StockqtyFrom + "&StockqtyTo=" + StockqtyTo + "&Status=" + Status,
            Contenttype: "application/Json",
            success: function (data) {
                self.products(data);
            }
        });
    }

    self.ddlchngstatus = function () {
        $('#loader').show();
        var pagesize = document.getElementById("pageSizeSelector").value;
        var filname = document.getElementById("FilterbyName").value;
        var oldpageno = self.currentPageIndex().value;
        var Status = $('#ddlstatus').val();
        var sort = "order by productrank asc";
        var StockqtyFrom = document.getElementById("frmstk").value;
        var StockqtyTo = document.getElementById("tostk").value;
        var productcatid = document.getElementById("ddlcategory").value;
        if (productcatid == "" || productcatid == null) {
            productcatid = 0;
        }

        self.currentPageIndex(1);
        $.ajax({
            type: "get",
            url: '/product/search?pagesize=' + pagesize + "&pageno=" + self.currentPageIndex() + "&productname=" + filname + "&sort=" + sort + "&productcategoryid=" + productcatid + "&StockqtyFrom=" + StockqtyFrom + "&StockqtyTo=" + StockqtyTo + "&Status=" + Status,
            contenttype: "application/json",
            success: function (data) {
                self.products(data);
                $('#loader').hide();
            }
        });

    }

    self.GetStock = function () {
        $('#loader').show();
        var pagesize = document.getElementById("pageSizeSelector").value;
        var Filname = document.getElementById("FilterbyName").value;
        var productcatid = document.getElementById("ddlcategory").value;
        if (productcatid == "" || productcatid == null) {
            productcatid = 0;
        }
        var sort = "order by productrank asc";
        var StockqtyFrom = document.getElementById("frmstk").value;
        var StockqtyTo = document.getElementById("tostk").value;
        var Status = document.getElementById("ddlstatus").value;
        $.ajax({
            type: "GET",
            url: '/Product/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&productname=" + Filname + "&sort=" + sort + "&productcategoryid=" + productcatid + "&StockqtyFrom=" + StockqtyFrom + "&StockqtyTo=" + StockqtyTo + "&Status=" + Status,
            Contenttype: "application/Json",
            success: function (data) {
                self.products(data);
                $('#loader').hide();
            }
        });
    }

    function getattributes(id) {

        $.ajax({
            type: "GET",
            url: '/Product/getattribute?id=' + id,
            Contenttype: "application/JSON",
            success: function (data) {
                self.productattribute(data);

                for (var i = 0; i < self.productattribute().length; i++) {
                    if (self.productattribute()[i].IsChecked == false) {
                        $('#col').append("<div class=\"form-group\"  id=\"prd" + self.productattribute()[i].attributeid + "\"></div>");
                    }
                    if (self.productattribute()[i].IsChecked == true) {
                        $('#col').append("<div class=\"form-group\"  id=\"prd" + self.productattribute()[i].attributeid + "\"></div>");
                        setTimeout(self.openmultiselect(self.productattribute()[i]), 5000);


                    }

                }
            }
        });
    }

    self.checkvalidation = function () {
        var IsError = true;

        self.MessageError('');

        if (self.pagetitle() == "") {
            self.MessageError(self.MessageError() + "<br/> - Page Title is required.");
            IsError = false;
        }
        if (self.productname() == "") {
            self.MessageError(self.MessageError() + "<br/> - Product Name is required.");
            IsError = false;
        }
        if (self.productcode() == "") {
            self.MessageError(self.MessageError() + "<br/> - Product Code is required.");
            IsError = false;
        }
        if ($('#ddlcategory').val() == "") {
            self.MessageError(self.MessageError() + "<br/> - Product Category Name is required.");
            IsError = false;
        }
        var count = 0;
        for (var i = 0; i < $('#fine-uploader-manual-trigger ul li').length; i++) {

            if ($('#fine-uploader-manual-trigger ul li:eq(' + i + ')').find('#imgproduct').attr('src').indexOf('data:image') > 0) {

            }
            else {
                count = parseInt(count) + 1;
            }
        }

        if (count > 10) {
            self.MessageError(self.MessageError() + "<br/> - Cannot upload more than 10 images.");
            IsError = false;
        }

        self.Error(true);
        return IsError;
    }

    self.Addnew = function () {
        $('#btngenral').hide();
        $('#btnCancel').show();
        getattributes(0);
        self.index(false);
        self.savebtn(true);
        self.updatebtn(false);
        self.insert(true);
        self.Error(false);
        self.Success(false);
        self.clear();
        self.Tab1();
        getmaxrank();

    }

    function getmaxrank() {
        $.ajax({
            type: "GET",
            url: '/Product/getrank',
            Contenttype: "application/JSON",
            success: function (data) {
                self.productrank(data[0].productrank + 1);
            }
        });
    }

    function getid() {
        $.ajax({
            type: "GET",
            url: '/Product/getid',
            Contenttype: "application/JSON",
            success: function (data) {
                self.Image(data[0].id);
            }
        });
    }

    self.Cancel = function () {
        self.Error(false);
        self.Success(false);
        self.insert(false);
        self.index(true);
        self.Error(false);
        self.Success(false);
    }

    self.searchKeyUp = function (d, e) {
        if (e.keyCode == 13) {
            self.Search();
        }
    }

    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 50),
            startIndex = pagesize * self.currentPageIndex(),
            endIndex = startIndex + pagesize;
        return self.products.slice(startIndex, endIndex);
    });

    function GetData() {
        $('#loader').show();
        rec = self.totalrecord();
        var Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        var productcategoryid = document.getElementById("ddlcategory").value;
        var sort = "order by productrank asc";
        var Status = document.getElementById("ddlstatus").value;
        var StockqtyFrom = document.getElementById("frmstk").value;
        var StockqtyTo = document.getElementById("tostk").value;
        if (self.currentPageIndex() > 1) {
            var ttl = rec / pagesize;
            var diff = self.currentPageIndex() - ttl;
            if (rec > 31) {
                if (diff > .9) {
                    self.currentPageIndex(self.currentPageIndex() - 1);
                }
            }
            else {
                if (diff > .8) {
                    self.currentPageIndex(self.currentPageIndex() - 1);
                }
            }
        }

        $.ajax({
            type: "GET",
            url: '/Product/search?PageSize=' + pagesize + "&PageNo=1" + "&productname=" + Filname + "&sort=" + sort + "&productcategoryid=" + productcategoryid + "&StockqtyFrom=" + StockqtyFrom + "&StockqtyTo=" + StockqtyTo + "&Status=" + Status,
            Contenttype: "application/JSON",
            success: function (data) {
                self.products(data);
                $('#loader').hide();
                if ((self.totalrecord() / pagesize) <= self.currentPageIndex()) {

                    document.getElementById("btnnext").disabled = true;
                }
                else {

                    document.getElementById("btnnext").disabled = false;
                }
                if (self.currentPageIndex() > 1) {

                    document.getElementById("btnprev").disabled = false;
                }
                else {
                    document.getElementById("btnprev").disabled = true;
                }
            }
        });
    }

    self.SortData = function () {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        if (document.getElementById("Hforder").value == "asc") {
            document.getElementById("Hforder").value = "desc";

            var sort = "order by productrank desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        }
        else {
            document.getElementById("Hforder").value = "asc";
            var sort = "order by productrank asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }
        var Status = document.getElementById("ddlstatus").value;
        var StockqtyFrom = document.getElementById("frmstk").value;
        var StockqtyTo = document.getElementById("tostk").value;
        var Filname = document.getElementById("FilterbyName").value;
        var pageno = self.currentPageIndex();
        var pagesize = document.getElementById("pageSizeSelector").value;
        var productcategoryid = document.getElementById("ddlcategory").value;
        $.ajax({
            type: "GET",
            url: "/Product/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&productname=" + Filname + "&sort=" + sort + "&productcategoryid=" + productcategoryid + "&StockqtyFrom=" + StockqtyFrom + "&StockqtyTo=" + StockqtyTo + "&Status=" + Status,

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.products(data); gettotalrecord();//GetEmployees();
                $('#loader').hide();
            },
            error: function (error) {
                //alert(error.Status + "<!----!>" + error.StatusText);
                $('#loader').hide();
            }
        });

    }

    self.Search = function (products) {

        $('#loader').show();
        self.Error(false);
        self.Success(false);
        if (document.getElementById("Hforder").value == "desc") {
            var sort = "order by productrank desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')
        }
        else {
            var sort = "order by productrank asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }
        var pagesize = document.getElementById("pageSizeSelector").value;
        var Filname = document.getElementById("FilterbyName").value;
        var oldpageno = self.currentPageIndex();
        var productcatid = document.getElementById("ddlcategory").value;
        var Status = document.getElementById("ddlstatus").value;
        var StockqtyFrom = document.getElementById("frmstk").value;
        var StockqtyTo = document.getElementById("tostk").value;
        // var stock = frmstock + "," + tostock;

        self.currentPageIndex(1);
        var pageno = 1;
        gettotalrecord();
        $.ajax({

            type: "GET",
            url: "/Product/Search?PageSize=" + pagesize + "&PageNo=" + pageno + "&productname=" + Filname + "&sort=" + sort + "&productcategoryid=" + productcatid + "&StockqtyFrom=" + StockqtyFrom + "&StockqtyTo=" + StockqtyTo + "&Status=" + Status,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",

            success: function (data) {

                if (data.length == 0) {
                    self.Success(false);
                    self.MessageError("No Record Found.");
                    $('#error').text("");
                    self.Error(true);
                    self.products(data);
                    $('#loader').hide();
                    return;

                }
                self.products(data);
                self.Error(false);
                $('#loader').hide();

                if ((self.totalrecord() / pagesize) <= self.currentPageIndex()) {

                    document.getElementById("btnnext").disabled = true;
                }
                else {

                    document.getElementById("btnnext").disabled = false;
                }
                if (self.currentPageIndex() > 1) {

                    document.getElementById("btnprev").disabled = false;
                }
                else {
                    document.getElementById("btnprev").disabled = true;
                }
            },
            error: function (error) {
                self.MessageError(error);
                self.Error(true);
                $("#loader").hide();
            }
        });

    };

    function gettotalrecord() {
        var sort = "order by productrank asc";
        var productcatid = document.getElementById("ddlcategory").value;
        var Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        var StockqtyFrom = document.getElementById("frmstk").value;
        var StockqtyTo = document.getElementById("tostk").value;
        var Status = document.getElementById("ddlstatus").value;
        $.ajax({
            type: "GET",
            url: '/Product/search?PageSize=' + pagesize + "&PageNo=-1" + "&productname=" + Filname + "&sort=" + sort + "&productcategoryid=" + productcatid + "&StockqtyFrom=" + StockqtyFrom + "&StockqtyTo=" + StockqtyTo + "&Status=" + Status,
            Contenttype: "application/JSON",
            success: function (data) {
                self.totalrecord(data[0].id);
            }
        });
    }

    self.nextPage = function () {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by productrank desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by productrank asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        self.currentPageIndex(self.currentPageIndex() + 1);
        if (self.currentPageIndex() > 1) {

            var pageno = self.currentPageIndex();
        }
        else {
            var pageno = 1;
        }
        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        var Status = document.getElementById("ddlstatus").value;
        $.ajax({
            type: "GET",
            url: "/Product/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&productname=" + Filname + "&sort=" + sort + "&Status=" + Status,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.products(data);//GetEmployees();
                $('#loader').hide();
                if ((self.totalrecord() / pagesize) <= self.currentPageIndex()) {

                    document.getElementById("btnnext").disabled = true;
                }
                else {

                    document.getElementById("btnnext").disabled = false;
                }
                if (self.currentPageIndex() > 1) {

                    document.getElementById("btnprev").disabled = false;
                }
                else {
                    document.getElementById("btnprev").disabled = true;
                }
            }
        });



    };

    self.previousPage = function () {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by productrank desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by productrank asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        if (self.currentPageIndex() > 1) {

            self.currentPageIndex(self.currentPageIndex() - 1);
            var pageno = self.currentPageIndex();

        }
        else {
            var pageno = 1;
        }
        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: "/Product/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&productname=" + Filname,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.products(data);//GetEmployees();
                $('#loader').hide();
                if ((self.totalrecord() / pagesize) <= self.currentPageIndex()) {

                    document.getElementById("btnnext").disabled = true;
                }
                else {

                    document.getElementById("btnnext").disabled = false;
                }
                if (self.currentPageIndex() > 1) {

                    document.getElementById("btnprev").disabled = false;
                }
                else {
                    document.getElementById("btnprev").disabled = true;
                }
            }
        });
    };

    self.selectionChanged = function (event) {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by productname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by productname asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        var pageno = self.currentPageIndex();
        var totalrec = document.getElementById("Hftotalrecord").value;
        var pagesize = document.getElementById("pageSizeSelector").value;

        if ((totalrec / pagesize) < pageno) {

            if (pageno == 1) {
                pageno = 1;
            }
            else {
                pageno = pageno - 1;
                self.currentPageIndex(self.currentPageIndex() - 1)
            }

        }

        Filname = document.getElementById("FilterbyName").value;
        $.ajax({
            type: "GET",
            url: "/Product/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&productname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.products(data);
                $('#loader').hide();
                if ((self.totalrecord() / pagesize) <= self.currentPageIndex()) {

                    document.getElementById("btnnext").disabled = true;
                }
                else {

                    document.getElementById("btnnext").disabled = false;
                }
                if (self.currentPageIndex() > 1) {

                    document.getElementById("btnprev").disabled = false;
                }
                else {
                    document.getElementById("btnprev").disabled = true;
                }
            }
        });
    }

    self.changestatus = function (products) {
        alert(active);
        var active = products.isactive;
        var id = products.id;

        $.ajax({
            type: "POST",
            url: "/Product/UpdateActive?id=" + id + "&Active=" + active,
            contentType: "application/json",
            success: function (data) {
                GetData();
                self.MessageSuccess("Record has been Updated Successfully.");
                self.Success(true);

            },
            error: function (error) {
            }
        });
    };

    self.Dataupside = function (products) {
        $.ajax({
            type: "POST",
            url: "/Product/updata?id=" + products.id + "&up=" + "up",
            contentType: "application/json",
            success: function (data) {
                self.MessageSuccess("Record has been Updated Successfully.");
                self.Success(true);
                GetData();
            },
            error: function (error) {
            }
        });
    }

    self.Datadownside = function (products) {
        $.ajax({
            type: "POST",
            url: "/Product/downdata?id=" + products.id + "&down=" + "down",
            contentType: "application/json",
            success: function (data) {
                self.MessageSuccess("Record has been Updated Successfully.");
                self.Success(true);
                GetData();
            },
            error: function (error) {
            }
        });
    }

    //self.openmultiselect=function(data)
    //{
    //    var attributeid = data.attributeid;
    //    getattrvaluebyattrid(attributeid);
    //    self.multiselectdropdown(true);
    //}

    //function getattrvaluebyattrid(id)
    //{

    //    $.ajax({
    //        type: "GET",
    //        url: '/Product/getattrvaluebyattrid?id=' + id,
    //        Contenttype: "application/JSON",
    //        success: function (data) {
    //            self.multiattribtevalue(data);
    //        }
    //    });
    //}


    self.openmultiselect = function (data) {


        if (data.IsChecked == true) {

            if ($("#prd" + data.attributeid).find("span").length == 0) {

                var attributeid = data.attributeid;

                getattrvaluebyattrid(attributeid);




            }
            else {
                $("#prd" + data.attributeid).showV();

            }

        }

        else {


            $("#prd" + data.attributeid).hideV();
        }


    }

    function getattrvaluebyattrid(id) {


        $.ajax({
            type: "GET",
            url: '/Product/getattrvaluebyattrid?id=' + id,
            Contenttype: "application/JSON",
            success: function (data) {
                if (data.length > 0) {
                    self.multiattribtevalue(data);
                    var option = "";
                    for (var i = 0; i < self.multiattribtevalue().length; i++) {
                        option = option + "<br/>" + "<option value=\"" + self.multiattribtevalue()[i].id + "\">" + self.multiattribtevalue()[i].attributevalue + "</option>";
                    }

                    $("#col").find("#prd" + id).append("<select  class=\"mul\" multiple=\"multiple\" data-bind=\"options:multiattribtevalue,optionsText:'attributevalue',optionsValue:'id',value:id\">" + option + "</select>");
                    //$('#col').append("<div class=\"form-group\"  id=\"prd" + id + "\"><select  class=\"mul\" multiple=\"multiple\" data-bind=\"options:multiattribtevalue,optionsText:'attributevalue',optionsValue:'id',value:id\">" + option + "</select></div>");

                    $('.mul').multiselect({
                        enableFiltering: true,
                        includeSelectAllOption: true,
                        maxHeight: 400
                    });
                }
                if (self.edit() == true) {
                    $.ajax({
                        type: "GET",
                        url: '/Product/getattributevalueFromProduct?id=' + self.id() + "&Attrbutid=" + id,
                        Contenttype: "application/JSON",
                        success: function (data1) {
                            var title = "";

                            for (var i = 0; i < data1.length; i++) {
                                for (var j = 1; j < $('#prd' + id + ' span div ul li').length; j++) {


                                    if (data1.length == $('#prd' + id + ' span div ul li').length - 2) {
                                        $('#prd' + id + ' span button span').text("All selected (" + data1.length + ")");
                                        $('#prd' + id + ' span div ul li:eq(' + j + ')').addClass("active");
                                        $('#prd' + id + ' span div ul li:eq(' + j + ') a').find("input[type='checkbox']").prop('checked', true);
                                    }
                                    if (data1.length > 3) {
                                        $('#prd' + id + ' span button span').text(data1.length + "selected");
                                    }

                                    if (data1[i].attributesvalueid == $('#prd' + id + ' span div ul li:eq(' + j + ') a').find("input[type='checkbox']").val()) {

                                        $('#prd' + id + ' span div ul li:eq(' + j + ')').addClass("active");
                                        $('#prd' + id + ' span div ul li:eq(' + j + ') a').find("input[type='checkbox']").prop('checked', true);

                                        if (data1.length < 3) {


                                            title = title + "," + $('#prd' + id + ' span div ul li:eq(' + j + ') a label').text();
                                            $('#prd' + id + ' span button').prop('title', title.substring(1));
                                            $('#prd' + id + ' span button span').text(title.substring(1));
                                        }
                                    }

                                }
                            }


                        }

                    })
                }


            }

        });
    }

    getnames();
    gettotalrecord();
    GetData();

}

