app.controller("expensesController", function ($scope, expenseService) {

    init();

    function init() {
        $scope.expenses = [];
        $scope.request = {};
        setPagination();
        getExpenses(3);//userid is set to 3 for now
    }

    function setPagination() {
        $scope.currentPage = 1; //current page
        $scope.pageSize = 5; //pagination max size
        $scope.entryLimit = 5; //max rows for data table
    }

    function formatDate(objProp) {
        var dateString = objProp;
        var momentDateObj = moment(dateString, 'YYYY-MM-DD');
        var momentDateString = momentDateObj.format('DD.MM.YYYY').split("T")[0];
        objProp = momentDateString;
        return objProp;
    }

    function getExpenses(userId) {
        $scope.request.userId = userId;
        var getExpensesCall = expenseService.getExpenseByUserId($scope.request);
        getExpensesCall.then(function success(d) {
            d.data.expenseDto.forEach(function (item) {
                item.createdDate = formatDate(item.createdDate);
                $scope.expenses.push(item);

            });
        }, function error(e) {
            console.log(e);
        });

        $scope.deleteExpense = function (expenseId) {
            $scope.request.ID = expenseId;
            var deleteExpenseCall = expenseService.deleteExpense($scope.request);
            deleteExpenseCall.then(function success() {
                console.log("Expense has been deleted successfully");
                init();
                },
                function error(e) {
                    console.log(e);
                });
        }
    }




});