function PagesKO() {

    var self = this;
     
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");
    self.Error = ko.observable(false);
    self.Success = ko.observable(false);

    self.id = ko.observable(0);
    self.contentname = ko.observable("");
    self.contentdesc = ko.observable("");
    self.metakeyword = ko.observable("");
    self.pagetitle = ko.observable("");
    self.metadescription = ko.observable("");
    self.active = ko.observable(false);


    self.insert = ko.observable(false);
    self.index = ko.observable(true);
    self.edit = ko.observable(false);

    self.AddButton = ko.observable(true);
    self.LoadScreen = ko.observable(false);
    self.pages = ko.observableArray();


    //for pageing 
    self.totalrecord = ko.observable(0);
    self.currentPage = ko.observable();
    self.pageSize = ko.observable(50);
    self.currentPageIndex = ko.observable(1);
    self.contacts = ko.observableArray();
    self.sortType = "ascending";
    self.currentColumn = ko.observable("");
    self.iconType = ko.observable("");



    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 50),
        startIndex = pagesize * self.currentPageIndex(),
        endIndex = startIndex + pagesize;
        return self.contacts.slice(startIndex, endIndex);

    });
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
    self.nextPage = function () {
        self.Error(false);
        self.Success(false);
        $("#loader").show();
        self.Error(false);
        self.currentPageIndex(self.currentPageIndex() + 1);
        if (self.currentPageIndex() > 1) {
            self.currentPageIndex();
        }
        else {
            self.currentPageIndex() = 1;
        }
        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;

        Filname = document.getElementById("FilterbyName").value;
        $.ajax({
            type: "GET",
            url: "/Pages/Search?PageSize=" + pagesize + "&PageNo=" + self.currentPageIndex() + "&Filname=" + Filname,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {
                self.pages(data);//GetEmployees();
                $('#loader').hide();
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


    };

    self.previousPage = function () {
        self.Error(false);
        self.Success(false);
        $("#loader").show();
        self.Error(false);

        if (self.currentPageIndex() > 1) {

            self.currentPageIndex(self.currentPageIndex() - 1);
            var pageno = self.currentPageIndex();

        }

        else {
            self.currentPageIndex() = 1;


        }


        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: "/Pages/Search?PageSize=" + pagesize + "&PageNo=" + self.currentPageIndex() + "&Filname=" + Filname,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {
                self.pages(data);//GetEmployees();
                $('#loader').hide();
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
    };

    self.selectionChanged = function (event) {
        self.Error(false);
        self.Success(false);
        $("#loader").show();
        self.Error(false);
        var pagesize = document.getElementById("pageSizeSelector").value;
        if ((self.totalrecord() / pagesize) < self.currentPageIndex()) {

            if (self.currentPageIndex() == 1) {
                self.currentPageIndex(1);
            }
            else {

                self.currentPageIndex(self.currentPageIndex() - 1)
            }

        }

        Filname = document.getElementById("FilterbyName").value;
        $.ajax({
            type: "GET",
            url: "/Pages/Search?PageSize=" + pagesize + "&PageNo=" + self.currentPageIndex() + "&Filname=" + Filname,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {
                self.pages(data);//GetEmployees();
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

    }

    self.SortData = function () {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        self.Error(false);

        if (document.getElementById("Hforder").value == "asc") {
            document.getElementById("Hforder").value = "desc";
            var sort = "order by contentname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');
        }
        else {
            document.getElementById("Hforder").value = "asc";
            var sort = "order by contentname asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }
        var pagesize = document.getElementById("pageSizeSelector").value;
        var Filname = document.getElementById("FilterbyName").value;
        $.ajax({
            type: "GET",
            url: "/Pages/Search?PageSize=" + pagesize + "&PageNo=" + self.currentPageIndex() + "&Filname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {
                self.pages(data); GetTotalRecord();
                $('#loader').hide();
            },
            error: function (error) {
                self.MessageError(error);
                self.Error(true);
                $('#loader').hide();
            }
        });

    }

    self.Search = function (pages) {
        $("#loader").show();
        self.Error(false);
        self.Success(false);
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by contentname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by contentname asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        var pagesize = document.getElementById("pageSizeSelector").value;
        var Filname = document.getElementById("FilterbyName").value;
        var oldpageno = self.currentPageIndex();
        self.currentPageIndex(1);
        var pageno = 1;
        GetTotalRecord();
        $.ajax({
            type: "GET",
            url: "/Pages/Search?PageSize=" + pagesize + "&PageNo=" + self.currentPageIndex() + "&Filname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                if (data.length == 0) {
                    self.MessageError("No Record Found.");
                    $('#error').text(""); 
                    self.Error(true);
                    self.pages(data);
                    $('#loader').hide();
                    return;

                }
                self.pages(data);
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


    self.changestatus = function (pages) {

        var active = pages.active;
        var id = pages.id;

        $.ajax({
            type: "POST",
            url: "/Pages/UpdateActive?id=" + id + "&Active=" + active,
            contentType: "application/json",
            success: function (data) {
                self.MessageSuccess("Record has been Updated Successfully.");
                self.Success(true);
                GetPages();
            },
            error: function (error) {

            }
        });
    };

    self.Canceldelpopup = function () {

        $("#dialog-confirm").dialog("close");
    }
    
    self.deletepopup = function (data) {
        $('#hiddenVal').val(data.id);
        $("#dialog-confirm").dialog("open");
    }
    
    self.save = function (data) {
        self.Error(false);
        if (checkvalidation()) {
            self.Error(false);
            jQuery.fn.CKEditorValFor = function (element_id) {
                return CKEDITOR.instances[element_id].getData();
            }
            data.contentdesc = $().CKEditorValFor('txtcontentdesc1');
            $.ajax({
                type: "POST",
                url: "/Pages/Insert",
                data: ko.toJSON(data),
                contentType: "application/json",
                async: false,
                success: function (data) {
                    self.Error(false);
                    self.MessageSuccess("Record has been Saved Successfully.");
                    GetPages();
                    self.Success(true);
                    self.Error(false);
                    self.insert(false);
                    self.edit(false);
                    self.index(true);
                    self.AddButton(true);
                    Clear();


                },

                error: function (error) {
                    self.MessageError(error);
                    self.Error(true);
                }
            });
        }
        //$("#Update").dialog("close");
    };
    
    self.update = function (data) {
        self.Error(false);
        if (checkvalidation()) {
            self.Error(false);
            jQuery.fn.CKEditorValFor = function (element_id) {
                return CKEDITOR.instances[element_id].getData();
            }
            data.contentdesc = $().CKEditorValFor('txtcontentdesc2');

            $.ajax({
                type: "POST",
                url: "/Pages/Update",
                data: ko.toJSON(data),
                contentType: "application/json",
                async: false,
                success: function (data) {
                    self.Error(false);
                    self.MessageSuccess("Record has been Updated Successfully.");
                    GetPages();
                    self.Success(true);
                    self.Error(false);
                    self.insert(false);
                    self.edit(false);
                    self.index(true);
                    self.AddButton(true);
                    Clear();


                },

                error: function (error) {
                    self.MessageError(error);
                    self.Error(true);
                }
            });
        }
        //$("#Update").dialog("close");
    };
    
    function checkvalidation() {
        var IsError = true;
        self.MessageError('');
        if (self.contentname() == "") {
            self.MessageError("<br/> - Content Name is required.");
            IsError = false;

        }

        self.Error(true);
        return IsError;
    }
    
    self.deletepopup = function (pages) {
        $("#myModal").modal('show');

        $('#DeleteID').val(pages.id);

    }
    
    self.deleterec = function () {
        var id = $('#DeleteID').val();
        $.ajax({
            type: "POST",
            url: '/Pages/Delete/' + id,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#myModal").modal('hide');
                self.MessageSuccess("Record has been Deleted Successfully.");
                self.Success(true);
                GetPages();
                self.edit(false);
                self.index(true);
                self.AddButton(true);


            },
            error: function (error) {
                self.MessageError(error);
                self.Error(true);
            }
        });
    };
      
    self.getselectedpages = function (pages) {

        self.id(pages.id),
 self.contentname(pages.contentname),

 self.metakeyword(pages.metakeyword),
          self.pagetitle(pages.pagetitle),
 self.metadescription(pages.metadescription),
        self.active(pages.active),
          CKEDITOR.instances.txtcontentdesc2.setData(pages.contentdesc);

        self.edit(true);
        self.insert(false);
        self.index(false);
        self.Success(false);
        self.Error(false);

        $('#txtproductname').focus();



    };
    
    self.Cancel = function () {
        Clear();
        self.Error(false);
        self.index(true);
        self.edit(false);
        self.insert(false);
        self.AddButton(true);
        //$("#Update").dialog("close");

    };
    
    self.Addnew= function () {
        Clear();
        self.Error(false);
        self.AddButton(false);
        self.edit(false);
        self.insert(true);
        self.index(false);
        self.Error(false);
        self.Success(false);
        $('#txtcn').focus();
    }
    
    function GetPages() {
        $("#loader").show();
        self.Error(false);
        rec = self.totalrecord();
        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        if (self.currentPageIndex() > 1) {
            var ttl = rec / pagesize;
            var diff = self.currentPageIndex() - ttl;
            if (diff > 0.9) {
                self.currentPageIndex(self.currentPageIndex() - 1);
            }
        }

        $.ajax({
            type: "GET",
            url: "/Pages/Search?PageSize=" + pagesize + "&PageNo=" + self.currentPageIndex() + "&Filname=" + Filname,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                self.pages(data);
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
    }

    function GetTotalRecord() {

        var Filname = document.getElementById("FilterbyName").value;

        var pagesize = document.getElementById("pageSizeSelector").value; 
        $.ajax({
            type: "GET",
            url: "/Pages/Search?PageSize=" + pagesize + "&PageNo=-1&Filname=" + Filname,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                self.totalrecord(data[0].id);

            },

        });
    }

    function Clear() {

        self.id(0);
        self.pagetitle("");
        self.contentname("");
        CKEDITOR.instances["txtcontentdesc1"].setData('');
        self.metadescription("");
        self.metakeyword("");
        self.active("")

    }

    self.searchKeyUp = function (d, e) {
        if (e.keyCode == 13) {
            self.Search();
        }
    }

    GetTotalRecord();
    GetPages();




}


