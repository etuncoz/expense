app.controller("approvalExpenseController", function ($scope, expenseService,$window) {
    init();

    function init() {
        var currentExpenseId = window.currentExpenseId;

        $scope.expenseItems = [];
        $scope.variables = { "totalAmount": 0, "rejectReason": ""};
        $scope.request = {};
        $scope.request.ExpenseId = currentExpenseId;
        $scope.request.ID = currentExpenseId;
        //getCurrentExpenseStatus();
        getExpenseItems();
    }
    function formatDate(date) {
        var dateString = date;
        var momentDateObj = moment(dateString, 'YYYY-MM-DD');
        var momentDateString = momentDateObj.format('DD.MM.YYYY').split("T")[0];
        date = momentDateString;
        return date;
    }

    function getExpenseItems() {
        var getExpenseServiceCall = expenseService.getExpenseItemByExpenseId($scope.request);
        getExpenseServiceCall.then(function success(d) {
            console.log(d.data);
            d.data.expenseItemDto.forEach(function (item) {
                $scope.variables.totalAmount += item.amount;
                item.expenseItemDate = formatDate(item.expenseItemDate, true);
                $scope.expenseItems.push(item);
            });
            console.log($scope.expenseItems);
        }, function error(e) {
            console.log(e);
        });
    }

    $scope.approve = function () {
        $scope.request.IsApproved = true;
        $scope.request.RejectReason = null;
        var approveCall = expenseService.approveOrRejectExpense($scope.request);
        approveCall.then(function success(d) {
            console.log("Expense is rejected successfully");
            $window.location = "/manager/index";
        }, function error(e) {
            console.log(e);
        });
    }

    $scope.reject = function () {
        $scope.request.IsApproved = false;
        $scope.request.RejectReason = $scope.variables.rejectReason;
        var approveCall = expenseService.approveOrRejectExpense($scope.request);
        approveCall.then(function success() {
            console.log("Expense is approved successfully");
            $window.location = "/manager/index";
        }, function error(e) {
            console.log(e);
        });
    }
});