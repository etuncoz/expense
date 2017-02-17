app.controller("employeeController", function ($scope, expenseService) {
    var userId = window.userId;

    init();

    function init() {
        $scope.expenses = [];
        $scope.request = {};
        $scope.statuses = [];
        $scope.variables = { "userId": 0 };
        setPagination();
        getExpenses(userId);//userid is set to 1 for now
    }

    function setPagination() {
        $scope.currentPage = 1;
        $scope.pageSize = 5;
    }

    //function findStatuses(lastExpenseActionId) {
    //    expenseStatuses.find(function (element, index, array) {
    //        return array.indexOf(element) + 1 == lastExpenseActionId;
    //    });
    //}

    function formatDate(date) {
        var dateString = date;
        var momentDateObj = moment(dateString, 'YYYY-MM-DD');
        var momentDateString = momentDateObj.format('DD.MM.YYYY').split("T")[0];
        date = momentDateString;
        return date;
    }

    function getExpenses(userId) {
        $scope.request.UserId = userId;
        var getExpensesCall = expenseService.getExpenseByUserId($scope.request);
        getExpensesCall.then(function success(d) {
            console.log("incoming expenses:");
            console.log(d.data);
            d.data.expenseDto.forEach(function (item) {
                item.createdDate = formatDate(item.createdDate);
                $scope.expenses.push(item);
            });
            $scope.variables.userId = d.data.userId;
        }, function error(e) {
            console.log(e);
        });

        $scope.sendForApproval = function (expenseId) {
            $scope.request.ID = expenseId;
            var approvalCall = expenseService.sendExpenseForApproval($scope.request);
            approvalCall.then(function success() {
                console.log("Expense has been sent successfully");
                init();
            }, function error(e) {
                console.log(e);
                init();
            });
        }

        $scope.deleteExpense = function (expenseId) {
            $scope.request.ID = expenseId;
            var deleteExpenseCall = expenseService.deleteExpense($scope.request);
            deleteExpenseCall.then(function success() {
                console.log("Expense has been deleted successfully");
                init();
            }, function error(e) {
                console.log(e);
                init();
            });
        }
    }




});