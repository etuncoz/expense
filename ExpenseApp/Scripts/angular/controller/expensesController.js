app.controller("expensesController", function ($scope, expenseService, filterFilter) {

    init();

    function init() {
        $scope.expenses = [];

        getExpenses(1);//userid is set to 1 for now


        $scope.currentPage = 1; //current page
        $scope.maxSize = 5; //pagination max size
        $scope.entryLimit = 5; //max rows for data table

        /* init pagination with $scope.list */
        $scope.noOfPages = Math.ceil($scope.expenses.length / $scope.entryLimit);

        $scope.$watch('search', function (term) {
            // Create $scope.filtered and then calculat $scope.noOfPages, no racing!
            $scope.filtered = filterFilter($scope.expenses, term);
            $scope.noOfPages = Math.ceil($scope.filtered.length / $scope.entryLimit);
        });

    }

    function formatDate(objProp) {
        var dateString = objProp;
        var momentDateObj = moment(dateString, 'YYYY-MM-DD');
        var momentDateString = momentDateObj.format('DD.MM.YYYY').split("T")[0];
        objProp = momentDateString;
        return objProp;
    }

    function getExpenses(userId) {
        $scope.request = {
            "userId": 0
            //"expenseId": null,
            //"lastExpenseActionId": null,
        };
        $scope.request.userId = userId;
        var getExpensesCall = expenseService.getExpenseByUserId($scope.request);
        getExpensesCall.then(function success(d) {
            d.data.expenseDto.forEach(function (item) {
                item.createdDate = formatDate(item.createdDate);
                $scope.expenses.push(item);
            });
            console.log($scope.expenses);
        }, function error(e) {
            console.log(e);
        });
    }




});