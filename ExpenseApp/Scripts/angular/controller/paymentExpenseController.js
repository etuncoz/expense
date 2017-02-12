app.controller("paymentExpenseController", function ($scope, expenseService,$window) {
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

    $scope.payExpense = function () {
        var approveCall = expenseService.payExpense($scope.request);
        approveCall.then(function success(d) {
            $scope.variables.isDecided = d.data.isSuccess;
            $window.location = "/accountant/index";
        }, function error(e) {
            console.log(e);
        });
    }

});