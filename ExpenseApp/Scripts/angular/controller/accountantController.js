app.controller("accountantController", function ($scope, expenseService) {
    init();

    function init() {
        $scope.expenses = [];
        $scope.request = {};
        setPagination();
        getExpenses(3);//actionId is set to 1 for now
    }

    function setPagination() {
        $scope.currentPage = 1;
        $scope.pageSize = 5;
    }

    function formatDate(date) {
        var dateString = date;
        var momentDateObj = moment(dateString, 'YYYY-MM-DD');
        var momentDateString = momentDateObj.format('DD.MM.YYYY').split("T")[0];
        date = momentDateString;
        return date;
    }

    function getExpenses(actionId) {
        $scope.request.ID = actionId;
        var getExpensesCall = expenseService.getExpenseByActionId($scope.request);
        getExpensesCall.then(function success(d) {
            d.data.expenseDto.forEach(function (item) {
                item.createdDate = formatDate(item.createdDate);
                $scope.expenses.push(item);
            });
        }, function error(e) {
            console.log(e);
        });

    }
});