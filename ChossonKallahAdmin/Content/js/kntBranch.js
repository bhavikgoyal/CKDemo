function branchKO() {
    var self = this;

    self.id = ko.observable(0);
    self.branchname = ko.observable("");
    self.branchcode= ko.observable("");
    self.branchaddress= ko.observable("");
    self.isactive = ko.observable("");

    self.branches = ko.observableArray();
    
    self.Success = ko.observable(false);
    self.Error = ko.observable(false);
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");
    self.savebtn = ko.observable(true);
    self.updatebtn = ko.observable(false);

    self.insert = ko.observable(false);
    self.index=ko.observable(true)

    self.clear = function () {
        self.id("");
        self.branchname("");
        self.branchcode("");
        self.branchaddress("");
        self.isactive("");
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
        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax(
                {
                    type: "POST",
                    url: "/Branch/Create",
                    data: ko.toJSON(data),
                    contentType: "application/json",
                    async: true,
                    success: function (data) {
                        gettotalrecord();
                        getdata();
                        self.Error(false);
                        self.MessageSuccess("Record has been Saved Successfully.");
                        self.Success(true);
                        self.Error(false);
                        self.insert(false);
                        self.index(true);
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
        if (self.branchname() == "") {
            self.MessageError(self.MessageError() + "<br/> - Branch Name is required.");
            IsError = false;
        }
        if (self.branchcode() == "") {
            self.MessageError(self.MessageError() + "<br/> - Branch Code is required.");
            IsError = false;
        }
        if (self.branchaddress() == "") {
            self.MessageError(self.MessageError() + "<br/> - Branch Address is required.");
            IsError = false;
        }
       


        self.Error(true);
        return IsError;
    }

    self.currentPage = ko.observable();
    self.pageSize = ko.observable(10);
    self.currentPageIndex = ko.observable(1);
    self.sortType = "ascending";
    self.currentColumn = ko.observable("");
    self.iconType = ko.observable("");
    self.totalrecord = ko.observable(0);
    self.search = ko.observable(true);

    self.searchKeyUp = function (d, e) {
        if (e.keyCode == 13) {
            self.Search();
        }
    }


    self.Addnew = function () {
        document.getElementById("add").innerText = "Add Branch";
        self.index(false);
        self.savebtn(true);
        self.updatebtn(false);
        self.insert(true);
        self.Error(false);
        self.Success(false);
        self.clear();
        $('#branchname').focus();
    }
    
    self.SortData = function () {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        if (document.getElementById("Hforder").value == "asc") {
            document.getElementById("Hforder").value = "desc";

            var sort = "order by branchname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        }
        else {
            document.getElementById("Hforder").value = "asc";
            var sort = "order by branchname asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        var Filname = document.getElementById("FilterbyName").value;
        var pageno = self.currentPageIndex();
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: "/Branch/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&branchname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.branches(data); gettotalrecord();//GetEmployees();
                $('#loader').hide();
            },
            error: function (error) {
                alert(error.status + "<!----!>" + error.statusText);
                $('#loader').hide();
            }
        });

    }

    self.Search = function (branches) {
        $('#loader').show();
        self.Error(false);
        self.Success(false);
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by branchname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by branchname asc"
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
            url: "/Branch/Search?PageSize=" + pagesize + "&PageNo=" + self.currentPageIndex() + "&branchname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                if (data.length == 0) {
                    self.Success(false);
                    self.MessageError("No Record Found.");
                    self.Error(true);
                    $('#error').text("");
                    self.branches(data);
                    $('#loader').hide();
                    return;

                }
                self.branches(data);
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

        $("#loader").hide();

    };


    function gettotalrecord() {

        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: '/Branch/search?PageSize=' + pagesize + "&PageNo=-1" + "&branchname=" + Filname,
            Contenttype: "application/JSON",
            success: function (data) {
                self.totalrecord(data[0].id);
            }
        });
    }

    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 10),
        startIndex = pagesize * self.currentPageIndex(),
        endIndex = startIndex + pagesize;
        return self.branches.slice(startIndex, endIndex);
    });

    self.nextPage = function () {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by branchname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by branchname asc"
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
            url: "/Branch/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&branchname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.branches(data);
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


            var sort = "order by branchname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by branchname asc"
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
            url: "/Branch/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&branchname=" + Filname,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.branches(data);//GetEmployees();
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


            var sort = "order by branchname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by branchname asc"
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
            url: "/Branch/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&branchname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.branches(data);
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
            url: '/Branch/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&branchname=" + Filname,
            Contenttype: "application/JSON",
            success: function (data) {
                self.branches(data);
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

    self.changestatus = function (branches) {

        var isactive = branches.isactive;
        var id = branches.id;

        $.ajax({
            type: "POST",
            url: "/Branch/UpdateActive?id=" + id + "&Active=" + isactive,
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

    self.getselectedpages = function (branches) {
        self.id(branches.id);
        self.branchname(branches.branchname);
        self.branchaddress(branches.branchaddress);
        document.getElementById("add").innerText = "Edit Branch";
        self.branchcode(branches.branchcode);
        self.isactive(branches.isactive);
        self.index(false);
        self.insert(true);
        self.updatebtn(true);
        self.savebtn(false);
        $('#branchname').focus();
    }

    self.Update = function (data) {
        if (self.checkvalidation()) {
            self.Error(false);
        $.ajax({
            url: '/Branch/Update/',
            type: "POST",
            dataType: 'json',
            data: ko.toJSON(data),
            contentType: "application/json",
            success: function (data) {
                self.insert(false);
                getdata();
                self.index(true);
                self.Success(true);
                self.MessageSuccess("Record has been Updated Successfully.");
                self.Error(false);
            }

        });
        }
        else {
            self.Error(true);

        }
    }

    self.deletepopup = function (branches) {
        $("#myModal").modal('show');

        $('#DeleteID').val(branches.id);

    }

    self.deleterec = function (branches) {
       id= $('#DeleteID').val();
        $.ajax({
            type: "POST",
            url: '/Branch/Deleteupdate/' + id,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#myModal").modal('hide');
                gettotalrecord();
                getdata();
                self.success(true);
                self.messagesuccess("Record has been Deleted Successfully.");
                self.Error(false);
             }
        });

    }

    gettotalrecord();
    getdata();
    
    



}