app.controller("loginController", function ($scope,expenseService,$window) {
    init();

    function init() {
        $scope.user = {
            "email": "",
            "password": ""
        };
        $scope.request = {};
    }

    $scope.login = function () {

        $scope.request.email = $scope.user.email;
        $scope.request.password = $scope.user.password;

        var loginCall =expenseService.login($scope.request);     
        loginCall.then(function success(d) {
            console.log(d.data);
            $window.location = d.data.RedirectionUrl;
        }, function error(e) {
            console.log(e);
            $window.location = "/home/login";
        });
    }
});