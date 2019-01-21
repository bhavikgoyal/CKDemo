function ProductReportKO()
{
    var self = this;
    self.productprice = ko.observableArray();
    self.adminnames = ko.observableArray();
    self.productnames = ko.observableArray();
    self.productid = ko.observable(0);
    self.adminid = ko.observable(0);

    self.currentPage = ko.observable();
    self.pageSize = ko.observable(50);
    self.currentPageIndex = ko.observable(1);
    self.totalrecord = ko.observable(0);

    ko.bindingHandlers.date = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var valueUnwrapped = ko.utils.unwrapObservable(valueAccessor());
            var textContent = moment(valueUnwrapped).format("MM/DD/YYYY");
            ko.bindingHandlers.text.update(element, function () { return textContent; });
        }
    };

    function Getadminname() {
        $.ajax({
            type: "GET",
            url: '/ProductReport/getadminname',
            Contenttype: "application/JSON",
            success: function (data) {
                self.adminnames(data);
            }
        });
    }

    function Getproductname() {
        $.ajax({
            type: "GET",
            url: '/ProductReport/getproductname',
            Contenttype: "application/JSON",
            success: function (data) {
                self.productnames(data);
            }
        });
    }
    
    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 10),
        startIndex = pagesize * self.currentPageIndex(),
        endIndex = startIndex + pagesize;
        return self.productprice.slice(startIndex, endIndex);
    });

    function gettotalrecord() {
        
        var aid = 0;
        var pid = 0;
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: '/ProductReport/search?PageSize=' + pagesize + "&PageNo=-1" + "&adminid=" + aid + "&productid=" + pid,
            Contenttype: "application/JSON",
            success: function (data) {
            self.totalrecord(data[0].id);
            }
        });
    }

    function getdata() {
        $('#loader').show();
        var rec = self.totalrecord()
        
        aid = 0;
        pid = 0;
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
            url: '/ProductReport/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&adminid=" + aid + "&productid=" + pid,
            Contenttype: "application/JSON",
            success: function (data) {
                self.productprice(data);
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

            var sort = "order by id desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        }
        else {
            document.getElementById("Hforder").value = "asc";
            var sort = "order by id asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        
        var pageno = self.currentPageIndex();
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: "/ProductReport/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.productprice(data);//GetEmployees();
                $('#loader').hide();

            },
            error: function (error) {
                alert(error.status + "<!----!>" + error.statusText);
            }
        });

    }

    self.nextPage = function () {
        self.Error(false);
        self.Success(false);
        $('#loader').show();

        if (document.getElementById("Hforder").value == "asc") {


            var sort = "order by id asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png')

        }
        else {

            var sort = "order by id desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');
        }

        self.currentPageIndex(self.currentPageIndex() + 1);
        if (self.currentPageIndex() > 1) {

            var pageno = self.currentPageIndex();
        }
        else {
            var pageno = 1;
        }
        
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: "/ProductReport/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.productprice(data);
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


            var sort = "order by id desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by id asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }

        if (self.currentPageIndex() > 1) {

            self.currentPageIndex(self.currentPageIndex() - 1);
            var pageno = self.currentPageIndex();

        }
        else {
            var pageno = 1;
        }
        
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: "/ProductReport/search?PageSize=" + pagesize + "&PageNo=" + pageno,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.productprice(data);//GetEmployees();
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


            var sort = "order by id desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by id asc"
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

        
        $.ajax({
            type: "GET",
            url: "/ProductReport/search?PageSize=" + pagesize + "&PageNo=" + pageno + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.productprice(data);
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

    self.adminchange = function () {
        if ($('#ddladmin').val() == "")
        {
            aid = 0;
        }
        else
        {
            aid = $('#ddladmin').val();
        }
        if ($('#ddlproduct').val() == "") {
            pid = 0;
        }
        else {
            pid = $('#ddlproduct').val();
        }

        var pagesize = document.getElementById("pageSizeSelector").value;
        var pageno = self.currentPageIndex();
        $.ajax({
            type: "GET",
            url: '/ProductReport/search?PageSize=' + pagesize + "&PageNo=" + pageno + "&adminid=" + aid + "&productid=" + pid,
            Contenttype: "application/JSON",
            success: function (data) {
                self.productprice(data);
            }
        });
    }
 
    Getadminname();
    Getproductname();
    gettotalrecord();
    getdata();

   

 

   
    
   
}