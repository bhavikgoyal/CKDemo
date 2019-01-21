function ProfileKO()
{
    $('#loader').hide();

    var self = this;
    self.id = ko.observable();
    self.loginid = ko.observable("");
    self.email = ko.observable("");
    self.active = ko.observable(false);

    self.Success = ko.observable(false);
    self.Error = ko.observable(false);
    self.MessageSuccess = ko.observable("");
    self.MessageError = ko.observable("");
    //self.savebtn = ko.observable(true);
    self.updatebtn = ko.observable(false);

    self.admins = ko.observableArray();

    self.Entersave = function (d, e) {
        if (e.keyCode == 13) {
            if (self.updatebtn() == true)
            {
                self.Update(d);
            }
            else {
                self.Update(d);
            }
        }
    }

    self.Cancel = function () {
        window.location.href = '../Dashboard/Index';
    }
    
    
    self.checkvalidation = function () {
        var IsError = true;
        self.MessageError('');
        var reg = /^([A-Za-z0-9_\-\.]+)@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,3})$/;
        if (self.loginid() == "") {
            self.MessageError(self.MessageError() + "<br/> - User Name is required.");
            IsError = false;
        }
        if (self.email() == "") {
            self.MessageError(self.MessageError() + "<br/> - Email is required.");
            IsError = false;
        }
        else {

            if (reg.test(self.email()) == false) {
                self.MessageError(self.MessageError() + " <br/> - Email is Invalid.");
               IsError = false;
           }

        }
        self.Error(true);
        return IsError;
    }

     self.getdata = function (data) {
      $.ajax(
            {
                type: "POST",
                url: "/Admin/getdetail",
                data: ko.toJSON(data),
                contentType: "application/json",
                async: true,
                success: function (data) {
                    self.loginid(data[0].loginid);
                    self.email(data[0].email);
                    self.active(data[0].active);
                    document.getElementById("username").innerHTML = data[0].loginid;
                 },
                error: function (error) {
                    self.MessageError(error);
                    self.Error(true);

                }

            });
     }

     self.getdata();

     self.Update = function (data) {
         if (self.checkvalidation()) {
             name = self.loginid();
             email = self.email();
             status = self.active();
             self.Error(false);
             $.ajax(
                 {
                     type: "POST",
                     url: "/Admin/Updateprofile?name="+ name+"&email="+email+"&status="+status,
                     data: ko.toJSON(data),
                     contentType: "application/json",
                     async: true,
                     success: function (data) {
                         self.Error(false);
                         self.MessageSuccess("Record has been updated successfully.");
                         self.Success(true);
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

}