
function CoupunosKO() {

    var self = this;

    self.id = ko.observable(0);
    self.userid = ko.observable(0);
    self.couponcode = ko.observable("");
    self.GenerateTicket = ko.observable("");
    self.username = ko.observable("");
    self.usernames = ko.observableArray();
    self.coupontype = ko.observable("");
    self.numberofuse = ko.observable("");
    self.startdate = ko.observable("");
    self.expirydate = ko.observable("");
    self.discountamount = ko.observable("");
    self.minimumorderamount = ko.observable("");
    self.maximumdiscountamount = ko.observable("");
    self.onetime = ko.observable("");
    self.isactive = ko.observable("");
    self.per = ko.observable(false);
    self.val = ko.observable(false);
    self.maxdis = ko.observable(false);
    self.copunos = ko.observableArray();
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

    ko.bindingHandlers.date = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var valueUnwrapped = ko.utils.unwrapObservable(valueAccessor());
            var textContent = moment(valueUnwrapped).format("MM/DD/YYYY");
            ko.bindingHandlers.text.update(element, function () { return textContent; });
        }
    };

    self.searchKeyUp = function (d, e) {
        if (e.keyCode == 13) {
            self.getdata();
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
        self.userid(null);
        self.couponcode("");
        self.coupontype("-Please Select-");
        self.numberofuse("-Please Select-");
        self.discountamount("");
        self.minimumorderamount("");
        self.maximumdiscountamount("");
        self.onetime("");
        self.startdate("");
        self.expirydate("");
        self.isactive("");

    }

    self.Cancel = function () {
        self.Success(false);
        self.Error(false);
        self.index(true);
        self.insert(false);
        self.clear();
    }

    self.Save = function (data) {
       
        data.expirydate(new Date($('#txtexpdate').val()));
        data.startdate(new Date($('#txtstrtdate').val()));
        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax(
                {
                    type: "POST",
                    url: "/Coupons/Create",
                    data: ko.toJSON(data),
                    contentType: "application/json",
                    async: true,
                    success: function (data) {
                        self.Error(false);
                        self.MessageSuccess("Record has been Saved Successfully.");
                        self.Success(true);
                        self.insert(false);
                        self.index(true);
                    
                        self.getdata();
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
 
    self.GenerateTicket = function () {
        $.ajax({
            type: "GET",
            url: '/Coupons/GetTicket',
            Contenttype: "application/JSON",
            success: function (data) {
                self.couponcode(data);
                $('#loader').hide();

            }
        });
    }

    self.ddlchange = function () {
        if (self.coupontype() == "fixed") {

            self.val(true);
            self.per(false);
            self.maxdis(false);
            self.discountamount("");
        }
        if (self.coupontype() == "Percentage") {
            self.per(true);
            self.val(false);
            self.maxdis(true);
            self.discountamount("");
        }
        if (self.coupontype() == "-Please Select-") {
            self.val(false);
            self.per(false);
            self.maxdis(true);
            self.discountamount("");
        }
    }

    self.ddluse = function () {
        if (self.numberofuse() == "onetime") {
            self.val(false);
        }
        if (self.numberofuse() == "multitime") {
            self.val(false);
        }
    }


    self.checkvalidation = function () {
        var IsError = true;
        self.MessageError('');

        if (self.couponcode() == "") {
            self.MessageError(self.MessageError() + "<br/> - Coupon Code is required.");
            IsError = false;
        }
        if ($('#usersId').val() == "") {
            self.MessageError(self.MessageError() + "<br/> - Please select username.");
            IsError = false;
        }

        if (self.coupontype() == "-Please Select-") {
            self.MessageError(self.MessageError() + "<br/> - Coupon Type  is required.");
            IsError = false;
        }

        if (self.startdate() == 'Invalid Date') {
            self.startdate("");
            if (self.startdate() == "") {
                self.MessageError(self.MessageError() + "<br/> - Start Date is Required. ");
                IsError = false;
            }
        }

        if (self.expirydate() == 'Invalid Date') {

            self.expirydate("");
            if (self.expirydate() == "") {
                self.MessageError(self.MessageError() + "<br/> - Expiry Date  is required.");
                IsError = false;
            }

        }
        if (self.discountamount() == "") {
            self.MessageError(self.MessageError() + "<br/> - Please Enter Discount amount");
            IsError = false;
        }

        self.Error(true);
        return IsError;

    }

    function getnames() {
        $.ajax({
            type: "GET",
            url: '/Coupons/getnames',
            Contenttype: "application/JSON",
            success: function (data) {
                self.usernames(data);
            }
        });
    }

    self.Addnew = function ()
    {
        document.getElementById("add").innerText = "Add Coupon";
        self.index(false);
        self.savebtn(true);
        self.updatebtn(false);
        self.insert(true);
        self.Error(false);
        self.Success(false);
        self.clear();
        $('#txtstrtdate').val('');
        $('#txtexpdate').val('');
        $('#ccd').focus();
        getnames();

    }

    self.getselectedpages = function (copunos)
    {
        getnames();
        self.Success(false);
        self.Error(false);
        self.id(copunos.id);
        self.coupontype(copunos.coupontype);
        self.userid(copunos.userid);
        self.couponcode(copunos.couponcode);
        self.startdate(new Date(parseInt(copunos.startdate.replace(/(^.*\()|([+-].*$)/g, ''))).toLocaleDateString());
        self.expirydate(new Date(parseInt(copunos.expirydate.replace(/(^.*\()|([+-].*$)/g, ''))).toLocaleDateString());
        self.isactive(copunos.isactive);
        self.onetime(copunos.onetime);
        self.minimumorderamount(copunos.minimumorderamount);
        self.maximumdiscountamount(copunos.maximumdiscountamount);
        document.getElementById("add").innerText = "Edit Coupon";
        if (self.coupontype() == "fixed") {
            self.val(true);
            self.per(false);
            self.maxdis(false);
        }

        if (self.coupontype() == "Percentage") {
            self.per(true);
            self.val(false);
            self.maxdis(true);
        }
        self.discountamount(copunos.discountamount);
        self.maximumdiscountamount(copunos.maximumdiscountamount);
        self.index(false);
        self.insert(true);
        self.updatebtn(true);
        self.savebtn(false);
        $('#ccd').focus();
    }

    self.currentPage = ko.computed(function () {
        var pagesize = parseInt(self.pageSize(), 50),
        startIndex = pagesize * self.currentPageIndex(),
        endIndex = startIndex + pagesize;
        return self.copunos.slice(startIndex, endIndex);
    });


    self.sortdata=function()
    {
        if (document.getElementById("Hforder").value == "asc") {
            document.getElementById("Hforder").value = "desc";
            sort = "order by username desc";
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');
        }
        else {
            document.getElementById("Hforder").value = "asc";
            sort = "order by username asc";
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }
        self.getdata();
    }

    self.getdata=function() {

        $('#loader').show();
        gettotalrecord();
        var rec = self.totalrecord();
        var Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        var sort = "order by username " +document.getElementById("Hforder").value;
       
        if (self.currentPageIndex() > 1) {
            var ttl = rec / pagesize;
            var diff = self.currentPageIndex() - ttl;
            if (rec > 31) {
                if (diff > .9)
                {
                    self.currentPageIndex(self.currentPageIndex() - 1);
                }
            }
            else {
                if (diff > .8)
                {
                    self.currentPageIndex(self.currentPageIndex() - 1);
                }
            }
        }
        $.ajax({
            type: "GET",
            url: '/Coupons/SearchGetData?PageSize=' + pagesize + "&PageNo=" + self.currentPageIndex() + "&username=" + Filname + "&sort=" + sort,
            Contenttype: "application/JSON",
            success: function (data) {
                self.copunos(data);
                $('#loader').hide();

                if ((self.totalrecord() / pagesize) <= self.currentPageIndex())
                {

                    document.getElementById("btnnext").disabled = true;
                }
                else
                {

                    document.getElementById("btnnext").disabled = false;
                }
                if (self.currentPageIndex() > 1)
                {

                    document.getElementById("btnprev").disabled = false;
                }
                else
                {
                    document.getElementById("btnprev").disabled = true;
                }

            }
        });
    }
     

    function gettotalrecord() {
     
       var Filname = document.getElementById("FilterbyName").value;
        var pagesize = document.getElementById("pageSizeSelector").value;
        var sort = "order by username " + document.getElementById("Hforder").value;
       
        $.ajax({
            type: "GET",
            url: '/Coupons/Searchtotalrecords?PageSize=' + pagesize + "&PageNo=-1" + "&username=" + Filname + "&sort=" + sort,
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
        self.currentPageIndex(self.currentPageIndex() + 1);
        if (self.currentPageIndex() > 1)
        {

            var pageno = self.currentPageIndex();
        }
        else
        {
            self.currentPageIndex(1);
        }
        self.getdata();



    };

    self.previousPage = function () {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by username desc"
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png')

        }
        else {

            var sort = "order by username asc"
            $("#imgasc").attr('src', '/Content/Images/up_arrow.png');
        }


        if (self.currentPageIndex() > 1) {

            self.currentPageIndex(self.currentPageIndex() - 1);
            var pageno = self.currentPageIndex();

        }
        else {
            self.currentPageIndex(1);
        }
        self.getdata();
    };

    self.selectionChanged = function (event) {
        self.Error(false);
        self.Success(false);
        $('#loader').show();
        if (document.getElementById("Hforder").value == "desc") {


            var sort = "order by username desc";
            $("#imgasc").attr('src', '/Content/Images/down_arrow.png');

        }
        else {

            var sort = "order by username asc";
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
            url: "/Coupons/SearchGetData?PageSize=" + pagesize + "&PageNo=" + pageno + "&username=" + Filname + "&sort=" + sort,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.copunos(data);
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

    self.changestatus = function (copunos) {
        var isactive = copunos.isactive;
        var id = copunos.id;
        $.ajax({
            type: "POST",
            url: "/Coupons/UpdateActive?id=" + id + "&Active=" + isactive,
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

    self.Action = function (copunos) {
        var hdnId = "";
        var status = $('#ddlchagneallstatus').val();

        $('#myTable tbody tr').find('td:eq(0)').find('#checkboxid').each(function () {
            if ($(this).is(":checked")) {
                if ($(this).text() != 'on') {
                    hdnId = hdnId + "," + $(this).text();
                }
            }
        });
        $.ajax({
            type: "POST",
            url: "/Coupons/SelectedApply?id=" + hdnId + "&Active=" + status,
            contentType: "application/json",
            success: function (data) {
                self.getdata();
                alert(status);
            }
        });
    }

    self.Update = function (data) {
        data.expirydate(new Date($('#txtexpdate').val()));
        data.startdate(new Date($('#txtstrtdate').val()));
        if (self.checkvalidation()) {
            self.Error(false);
            $.ajax(
                {
                    type: "POST",
                    url: "/Coupons/Update",
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

    self.deletepopup = function (copunos) {
        $("#myModal").modal('show');
        $('#DeleteID').val(copunos.id);
    }

    self.deleterec = function () {

        id = $('#DeleteID').val();
        $.ajax(
            {
                type: "POST",
                url: "/Coupons/Deleteupdate/" + id,
                success: function (data) {
                    $("#myModal").modal('hide');
                    self.Error(false);
                    self.MessageSuccess("Record has been Deleted Successfully.");
                    self.Success(true);
                    self.Error(false);
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

    function isNumberKey(evt) {
        //var e = evt || window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode
        if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }

  
    self.getdata();

   

}
