function MenuKO() {
    self = this;
    self.id = ko.observable(0);
    self.contentid = ko.observable("");
    self.contentname = ko.observable("");
    self.title = ko.observable("");
    self.parent = ko.observable("");
    self.rank = ko.observable("");
    self.active = ko.observable(false);
    self.linkurl = ko.observable("");
    self.linkurlcount = ko.observable(0);
    self.updatebtn = ko.observable(false);
    self.savebtn = ko.observable(true);
    self.menutype = ko.observable("");
    self.pageddl = ko.observable(false);

    self.contents = ko.observableArray();
    self.parents = ko.observableArray();

    self.index = ko.observable(true);
    self.insert = ko.observable(false);

    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");
    self.Success = ko.observable(false);
    self.Error = ko.observable(false);

    //index
    self.menu = ko.observableArray();
    self.totalrecord = ko.observable(0);
    self.search = ko.observable(true);

    //paging
    self.currentPage = ko.observable();
    self.pageSize = ko.observable(50);
    self.currentPageIndex = ko.observable(1);
    self.sortType = "ascending";
    self.currentColumn = ko.observable("");
    self.iconType = ko.observable("");

    self.changeddl = function () {
        if ($('#ddlmenutype').val() == 4) {
            self.pageddl(true);
        }
        else {
            self.pageddl(false);
        }
    }

    self.searchKeyUp = function (d, e) {
        if (e.keyCode == 13) {
            self.Search();
        }

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

    self.Addnew = function (menu) {
        document.getElementById("add").innerText = "Add Menu";
        self.index(false);
        self.rank(menu.rank + 1);
        self.savebtn(true);
        self.updatebtn(false);
        self.insert(true);
        self.Error(false);
        self.Success(false);
        self.clear();
        getmaxrank();
        $('#txttitle').focus();
    }

    self.SortData = function () {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        if (document.getElementById("Hforder").value == "asc") {
            document.getElementById("Hforder").value = "desc";

            var sort = "order by rank desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        }
        else {
            document.getElementById("Hforder").value = "asc";
            var sort = "order by rank asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        var Filname = document.getElementById("FilterbyName").value;
        var pageno = self.currentPageIndex();
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: "/Menu/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&title=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.menu(data); gettotalrecord();//GetEmployees();
                $('#loader').hide();
            },
            error: function (error) {
                alert(error.status + "<!----!>" + error.statusText);
                $('#loader').hide();
            }
        });

    }

    function gettotalrecord() {

        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: '/Menu/search?PageSize=' + pagesize + "&PageNo=-1" + "&title=" + Filname,
            Contenttype: "application/JSON",
            success: function (data) {
                self.totalrecord(data[0].id);
            }
        });
    }

    function getmaxrank() {
        $.ajax({
            type: "GET",
            url: '/Menu/getrank',
            Contenttype: "application/JSON",
            success: function (data) {
                self.rank(data[0].rank + 1);
            }
        });
    }

    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 50),
        startIndex = pagesize * self.currentPageIndex(),
        endIndex = startIndex + pagesize;
        return self.menu.slice(startIndex, endIndex);
    });

    self.nextPage = function () {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by rank desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by rank asc"
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
            url: "/Menu/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&title=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.menu(data);//GetEmployees();
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


            var sort = "order by rank desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by rank asc"
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
            url: "/Menu/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&title=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.menu(data);//GetEmployees();
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


            var sort = "order by title desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by title asc"
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
            url: "/Menu/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&title=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.menu(data);
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

    self.Dataupside = function (menu) {
        $.ajax({
            type: "POST",
            url: "/Menu/updata?id=" + menu.id + "&up=" + "up",
            contentType: "application/json",
            success: function (data) {
                self.MessageSuccess("Record has been Updated Successfully.");
                self.Success(true);
                getdata();
            },
            error: function (error) {
            }
        });
    }

    self.Datadownside = function (menu) {
        $.ajax({
            type: "POST",
            url: "/Menu/downdata?id=" + menu.id + "&down=" + "down",
            contentType: "application/json",
            success: function (data) {
                self.MessageSuccess("Record has been Updated Successfully.");
                self.Success(true);
                getdata();
            },
            error: function (error) {
            }
        });
    }

    function getparent(id) {
        $.ajax({
            type: "GET",
            url: '/Menu/getparent?id=' + id,
            Contenttype: "application/JSON",
            success: function (data) {
                self.parents(data);
            }
        });
    }

    function getnames() {
        $.ajax({
            type: "GET",
            url: '/Menu/getnames',
            Contenttype: "application/JSON",
            success: function (data) {
                self.contents(data);

            }
        });
    }

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
            url: '/Menu/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&title=" + Filname,
            Contenttype: "application/JSON",
            success: function (data) {
                self.menu(data);
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

    self.changestatus = function (menu) {

        var active = menu.active;
        var id = menu.id;

        $.ajax({
            type: "POST",
            url: "/Menu/UpdateActive?id=" + id + "&Active=" + active,
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

    self.getselectedpages = function (menu) {
        self.id(menu.id);
        getparent(menu.id);
        self.contentid(menu.contentid);
        self.title(menu.title);
        self.parent(menu.parent);
        self.rank(menu.rank);
        self.active(menu.active);
        self.linkurl(menu.linkurl);
        self.menutype(menu.menutype);
        if ($('#ddlmenutype').val() == 4) {
            self.pageddl(true);
        }
        self.index(false);
        self.insert(true);
        document.getElementById("add").innerHTML = "Edit Menu";
        self.updatebtn(true);
        self.savebtn(false);
        self.Error(false);
        self.Success(false);
        $('#txttitle').focus();
    }

    self.Update = function (data) {

        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax({
                url: '/Menu/Update/',
                type: "POST",
                dataType: 'json',
                data: ko.toJSON(data),
                contentType: "application/json",
                success: function (data) {
                    self.Error(false);
                    self.insert(false);
                    getdata();
                    self.index(true);
                    self.Success(true);
                    self.MessageSuccess("Record has been Updated Successfully.")
                    getparent(id);
                }

            });
        }
    }

    self.deletepopup = function (menu) {
        $("#myModal").modal('show');

        $('#DeleteID').val(menu.id);

    }

    self.deleterec = function () {
        var id = $('#DeleteID').val();
        $.ajax({
            type: "POST",
            url: '/Menu/Delete/' + id,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#myModal").modal('hide');
                gettotalrecord();
                getdata();
                self.Error(false);
                self.Success(true);
                self.MessageSuccess("Record has been Deleted Successfully.");

            }
        });

    }

    self.checkvalidation = function () {
        var IsError = true;
        self.MessageError('');
        if ($('#ddlmenutype').val() == 0) {
            self.MessageError(self.MessageError() + "<br/> - Page Type is required.");
            IsError = false;
        }
        if (self.title() == "") {
            self.MessageError(self.MessageError() + "<br/> - Title is required.");
            IsError = false;
        }
        if (self.linkurl() == "") {
            self.MessageError(self.MessageError() + "<br/> - Link URL is required.");
            IsError = false;
        }
        if ($('#ddlmenutype').val() == "4") {
            if ($('#page').val() == "") {
                self.MessageError(self.MessageError() + "<br/> - Content Name is required. ")
                // IsError = false;
            }
            else {
                IsError = false;
            }
        }
         
        self.Error(true);
        return IsError;
    }

    self.Save = function (data1) {
        self.linkurl($('#txtlink').val());
        self.Error(false);
        self.MessageError("");

        $.ajax({
            type: "GET",
            url: '/Menu/checklink?link=' + self.linkurl(),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

                if (data[0].id > 0) {
                    self.MessageError(self.MessageError() + "<br/> - Page Url is already exists.");
                    self.Error(true);
                    return;
                }
                else {

                    if (self.checkvalidation()) {
                        self.Error(false);
                        $.ajax({
                            type: "POST",
                            url: '/Menu/Create',
                            dataType: 'json',
                            data: ko.toJSON(data1),
                            contentType: 'application/json; charset=utf-8',
                            success: function (data) {
                                self.Error(false);
                                self.index(true);
                                self.insert(false);
                                self.MessageSuccess("Record has been Saved Successfully.");
                                self.Success(true);
                                gettotalrecord();
                                getdata();
                                getparent(id);
                            }
                        });

                    }
                    else {
                        self.Error(true);
                    }
                }



            },
            error: function (ex) {
                alert(ex);
            }
        });


    }

    self.clear = function (menu) {
        self.id(0);
        self.contentid(null);
        self.title("");
        self.linkurl("");
        self.parent(null);
        self.rank(1);
        self.active("");
        self.menutype(0);
        $('#txtlink').val("");
    }

    self.Cancel = function () {
        self.insert(false);
        self.index(true);
        self.Error(false);
        self.Success(false);
        self.pageddl(false);
    }

    self.Search = function (menu) {
        $('#loader').show();
        self.Error(false);
        self.Success(false);
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by rank desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by rank asc"
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
            url: "/Menu/Search?PageSize=" + pagesize + "&PageNo=" + pageno + "&title=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                if (data.length == 0) {
                    self.Success(false);
                    self.MessageError("No Record Found."); $('#error').text("");
                    self.Error(true);
                    self.menu(data);
                    $('#loader').hide();
                    return;

                }
                self.Error(false);
                self.menu(data);
                $('#loader').hide();
                self.Success(false);

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

    getnames();
    gettotalrecord();
    getdata();
    getparent(0);


}