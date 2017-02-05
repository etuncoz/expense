app.controller("saveExpenseController", function ($scope, expenseService, $window) {
    init();

    function init() {
        var currentExpenseId = window.currentExpenseId;

        $scope.expenseItems = [];
        $scope.request = {};
        $scope.variables = { "totalAmount": 0 };
        $scope.request.ExpenseId = currentExpenseId;
        setNewExpenseItem();
        if (currentExpenseId>0)//
            getExpenseItems();

    }
    function formatDate(objProp, isComingFromDb) {
        var dateString = objProp;
        if (isComingFromDb) {    
            var momentDateObj = moment(dateString, 'YYYY-MM-DD');
            var momentDateString = momentDateObj.format('DD.MM.YYYY').split("T")[0];
            objProp = momentDateString;
            return objProp;
        }
        else {
            var momentDateObj = moment(dateString, 'DD.MM.YYYY');
            var momentDateString = momentDateObj.format().split("+")[0];
            objProp = momentDateString;
            return objProp;
        }

    }

    $scope.getTotal = function () {
        var total = 0;
        $scope.expenseItems.forEach(function (item) {
            if (item.amount)
                total += item.amount;
        });
        $scope.variables.totalAmount = total;
    }

    function getExpenseItems() {
        var getExpenseServiceCall = expenseService.getExpenseItemByExpenseId($scope.request);
        getExpenseServiceCall.then(function success(d) {
            console.log(d.data);
            d.data.expenseItemDto.forEach(function (item) {
                $scope.variables.totalAmount += item.amount;
                item.expenseItemDate = formatDate(item.expenseItemDate,true);
                $scope.expenseItems.push(item);
            });
            console.log($scope.expenseItems);
        }, function error(e) {
            console.log(e);
        });
    }

    function setNewExpenseItem() {
        $scope.newExpenseItem = {
            id: 0,
            expenseId: 0,
            amount: '',
            description: '',
            expenseItemDate: ''
        };
    }

    $scope.addExpenseItem = function () {
        $scope.expenseItems.push($scope.newExpenseItem);
        setNewExpenseItem();
        $scope.getTotal();
    }

    $scope.saveExpense = function () {

        $scope.expenseItems.forEach(function (item) { //format expenseItem dates
            item.expenseItemDate = formatDate(item.expenseItemDate,false);
        });

        $scope.request.ExpenseItemsDto = $scope.expenseItems;

        console.log($scope.request);

        var saveExpenseCall = expenseService.saveExpense($scope.request);
        saveExpenseCall.then(function success() {
            console.log("expense saved successfully");
            $scope.expenseItems = {};
            init();
            $window.location = "/employee/index";
        }, function error(e) {
            console.log(e);
        });
    }

});