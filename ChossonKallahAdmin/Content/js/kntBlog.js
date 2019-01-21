function BlogKO()
{
    var self = this;
    self.Success = ko.observable(false);
    self.Error = ko.observable(false);
    self.insert = ko.observable(false);
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");
    self.updatebtn = ko.observable(false);


    self.id = ko.observable(0);
    self.blogcatid = ko.observable("");
    self.topictitle = ko.observable("");
    self.active = ko.observable(false);
    self.blogdescription = ko.observable("");
    self.createddate = ko.observable("");
    self.ishome = ko.observable(false);
    self.shortdesc = ko.observable("");
    self.linkurl = ko.observable("");
    self.metatitle = ko.observable("");
    self.metakeyword = ko.observable("");
    self.metadescription = ko.observable("");
     
    self.savebtn = ko.observable(true);
    self.title = ko.observableArray();
    self.blog = ko.observableArray();

    self.index = ko.observable(true);
    self.blog = ko.observableArray();
    self.totalrecord = ko.observable(0);
    self.search = ko.observable(true);



    //paging
    self.currentPage = ko.observable();
    self.pageSize = ko.observable(50);
    self.currentPageIndex = ko.observable(1);
    self.sortType = "ascending";
    self.currentColumn = ko.observable("");
    self.iconType = ko.observable("");

    self.clear = function () {
        self.id(0);
        self.blogcatid(null);
        self.topictitle("");
        self.active(false);
        self.createddate("");
        self.ishome(false);
        self.shortdesc("");
        self.linkurl("");
        self.metatitle("");
        self.metakeyword("");
        self.metadescription("");
        CKEDITOR.instances["txtblgdesc"].setData('');
    }


    self.getselectedpages = function (blog) {
        document.getElementById("add").innerText = "Edit Blog";
        self.id(blog.id);
        self.topictitle(blog.topictitle);
        self.blogcatid(blog.blogcatid);
        self.metatitle(blog.metatitle);
        self.metadescription(blog.metadescription);
        self.metakeyword(blog.metakeyword);
        self.active(blog.active);
        self.linkurl(blog.linkurl);
        self.ishome(blog.ishome);
        self.shortdesc(blog.shortdesc);
        self.index(false);
        self.insert(true);
        self.updatebtn(true);
        self.savebtn(false);
        self.Error(false);
        self.Success(false);
        $('#topictitle').focus();
    }

    
    self.Update = function (data) {
        jQuery.fn.CKEditorValFor = function (element_id) {
            return CKEDITOR.instances[element_id].getData();
        }
        data.blogdescription = $().CKEditorValFor('txtblgdesc');
        
        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax({
            url: '/Blog/Update/',
            type: "POST",
            dataType: 'json',
            data: ko.toJSON(data),
            contentType: "application/json",
            success: function (data) {
                self.Error(false);
                self.MessageSuccess("Record has been Updated Successfully");
                self.Success(true);
                self.insert(false);
                self.index(true);
                getdata();
                self.clear();
               
            }

        });
        }
        else {
            self.Error(true);
            self.MessageError("Check validation Missing");

        }
    }

    self.deletepopup = function (blog) {
        $("#myModal").modal('show');

        $('#DeleteID').val(blog.id);

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

    self.deleterec = function () {
        id = $('#DeleteID').val(),
        $.ajax({
            type: "POST",
            url: '/Blog/Deleteupdate/' + id,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#myModal").modal('hide');
                gettotalrecord();
                getdata();
                self.Error(false);
                self.Success(true);
                self.MessageSuccess("Record has been Deleted Successfully");
            }
        });

    }

    self.Save = function (data) {
        
        jQuery.fn.CKEditorValFor = function (element_id) {
            return CKEDITOR.instances[element_id].getData();
        }
        data.blogdescription =$().CKEditorValFor('txtblgdesc');
      
        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax({
                type: "POST",
                url: "/Blog/Create",
                data: ko.toJSON(data),
                contentType: "application/json",
                async: true,
                success: function (data) {
                    self.Error(false);
                    gettotalrecord();
                    getdata();
                    self.index(true);
                    self.insert(false);
                    self.MessageSuccess("Record has been Saved Successfully..");
                    self.Success(true);

                }
            });
        }
        else {
            self.Error(true);
         
        }
    }

    function getnames() {
        $.ajax({
            type: "GET",
            url: '/Blog/getnames',
            Contenttype: "application/JSON",
            success: function (data) {
                self.title(data);
            }
        });
    }

    self.checkvalidation = function () {
        var IsError = true;
        self.MessageError('');
        if ($('#ddlblogcat').val() == "") {
            self.MessageError(self.MessageError() + "<br/> - Blog Category Title is required.");
            IsError = false;
        }
        if (self.topictitle() == "") {
            self.MessageError(self.MessageError() + "<br/> - Topic Title is required.");
            IsError = false;
        } 
        self.Error(true); 
        return IsError;
    }

    self.Addnew = function () {
        document.getElementById("add").innerText = "Add Blog";
        self.index(false);
        self.savebtn(true);
        self.updatebtn(false);
        self.insert(true);
        self.Error(false);
        self.Success(false);
        self.clear();
        $('#topictitle').focus();
    }


    self.Cancel = function () {
       
        self.insert(false);
        self.index(true);
        self.Error(false);
        self.Success(false);
    }


    self.searchKeyUp = function (d, e) {
        if (e.keyCode == 13) {
            self.Search();
        }
    }


    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 50),
        startIndex = pagesize * self.currentPageIndex(),
        endIndex = startIndex + pagesize;
        return self.blog.slice(startIndex, endIndex);
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
            url: '/Blog/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&topictitle=" + Filname,
            Contenttype: "application/JSON",
            success: function (data) {
                self.blog(data);
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

            var sort = "order by topictitle desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        }
        else {
            document.getElementById("Hforder").value = "asc";
            var sort = "order by topictitle asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        var Filname = document.getElementById("FilterbyName").value;
        var pageno = self.currentPageIndex();
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: "/Blog/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&topictitle=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.blog(data);//GetEmployees();
                $('#loader').hide(); gettotalrecord();
            },
            error: function (error) {
                alert(error.status + "<!----!>" + error.statusText);
                $('#loader').hide();
            }
        });

    }


    self.Search = function (blog) {
        $('#loader').show();
        self.Error(false);
        self.Success(false);
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by topictitle desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by topictitle asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }
        var pagesize = document.getElementById("pageSizeSelector").value;
        var Filname = document.getElementById("FilterbyName").value;
        var pageno = self.currentPageIndex();
        gettotalrecord();
        $.ajax({
            type: "GET",
            url: "/Blog/Search?PageSize=" + pagesize + "&PageNo=" + pageno + "&topictitle=" + Filname + "&sort" + sort,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                if (data.length == 0) {
                    self.Success(false);
                    self.MessageError("No Record Found.");
                    self.Error(true);
                    $('#error').text("");
                    self.blog(data);
                    $('#loader').hide();
                    return;

                }
                self.blog(data);
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

        

    };


    function gettotalrecord() {

        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: '/Blog/search?PageSize=' + pagesize + "&PageNo=-1" + "&topictitle=" + Filname,
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


            var sort = "order by topictitle desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by topictitle asc"
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
            url: "/Blog/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&topictitle=" + Filname + "&sort=" + sort ,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.blog(data);//GetEmployees();
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


            var sort = "order by topictitle desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by topictitle asc"
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
            url: "/Blog/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&topictitle=" + Filname,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.blog(data);//GetEmployees();
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


            var sort = "order by topictitle desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by topictitle asc"
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
            url: "/Blog/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&topictitle=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.blog(data);
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


    self.changestatus = function (blog) {

        var active = blog.active;
        var id = blog.id;

        $.ajax({
            type: "POST",
            url: "/Blog/UpdateActive?id=" + id + "&Active=" + active,
            contentType: "application/json",
            success: function (data) {
                getdata();
                self.MessageSuccess("Record has been Updated Successfully.");
                self.Success(true);

            },
            error: function (error) {
            }
        });
    };


    gettotalrecord();
    getnames();
    getdata();
}