function StockKO()
{
    var self = this;

    self.id = ko.observable(0);
    self.productid = ko.observable("");
    self.stockquantity = ko.observable("");
    self.stocktype = ko.observable("");
    self.stockdatetime = ko.observableArray();
    self.stockdescription = ko.observable("");
    self.branchid = ko.observable("");

    self.productname = ko.observable("");
    self.branchname = ko.observable("");

    
    self.stockes = ko.observableArray();

    self.Success = ko.observable(false);
    self.Error = ko.observable(false);
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");

    self.savebtn = ko.observable(true);
    self.updatebtn = ko.observable(false);

    self.insert = ko.observable(false);
    self.index = ko.observable(true);

    self.Branches = ko.observableArray();
    self.Productes = ko.observableArray();

    self.currentPage = ko.observable();
    self.pageSize = ko.observable(10);
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
        self.productid(null);
        self.stockquantity(1);
        self.stocktype("");
        self.stockdatetime("");
        self.stockdescription("");
        self.branchid(null);
      }

    self.Cancel = function () {
        self.Success(false);
        self.Error(false);
        self.index(true);
        self.insert(false);
        self.clear();
    }

    self.Save = function (data) {
        data.stockdatetime(new Date($('#txtstckdt').val()));
       

        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax(
                {
                    type: "POST",
                    url: "/Stock/Create",
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
        var IsError = true;
        self.MessageError('');
       
        if ($('#ddlproduct').val()=="") {
            self.MessageError(self.MessageError() + "<br/> - Product Name is required.");
            IsError = false;
        }
       
        //if ($('#ddlbranch').val() == "") {
          
        //    self.MessageError(self.MessageError() + "<br/> - Branch Name is required.");
        //    IsError = false;
        //}
      
       
        //if (self.stocktype() == "-Please Select-") {
        //    self.MessageError(self.MessageError() + "<br/> - Stock Type  is required.");
        //    IsError = false;
        //}
   
        if (self.stockdatetime() == 'Invalid Date')
        {

            self.stockdatetime(Date);

        }


        self.Error(true);
        return IsError;
        
    }

    function Getbranchname() {
        
        $.ajax({
            type: "GET",
            url: '/Stock/getbranch',
            Contenttype: "application/JSON",
            success: function (data) {
                self.Branches(data);
               
            }
        });
    }

    function Getproductname() {
      
          $.ajax({
            type: "GET",
            url: '/Stock/getproduct',
            Contenttype: "application/JSON",
            success: function (data) {
                self.Productes(data);
           
            }
        });
    }

    self.Addnew = function () {
        var id = 0;
        
        if (window.location.href.indexOf('id') >= 0) {
            var splitdata = window.location.href.split('=');
            id = splitdata[1];
        }
        document.getElementById("add").innerText = "Add Stock";
        self.index(false);
        self.savebtn(true);
        self.updatebtn(false);
        self.insert(true);
        self.Error(false);
        self.Success(false);
        self.clear();
        self.productid(id);
        var today = moment().format('MM/DD/YYYY');
        self.stockdatetime(today);
        $('#ddlbranch').focus(); 
    }

    self.getselectedpages = function (stockes) {
       
        self.Success(false);
        self.Error(false);
        self.id(stockes.id);
        self.productid(stockes.productid);
        self.stockquantity(stockes.stockquantity);
        self.stocktype(stockes.stocktype);
        self.stockdatetime(new Date(parseInt(stockes.stockdatetime.replace(/(^.*\()|([+-].*$)/g, ''))).toLocaleDateString());
        self.stockdescription(stockes.stockdescription);
        self.branchid(stockes.branchid);
        document.getElementById("add").innerText = "Edit Stock";
        self.index(false);
        self.insert(true);
        self.updatebtn(true);
        self.savebtn(false);
        $('#ddlbranch').focus();
    }

    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 10),
        startIndex = pagesize * self.currentPageIndex(),
        endIndex = startIndex + pagesize;
        return self.stockes.slice(startIndex, endIndex);
    });

    function getdata() {
        $('#loader').show();
       
        var id =0;
        if (window.location.href.indexOf('id') >= 0)
        {
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
            url: '/Stock/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&productname=" + Filname + "&id=" + id,
            Contenttype: "application/JSON",
            success: function (data) {
                self.stockes(data);
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

    self.Stktransfer= function()
    {
        window.location = "/Stocktransfer/index";
    }

    self.SortData = function () {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        var id = 0;
        if (window.location.href.indexOf('id') >= 0) {
            var splitdata = window.location.href.split('=');
            id = splitdata[1];
        }
        var sort = "order by productid asc";
        if (document.getElementById("Hforder").value == "asc") {
            document.getElementById("Hforder").value = "desc";

            sort = "order by productid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        }
        else {
            document.getElementById("Hforder").value = "asc";
            sort = "order by productid asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        var Filname = document.getElementById("FilterbyName").value;
        var pageno = self.currentPageIndex();
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: "/Stock/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&productname=" + Filname + "&sort=" + sort + "&id=" + id,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.stockes(data); gettotalrecord();
                $('#loader').hide();
            },
            error: function (error) {
                alert(error.status + "<!----!>" + error.statusText);
                $('#loader').hide();
            }
        });

    }

    self.Search = function (stockes) {
        $('#loader').hide();

        var id = 0;
        if (window.location.href.indexOf('id') >= 0) {
            var splitdata = window.location.href.split('=');
            id = splitdata[1];
        }
        var sort = "order by productid asc";
        if (document.getElementById("Hforder").value == "asc") {
            document.getElementById("Hforder").value = "desc";

            sort = "order by productid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        }
        else {
            document.getElementById("Hforder").value = "asc";
            sort = "order by productid asc"
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
            url: "/Stock/Search?PageSize=" + pagesize + "&PageNo=" + pageno + "&productname=" + Filname + "&sort=" + sort + "&id=" + id,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                if (data.length == 0) {
                    self.Success(false);
                    self.MessageError("No Record Found."); $('#error').text("");
                    self.Error(true);
                    self.stockes(data);
                    $('#loader').hide();
                    return;

                }
                self.stockes(data);
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

        $("#loader").hide();

    };

    function gettotalrecord() {
        var id = 0;
        if (window.location.href.indexOf('id') >= 0) {
            var splitdata = window.location.href.split('=');
            id = splitdata[1];
        }
        Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: '/Stock/search?PageSize=' + pagesize + "&PageNo=-1" + "&productname=" + Filname+"&id=" + id,
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
        var id = 0;
        if (window.location.href.indexOf('id') >= 0) {
            var splitdata = window.location.href.split('=');
            id = splitdata[1];
        }
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by productid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by productid asc"
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
            url: "/Stock/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&productname=" + Filname + "&sort=" + sort + "&id=" + id,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.stockes(data);//GetEmployees();
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


            var sort = "order by productid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by productid asc"
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
            url: "/Stock/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&productname=" + Filname + "&sort=" + sort + "&id=" + id,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.stockes(data);//GetEmployees();
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
        var id = 0;
        if (window.location.href.indexOf('id') >= 0) {
            var splitdata = window.location.href.split('=');
            id = splitdata[1];
        }
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by productid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by productid asc"
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
            url: "/Stock/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&productname=" + Filname + "&sort=" + sort + "&id=" + id,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.stockes(data);
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

    self.Back =function()
    {
        window.location.href="/Product/index";
    }
 
    self.Update = function (data) {
        data.stockdatetime(new Date($('#txtstckdt').val()));
       
        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax(
                {
                    type: "POST",
                    url: "/Stock/Update",
                    data: ko.toJSON(data),
                    contentType: "application/json",
                    async: true,
                    success: function (data) {
                        self.index(true);
                        self.Error(false);
                        self.MessageSuccess("Record has been Updated Successfully.");
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

    self.deletepopup = function (stockes) {
        $("#myModal").modal('show');

        $('#DeleteID').val(stockes.id);

    }

    self.deleterec = function () {

        id = $('#DeleteID').val();
        $.ajax(
            {
                type: "POST",
                url: "/Stock/Delete/" + id,
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

    Getbranchname();
    Getproductname();
    gettotalrecord();
    getdata();


}