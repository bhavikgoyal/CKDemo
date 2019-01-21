function BannersKO() {

    var self = this;
    self.savebtn = ko.observable(true);
    self.updatebtn = ko.observable(false);
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");
    self.Error = ko.observable(false);
    self.Success = ko.observable(false);
    self.id = ko.observable(0);
    self.title = ko.observable("");
    self.bannerimage = ko.observable("");
    self.bannertext = ko.observable("");
    self.bannertext2 = ko.observable("");
    self.isactive = ko.observable(false);
    self.bannerrank = ko.observable("");
    self.totalrecord = ko.observable(0);
    self.Banners = ko.observableArray();
    self.updatemethod = ko.observable(false);
    self.insert = ko.observable(false);
    self.index = ko.observable(true);
    self.SortBy = ko.observable("");

    self.AddButton = ko.observable(true);
    self.SaveButton = ko.observable(true);
    self.UpdateButton = ko.observable(false);
    self.LoadScreen = ko.observable(false);

    //for pageing 
    self.totalrecord = ko.observable(0);
    self.currentPage = ko.observable();
    self.pageSize = ko.observable(50);
    self.currentPageIndex = ko.observable(1);
    self.contacts = ko.observableArray();
    self.sortType = "ascending";
    self.currentColumn = ko.observable("");
    self.iconType = ko.observable("");

    self.searchKeyUp = function (d, e) {
        if (e.keyCode == 13) {
            self.Search();
        }
    }

    self.ClickSave = function (d, e) {
        if (e.keyCode == 13) {

            self.Save();
        }
    }

    self.ClickUpdate = function (d, e) {
        if (e.keyCode == 13) {

            self.Update();
        }
    }

    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 50),
        startIndex = pagesize * self.currentPageIndex(),
        endIndex = startIndex + pagesize;
        return self.contacts.slice(startIndex, endIndex);
    });

    self.nextPage = function () {
        self.Error(false);
        self.Success(false);
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by bannerrank desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by bannerrank asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }
        self.Error(false);
        self.currentPageIndex(self.currentPageIndex() + 1);
        if (self.currentPageIndex() > 1) {
            self.currentPageIndex();
        }
        else {
            self.currentPageIndex() = 1;
        }
        var pagesize = document.getElementById("pageSizeSelector").value;

        getData(pagesize, self.currentPageIndex());
    };

    self.entrsv = function (d, e) {
        if (e.keyCode == 13) {
            self.Save(d);
        }
    }

    function getData(pagesize, PageNo) {
        $("#loader").show();
        //var sort = "order by bannerrank asc";
        $.ajax({
            type: "Get",
            url: "/Banners/Search?Pagesize=" + pagesize + "&PageNo=" + PageNo + "&Filname=" + $("#FilterbyName").val() + "&sort=" + $('#SortValue').val(),
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {
                if (data.length == 0) {
                    if (self.currentPageIndex() > 1) {
                        self.currentPageIndex(self.currentPageIndex() - 1);
                        getData(pagesize, self.currentPageIndex(), search, sort);
                        $("#loader").hide();
                        return;
                    }
                    self.MessageError("No Record Found...");
                    self.Error(true);
                    self.Banners(data);
                    $("#loader").hide();
                    return;

                }
                self.totalrecord(data[0].id);
                self.Banners(data);
                $("#loader").hide();
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
                self.SaveButton(false);
                self.UpdateButton(false);
            },
            error: function (error) {
                self.MessageError(error);
                self.Error(true);
                $("#loader").hide();
            }

        })

    }

    self.previousPage = function () {
        self.Error(false);
        self.Success(false);
        if (self.currentPageIndex() > 1) {
            self.currentPageIndex(self.currentPageIndex() - 1);
            var pageno = self.currentPageIndex();
        }

        else {
            self.currentPageIndex() = 1;
        }
        var pagesize = document.getElementById("pageSizeSelector").value;
        getData(pagesize, self.currentPageIndex());
    };

    self.selectionChanged = function (event) {
        self.Error(false);
        self.Success(false);
        $("#loader").hide();
        var pagesize = document.getElementById("pageSizeSelector").value;
        if ((self.totalrecord / pagesize) <= self.currentPageIndex()) {

            if (self.currentPageIndex() == 1) {
                self.currentPageIndex(1);
            }
            else {

                self.currentPageIndex(self.currentPageIndex() - 1)
            }
        }
        getData(pagesize, self.currentPageIndex());
    }

    self.SortData = function () {
        self.Error(false);
        self.Success(false);

        if (document.getElementById("Hforder").value == "asc") {
            document.getElementById("Hforder").value = "desc";
            self.SortBy("order by bannerrank desc");
            $('#SortValue').val('order by bannerrank desc');
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');
        }
        else {
            document.getElementById("Hforder").value = "asc";
            self.SortBy("order by bannerrank asc");
            $('#SortValue').val('order by bannerrank asc');
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        getData(document.getElementById("pageSizeSelector").value, self.currentPageIndex());

    }

    self.Search = function (Banners) {
        $("#loader").show();
        self.Error(false);
        self.Success(false);
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by bannerrank desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by bannerrank asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }
        var pagesize = document.getElementById("pageSizeSelector").value;
        var Filname = document.getElementById("FilterbyName").value;
        GetTotalRecord();
        $.ajax({
            type: "GET",
            url: "/Banners/Search?PageSize=" + pagesize + "&PageNo=" + self.currentPageIndex() + "&Filname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                if (data.length == 0) {
                    self.Success(false);
                    self.MessageError("No Record Found.");
                    self.Error(true);
                    $('#error').text("");
                    self.Banners(data);
                    $("#loader").hide();
                    return;

                }
                self.Error(false);
                self.Success(false);
                self.Banners(data);
                $("#loader").hide();

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

    self.changestatus = function (Banners) {

        var isactiv = Banners.isactive;
        var ids = Banners.id;

        $.ajax({
            type: "POST",
            url: "/Banners/UpdateActive?id=" + ids + "&Active=" + isactiv,
            contentType: "application/json",
            success: function (data) {
                self.MessageSuccess("Record has been Updated Successfully.");
                self.Success(true);
                GetBanners();
            },
            error: function (error) {

            }
        });
    };

    self.deletepopup = function (Banners) {
        $("#myModal").modal('show');

        $('#DeleteID').val(Banners.id);

    }

    self.checkvalidation = function () {
        var IsError = true;
        self.MessageError('');

        if (self.title() == "") {
            self.MessageError(self.MessageError() + "<br/> - Title is required.");
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

    self.Save = function (data) {
        
        data.bannerimage($('#area').text());

        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax({

                url: "/Banners/Insert/",
                type: "POST",
                data: ko.toJSON(data),
                contentType: "application/json",
                async: true,
                success: function (response) {
                    self.Error(false);
                    self.MessageSuccess("Record Inserted Successfully.");
                    GetBanners();
                    self.Success(true);
                    self.Error(false);
                    self.insert(false);

                    self.index(true);
                    self.AddButton(true);
                    Clear();

                },
                error: function (ex) {

                    Error(true);
                }

            });
        }
        else {



            self.Error(true);
        }
        $("#loader").hide();
    };

    self.Update = function (data) {
        self.updatemethod(true);

        if ($('#area').text() != '') {
            data.bannerimage($('#area').text());

        } else {
            data.bannerimage($("#lblview").val());
        }

        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax({
                url: "/Banners/Update",
                type: "POST",
                data: ko.toJSON(data),
                contentType: "application/json",
                async: true,
                success: function (response) {
                    self.Error(false);
                    self.MessageSuccess("Record has been Updated Successfully.");
                    self.Success(true);
                    self.Error(false);
                    self.insert(false);

                    self.index(true);
                    self.AddButton(true);

                    Clear();
                    GetBanners();
                    self.updatemethod(false);
                },
                error: function (er) {

                }

            });
        }
        else {



            self.Error(true);
        }
        $("#loader").hide();
    };

    self.deleterec = function () {
        var id = $('#DeleteID').val();
         
        $.ajax({
            type: "POST",
            url: "/Banners/Delete/" + id,
            success: function (data) {
                $("#myModal").modal('hide');
                self.Error(false);
                self.MessageSuccess("Record has been Deleted Successfully");
                self.Success(true);
                GetBanners();

                self.index(true);
                self.AddButton(true);


            },
            error: function (error) {
                self.MessageError(error);
                self.Error(true);
            }
        })



    }

    self.Cancel = function () {
        Clear();
        self.AddButton(true);
        self.Error(false);
        self.index(true);

        self.insert(false);

        //$("#Update").dialog("close");

    };

    self.getselectedBanners = function (Banners) {

        document.getElementById("add").innerText = "Edit Website Banners";
        self.id(Banners.id),
        self.title(Banners.title);
        document.getElementById("viewimg").hidden = false;
        $("#lblview").val(Banners.bannerimage);
        $("#viewimg").attr('href', "../UploadImages/" + Banners.bannerimage);
        self.bannertext(Banners.bannertext);
        self.bannertext2(Banners.bannertext2);
        self.isactive(Banners.isactive);
        self.bannerrank(Banners.bannerrank);
        self.insert(true);
        self.index(false);
        self.Error(false);
        self.Success(false);
        self.savebtn(false);
        self.updatebtn(true);


    };

    function GetBanners() {


        $("#loader").show();

        var pagesize = document.getElementById("pageSizeSelector").value;

        var Filname = document.getElementById("FilterbyName").value;

        $.ajax({
            type: "GET",
            url: "/Banners/Search?PageSize=" + pagesize + "&PageNo=" + self.currentPageIndex() + "&Filname=" + Filname,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {
                self.Banners(data);
                if ((self.totalrecord() / pagesize) < self.currentPageIndex()) {

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

    }

    function GetTotalRecord() {
        var Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        var sort = "order by bannerrank asc"
        $.ajax({
            type: "GET",
            url: "/Banners/Search?PageSize=" + pagesize + "&PageNo=-1" +"&Filname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {
                self.totalrecord(data[0].id);
            },
        });
    }

    self.Addnew = function () {
        document.getElementById("add").innerText = "Add Website Banners";
        document.getElementById("viewimg").hidden = true;
        Clear();
        self.Error(false);
        self.savebtn(true);
        self.updatebtn(false);
        self.insert(true);
        self.index(false);
        self.Error(false);
        self.Success(false);
        getmaxrank();
        $('#title').focus();
    }

    function getmaxrank() {
        $.ajax({
            type: "GET",
            url: '/Banners/getrank',
            Contenttype: "application/JSON",
            success: function (data) {
                self.bannerrank(data[0].bannerrank + 1);
            }
        });
    }
     
    function Clear() {

        self.id = ko.observable(0);
        self.title("");
        self.bannerimage("");
        self.bannertext("");
        self.bannertext2("");
        self.isactive(false);
        self.bannerrank(1);
        $("#area").text("");
        $('#inputFileToLoad').val('');

    }

    self.Dataupside = function (Banners) {
        $.ajax({
            type: "POST",
            url: "/Banners/updata?id=" + Banners.id + "&up=" + "up",
            contentType: "application/json",
            success: function (data) {
                self.MessageSuccess("Record has been Updated Successfully.");
                self.Success(true);
                GetBanners();
            },
            error: function (error) {
            }
        });
    }

    self.Datadownside = function (Banners) {
        $.ajax({
            type: "POST",
            url: "/Banners/downdata?id=" + Banners.id + "&down=" + "down",
            contentType: "application/json",
            success: function (data) {
                self.MessageSuccess("Record has been Updated Successfully.");
                self.Success(true);
                GetBanners();
            },
            error: function (error) {
            }
        });
    }

    GetTotalRecord();

    GetBanners();
}

function validateNumber(event) {
    var key = window.event ? event.keyCode : event.which;

    if (event.keyCode === 8 || event.keyCode === 46
        || event.keyCode === 37 || event.keyCode === 39) {
        return true;
    }
    else if (key < 48 || key > 57) {
        return false;
    }
    else return true;
};