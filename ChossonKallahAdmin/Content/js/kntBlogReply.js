function BlogReplyKO()
{
    var self = this;

    //message
    self.Success = ko.observable(false);
    self.Error = ko.observable(false);
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");
    self.true = ko.observable("");

    //addpage
    self.insert = ko.observable(false);
    self.id = ko.observable(0);
    self.blogid = ko.observable();
    self.userid = ko.observable();
    self.replydate = ko.observable();
    self.isactive = ko.observable(false);
    self.replydescription = ko.observable();
    self.username = ko.observable();
    self.topictitle = ko.observable();

    self.updatebtn = ko.observable(false);
    self.savebtn = ko.observable(true);

    self.topictitles = ko.observableArray();
    self.usernames = ko.observableArray();

    //index
    self.index = ko.observable(true);
    self.userblogreply = ko.observableArray();
    self.totalrecord = ko.observable(0);
    self.search = ko.observable(true);
   


    //paging
    self.currentPage = ko.observable();
    self.pageSize = ko.observable(10);
    self.currentPageIndex = ko.observable(1);
    self.sortType = "ascending";
    self.currentColumn = ko.observable("");
    self.iconType = ko.observable("");



    self.Addnew = function () {
        var id = 0;
        if (window.location.href.indexOf('id') >= 0) {
            var splitdata = window.location.href.split('=');
            id = splitdata[1];
        }
        document.getElementById("add").innerText = "Add Blog Reply";

        self.Error(false);
        self.Success(false);
        self.index(false);
        self.savebtn(true);
        self.updatebtn(false);
        self.insert(true);
        self.cleare();
        self.blogid(id);
        var today = moment().format('MM/DD/YYYY');
        self.replydate(today);
        $('#userid').focus();
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

    self.cleare=function ()
    {
        self.id(0);
        self.blogid(null);
        self.userid(null);
        self.replydate("");
        self.isactive(false);
        self.replydescription("");
    }

    self.Cancel = function () {
        self.insert(false);
        self.index(true);
        self.Error(false);
        self.true(false);
    }

    self.Back = function () {
        window.location.href = '../Blog/Index';
    }

    self.SortData = function () {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        if (document.getElementById("Hforder").value == "asc") {
            document.getElementById("Hforder").value = "desc";

            var sort = "order by userid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        }
        else {
            document.getElementById("Hforder").value = "asc";
            var sort = "order by userid asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        var Filname = document.getElementById("FilterbyName").value;
        var pageno = self.currentPageIndex();
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: "/BlogReply/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&username=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.userblogreply(data); gettotalrecord();//GetEmployees();
                $('#loader').hide();
            },
            error: function (error) {
                alert(error.status + "<!----!>" + error.statusText);
                $('#loader').hide();
            }
        });

    }
    
    self.changestatus = function (userblogreply) {

        var isactive = userblogreply.isactive;
        var id = userblogreply.id;

        $.ajax({
            type: "POST",
            url: "/BlogReply/UpdateActive?id=" + id + "&Active=" + isactive,
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

    function gettotalrecord() {

        var id = 0;
        if (window.location.href.indexOf('id') >= 0) {
            var splitdata = window.location.href.split('=');
            id = splitdata[1];
        }
       var Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: "/BlogReply/search?PageSize=10" + "&PageNo=-1" + "&username=" + Filname + "&id=" + id,
            Contenttype: "application/JSON",
            success: function (data) {
            self.totalrecord(data[0].id);
            }
        });
    }

    function getdata() {
        $('#loader').show();
        var id = 0;
        if (window.location.href.indexOf('id') >= 0) {
            var splitdata = window.location.href.split('=');
            id = splitdata[1];
        }
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
            url: '/BlogReply/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&username=" + Filname+  "&id=" +id,
            Contenttype: "application/JSON",
            success: function (data) {
                self.userblogreply(data);
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

    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 10),
        startIndex = pagesize * self.currentPageIndex(),
        endIndex = startIndex + pagesize;
        return self.userblogreply.slice(startIndex, endIndex);
    });
    
    self.nextPage = function () {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        var id = 0;
        if (window.location.href.indexOf('id') >= 0) {
            var splitdata = window.location.href.split('=');
            id = splitdata[1];
           
        }
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by userid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by userid asc"
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
            url: "/BlogReply/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&username=" + Filname + "&sort" + sort + "&id" + id,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.userblogreply(data);//GetEmployees();
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
        var id = 0;
        if (window.location.href.indexOf('id') >= 0) {
            var splitdata = window.location.href.split('=');
            id = splitdata[1];
        }
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by userid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by userid asc"
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
            url: "/BlogReply/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&username=" + Filname + "&sort" + sort + "&id" + id,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.userblogreply(data);//GetEmployees();
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
        $('#loader').hide();
        var id = 0;
        if (window.location.href.indexOf('id') >= 0) {
            var splitdata = window.location.href.split('=');
            id = splitdata[1];
        }
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by userid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by userid asc"
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
            url: "/BlogReply/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&username=" + Filname + "&sort=" + sort + "&id"+ id,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.userblogreply(data);
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


    function checkvalidation() {
        var IsError = true;
        self.MessageError('');
       
        if ($('#blogid').val() == "") {
            
            self.MessageError(self.MessageError() + "<br/> - Blog Title is required.");
            IsError = false;
        }
        if ($('#userid').val()== "") {
            self.MessageError(self.MessageError() + "<br/> - User Name is required.");
            IsError = false;
        }
        if (self.replydate() == "") {
            self.MessageError(self.MessageError() + "<br/> - Reply Date is required.");
            IsError = false;
        }
        self.Error(true);
        return IsError;
    }


    self.getselectedpages = function (userblogreply) {
        self.id(userblogreply.id);
        document.getElementById("add").innerText = "Edit Blog Reply";
        self.userid(userblogreply.userid);
        self.blogid(userblogreply.blogid);
        self.replydescription(userblogreply.replydescription);
        self.isactive(userblogreply.isactive);
        self.replydate(new Date(parseInt(userblogreply.replydate.replace(/(^.*\()|([+-].*$)/g, ''))).toLocaleDateString());
        self.index(false);
        self.insert(true);
        self.updatebtn(true);
        self.savebtn(false);
        $('#userid').focus();
    }


    self.Update = function (data) {
        if (checkvalidation())
        {
            self.Error(false);
        $.ajax({
            url: '/BlogReply/Update/',
            type: "POST",
            dataType: 'json',
            data: ko.toJSON(data),
            contentType: "application/json",
            success: function (data) {
                self.insert(false);
                getdata();
                self.index(true);
                self.Success(true);
                self.MessageSuccess("Record Successfully Updated")
                
            }

        });
        }
        else {
            Error(true);
            self.Success(false);
        }
    }

    self.deletepopup = function (userblogreply) {
        $("#myModal").modal('show');

        $('#DeleteID').val(userblogreply.id);

    }


    self.deleterec = function () {
        var id = $('#DeleteID').val();
        $.ajax({
            type: "POST",
            url: '/BlogReply/Deleteupdate/' + id,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#myModal").modal('hide');
                gettotalrecord();
                getdata();
                self.Success(true);
                self.MessageSuccess("Record Delete Successfully.");
            }
        });

    }


    self.Save = function (data) {
        if (checkvalidation())
        {
            self.Error(false);
            $.ajax({
                type: "POST",
                url: '/BlogReply/Create',
                dataType: 'json',
                data: ko.toJSON(data),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    gettotalrecord();
                    getdata();
                    self.index(true);
                    self.insert(false);
                    self.MessageSuccess("Record has been Saved Successfully.");
                    self.Success(true);
                    self.Error(false);
                }
            });
        }
        else {
            Error(true);
            self.Success(false);
        }
        
    }

    function getnames() {
       
        $.ajax({
            type: "GET",
            url: '/BlogReply/getnames',
            Contenttype: "application/JSON",
            success: function (data) {
                self.topictitles(data);
              }
        });
    }

    function getusersname() {
        $.ajax({
            type: "GET",
            url: '/BlogReply/getusersname',
            Contenttype: "application/JSON",
            success: function (data) {
                self.usernames(data);
            }
        });
    }

    self.searchKeyUp = function (d, e) {
        if (e.keyCode == 13) {
            self.Search();
        }
    }

    self.Search = function (userblogreply) {
        $('#loader').show();
        self.Error(false);
        self.Success(false);
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by userid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by userid asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }
        var pagesize = document.getElementById("pageSizeSelector").value;
        var Filname = document.getElementById("FilterbyName").value;
        var pageno = self.currentPageIndex();
        gettotalrecord();
        $.ajax({
            type: "GET",
            url: "/BlogReply/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&username=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                if (data.length == 0) {
                    self.Success(false);
                    self.MessageError("No Record Found.");
                    self.Error(true);
                    $('#error').text("");
                    self.userblogreply(data);
                    $('#loader').hide();
                    return;

                }
                self.userblogreply(data);
                $('#loader').hide();
                self.Error(false);
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

    getusersname();
    getnames();

    gettotalrecord();

    getdata();

   
    
}
   