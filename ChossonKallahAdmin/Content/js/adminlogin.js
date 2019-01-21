function adminloginKO()
{
    
        var self = this;
        self.email = ko.observable("");
        self.password = ko.observable("");
        self.MessageError = ko.observable("");
        self.Error = ko.observable(false);
        self.enteronlogin = function (d, e) {
            if (e.keyCode == 13) {
                self.Login();
            }
        }


        self.Error(false);
        self.Login = function (data)
        {
            $('.login-panel.panel.panel-default').prepend('<div class="loadersm"></div>');
            if (checkvalidation())
            {
               
              
                self.Error(false);
                  $.ajax({
                    type: "POST",
                    url: "/Adminlogin/CheckUserNamePass?email=" + self.email() + "&password=" + self.password(),
                    data: ko.toJSON(data),
                    contentType: "application/json",
                    async: false,
                    success: function (data) {
                        if (data.length == 0) {
                            $('.login-panel.panel.panel-default div.loadersm').remove();
                            self.MessageError(self.MessageError() + "<br/> -Email-Id or Password is incorrect.");
                            self.Error(true);
                            Clear();
                            return;
                        }
                          else
                          {
                        
                            window.location.href = "../Dashboard/index";
                        }



                    },
                    error: function (error) {
                        $('.login-panel.panel.panel-default div.loadersm').remove();
                        self.MessageError(error);
                        self.Error(true);

                    }
                });

            }
            else {
                $('.login-panel.panel.panel-default div.loadersm').remove();
                self.Error(true);

            }


        };

        function checkvalidation() {

            IsError = true;
            self.MessageError("");
            var reg = /^([A-Za-z0-9_\-\.]+)@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,3})$/;
            if (self.email() == "") {
                self.MessageError(self.MessageError() + " <br/> - Email-Id is required.");
                IsError = false;
                Clear();
            }
            else
            {

                if (reg.test(self.email())==false)
                {
                    self.MessageError(self.MessageError() + " <br/> - Email-Id is invalid.");
                    IsError = false;
                    Clear();

                }

            }
           
           if ($("#txtPsw").val() == "") {
                self.MessageError(self.MessageError() + "<br/> - Password is required.");
                IsError = false;
                Clear();
           }
          

            return IsError;
        }

        function Clear() {

           self.password("");


        }


    
}