app.controller("layoutController", function ($scope,$location,expenseService) {
    $scope.currentLocation = $location;

    $scope.ShowButton = checkUserActivity();

    function checkUserActivity () {
        var activityCall = expenseService.checkUserActivity();
        activityCall.then(function success(d) {
            return d.data.IsActive;
        }, function error(e) {
            console.log(e);
            return false;
        });
    }
});