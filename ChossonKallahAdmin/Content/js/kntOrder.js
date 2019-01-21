function OrderKO()
{
    var self = this;
    self.id = ko.observable(0);
    self.username = ko.observable("");
    self.orderdate = ko.observable("");
    self.userid = ko.observable("");
    self.orderdescription = ko.observable("");
    self.orderstatus = ko.observable("");
    self.totalamount = ko.observable("");
    self.shippingamount = ko.observable("");
    self.taxamount = ko.observable("");
    self.discountamount = ko.observable("");
    self.finalamount = ko.observable("");
    self.email = ko.observable("");
    self.address = ko.observable("");
    self.city = ko.observable("");
    self.state = ko.observable("");
    self.countryid = ko.observable("");
    self.zipcode = ko.observable("");

    self.changedate = ko.observable("");
    self.discription = ko.observable("");
    self.orderid = ko.observable(0);

    self.orders = ko.observableArray();

    self.index = ko.observable(true);
    self.detail = ko.observable(false);
    
    self.Success = ko.observable(false);
    self.Error = ko.observable(false);
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");

    //variable og paging
    self.currentPage = ko.observable();
    self.pageSize = ko.observable(50);
    self.currentPageIndex = ko.observable(1);
    self.totalrecord = ko.observable(0);
    self.search = ko.observable(true);

    //pagging
    self.searchKeyUp = function (d, e) {
        if (e.keyCode == 13) {
            self.Search();
        }
    }

    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 50),
        startIndex = pagesize * self.currentPageIndex(),
        endIndex = startIndex + pagesize;
        return self.orders.slice(startIndex, endIndex);
    });

    function getdata() {
        $('#loader').show();
        var id = 0;
        if (window.location.href.indexOf('id') >= 0) {
            var splitdata = window.location.href.split('=');
            id = splitdata[1];
        }
        rec = self.totalrecord();
        var Filname = document.getElementById("FilterbyName").value;
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
        var sort = "order by userid asc"
        $.ajax({
            type: "GET",
            url: '/Order/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&username=" + Filname + "&sort=" + sort + "&id=" + id,
            Contenttype: "application/JSON",
            success: function (data) {
                self.orders(data);
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

    self.Print = function (printarea)
    {
        {
            var prtContent = document.getElementById("printarea");
            var WinPrint = window.open('', '', 'left=0,top=0,width=1500,height=900,toolbar=0,scrollbars=0,status=0');
            $(".print-section").hide();
            $("#order_p").show();
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.close();
            $(".print-section").show();
            $("#order_p").hide();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
        }
    }

    self.Back=function()
    {
        self.detail(false);
        self.index(true);
        self.Error(false);
        self.Success(false);
    }

    self.Detail = function (orders) {
        orderid = orders.id;
        //self.username(orders.username);
        //self.userid(orders.userid);
        //self.address(orders.address);
        //self.city(orders.city);
        //self.state(orders.state);
        //self.countryid(orders.countryid);
        //self.zipcode(orders.zipcode);
        //self.orderdate(orders.orderdate)
        self.index(false);
        self.detail(true);
        getstatus(orderid);
    }

    ko.bindingHandlers.date = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var valueUnwrapped = ko.utils.unwrapObservable(valueAccessor());
            var textContent = moment(valueUnwrapped).format("DD/MM/YYYY");
            ko.bindingHandlers.text.update(element, function () { return textContent; });
        }
    };

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
        var id = 0;
        var Filname = document.getElementById("FilterbyName").value;
        var pageno = self.currentPageIndex();
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: '/Order/search?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&username=" + Filname + "&sort=" + sort + "&id=" + id,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.orders(data);
                gettotalrecord();//GetEmployees();
                $('#loader').hide();
            },
            error: function (error) {
               // alert(error.status + "<!----!>" + error.statusText);
                $('#loader').hide();
            }
        });

    }

    self.Search = function (orders) {
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
        var oldpageno = self.currentPageIndex();
        var sort = "order by userid asc"
        self.currentPageIndex(1);
        var pageno = 1;
        var id = 0;
        gettotalrecord();
        $.ajax({
            type: "GET",
            url: "/Order/Search?PageSize=" + pagesize + "&PageNo=" + pageno + "&username=" + Filname + "&sort=" + sort + "&id=" + id,
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {

                if (data.length == 0) {
                    self.Success(false);
                    self.MessageError("No Record Found.");
                    self.Error(true);
                    self.orders(data);
                    $('#loader').hide();
                    return;

                }
                self.Success(false);
                self.Error(false);
                self.orders(data);
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
                alert('error');
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
        
        var sort = "order by userid asc"
        var Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        $.ajax({
            type: "GET",
            url: "/Order/Search?PageSize=" + pagesize + "&PageNo=-1" + "&username=" + Filname + "&sort=" + sort + "&id=" + id,
            Contenttype: "application/JSON",
            success: function (data) {
            self.totalrecord(data[0].id);
                //alert(data[0].id);
            }
        });
    }

    self.nextPage = function () {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        var sort = '';
        if (document.getElementById("Hforder").value == "desc") {
            sort = "order by userid desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {
            cc
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }


        self.currentPageIndex(self.currentPageIndex() + 1);
        if (self.currentPageIndex() > 1) {

            var pageno = self.currentPageIndex();
        }
        else {
            var pageno = 1;
        }
        var Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        sort = "order by userid asc"
        $.ajax({
            type: "GET",
            url: "/Order/Search?PageSize=" + pagesize + "&PageNo=" + pageno + "&username=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.orders(data);//GetEmployees();
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
        var sort = "order by userid asc"
        $.ajax({
            type: "GET",
            url: "/Order/Search?PageSize=" + pagesize + "&PageNo=" + pageno + "&username=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.orders(data);//GetEmployees();
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
        var id = 0;
        
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        var pageno = self.currentPageIndex();
        var totalrec = document.getElementById("Hftotalrecord").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        var sort = document.getElementById("Hforder").value;
        sort = 'order by userid ' + sort;
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
            url: "/Order/Search?PageSize=" + pagesize + "&PageNo=" + pageno + "&username=" + Filname + "&sort=" + sort + "&id=" + id,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.orders(data);
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

    self.changestatus = function (orders) {

        var ispaid = orders.ispaid;
        var id = orders.id;

        $.ajax({
            type: "POST",
            url: "/Order/UpdateActive?id=" + id + "&Active=" + ispaid,
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

    self.deletepopup = function (orders) {
        
        $("#myModal").modal('show');
        var today = moment().format('MM/DD/YYYY');
        self.changedate(today);
        $('#addeditbox').val(orders.id);
        self.discription("");
       
    }

    self.updatedesc = function () {
        var statusid = "0";
        statusid=  $('#statusid').val();
        orderid = $('#addeditbox').val();
        desc = $('#desc').val();
        date= $('#txtchngdt').val();
         $.ajax(
            {
                type: "POST",
                url: "/Order/Addstatus?id="+orderid+"&desc="+desc + "&date=" +date +"&statusid=" +statusid ,
                success: function () {
                    $("#myModal").modal('hide');
                    getstatus(orderid);
                    self.Error(false);
                    self.Success(true);
                    self.MessageSuccess("Record has been Updated Successfully.");
                    self.detail(true);
                    self.index(false);
              },
                error: function (error) {
                    self.MessageError(error);
                    self.Error(true);
                }
            });
    }

    function getstatus(orderid)
    {
        $.ajax(
                      {
                          type: "POST",
                          url: "/Order/getdtatus?id=" + orderid,
                          success: function (data) {
                              
                               if(data==1)
                               {
                                   document.getElementById("orderstatus").innerText = "Accepted";
                                   $('#btnaccepted').hide();
                              }
                              if(data==2)
                              {
                                  document.getElementById("orderstatus").innerText = "Shipped";
                                  $('#btnaccepted').hide();
                                  $('#btnShippeed').hide();
                              }
                              if(data==3)
                              {
                                  document.getElementById("orderstatus").innerText = "Completed";
                                  $('#btnaccepted').hide();
                                  $('#btnShippeed').hide();
                                  $('#btnCompleated').hide();
                              }
                              
                          }
                      });
    }

    gettotalrecord();
    getdata();
}