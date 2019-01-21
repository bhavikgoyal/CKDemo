function ChangepasswordKO()
{
    var self = this;
    $('#loader').hide();
    $('#oldpassword').focus();
    self.id = ko.observable(0);
    self.oldpassword = ko.observable("");
    self.newpassword = ko.observable("");
    self.ConfirmNewPassword = ko.observable("");
   
    self.Success = ko.observable(false);
    self.Error = ko.observable(false);
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");
    
    self.checkvalidation = function () {
        var IsError = true;
        self.MessageError('');
       

        if (self.oldpassword() == "") {
            self.MessageError(self.MessageError() + "<br/> - Old Password is required.");
            IsError = false;
        }
        if (self.newpassword() == "") {
            self.MessageError(self.MessageError() + "<br/> - New Password is required.");
            IsError = false;
        }
        if (self.ConfirmNewPassword() == "") {
            self.MessageError(self.MessageError() + "<br/> - Confirm Password is required.");
            IsError = false;
        }
       if (self.ConfirmNewPassword() != self.newpassword()) {
            self.MessageError(self.MessageError() + "<br/> - Password and Confirm Password does not match.");
            IsError = false;
        }

        self.Error(true);
        return IsError;
    }

    self.clear = function ()
    {
        self.oldpassword("");
        self.newpassword("");
        self.ConfirmNewPassword("");
    }

    self.Cancel=function()
    {
        window.location.href = '../Dashboard/Index';
    }

    self.Update = function (data) {
        if (self.checkvalidation()) {
            self.Error(false);
            oldpass = self.oldpassword();
            newpass = self.newpassword();
            $.ajax(
                {
                    type: "POST",
                    url: "/ChangePassword/Update?oldpass="+ oldpass +"&newpass=" + newpass,
                    data: ko.toJSON(data),
                    contentType: "application/json",
                    async: true,
                    success: function (data) {
                        if (data == oldpass)
                        {
                            self.Success(false);
                            self.Error(true);
                            self.MessageError("Old Password is Not Correct.");
                        }
                        else
                        {
                            self.Error(false);
                            self.MessageSuccess("Record has been Updated Successfully.");
                            self.Success(true);
                        }
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
}