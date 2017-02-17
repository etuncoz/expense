app.controller("loginController", function ($scope,expenseService,$window) {
    init();

    function init() {
        $scope.user = {
            "email": "",
            "password": ""
        };
        $scope.request = {};
        $scope.loginFailed = false;
    }

    $scope.login = function () {

        $scope.request.email = $scope.user.email;
        $scope.request.password = $scope.user.password;

        var loginCall = expenseService.login($scope.request);     
        loginCall.then(function success(d) {
            if(d.data.IsSuccess)
                switch (d.data.UserRoleId) {
                    case 1:
                        $window.location.href = window.actionUrls.employeePage +"/"+ d.data.UserId; //düzelt
                        break;
                    case 2:
                        $window.location = window.actionUrls.managerPage;
                        break;
                    case 3:
                        $window.location = window.actionUrls.accountantPage;
                        break;
                }
            else {
                $scope.loginFailed = true;
                $scope.request = {};
            }

        }, function error(e) {
            console.log(e);
            $window.location = window.actionUrls.loginPage;
        });
    }
});