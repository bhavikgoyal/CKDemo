function ProductcatKO()
{
    var self = this;
    self.id = ko.observable(0);
    self.categoryname = ko.observable("");
    self.isactive = ko.observable("");
    self.pagetitle = ko.observable("");
    self.metakeywords = ko.observable("");
    self.metadescription = ko.observable("");
    self.categoryurl = ko.observable("");
    self.images = ko.observable("");
    self.categorys = ko.observableArray();

    self.insert = ko.observable(false);
    self.index = ko.observable(true);
    
    self.Success = ko.observable(false);
    self.Error = ko.observable(false);
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");
    self.savebtn = ko.observable(true);
    self.updatebtn = ko.observable(false);

    self.updatemethod = ko.observable(false);
    self.currentPage = ko.observable();
    self.pageSize = ko.observable(50);
    self.currentPageIndex = ko.observable(1);
    self.totalrecord = ko.observable(0);
    self.search = ko.observable(true);

    self.clear = function () {
        self.id("");
        self.images("");
        self.categoryname("");
        self.pagetitle("");
        self.metakeywords("");
        self.metadescription("");
        self.isactive("");
        self.categoryurl("");
        self.updatemethod(false);

    }

    self.Addnew = function () {
        document.getElementById("add").innerText = "Add Product Category";
        self.clear();
        self.Success(false);
        self.Error(false);
        self.index(false);
        self.insert(true);
        self.savebtn(true);
        self.updatebtn(false);
        $('#txtpcn').focus();
    }

    self.Cancel = function () {
        self.Success(false);
        self.Error(false);
        self.index(true);
        self.insert(false);
        self.clear();
    }

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

    self.Save = function (data) {
        data.images($('#area').text());
        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax(
                {
                    type: "POST",
                    url: "/ProductCat/Insert",
                    data: ko.toJSON(data),
                    contentType: "application/json",
                    async: true,
                    success: function (data) {
                        self.Error(false);
                        self.MessageSuccess("Record has been Saved Successfully.");
                        self.Success(true);
                        self.insert(false);
                        self.index(true);
                        getdata();
                        gettotalrecord();
                        self.clear();
                    },
                    error: function (error) {
                        self.MessageError(error);
                        self.Error(true);
                    }
                });
        }
        else {
            self.Error(true);
        }
    }

    self.checkvalidation = function () {
        var IsError = true;
        self.MessageError('');
        if (self.categoryname() == "") {
            self.MessageError(self.MessageError() + "<br/> - Category Name is required.");
            IsError = false;
        }
        if (self.pagetitle() == "") {
            self.MessageError(self.MessageError() + "<br/> - Page Title  is required.");
            IsError = false;
        }
        if (self.categoryurl() == "") {
            self.MessageError(self.MessageError() + "<br/> - Category URL is required.");
            IsError = false;
        }
        if (self.updatemethod() == true) {
            if ($('#area').text() == "" && $('#viewimg').text() == "") {
                self.MessageError(self.MessageError() + "<br/> - Please Select Image.");
                IsError = false;
            }
        }

        self.Error(true);
        return IsError;
    }

    //Paging

    self.getselectedpages = function (categorys) {
        document.getElementById("add").innerText = "Edit Product Category";
        self.Success(false);
        self.Error(false);
        self.id(categorys.id);
        $("#lblview").val(categorys.images);
        $("#viewimg").attr('href', "../UploadImages/" + categorys.images);
        self.categoryname(categorys.categoryname);
        self.pagetitle(categorys.pagetitle);
        self.metakeywords(categorys.metakeywords);
        self.metadescription(categorys.metadescription);
        self.isactive(categorys.isactive);
        self.categoryurl(categorys.categoryurl);
        self.index(false);
        self.insert(true);
        self.updatebtn(true);
        self.savebtn(false);
        $('#txtpcn').focus();
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
        return self.categorys.slice(startIndex, endIndex);
    });

    function getdata() {
        $('#loader').show();
        rec = self.totalrecord();
        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
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
            url: '/ProductCat/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&categoryname=" + Filname,
            Contenttype: "application/JSON",
            success: function (data) {
                self.categorys(data);
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

            var sort = "order by categoryname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        }
        else {
            document.getElementById("Hforder").value = "asc";
            var sort = "order by categoryname asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        var Filname = document.getElementById("FilterbyName").value;
        var pageno = self.currentPageIndex();
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: "/ProductCat/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&categoryname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.categorys(data); gettotalrecord();//GetEmployees();
                $('#loader').hide();
            },
            error: function (error) {
                alert(error.status + "<!----!>" + error.statusText);
                $('#loader').hide();
            }
        });

    }

    self.Search = function (categorys) {

        $('#loader').show();
        self.Error(false);
        self.Success(false);
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by categoryname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by categoryname asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }
        var pagesize = document.getElementById("pageSizeSelector").value;
        var Filname = document.getElementById("FilterbyName").value;
        var oldpageno = self.currentPageIndex();
        self.currentPageIndex(1);
        var pageno = 1;
        gettotalrecord();
        $.ajax({
            type: "GET",
            url: "/ProductCat/Search?PageSize=" + pagesize + "&PageNo=" + pageno + "&categoryname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                if (data.length == 0) {
                    self.Success(false);
                    self.MessageError("No Record Found."); $('#error').text(""); 
                    self.Error(true);
                    self.categorys(data);
                    $('#loader').hide();
                    return;

                }
                self.categorys(data);
                $('#loader').hide();
                self.Success(false);
                self.Error(false);

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

        $("#loader").hide();

    };

    function gettotalrecord() {

        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: '/ProductCat/search?PageSize=' + pagesize + "&PageNo=-1" + "&categoryname=" + Filname,
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


            var sort = "order by categoryname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by categoryname asc"
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
        $.ajax({
            type: "GET",
            url: "/ProductCat/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&categoryname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.categorys(data);//GetEmployees();
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


            var sort = "order by categoryname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by categoryname asc"
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
            url: "/ProductCat/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&categoryname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.categorys(data);//GetEmployees();
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


            var sort = "order by categoryname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by categoryname asc"
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
            url: "/ProductCat/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&categoryname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.categorys(data);
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

    self.changestatus = function (categorys) {

        var isactive = categorys.isactive;
        var id = categorys.id;

        $.ajax({
            type: "POST",
            url: "/ProductCat/UpdateActive?id=" + id + "&Active=" + isactive,
            contentType: "application/json",
            success: function (data) {

                self.MessageSuccess("Record has been Updated Successfully.");
                self.Success(true);
                getdata();
            },
            error: function (error) {
            }
        });
    };

    self.Update = function (data) {
        self.updatemethod(true);
        if ($('#area').text() != '') {
            data.images($('#area').text());

        } else {
            data.images($("#lblview").val());
        }
        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax(
                {
                    type: "POST",
                    url: "/ProductCat/Update",
                    data: ko.toJSON(data),
                    contentType: "application/json",
                    async: true,
                    success: function (data) {
                        self.index(true);
                        self.Error(false);
                        self.MessageSuccess("Record has been Updated Successfully.");
                        self.Success(true);
                        self.insert(false);
                        getdata();
                        self.updatemethod(false);
                    },
                    error: function (error) {
                        self.MessageError(error);
                        self.Error(true);

                    }

                });
        }
        else {
            self.Error(true);

        }

    }

    self.deletepopup = function (categorys) {
        $("#myModal").modal('show');

        $('#DeleteID').val(categorys.id);

    }

    self.deleterec = function () {
        var id = $('#DeleteID').val();

        $.ajax(
            {
                type: "POST",
                url: "/ProductCat/Delete/" + id,
                success: function (data) {
                    $("#myModal").modal('hide');
                    self.Error(false);
                    self.MessageSuccess("Record has been Deleted Successfully.");
                    self.Success(true);
                    self.Error(false);
                    gettotalrecord();
                    getdata();
                    self.index(true);
                    self.insert(false);
                    getdata(document.getElementById("pageSizeSelector").value, self.currentPageIndex(), "", "order by categoryname asc");

                },
                error: function (error) {
                    self.MessageError(error);
                    self.Error(true);
                }
            })
    }

    gettotalrecord();
    getdata();


}