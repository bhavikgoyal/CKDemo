function attributesKO() {
    var self = this;
    self.id = ko.observable(0);
    self.attributesname = ko.observable("");
    self.attributevalue = ko.observable("");
    self.defaultvalue = ko.observable("");
    self.isactive = ko.observable("");
    self.ismultivalued = ko.observable(false);
    self.isrequired = ko.observable(false);

    self.attributes = ko.observableArray();
    self.attributevalues = ko.observableArray();

    self.attributeid = ko.observable(0);
    self.Success = ko.observable(false);
    self.Error = ko.observable(false);
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");
    self.savebtn = ko.observable(true);
    self.updatebtn = ko.observable(false);

    self.insert = ko.observable(false);
    self.index = ko.observable(true);

    self.currentPage = ko.observable();
    self.pageSize = ko.observable(50);
    self.currentPageIndex = ko.observable(1);
    self.totalrecord = ko.observable(0);
    self.search = ko.observable(true);

    self.Addnew = function () {
        document.getElementById("add").innerText = "Add Attribute";
        self.clear();
        self.Success(false);
        self.Error(false);
        self.index(false);
        self.insert(true);
        self.savebtn(true);
        self.updatebtn(false);
        $('#attname').focus();
    }

    self.clear = function () {
        self.id("");
        self.attributevalue("");
        self.attributesname("");
        self.defaultvalue("");
        self.ismultivalued("");
        self.isrequired("");
        self.isactive("");

        for (var i = 0; i < $('#tags_1_tagsinput span').size() ; i++) {

            $('#tags_1_tagsinput span').remove('span');

        }
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

    self.Cancel = function () {
        self.Success(false);
        self.Error(false);
        self.index(true);
        self.insert(false);
        self.clear();
    }

    self.Save = function (data) {
        //var att = self.attributevalue;
        var idid = self.id;
        var attributeval = ''
        for (var i = 0; i < $('#tags_1_tagsinput span') ; i++) {
              if (i % 2) { 
                attributeval = attributeval + ',' + $('#tags_1_tagsinput span').eq(i).text();
            }
        }
        self.attributevalue(attributeval);
        if (self.checkvalidation()) {
            
            self.Error(false);
            $.ajax(
               {
                   type: "POST",
                   url: "/Attributes/Create",
                   data: ko.toJSON(data),
                   contentType: "application/json",
                   async: true,
                   success: function (data) {

                       self.Error(false);
                       self.MessageSuccess("Details Saved Successfully.");
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
        var IsError = true;
        self.MessageError('');
        if (self.attributesname() == "") {
            self.MessageError(self.MessageError() + "<br/> - Attribute Name  is required.");
            IsError = false;
        }
        if(self.defaultvalue() == "")
        {
            self.MessageError(self.MessageError() + "<br/> -Default Name is required. ")
            IsError = false;
        }
        self.Error(true);
        return IsError;
    }

    self.getselectedpages = function (attributes) {
        document.getElementById("add").innerText = "Edit Attribute";
        self.Success(false);
        self.Error(false);
        self.id(attributes.id);
        self.attributesname(attributes.attributesname);
        self.defaultvalue(attributes.defaultvalue);
        self.ismultivalued(attributes.ismultivalued);
        self.isrequired(attributes.isrequired);
        self.isactive(attributes.isactive);
        getAttributeValues();
        var attributevalue = '';

        for (var i = 0; i < self.attributevalues().length; i++) {
            if (attributes.id == self.attributevalues()[i].attributesid) {

                attributevalue = attributevalue + ',' + self.attributevalues()[i].attributevalue;
                attributevalue = attributevalue.replace(/^,|,$/g, '');
            }

        }
        var data = attributevalue.split(',');
        
        for (var j = 0; j < data.length ; j++) {

            $('#tags_1_tagsinput').prepend('<span class="tag1"><span>' + data[j] + '</span><a title="Removing tag" href="#" onclick="onRemoveTag(' + "'" + data[j] + "'" + ');">x</a></span>');

            $('#attname').focus();
        }
        $('#tags_1_tagsinput').find('.tag1').hide();
        self.index(false);
        self.insert(true);
        self.updatebtn(true);
        self.savebtn(false);
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
        return self.attributes.slice(startIndex, endIndex);
    });

    function getdata() {
        $('#loader').show();
        var sort = "order by attributesname asc"
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
            url: '/Attributes/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&attributesname=" + Filname+ "&sort" + sort,
            Contenttype: "application/JSON",
            success: function (data) {
                self.attributes(data);
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

            var sort = "order by attributesname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        }
        else {
            document.getElementById("Hforder").value = "asc";
            var sort = "order by attributesname asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        var Filname = document.getElementById("FilterbyName").value;
        var pageno = self.currentPageIndex();
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: "/Attributes/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&attributesname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.attributes(data);//GetEmployees();
                gettotalrecord();
                $('#loader').hide();
            },
            error: function (error) {
                alert(error.status + "<!----!>" + error.statusText);
                $('#loader').hide();
            }
        });

    }

    self.Search = function (attributes) {
        $('#loader').show();
        self.Error(false);
        self.Success(false);
        if (document.getElementById("Hforder").value == "desc") {
            var sort = "order by attributesname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')
        }
        else {
            var sort = "order by attributesname asc"
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
            url: "/Attributes/Search?PageSize=" + pagesize + "&PageNo=" + pageno + "&attributesname=" + Filname+ "&sort"+ sort,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                if (data.length == 0) {
                    self.Success(false);
                    self.MessageError("No Record Found.");
                    self.Error(true);
                    $('#error').text("");
                    self.attributes(data);
                    $('#loader').hide();
                    return;

                }
                self.Error(false);
                self.Success(false);
                self.attributes(data);
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
        var sort = "order by attributesname asc"
        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: '/Attributes/search?PageSize=' + pagesize + "&PageNo=-1" + "&attributesname=" + Filname+ "&sort" + sort,
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


            var sort = "order by attributesname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by attributesname asc"
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
            url: "/Attributes/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&attributesname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.attributes(data);//GetEmployees();
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


            var sort = "order by attributesname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by attributesname asc"
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
            url: "/Attributes/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&attributesname=" + Filname,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.attributes(data);//GetEmployees();
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


            var sort = "order by attributesname desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by attributesname asc"
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
            url: "/Attributes/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&attributesname=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.attributes(data);
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

    self.changestatus = function (attributes) {
        var isactive = attributes.isactive;
        var id = attributes.id;

        $.ajax({
            type: "POST",
            url: "/Attributes/UpdateActive?id=" + id + "&Active=" + isactive,
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

    self.Update = function (data) {
        // var idid = self.id;
       var attributeval = ''
        for (var i = 0; i < $('#tags_1_tagsinput span').size() ; i++) {
            if (i % 2) {
                attributeval = attributeval + ',' + $('#tags_1_tagsinput span').eq(i).text();
            }
                   }
        self.attributevalue(attributeval);
        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax(
                {
                    type: "POST",
                    url: "/Attributes/Update",
                    data: ko.toJSON(data),
                    contentType: "application/json",
                    async: true,
                    success: function (data) {
                        self.index(true);
                        self.Error(false);
                        self.MessageSuccess("Detail Update Successfully.");
                        self.Success(true);
                        self.insert(false);
                        getdata();
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

    self.deletepopup = function (attributes) {
        $("#myModal").modal('show');

        $('#DeleteID').val(attributes.id);

    }


    self.deleterec = function () {
        var id = $('#DeleteID').val();

        $.ajax(
            {
                type: "POST",
                url: "/Attributes/Delete/" + id,
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


    function getAttributeValues() {

        $.ajax({
            type: "GET",
            url: '/Attributes/AttributeValues',
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {
                self.attributevalues(data);

            }
        });
    }

    gettotalrecord();
    getdata();
    

}