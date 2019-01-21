function BlogCatKO()
{
    self = this;
    self.id = ko.observable(0);
    self.title = ko.observable("");
    self.parent = ko.observable("");
    self.rank = ko.observable("");
    self.active = ko.observable(false);
    self.linkurl = ko.observable("");
    self.metatitle = ko.observable("");
    self.metakeyword = ko.observable("");
        
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");
    self.Success = ko.observable(false);
    self.Error = ko.observable(false);
    
    self.index = ko.observable(true);
    self.insert = ko.observable(false);
    
    self.updatebtn = ko.observable(false);
    self.savebtn = ko.observable(false);
    
    //index
    self.blogcat = ko.observableArray();
    self.parents = ko.observableArray();
    self.totalrecord = ko.observable(0);
    self.search = ko.observable(true);

    //paging
    self.currentPage = ko.observable();
    self.pageSize = ko.observable(50);
    self.currentPageIndex = ko.observable(1);
    self.sortType = "ascending";
    self.currentColumn = ko.observable("");
    self.iconType = ko.observable("");

    self.searchKeyUp = function (d, e) {
        if (e.keyCode == 13) {
            self.Search();
        }
    }

    self.Addnew = function ()
    {
        document.getElementById("add").innerText = "Add Blog Category";
        self.index(false);
        self.savebtn(true);
        self.cleare();
        self.updatebtn(false);
        self.insert(true);
        self.Success(false);
        self.Error(false);
        getmaxrank();
        $('#title').focus();
    }

    function getmaxrank() {
        $.ajax({
            type: "GET",
            url: '/BlogCat/getrank',
            Contenttype: "application/JSON",
            success: function (data) {
                self.rank(data[0].rank + 1);
            }
        });
    }

    self.cleare = function ()
    {
        self.id(0);
        self.title("");
        self.parent(null);
        self.rank(1);
        self.active(false);
        self.linkurl('');
        self.metatitle("");
        self.metakeyword("");
        $('#txtlink').val('');
    }

    self.Cancel = function ()
    {
        self.insert(false);
        self.index(true);
        self.Error(false);
        self.Success(false);
        self.linkurl('');
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
            url: "/BlogCat/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&title=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.blogcat(data); gettotalrecord();//GetEmployees();
                $('#loader').hide();
            },
            error: function (error) {
                alert(error.status + "<!----!>" + error.statusText);
                $('#loader').hide();
            }
        });

    }

    self.Search = function (blogcat) {
        $('#loader').show();
        self.Error(false);
        self.Success(false);
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by rank desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by rank asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }
        var pagesize = document.getElementById("pageSizeSelector").value;
        var Filname = document.getElementById("FilterbyName").value;
        gettotalrecord();
        $.ajax({
            type: "GET",
            url: "/BlogCat/Search?PageSize=" + pagesize + "&PageNo=" + self.currentPageIndex() + "&title=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                if (data.length == 0) {
                    self.Success(false);
                    self.MessageError("No Record Found.");
                    self.Error(true);
                    $('#error').text("");
                    self.blogcat(data);
                    $('#loader').hide();
                    return;

                }
                self.Error(false);
                self.Success(false);
                self.blogcat(data);
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
            url: '/BlogCat/search?PageSize=' + pagesize + "&PageNo=-1" + "&title=" + Filname,
            Contenttype: "application/JSON",
            success: function (data) {
                self.totalrecord(data[0].id);
            }
        });
    }
    
    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 50),
        startIndex = pagesize * self.currentPageIndex(),
        endIndex = startIndex + pagesize;
        return self.blogcat.slice(startIndex, endIndex);
    });
    
    self.nextPage = function () {
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
            url: "/BlogCat/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&title=" + Filname + "&sort="+ sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.blogcat(data);//GetEmployees();
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
            url: "/BlogCat/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&title=" + Filname,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.blogcat(data);//GetEmployees();
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
            url: "/BlogCat/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&title=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.blogcat(data);
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
    function checkvelidation() {
        
        var IsError = true;
        self.MessageError('');

        if (self.title() == "") {
            self.MessageError("<br/> - Title is required.");
            IsError = false;
        }
        if ($('#parent').val() == 0) {
            self.MessageError("<br/> - Parent is required ");
            IsError = false;
        } 
        self.Error(true);
        return IsError;
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
                url: '/BlogCat/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&title=" + Filname,
                Contenttype: "application/JSON",
                success: function (data) {
                    self.blogcat(data);
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
                }
            });
        }

    self.changestatus = function (blogcat) {

            var active = blogcat.active;
            var id = blogcat.id;

            $.ajax({
                type: "POST",
                url: "/BlogCat/UpdateActive?id=" + id + "&Active=" + active,
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
    
    self.getselectedpages = function (blogcat)
    {
        
        self.id(blogcat.id);
        getparent(self.id);
            self.title(blogcat.title);
            self.parent(blogcat.parent);
            self.rank(blogcat.rank);
            self.active(blogcat.active);
            self.linkurl(blogcat.linkurl);
            self.metakeyword(blogcat.metakeyword);
            self.metatitle(blogcat.metatitle);
            self.index(false);
            self.insert(true);
            document.getElementById("add").innerText = "Edit Blog Category";
            self.updatebtn(true);
            self.savebtn(false);
            $('#title').focus();
        }
    
    self.Update = function (data) {
        if (checkvelidation()) {
            self.Error(false);
            $.ajax({
                url: '/BlogCat/Update/',
                type: "POST",
                dataType: 'json',
                data: ko.toJSON(data),
                contentType: "application/json",
                success: function (data) {
                    self.insert(false);
                    getdata();
                    self.index(true);
                    self.Error(false);
                    self.Success(true);
                    self.MessageSuccess("Record Successfully Updated.")
                    
                }

            });
        }
        else {
            self.error(true);
        }
    }

    self.Dataupside = function (blogcat) {
        $.ajax({
            type: "POST",
            url: "/BlogCat/updata?id=" + blogcat.id + "&up=" + "up",
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

    self.Datadownside = function (blogcat) {
        $.ajax({
            type: "POST",
            url: "/BlogCat/downdata?id=" + blogcat.id + "&down=" + "down",
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

    self.deleterec = function () {
        id = $('#DeleteID').val(),
            $.ajax({
                type: "POST",
                url: '/BlogCat/Deleteupdate/' + id,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#myModal").modal('hide');
                    gettotalrecord();
                    getdata();
                    self.Success(true);
                    self.MessageSuccess("Record has been Deleted Successfully.");
                   
                }
            });

    }

    self.Save = function (data) {
        if (checkvelidation()) {
            self.Error(false);
            $.ajax({
                type: "POST",
                url: '/BlogCat/Create',
                dataType: 'json',
                data: ko.toJSON(data),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    gettotalrecord();
                    getdata();
                    self.index(true);
                    self.insert(false);
                    self.Error(false);
                    self.MessageSuccess("Record has been Saved Successfully.");
                    self.Success(true);
                    
                }
            });

        }
        else {
            self.Error(true);
        }
    }

    self.deletepopup = function (blogcat) {
        $("#myModal").modal('show');

        $('#DeleteID').val(blogcat.id);

    }

    function getparent(id) {
        $.ajax({
            type: "GET",
            url: '/BlogCat/getparent?id=' + id,
            Contenttype: "application/JSON",
            success: function (data) {
                self.parents(data);
            }
        });
    }
    
    gettotalrecord();
    getdata();
    getparent(0);
}