
function adminKO()
{
    var self = this;
    self.id = ko.observable("");
    self.loginid = ko.observable("");
    self.password = ko.observable("");
    self.email = ko.observable("");
    self.admintype = ko.observable("");
    self.lastlogindate = ko.observable("");
    self.active = ko.observable(false);
    self.admins = ko.observableArray();
    self.insert = ko.observable(false);
    self.role = ko.observableArray();
    self.roles = ko.observable("");
    self.ConfirmPassword = ko.observable("");
    self.index = ko.observable(true);

    self.Success = ko.observable(false);
    self.Error = ko.observable(false);
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");
    self.savebtn = ko.observable(true);
    self.updatebtn = ko.observable(false);


    //variable og paging
    self.currentPage = ko.observable();
    self.pageSize = ko.observable(50);
    self.currentPageIndex = ko.observable(1);
    self.totalrecord = ko.observable(0);
    self.search = ko.observable(true);

    //insert & update coad

    

    self.clear = function () {
        self.id("");
        self.loginid("");
        self.password("");
        self.ConfirmPassword("");
        self.email("");
        self.admintype(null);
        self.active("");
    }

    self.Addnew = function () {
        document.getElementById("add").innerHTML = "Add Admin";
        self.clear();
        self.Success(false);
        self.Error(false);
        self.index(false);
        self.insert(true);
        self.savebtn(true);
        self.updatebtn(false);
        $('#loginid').focus();
    }

    self.Cancel = function () {
        self.Success(false);
        self.Error(false);
        self.index(true);
        self.insert(false);
         self.clear();
    }

    self.Save = function (data) {
       
        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax(
                {
                    type: "POST",
                    url: "/Admin/Insert",
                    data: ko.toJSON(data),
                    contentType: "application/json",
                    async: true,
                    success: function (data) {
                            
                        if (data.indexOf("Email already exists.") >= 0) {
                            $('#WarningShow').show();
                            $('#WarningShow').html('Email is already exists.');
                        }
                        else {
                            if (data.length > 0) {
                                gettotalrecord();
                            }
                            self.Error(false);
                            self.MessageSuccess("Record has been Saved Successfully.");
                            self.Success(true);
                            self.insert(false);
                            self.index(true);
                            getdata();
                            self.clear();
                        }
                    },
                    error: function (error) {
                        self.MessageError(error);
                        self.Error(true);

                    }
                });
        }
        else {
            self.Error(true);
            self.Success(false);

        }

    }

    self.checkvalidation = function () {
        var IsError = true;
        self.MessageError('');
        var reg = /^([A-Za-z0-9_\-\.]+)@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,3})$/;
        if (self.loginid() == "") {
            self.MessageError(self.MessageError() + "<br/> - User Name is required.");
            IsError = false;
        }
        if (self.email() == "") {
            self.MessageError(self.MessageError() + "<br/> - Email is required.");
            IsError = false;
        }
        else {

            if (reg.test(self.email()) == false) {
                self.MessageError(self.MessageError() + " <br/> - Email is Invalid.");
                IsError = false;
            }
        }
        if (self.admintype() == "") {
            self.MessageError(self.MessageError() + "<br/> - Admin Type is required.");
            IsError = false;
        }
        
        if (self.password() == "") {
            self.MessageError(self.MessageError() + "<br/> - Password is required.");
            IsError = false;
        }
        if (self.ConfirmPassword() == "") {
            self.MessageError(self.MessageError() + "<br/> - Confirm Password is required.");
            IsError = false;
        }
       
        if ($('#admintype').val() == "") {
            self.MessageError(self.MessageError() + "<br/> - Admin Type is required.");
            IsError = false;
        }

        if (self.ConfirmPassword() != self.password()) {
            self.MessageError(self.MessageError() + "<br/> - Password and Confirm Password does not match.");
            IsError = false;
        }

        self.Error(true);
        return IsError;
    }
    
    function getnames() {
        $.ajax({
            type: "GET",
            url: '/Admin/getnames',
            Contenttype: "application/JSON",
            success: function (data) {
                self.roles(data);
            }
        });
    }

    self.getselectedpages = function (admins) {
        self.Success(false);
        self.Error(false);
        self.id(admins.id);
        self.loginid(admins.loginid);
        self.password(admins.password);
        self.email(admins.email);
        self.ConfirmPassword(admins.password);
        self.admintype(admins.admintype);
        document.getElementById("add").innerHTML = "Edit Admin";
        self.lastlogindate(admins.lastlogindate);
        self.active(admins.active);
        self.index(false);
        self.insert(true);
        self.updatebtn(true);
        self.savebtn(false);
        $('#loginid').focus();
    }

    self.deleterec = function () {
        self.Error(false);
        
          id = $('#DeleteID').val(),
            $.ajax(
                {
                    type: "POST",
                    url: "/Admin/Delete/" + id,
                    success: function (data) {
                        $("#myModal").modal('hide');
                        self.Error(false);
                        self.MessageSuccess("Record has been Deleted Successfully.");
                        self.Success(true);
                        self.Error(false);
                        self.index(true);
                        self.insert(false);
                        gettotalrecord();
                        getdata();
                                                
                  },
                    error: function (error) {
                        self.MessageError(error);
                        self.Error(true);
                    }
                })
    }

    self.Update = function (data) {
        if (self.checkvalidation()) {
            
            self.Error(false);
                $.ajax(
                    {
                        type: "POST",
                        url: "/Admin/Update",
                        data: ko.toJSON(data),
                        contentType: "application/json",
                        async: true,
                        success: function (data) {
                            gettotalrecord();
                            getdata();
                            self.index(true);
                            self.Error(false);
                            self.MessageSuccess("Record has been Updated Successfully.");
                            self.Success(true);
                            self.insert(false);
                            

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

    self.Entersave = function (d, e) {
        if (e.keyCode == 13) {
            if (self.updatebtn() == true)
            {
                self.Update(d);
            }
            else
            {
                self.Save(d);
            }
          }
    }

    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 50),
        startIndex = pagesize * self.currentPageIndex(),
        endIndex = startIndex + pagesize;
        return self.admins.slice(startIndex, endIndex);
    });

    function getdata() {
        $('#loader').show();
        var rec = self.totalrecord();
        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        if (self.currentPageIndex() > 1) {
            var ttl = rec / pagesize;
            var diff = self.currentPageIndex() - ttl;
            if (rec > 31)
            {
                if (diff > .9) {
                    self.currentPageIndex(self.currentPageIndex() - 1);
                }
            }
            else
            {
                if (diff > .8) {
                    self.currentPageIndex(self.currentPageIndex() - 1);
                }
            }
            
        }
        
        $.ajax({
            type: "GET",
            url: '/Admin/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&loginid=" + Filname,
            Contenttype: "application/JSON",
            success: function (data) {
                    self.admins(data);
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

            var sort = "order by loginid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        }
        else {
            document.getElementById("Hforder").value = "asc";
            var sort = "order by loginid asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        var Filname = document.getElementById("FilterbyName").value;
        var pageno = self.currentPageIndex();
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: "/Admin/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&loginid=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.admins(data);//GetEmployees();
                gettotalrecord();
                $('#loader').hide();

            },
            error: function (error) {
                alert(error.status + "<!----!>" + error.statusText);
            }
        });
     
    }

    self.Search = function (admins) {
        $('#loader').show();
        var pagesize = document.getElementById("pageSizeSelector").value;
        var Filname = document.getElementById("FilterbyName").value;
        var oldpageno = self.currentPageIndex();
        self.currentPageIndex(1);
        var pageno = 1;
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by loginid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by loginid asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }
        gettotalrecord();
        $.ajax({
            type: "GET",
            url: "/Admin/Search?PageSize=" + pagesize + "&PageNo=" + pageno + "&loginid=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                if (data.length == 0) {
                    self.Success(false);
                    self.MessageError("No Record Found.");
                    self.Error(true);
                    $('#error').text("");
                    self.admins(data);
                    $('#loader').hide();
                    return;

                }
                self.admins(data);
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

      

    };

    function gettotalrecord()  {

        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: '/Admin/search?PageSize=' + pagesize + "&PageNo=-1" + "&loginid=" + Filname,
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
        
        if (document.getElementById("Hforder").value == "asc") {


            var sort = "order by loginid asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png')

        }
        else {

            var sort = "order by loginid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');
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
            url: "/Admin/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&loginid=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.admins(data);
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


            var sort = "order by loginid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by loginid asc"
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
            url: "/Admin/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&loginid=" + Filname,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.admins(data);//GetEmployees();
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
            

            var sort = "order by loginid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {
     
            var sort = "order by loginid asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }
        var pageno = self.currentPageIndex();
        var totalrec = document.getElementById("Hftotalrecord").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        gettotalrecord();
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
            url: "/Admin/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&loginid=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.admins(data);
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

    self.changestatus = function (admins) {
        $('#loader').show();
        var active = admins.active;
        var id = admins.id;

        $.ajax({
            type: "POST",
            url: "/Admin/UpdateActive?id=" + id + "&Active=" + active,
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

    self.deletepopup = function (admins) {
        $("#myModal").modal('show');

        $('#DeleteID').val(admins.id);
        
    }
    
    getnames();

    gettotalrecord();

    getdata();
   
}