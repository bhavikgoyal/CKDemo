﻿function NewslettersubscriberKO()
{
    var self = this;
    self.id = ko.observable(0);
    self.emailid = ko.observable("");

    self.Success = ko.observable(false);
    self.Error = ko.observable(false);
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");

    self.savebtn = ko.observable(true);
    self.updatebtn = ko.observable(false);
       
    self.newses = ko.observableArray();

    self.insert = ko.observable(false);
    self.index = ko.observable(true);

    //variable og paging
    self.currentPage = ko.observable();
    self.pageSize = ko.observable(50);
    self.currentPageIndex = ko.observable(1);
    self.totalrecord = ko.observable(0);
    self.search = ko.observable(true);

    self.clear = function () {
        self.id("");
        self.emailid("");
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

    self.Addnew = function () {
        self.clear();
        self.Success(false);
        self.Error(false);
        self.index(false);
        self.insert(true);
        self.savebtn(true);
        self.updatebtn(false);
        $('#txtemail').focus();
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
                    url: "/Newslettersubscriber/Insert",
                    data: ko.toJSON(data),
                    contentType: "application/json",
                    async: true,
                    success: function (data) {
                        self.Error(false);
                        self.MessageSuccess("Record has been Saved Successfully.");
                        self.Success(true);
                        self.insert(false);
                        self.index(true);
                        gettotalrecord();
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
        var reg = /^([A-Za-z0-9_\-\.]+)@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,3})$/;
        var IsError = true;
        self.MessageError('');
        if (self.emailid() == "") {
            self.MessageError(self.MessageError() + "<br/> - Email Id is required.");
            IsError = false;
        } else {

            if (reg.test(self.emailid()) == false) {
                self.MessageError(self.MessageError() + " <br/> - Email-Id is invalid.");
                IsError = false;
            

            }

        }

        self.Error(true);
        return IsError;
    }

    ////pagging

    self.searchKeyUp = function (d, e) {
        if (e.keyCode == 13) {
            self.Search();
        }
    }

    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 50),
        startIndex = pagesize * self.currentPageIndex(),
        endIndex = startIndex + pagesize;
        return self.newses.slice(startIndex, endIndex);
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
            url: '/Newslettersubscriber/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&emailid=" + Filname,
            Contenttype: "application/JSON",
            success: function (data) {
                self.newses(data);
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

            var sort = "order by emailid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        }
        else {
            document.getElementById("Hforder").value = "asc";
            var sort = "order by emailid asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        var Filname = document.getElementById("FilterbyName").value;
        var pageno = self.currentPageIndex();
        var pagesize = document.getElementById("pageSizeSelector").value;
        
        $.ajax({
            type: "GET",
            url: "/Newslettersubscriber/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&emailid=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            
            success: function (data) {
                self.newses(data); gettotalrecord();//GetEmployees();
                $('#loader').hide();
            },
            error: function (error) {
                alert(error.status + "<!----!>" + error.statusText);
                $('#loader').hide();
            }
        });

    }

    self.Search = function (newses) {
        $('#loader').show();
        self.Error(false);
        self.Success(false);
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by emailid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by emailid asc"
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
            url: "/Newslettersubscriber/Search?PageSize=" + pagesize + "&PageNo=" + pageno + "&emailid=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {
               
                if (data.length == 0) {
                    self.Success(false);
                    self.MessageError("No Record Found.");
                    $('#error').text("");
                    self.Error(true);
                    self.newses(data);
                    $('#loader').hide();
                    return;

                }
                self.newses(data);
                $('#loader').hide();
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
        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: '/Newslettersubscriber/search?PageSize=' + pagesize + "&PageNo=-1" + "&emailid=" + Filname,
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


            var sort = "order by emailid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by emailid asc"
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
            url: "/Newslettersubscriber/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&emailid=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.newses(data);//GetEmployees();
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


            var sort = "order by emailid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by emailid asc"
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
            url: "/Newslettersubscriber/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&emailid=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.newses(data);//GetEmployees();
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
        var pageno = self.currentPageIndex();
        var totalrec = document.getElementById("Hftotalrecord").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by emailid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by emailid asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }
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
            url: "/Newslettersubscriber/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&emailid=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.newses(data);
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

    self.changestatus = function (newses) {

        var isactive = newses.isactive;
        var id = newses.id;

        $.ajax({
            type: "POST",
            url: "/Newslettersubscriber/UpdateActive?id=" + id + "&Active=" + isactive,
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

    self.deletepopup = function (newes) {
        $("#myModal").modal('show');

        $('#DeleteID').val(newes.id);

    }

    self.deleterec = function () {
        var id = $('#DeleteID').val();
        $.ajax(
            {
                type: "POST",
                url: "/Newslettersubscriber/Delete/" + id,
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