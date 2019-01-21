function ConfigurationKO() {
    var self = this;
    self.sitename = ko.observable("");
    self.siteurl = ko.observable("");
    self.oflinemode = ko.observable(false);
    self.oflinemessage = ko.observable("");
    self.oflinemessage2 = ko.observable("");
    self.keyword = ko.observable("");
    self.description = ko.observable("");
    self.defaulttitle = ko.observable("");
    self.analyticcode = ko.observable("");
    self.adminemailid = ko.observable("");
    self.systememailid = ko.observable("");
    self.systememailpassword = ko.observable("");
    self.address = ko.observable("");
    self.fax = ko.observable("");
    self.mobileno = ko.observable("");
    self.hostname = ko.observable("");
    self.ssl = ko.observable(false);
    self.dfltcrd = ko.observable(false);
    self.portno = ko.observable("");
    self.frmemail = ko.observable("");
    self.isadmin = ko.observable(false);
    self.emailtemp = ko.observable("");
    self.actv = ko.observable(false);
    self.subject = ko.observable("");
    self.emailtitle = ko.observable("");
    self.footertext = ko.observable("");
    self.pagesize = ko.observable(null);
    self.websitelogo = ko.observable("");
    self.tb1 = ko.observable(true);
    self.tb2 = ko.observable(false);
    self.tb3 = ko.observable(false);
    self.tb4 = ko.observable(false);
    self.tb5 = ko.observable(false);

    self.Success = ko.observable(false);
    self.Error = ko.observable(false);
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");
    self.savebtn = ko.observable(true);
    self.updatebtn = ko.observable(false);
    self.configs = ko.observableArray();
    self.insert = ko.observable(true);
    self.index = ko.observable(false);

    self.Tab1 = function () {
        self.tb1(true);
        self.tb2(false);
        self.tb3(false);
        self.tb4(false);
        self.tb5(false);
        $('#tb1').addClass('active');
        $('#tb2').removeClass('active');
        $('#tb3').removeClass('active');
        $('#tb4').removeClass('active');
        $('#tb5').removeClass('active');
    }

    self.Tab2 = function () {
        self.tb2(true);
        self.tb1(false);
        self.tb3(false);
        self.tb4(false);
        self.tb5(false);
        $('#tb2').addClass('active');
        $('#tb1').removeClass('active');
        $('#tb3').removeClass('active');
        $('#tb4').removeClass('active');
        $('#tb5').removeClass('active');
    }

    self.Tab3 = function () {
        self.tb3(true);
        self.tb2(false);
        self.tb1(false);
        self.tb4(false);
        self.tb5(false);
        $('#tb3').addClass('active');
        $('#tb2').removeClass('active');
        $('#tb1').removeClass('active');
        $('#tb4').removeClass('active');
        $('#tb5').removeClass('active');
    }

    self.Tab4 = function () {
        self.tb4(true);
        self.tb2(false);
        self.tb3(false);
        self.tb1(false);
        self.tb5(false);
        $('#tb4').addClass('active');
        $('#tb2').removeClass('active');
        $('#tb1').removeClass('active');
        $('#tb3').removeClass('active');
        $('#tb5').removeClass('active');
    }

    self.Tab5 = function () {
        self.tb5(true);
        self.tb1(false);
        self.tb2(false);
        self.tb3(false);
        self.tb4(false);
        $('#tb5').addClass('active');
        $('#tb1').removeClass('active');
        $('#tb2').removeClass('active');
        $('#tb3').removeClass('active');
        $('#tb4').removeClass('active');
    }

    self.checkvalidation = function () {
        var IsError = true;
        self.MessageError('');
        var reg = /^([A-Za-z0-9_\-\.]+)@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,3})$/;
        if (self.adminemailid() == "") {
            self.MessageError(self.MessageError() + "<br/> - Username is required.");
            IsError = false;
        }
        else {
            if (reg.test(self.adminemailid()) == false) {
                self.MessageError(self.MessageError() + " <br/> - Admin Email ID is Invalid.");
                IsError = false;
            }
        }
        if (self.systememailid() == "") {
            self.MessageError(self.MessageError() + "<br/> - System Email ID is required.");
            IsError = false;
        }
        else {
            if (reg.test(self.systememailid()) == false) {
                self.MessageError(self.MessageError() + " <br/> -System Email ID is Invalid.");
                IsError = false;
            }
            if ($('#area').val() == "") {
                self.MessageError(self.MessageError() + " <br/> -Please Select the Image.");
                IsError = false;
            }

        }
        if (self.subject() == "")
        {
            self.MessageError(self.MessageError() + "<br/> - Please Enter Subject.");
            IsError = false;
        }
        if (self.emailtitle() == "")
        {
            self.MessageError(self.MessageError() + "<br/> Please Enter Email Title. ");
        }

        self.Error(true);
        return IsError;
    }

    self.Save = function (data) {
        jQuery.fn.CKEditorValFor = function (element_id) {
            return CKEDITOR.instances[element_id].getData();
        }

        data.websitelogo($('#area').val());
        data.oflinemessage($().CKEditorValFor('txtoflinemessage'));
        data.oflinemessage2($().CKEditorValFor('txtoflinemessage2'));
        if (self.checkvalidation()) {

            $.ajax(
                {
                    type: "POST",
                    url: "/Configuration/Update",
                    data: ko.toJSON(data),
                    contentType: "application/json",
                    async: true,
                    success: function (data) {
                        self.Error(false);
                        self.MessageSuccess("Record has been Updated Successfully.");
                        self.Success(true);
                        $('#inputFileToLoad').val('');
                        alert(0);
                    },

                    error: function (error) {
                        self.MessageError(error);
                        self.Error(true);
                    }

                });
        }
        else {
            self.Error(false);
            self.Success(true);
        }
    }

    function configs() {
        $('#loader').show();
        $.ajax({
            type: "GET",
            url: "/Configuration/GetAll",
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (data) {
                self.configs(data);
                self.sitename(data[0].sitename);
                self.siteurl(data[0].siteurl);
                self.oflinemode(data[0].oflinemode);
                self.oflinemessage(data[0].oflinemessage);
                self.oflinemessage2(data[0].oflinemessage2);
                self.keyword(data[0].keyword);
                self.description(data[0].description);
                self.defaulttitle(data[0].defaulttitle);
                self.analyticcode(data[0].analyticcode);
                self.adminemailid(data[0].adminemailid);
                self.systememailid(data[0].systememailid);
                self.systememailpassword(data[0].systememailpassword);
                self.hostname(data[0].hostname);
                self.ssl(data[0].ssl);
                self.dfltcrd(data[0].dfltcrd);
                self.portno(data[0].portno);
                self.frmemail(data[0].frmemail);
                self.isadmin(data[0].isadmin);
                self.emailtemp(data[0].emailtemp);
                self.actv(data[0].actv);
                self.subject(data[0].subject);
                self.emailtitle(data[0].emailtitle);
                self.footertext(data[0].footertext);
                self.pagesize(data[0].pagesize);
                self.address(data[0].address);
                self.mobileno(data[0].mobileno);
                self.fax(data[0].fax);
                CKEDITOR.instances.txtoflinemessage.setData(data[0].oflinemessage);
                CKEDITOR.instances.txtoflinemessage2.setData(data[0].oflinemessage2);
                if (data[0].websitelogo != "") {
                    $("a#viewimg").attr('href', "../UploadImages/" + data[0].websitelogo);
                }
                else {
                    $("a#viewimg").css('display', 'none');
                }
                $('#loader').hide();
            },
            error: function (error) {
                self.MessageError(error);
                self.Error(true);

            }
        });


    }
    configs();




}