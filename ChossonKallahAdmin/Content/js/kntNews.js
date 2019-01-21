function NewsKO()
{
    var self = this;

    self.id = ko.observable(0);
    self.title = ko.observable("");
    self.shortdescription = ko.observable("");
    self.largedescription = ko.observable("");
    self.newdate = ko.observable("");
    self.isactive = ko.observable(false);
    self.isfeatured = ko.observable(false);
    self.createdate = ko.observable("");

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

    ko.bindingHandlers.date = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var valueUnwrapped = ko.utils.unwrapObservable(valueAccessor());
            var textContent = moment(valueUnwrapped).format("MM/DD/YYYY");
            ko.bindingHandlers.text.update(element, function () { return textContent; });
        }
    };

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

    self.clear = function () {
        self.id("");
        self.title("");
        self.shortdescription("");
        CKEDITOR.instances['txtlrgdesc'].setData('');
        self.newdate("");
        self.isactive("");
        self.isfeatured("");
    }

    self.Addnew = function () {
        document.getElementById("add").innerText = "Add News";
        self.clear();
        self.Success(false);
        self.Error(false);
        self.index(false);
        self.insert(true);
        self.savebtn(true);
        self.updatebtn(false);
        var today = moment().format('MM/DD/YYYY');
        self.newdate(today);
        $('#txttitle').focus();
    }

    self.Cancel = function () {
        self.Success(false);
        self.Error(false);
        self.index(true);
        self.insert(false);
        self.clear();
        CKEDITOR.instances('txtlrgdesc').setData('');
    }

    self.Save = function (data) {
        self.Error(false);
      
      
      
     

        if (self.checkvalidation()) {
            data.newdate(new Date($('#txtnewsdat').val()));
            jQuery.fn.CKEditorValFor = function (element_id) {
                return CKEDITOR.instances[element_id].getData();
            }
            data.largedescription = $().CKEditorValFor('txtlrgdesc');
           
            $.ajax(
                {
                    type: "POST",
                    url: "/News/Insert",
                    data: ko.toJSON(data),
                    contentType: "application/json",
                    async: true,
                    success: function (data) {
                        self.Error(false);
                        self.MessageSuccess("Record Save Successfully.");
                        self.Success(true);
                        self.insert(false);
                        self.index(true);
                        self.getdata();
                        gettotalrecord();
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

    self.checkvalidation = function () {
        var IsError = true;
        self.MessageError('');
        if (self.title() == "") {
            self.MessageError(self.MessageError() + "<br/> - Title is required.");
            IsError = false;
        }
        if (self.newdate() == "Invalid Date")
        {
            self.newdate("");
        }
       
        self.Error(true);
        return IsError;
    }

   
    //pagging

    self.getselectedpages = function (newses) {
        document.getElementById("add").innerText = "Edit News";
        self.Success(false);
        self.Error(false);
        self.id(newses.id);
        self.title(newses.title);
        self.shortdescription(newses.shortdescription);
        //self.largedescription(newses.largedescription);
        CKEDITOR.instances.txtlrgdesc.setData(newses.largedescription);
        self.newdate(new Date(parseInt(newses.newdate.replace(/(^.*\()|([+-].*$)/g, ''))).toLocaleDateString ());
        self.isactive(newses.isactive);
        self.isfeatured(newses.isfeatured);
        self.index(false);
        self.insert(true);
        self.updatebtn(true);
        self.savebtn(false);
        $('#txttitle').focus();
    }

    


  

    self.getdata = function () {
        $('#loader').show();
        gettotalrecord();
       var rec = self.totalrecord();
       var Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        var sort = "order by title " + document.getElementById("Hforder").value;

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
            url: '/News/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&title=" + Filname + "&sort=" + sort,
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
        self.getdata();
       

    }

    self.Search = function (newses) {
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
        gettotalrecord();
        $.ajax({
            type: "GET",
            url: "/News/Search?PageSize=" + pagesize + "&PageNo=" + pageno + "&title=" + Filname + "&sort=" + sort,
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

        $("#loader").hide();

    };

    function gettotalrecord() {
      
        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
      

        $.ajax({
            type: "GET",
            url: '/News/TotalRecord?PageSize=' + pagesize + "&PageNo=-1" + "&title=" + Filname,
            Contenttype: "application/JSON",
            success: function (data) {
             
                self.totalrecord(data);
            }
        });
    }

    self.nextPage = function () {
        self.Error(false);
        self.Success(false);

        $('#loader').show();
        self.Error(false);
        self.currentPageIndex(self.currentPageIndex() + 1);
        if (self.currentPageIndex() > 1) {

            var pageno = self.currentPageIndex();
        }
        else {
            self.currentPageIndex() = 1;
        }
        self.getdata();
    };

    self.previousPage = function () {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        //if (document.getElementById("Hforder").value == "desc") {


        //    var sort = "order by title desc"
        //    $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        //}
        //else {

        //    var sort = "order by title asc"
        //    $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        //}


        if (self.currentPageIndex() > 1) {

            self.currentPageIndex(self.currentPageIndex() - 1);
            var pageno = self.currentPageIndex();

        }
        else {
            self.currentPageIndex() = 1;
        }
        self.getdata();
     
    };

    self.selectionChanged = function (event) {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        //if (document.getElementById("Hforder").value == "desc") {


        //    var sort = "order by title desc"
        //    $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        //}
        //else {

        //    var sort = "order by title asc"
        //    $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        //}
        self.Error(false);
        //var pageno = self.currentPageIndex();
        //var totalrec = document.getElementById("Hftotalrecord").value;
        var pagesize = document.getElementById("pageSizeSelector").value;

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


    self.changestatus = function (newses) {

        var isactive = newses.isactive;
        var id = newses.id;

        $.ajax({
            type: "POST",
            url: "/News/UpdateActive?id=" + id + "&Active=" + isactive,
            contentType: "application/json",
            success: function (data) {

                self.MessageSuccess("Record has been Updated Successfully.");
                self.Success(true);
                self.getdata();
              
            },
            error: function (error) {
            }
        });
    };

    self.Update = function (data) {
        self.Error(false);
        if (self.checkvalidation()) {
          
            data.newdate(new Date($('#txtnewsdat').val()));
            jQuery.fn.CKEditorValFor = function (element_id) {
                return CKEDITOR.instances[element_id].getData();
            }
            data.largedescription = $().CKEditorValFor('txtlrgdesc');
            $.ajax(
                {
                    type: "POST",
                    url: "/News/Update",
                    data: ko.toJSON(data),
                    contentType: "application/json",
                    async: true,
                    success: function (data) {
                        self.index(true);
                        self.Error(false);
                        self.MessageSuccess("Record has been Updated Successfully.");
                        self.Success(true);
                        self.insert(false);
                        self.getdata();
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

    self.deletepopup = function (newes) {
        $("#myModal").modal('show');

        $('#DeleteID').val(newes.id);

    }

    self.deleterec = function () {
        var id = $('#DeleteID').val();

        $.ajax(
            {
                type: "POST",
                url: "/News/Delete/" + id,
                success: function (data) {
                    $("#myModal").modal('hide');
                    self.Error(false);
                    self.MessageSuccess("Record has been Deleted Successfully.");
                    self.Success(true);
                    self.Error(false);
                    gettotalrecord();
                    self.getdata();
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
    self.getdata();
   

}