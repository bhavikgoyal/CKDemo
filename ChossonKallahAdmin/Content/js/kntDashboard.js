function DashboardKO()
{
    $('#loader').hide();
    var self = this;
    self.firstname = ko.observable("");
    self.users = ko.observableArray();
    self.orders = ko.observableArray();
    self.logs = ko.observableArray();

    ko.bindingHandlers.date = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var valueUnwrapped = ko.utils.unwrapObservable(valueAccessor());
            var textContent = moment(valueUnwrapped).format("MM/DD/YYYY");
            ko.bindingHandlers.text.update(element, function () { return textContent; });
        }
    };

    function getusers()
    {
        $('#loader').show();
        $.ajax({
            type: "GET",
            url: '/Dashboard/getusers',
            Contenttype: "application/JSON",
            success: function (data) {
                self.users(data);
                $('#loader').hide();
            }
        });

    }

    function getorder() {
        $('#loader').show();
        $.ajax({
            type: "GET",
            url: '/Dashboard/getorder',
            Contenttype: "application/JSON",
            success: function (data) {
                self.orders(data);
                $('#loader').hide();
            }
        });

    }
    function getproductlog() {
        $('#loader').show();
        $.ajax({
            type: "GET",
            url: '/Dashboard/getproductlog',
            Contenttype: "application/JSON",
            success: function (data) {
                self.logs(data);
                $('#loader').hide();
            }
        });

    }

    getorder();
    getusers();
    getproductlog();
   
}