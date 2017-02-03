app.controller("editExpenseController", function ($scope, expenseService) {
    init();

    function init() {
        var currentExpenseId = window.currentExpenseId;
        console.log(currentExpenseId);

        $scope.expenseItems = [];
        $scope.request.ExpenseId = currentExpenseId;
        $scope.variables = { "totalAmount": 0 }
        setNewExpenseItem();
        getExpenses(currentExpenseId);

    }
    function formatDate(objProp, getOrSend) {
        if (getOrSend === "get") {
            var dateString = objProp;
            var momentDateObj = moment(dateString, 'YYYY-MM-DD');
            var momentDateString = momentDateObj.format('DD.MM.YYYY').split("T")[0];
            objProp = momentDateString;
            return objProp;
        }
        if (getOrSend === "send") {
            var dateString = objProp;
            var momentDateObj = moment(dateString, 'DD.MM.YYYY');
            var momentDateString = momentDateObj.format().split("+")[0];
            objProp = momentDateString;
            return objProp;
        }

    }

    function getTotal() {
        var total = 0;
        $scope.expenseItems.forEach(function (item) {
            var amount = item.amount;
            if (amount)
                total += amount;
        });
        $scope.variables.totalAmount = total;
    }

    function getExpenses(expenseId) {
        console.log("hey");
        var getExpenseServiceCall = expenseService.GetExpenseItemByExpenseId($scope.request);
        getExpenseServiceCall.then(function success(d) {
            console.log(d.data);
            d.data.expenseItemDto.forEach(function (item) {
                item.expenseItemDate = formatDate(item.expenseItemDate,"get");
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
        }
    }

    $scope.addExpenseItem = function () {
        $scope.expenseItems.push($scope.newExpenseItem);
        setNewExpenseItem();
        getTotal();
    }

    $scope.saveExpense = function () {

        $scope.expenseItems.forEach(function (item) { //format expenseItem dates
            item.expenseItemDate = formatDate(item.expenseItemDate,"send");
        });

        $scope.request.ExpenseItemsDto = $scope.expenseItems;

        console.log($scope.request);

        var saveExpenseCall = expenseService.saveExpense($scope.request);
        saveExpenseCall.then(function success(d) {
            console.log("expense saved successfully");
            $scope.expenseItems = {};
            init();
            $window.location = "/employee/index";
        }, function error(e) {
            alert("Error: " + e);
            console.log(e);
        });
    }

});