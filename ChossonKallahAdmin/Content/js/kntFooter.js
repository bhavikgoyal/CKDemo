function FooterKO()
{
    var self = this;
    self.id = ko.observable(0);
    self.title = ko.observable("");
    self.contentname = ko.observable("");
    self.contentid = ko.observable();
    self.linkurl = ko.observable("");
    self.rank = ko.observable("");
    self.active = ko.observable(false);
    self.pageddl = ko.observable(false);
    self.menutype = ko.observable("");
    self.mainmenu = ko.observable("");

    
    self.footers = ko.observableArray();
    self.contents = ko.observableArray();

    self.insert = ko.observable(false);
    self.index = ko.observable(true);

    self.Success = ko.observable(false);
    self.Error = ko.observable(false);
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");
    self.savebtn = ko.observable(true);
    self.updatebtn = ko.observable(false);


    //variable og paging
    self.currentPage = ko.observable();
    self.pageSize = ko.observable(10);
    self.currentPageIndex = ko.observable(1);
    self.totalrecord = ko.observable(0);
    self.search = ko.observable(true);

    //insert & update coad

    self.clear = function () {
        self.id("");
        self.title("");
        self.contentid(0);
        self.linkurl("");
        self.rank("");
        self.active("");
        self.mainmenu(0);
        self.menutype(0);
    }

    self.Addnew = function () {
        document.getElementById("add").innerHTML = "Add Footer";
        self.clear();
        self.Success(false);
        self.Error(false);
        self.index(false);
        self.insert(true);
        self.savebtn(true);
        self.updatebtn(false);
        getmaxrank();
        
        $('#txttitle').focus();
    }

    self.Cancel = function () {
        self.Success(false);
        self.Error(false);
        self.index(true);
        self.insert(false);
        self.clear();
        Self.pageddl(false);
    }

    self.Save = function (data) {
        self.linkurl($('#txtlink').val());
        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax(
                {
                    type: "POST",
                    url: "/Footerlink/Insert",
                    data: ko.toJSON(data),
                    contentType: "application/json",
                    async: true,
                    success: function (data) {
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

    self.changeddl = function () {
        if ($('#ddlmenutype').val() == 4) {
            self.pageddl(true);
        }
        else {
            self.pageddl(false);
        }
    }


    self.checkvalidation = function () {
        var IsError = true;
        self.MessageError('');
        var reg = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/;
        if (self.title() == "") {
            self.MessageError(self.MessageError() + "<br/> - Title is required.");
            IsError = false;
        }
        
        if ($('#ddlmenutype').val() == 0) {
            self.MessageError(self.MessageError() + "<br/> - Menu type is required.");
            IsError = false;
        }
        if ($('#ddlmenutype').val() == 3) {
            if ($('#page').val() == "") {
                self.MessageError(self.MessageError() + "<br/> - Content Name is required.");
                IsError = false;
            }
        }
        if ($('#ddlmainmenu').val() == 0) {
            self.MessageError(self.MessageError() + "<br/> - Main Menu is required.");
            IsError = false;
        }

        self.Error(true);
        return IsError;
    }

    function getnames() {
        $.ajax({
            type: "GET",
            url: '/Footerlink/getcontent',
            Contenttype: "application/JSON",
            success: function (data) {
                self.contents(data);
            }
        });
    }

    function getmaxrank() {
        $.ajax({
            type: "GET",
            url: '/Footerlink/getrank',
            Contenttype: "application/JSON",
            success: function (data) {
                self.rank(data[0].rank+1);
            }
        });
    }

    self.getselectedpages = function (footers) {
        self.Success(false);
        self.Error(false);
        self.id(footers.id);
        self.linkurl(footers.linkurl);
        self.title(footers.title);
        self.contentid(footers.contentid);
        self.rank(footers.rank);
        document.getElementById("add").innerHTML = "Edit Footer";
        self.active(footers.active);
        self.menutype(footers.menutype);
        self.mainmenu(footers.mainmenu);
        if ($('#ddlmenutype').val() == 4) {
            self.pageddl(true);
        }
        self.index(false);
        self.insert(true);
        self.updatebtn(true);
        self.savebtn(false);
        $('#txttitle').focus();
    }

    self.deleterec = function () {
        self.Error(false);
        id = $('#DeleteID').val(),
          $.ajax(
              {
                  type: "POST",
                  url: "/Footerlink/Delete/" + id,
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
                    url: "/Footerlink/Update",
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
            if (self.updatebtn() == true) {
                self.Update(d);
            }
            else {
                self.Save(d);
            }
        }
    }

    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 10),
        startIndex = pagesize * self.currentPageIndex(),
        endIndex = startIndex + pagesize;
        return self.footers.slice(startIndex, endIndex);
    });

    function getdata() {
        $('#loader').hide();
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
            url: '/Footerlink/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&title=" + Filname,
            Contenttype: "application/JSON",
            success: function (data) {
                self.footers(data);
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
            url: "/Footerlink/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&title=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.footers(data); gettotalrecord();//GetEmployees();
                $('#loader').hide();

            },
            error: function (error) {
                alert(error.status + "<!----!>" + error.statusText);
            }
        });

    }

    self.Search = function (footers) {
        $('#loader').show();
        var pagesize = document.getElementById("pageSizeSelector").value;
        var Filname = document.getElementById("FilterbyName").value;
        var oldpageno = self.currentPageIndex();
        self.currentPageIndex(1);
        var pageno = 1;
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by rank desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by rank asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }
        gettotalrecord();
        $.ajax({
            type: "GET",
            url: "/Footerlink/Search?PageSize=" + pagesize + "&PageNo=" + pageno + "&title=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                if (data.length == 0) {
                    self.Success(false);
                    self.MessageError("No Record Found.");
                    $('#error').text("");
                    self.Error(true);
                    self.footers(data);
                    $('#loader').hide();
                    return;

                }
                self.footers(data);
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

    function gettotalrecord() {

        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: '/Footerlink/search?PageSize=' + pagesize + "&PageNo=-1" + "&title=" + Filname,
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


            var sort = "order by rank asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png')

        }
        else {

            var sort = "order by rank desc"
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
            url: "/Footerlink/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&title=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.footers(data);
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
            url: "/Footerlink/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&title=" + Filname,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.footers(data);//GetEmployees();
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


            var sort = "order by rank desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by rank asc"
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
            url: "/Footerlink/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&title=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.footers(data);
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

    self.changestatus = function (footers) {
        $('#loader').show();
        var active = footers.active;
        var id = footers.id;

        $.ajax({
            type: "POST",
            url: "/Footerlink/UpdateActive?id=" + id + "&Active=" + active,
            contentType: "application/json",
            success: function (data) {

                self.MessageSuccess("Record has been Updated Successfully.");
                self.Success(true);
                getdata();
                $('#loader').hide();
            },
            error: function (error) {
            }
        });

    };

    self.deletepopup = function (footers) {
        $("#myModal").modal('show');

        $('#DeleteID').val(footers.id);

    }


    self.Dataupside = function (footers) {
        $.ajax({
            type: "POST",
            url: "/Footerlink/updata?id=" + footers.id + "&up=" + "up",
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

    self.Datadownside = function (footers) {
        $.ajax({
            type: "POST",
            url: "/Footerlink/downdata?id=" + footers.id + "&down=" + "down",
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

    getnames();
    gettotalrecord();
     getdata();


}