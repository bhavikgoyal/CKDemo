function MailTemplateKO() {

    var self = this;

    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");
    self.Error = ko.observable(false);
    self.Success = ko.observable(false);

    self.id = ko.observable(0);
    self.title = ko.observable("");
    self.searchkey = ko.observable("");
    self.subject = ko.observable("");
    self.mailbody = ko.observable("");
    self.index = ko.observable(true);
    self.Insert = ko.observable(false);

    self.totalrecord = ko.observable(0);
    self.mailtemplates = ko.observableArray();
    //self.mailtemplatesbyId = ko.observableArray();
    

    //for pageing 
    self.totalrecord = ko.observable(0);
    self.currentPage = ko.observable();
    self.pageSize = ko.observable(50);
    self.currentPageIndex = ko.observable(1);
    self.sortType = "ascending";
    self.currentColumn = ko.observable("");
    self.iconType = ko.observable("");
    
    self.savebtn = ko.observable(true);
    self.updatebtn = ko.observable(false);
   
    self.searchKeyUp = function (d, e) {
        if (e.keyCode == 13) {
            self.Search();
        }
    }

    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 50),
        startIndex = pagesize * self.currentPageIndex(),
        endIndex = startIndex + pagesize;
        return self.mailtemplates.slice(startIndex, endIndex);

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
        //var sort = "order by userid asc";
        //if (document.getElementById("Hforder").value == "asc") {
        //    document.getElementById("Hforder").value = "desc";

        //    sort = "order by couponcode desc"
        //    $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        //}
        //else {
        //    document.getElementById("Hforder").value = "asc";
        //    sort = "order by couponcode asc"
        //    $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        //}
        
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
            url: "/MailTemplate/Search?PageSize=" + pagesize + "&PageNo=" + self.currentPageIndex() + "&Filname=" + Filname,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {
                self.mailtemplates(data);//GetEmployees();
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

    self.previousPage = function () {
        self.Error(false);
        self.Success(false);
        $("#loader").show();
        self.Error(false);
        //var sort = "order by userid asc";
        //if (document.getElementById("Hforder").value == "asc") {
        //    document.getElementById("Hforder").value = "desc";

        //    sort = "order by couponcode desc"
        //    $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        //}
        //else {
        //    document.getElementById("Hforder").value = "asc";
        //    sort = "order by couponcode asc"
        //    $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        //}

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
            url: "/MailTemplate/Search?PageSize=" + pagesize + "&PageNo=" + self.currentPageIndex() + "&Filname=" + Filname,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {
                self.mailtemplates(data);//GetEmployees();
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
        
    };

    self.selectionChanged = function (event) {
        self.Error(false);
        self.Success(false);
        $("#loader").show();
        //var sort = "order by userid asc";
        //if (document.getElementById("Hforder").value == "asc") {
        //    document.getElementById("Hforder").value = "desc";

        //    sort = "order by couponcode desc"
        //    $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        //}
        //else {
        //    document.getElementById("Hforder").value = "asc";
        //    sort = "order by couponcode asc"
        //    $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        //}
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
            url: "/MailTemplate/Search?PageSize=" + pagesize + "&PageNo=" + self.currentPageIndex() + "&Filname=" + Filname,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {
                self.mailtemplates(data);//GetEmployees();
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

        

    }

    self.SortData = function () {
        self.Error(false);
        self.Success(false);

        if (document.getElementById("Hforder").value == "asc") {
            document.getElementById("Hforder").value = "desc";
            var sort = "order by title desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');
        }
        else {
            document.getElementById("Hforder").value = "asc";
            var sort = "order by title asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }
        var pagesize = document.getElementById("pageSizeSelector").value;
        var Filname = document.getElementById("FilterbyName").value;
        $.ajax({
            type: "GET",
            url: "/MailTemplate/Search?PageSize=" + pagesize + "&PageNo=" + self.currentPageIndex() + "&Filname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {
                self.mailtemplates(data); 
            },
            error: function (error) {
                self.MessageError(error);
                self.Error(true);
            }
        });

    }

    self.Search = function (mailtemplates) {
        $('#loader').show();
        self.Error(false);
        self.Success(false);
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by title desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by title asc"
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
            url: "/MailTemplate/Search?PageSize=" + pagesize + "&PageNo=" + self.currentPageIndex() + "&Filname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                if (data.length == 0) {
                    self.MessageError("No Record Found.");
                    $('#error').text("");
                    self.Error(true);
                    self.Success(false);
                    self.mailtemplates(data);
                    $("#loader").hide();
                    return;

                }
                self.mailtemplates(data);
                self.Success(false);
                self.Error(false);
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



    };

    self.Update = function (data) {
        self.Error(false);
        if (checkvalidation()) {
            self.Error(false);
            jQuery.fn.CKEditorValFor = function (element_id) {
                return CKEDITOR.instances[element_id].getData();
            }
            data.mailbody = $().CKEditorValFor('txtmailbody1');

            $.ajax({
                type: "POST",
                url: "/MailTemplate/Update",
                data: ko.toJSON(data),
                contentType: "application/json",
                async: false,
                success: function (data) {
                    self.Error(false);
                    self.MessageSuccess("Record has been Updated Successfully.");
                    Getmailtemplates();
                    self.Success(true);
                    self.Error(false);
                    self.Insert(false);
                    self.index(true);

                    Clear();


                },

                error: function (error) {
                    self.MessageError(error);
                    self.Error(true);
                }
            });
        }
       
    };

    function checkvalidation() {
        var IsError = true;
        self.MessageError('');
        if (self.title() == "") {
            self.MessageError("<br/> - Title is required.");
            IsError = false;

        }
        if (self.subject() == "") {
            self.MessageError(self.MessageError() + "<br/> - Subject is required.");

            IsError = false;
        }
        self.Error(true);
        return IsError;
    }

    self.getselectedmailtemplate = function (mailtemplates) {

        self.id(mailtemplates.id),
         self.title(mailtemplates.title),
         self.searchkey(mailtemplates.searchkey),
         self.subject(mailtemplates.subject),
        document.getElementById("add").innerText = "Edit Mail Template";
        //self.mailbody(mailtemplates.mailbody),
        CKEDITOR.instances.txtmailbody1.setData(mailtemplates.mailbody);
        self.Insert(true);
        self.index(false);
        self.Success(false);
        self.Error(false);
        self.updatebtn(true);
        self.savebtn(false);
       };

    self.Cancel = function () {
        Clear();
        self.Error(false);
        self.index(true);
        self.Insert(false);
        
      };

    self.Save = function (data) {
        self.Error(false);
        if (checkvalidation()) {
            jQuery.fn.CKEditorValFor = function (element_id) {
                return CKEDITOR.instances[element_id].getData();
            }
            data.mailbody = $().CKEditorValFor('txtmailbody1');

        

            $.ajax(
                {
                    type: "POST",
                    url: "/MailTemplate/Insert",
                    data: ko.toJSON(data),
                    contentType: "application/json",
                    async: true,
                    success: function (data) {
                        self.Error(false);
                        self.MessageSuccess("Record has been Saved Successfully.");
                        self.Success(true);
                        self.Insert(false);
                        self.index(true);
                        Getmailtemplates();
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

    function Getmailtemplates() {
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
            url: "/MailTemplate/Search?PageSize=" + pagesize + "&PageNo=" + self.currentPageIndex() + "&Filname=" + Filname,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                self.mailtemplates(data);
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
       

    }

    function GetTotalRecord() {

        var Filname = document.getElementById("FilterbyName").value;

        var pagesize = document.getElementById("pageSizeSelector").value;


        $.ajax({
            type: "GET",
            url: "/MailTemplate/Search?PageSize=" + pagesize + "&PageNo=-1&Filname=" + Filname,
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
        self.title("");
        self.searchkey("");
        self.subject("");
         //self.mailbody("");

    }


    if (window.location.href.includes('add'))
    {
        $('#loader').hide();
        self.Insert(true);
        self.index(false);
        self.Success(false);
        self.Error(false);
        self.savebtn(true);
        self.updatebtn(false);
        document.getElementById("add").innerText = "Add Mail Template";
        $('#txttitle').focus();
      
    }
    else
    {
        GetTotalRecord();
        Getmailtemplates();
    }
    




}


