function UseraddressKo()
{
    var self = this;
    self.id = ko.observable(0);
    self.userid = ko.observable("");
    $('#loader').hide();
    self.countryid = ko.observable("");
    self.firstname = ko.observable("");
    self.lastname = ko.observable("");
    self.ismainaddress= ko.observable(false);
    self.addressline1 = ko.observable("");
    self.addressline2 = ko.observable("");
    self.city= ko.observable("");
    self.state= ko.observable("");
    self.zipcode= ko.observable("");
    self.insert = ko.observable(false);
    self.index = ko.observable(true);

    self.usernames = ko.observableArray();
    self.Countrys = ko.observableArray();

    self.Success = ko.observable(false);
    self.Error = ko.observable(false);
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");

    self.savebtn = ko.observable(true);
    self.updatebtn = ko.observable(false);

    self.currentPage = ko.observable();
    self.pageSize = ko.observable(10);
    self.currentPageIndex = ko.observable(1);
    self.totalrecord = ko.observable(0);
    
 
    self.usersadd = ko.observableArray();
   
    function Getusername() {
       
        $.ajax({
            type: "GET",
            url: '/Useraddress/getusername',
            Contenttype: "application/JSON",
            success: function (data) {
                self.usernames(data);
                
           }
       });
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

    function Getcountryname() {
        $.ajax({
            type: "GET",
            url: '/Useraddress/getcountry',
            Contenttype: "application/JSON",
            success: function (data) {
                self.Countrys(data);
            }
        });
    }

    self.clear = function () {
        self.id("");
        self.firstname("");
        self.lastname("");
        self.userid(null);
        self.countryid(null);
        self.addressline1("");
        self.addressline2("");
        self.city("");
        self.state("");
        self.zipcode("");
        self.ismainaddress(false);
    }

    self.Cancel = function () {
        self.Success(false);
        self.Error(false);
        self.index(true);
        self.insert(false);
        self.clear();
    }

    self.Addnew = function () {
        var id = 0;
        if (window.location.href.indexOf('id') >= 0) {
            var splitdata = window.location.href.split('=');
            id = splitdata[1];
        }
        document.getElementById("add").innerText = "Add User Address";
        self.clear();
        self.Success(false);
        self.Error(false);
        self.index(false);
        self.insert(true);
        self.savebtn(true);
        self.updatebtn(false);
        self.userid(id);
        $('#txtfrstnam').focus();
    }

    self.Back = function () {
         window.location.href="/Users/index";
     }

    self.Save = function (data) {
        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax(
                {
                    type: "POST",
                    url: "/Useraddress/Insert",
                    data: ko.toJSON(data),
                    contentType: "application/json",
                    async: true,
                    success: function (data) {
                        self.Error(false);
                        self.MessageSuccess("Record has been Saved Successfully.");
                        self.Success(true);
                        self.index(true);
                        self.insert(true);
                        getdata();
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
        if (self.firstname() == "") {
            self.MessageError(self.MessageError() + "<br/> - First name is required.");
            IsError = false;
        }
        if (self.addressline1() == "") {
            self.MessageError(self.MessageError() + "<br/> - Address 1 is required.");
            IsError = false;
        }
        if (self.state() == "") {
            self.MessageError(self.MessageError() + "<br/> - State is required.");
            IsError = false;
        }
        if (self.city() == "") {
            self.MessageError(self.MessageError() + "<br/> - City is required.");
            IsError = false;
        }
        if (self.zipcode() == "") {
            self.MessageError(self.MessageError() + "<br/> - Zipcode is required.");
            IsError = false;
        }
        if ($('#Countryid').val() == "") {
            self.MessageError(self.MessageError() + "<br/> - Country Name is required.");
            IsError = false;
        }

        self.Error(true);
        return IsError;
    }

    self.getselectedpages = function (usersadd) {
        document.getElementById("add").innerText = "Edit User";
        self.Success(false);
        self.Error(false);
        self.id(usersadd.id);
        self.firstname(usersadd.firstname);
        self.userid(usersadd.userid);
        self.countryid(usersadd.countryid);
        self.addressline1(usersadd.addressline1);
        self.addressline2(usersadd.addressline2);
        self.city(usersadd.city);
        self.state(usersadd.state);
        self.zipcode(usersadd.zipcode);
        self.ismainaddress(usersadd.ismainaddress);
        self.index(false);
        self.insert(true);
        self.updatebtn(true);
        self.savebtn(false);
        $('#txtfrstnam').focus();
    }

    self.deletepopup = function (usersadd) {
        $("#myModal").modal('show');

        $('#DeleteID').val(usersadd.id);

    }
   
    self.deleterec = function () {
        var id = $('#DeleteID').val();

        $.ajax(
            {
                type: "POST",
                url: "/Useraddress/Delete/" + id,
                success: function (data) {
                    $("#myModal").modal('hide');
                    self.Error(false);
                    self.MessageSuccess("Record has been Deleted Successfully");
                    self.Success(true);
                    self.Error(false);
                    getdata();
                    self.index(true);
                    self.insert(false);
                    

                },
                error: function (error) {
                    self.Success(false);
                    self.MessageError(error);
                    self.Error(true);
                }
            })
    }

    self.Update = function (data) {
        if (self.checkvalidation()) {
            $.ajax(
                {
                    type: "POST",
                    url: "/Useraddress/Update",
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

    self.searchKeyUp = function (d, e) {
        if (e.keyCode == 13) {
            self.Search();
        }
    }

    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 10),
        startIndex = pagesize * self.currentPageIndex(),
        endIndex = startIndex + pagesize;
        return self.usersadd.slice(startIndex, endIndex);
    });

    function getdata() {
         var id = 0;
        if (window.location.href.indexOf('id') >= 0) {
            var splitdata = window.location.href.split('=');
            id = splitdata[1];
        }
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
            url: '/Useraddress/Search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&firstname=" + Filname + "&tempid=" + id,
            Contenttype: "application/JSON",
            success: function (data) {
                self.usersadd(data);
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
        if (document.getElementById("Hforder").value == "asc") {
            document.getElementById("Hforder").value = "desc";

            var sort = "order by firstname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        }
        else {
            document.getElementById("Hforder").value = "asc";
            var sort = "order by firstname asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        var Filname = document.getElementById("FilterbyName").value;
        var pageno = self.currentPageIndex();
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: "/Useraddress/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&firstname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.usersadd(data); gettotalrecord();//GetEmployees();
            },
            error: function (error) {
                alert(error.status + "<!----!>" + error.statusText);
            }
        });

    }

    self.Search = function (usersadd) {
        var pagesize = document.getElementById("pageSizeSelector").value;
        var Filname = document.getElementById("FilterbyName").value;
        var pageno = self.currentPageIndex();
        gettotalrecord();
        $.ajax({
            type: "GET",
            url: "/Useraddress/Search?PageSize=" + pagesize + "&PageNo=" + pageno + "&firstname=" + Filname,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                if (data.length == 0) {
                    self.Success(false);
                    self.MessageError("No Record Found."); $('#error').text("");
                    self.Error(true);
                    self.usersadd(data);
                    return;

                }
                self.usersadd(data);
                self.Error(false);
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

    function gettotalrecord() {
        var id = 0;
        if (window.location.href.indexOf('id') >= 0) {
            var splitdata = window.location.href.split('=');
            id = splitdata[1];
        }
        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value; 
        $.ajax({
            type: "GET",
            url: '/Useraddress/search?PageSize=' + pagesize + "&PageNo=-1" + "&firstname=" + Filname + "&tempid=" +id,
            Contenttype: "application/JSON",
            success: function (data) {
                self.totalrecord(data[0].id);                
            }
        });
    }

    self.nextPage = function () {
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by firstname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by firstname asc"
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
            url: "/Useraddress/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&firstname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.usersadd(data);//GetEmployees();
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
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by firstname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by firstname asc"
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
            url: "/Useraddress/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&firstname=" + Filname + "&sort=" + sortf,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.usersadd(data);//GetEmployees();
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
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by firstname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by firstname asc"
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
            url: "/Useraddress/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&firstname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.usersadd(data);
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

    Getcountryname();

    Getusername();

    gettotalrecord();

    getdata();


}