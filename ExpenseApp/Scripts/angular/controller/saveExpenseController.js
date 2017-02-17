app.controller("saveExpenseController", function ($scope, expenseService, $window) {

    init();

    function init() {
        var currentExpenseId = window.currentExpenseId,
            currentUserId = window.currentUserId;

        $scope.expenseItems = [];
        $scope.deletedExpenseItems = [];
        $scope.rejectReason = "";
        $scope.request = {};
        $scope.variables = { "totalAmount": 0 };
        $scope.request.ExpenseId = currentExpenseId;
        $scope.request.UserId = currentUserId;
        setDefaults();

        //if its not a new expense
        if (currentExpenseId > 0) {
            getExpenseItems();
            getRejectReason();
        }
    }

    function formatDate(date, isComingFromDb) {
        var dateString = date;
        if (isComingFromDb) {
            var momentDateObj = moment(dateString, 'YYYY-MM-DD');
            var momentDateString = momentDateObj.format('DD.MM.YYYY').split("T")[0];
            date = momentDateString;
            return date;
        }
        else {
            var momentDateObj = moment(dateString, 'DD.MM.YYYY');
            var momentDateString = momentDateObj.format().split("+")[0];
            date = momentDateString;
            return date;
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
                item.expenseItemDate = formatDate(item.expenseItemDate, true);
                $scope.expenseItems.push(item);
            });
            console.log($scope.expenseItems);
        }, function error(e) {
            console.log(e);
        });
    }

    function getRejectReason() {
        var getRejectReasonCall = expenseService.getRejectReason($scope.request);
        getRejectReasonCall.then(function success(d) {
            console.log(d.data);
            $scope.rejectReason = d.data.rejectReason;
        }, function error(e) {
            console.log(e);
        });
    }

    function setDefaults() {
        $scope.newExpenseItem = {
            id: 0,
            expenseId: 0,
            amount: '',
            description: '',
            expenseItemDate: ''
        };
        deletedExpenseItem = {
            id: 0
        };
    }

    $scope.addExpenseItem = function () {
        $scope.expenseItems.push($scope.newExpenseItem);
        setDefaults();
        $scope.getTotal();
        $scope.NewExpenseItem.$setPristine();
    }

    $scope.deleteExpenseItem = function (expenseItemId) {
        var indexOfDeletedItem = null;
        $scope.expenseItems.forEach(function (item, index) { //find item index to call splice
            if (item.id === expenseItemId)
                indexOfDeletedItem = index;
        });

        deletedExpenseItem.id = expenseItemId;
        $scope.deletedExpenseItems.push(deletedExpenseItem);

        $scope.expenseItems.splice(indexOfDeletedItem, 1);
        $scope.getTotal();
    }

    $scope.saveExpense = function () {

        $scope.expenseItems.forEach(function (item) {
            item.expenseItemDate = formatDate(item.expenseItemDate, false);
        });

        $scope.request.DeletedExpenseItems = $scope.deletedExpenseItems;
        $scope.request.ExpenseItemsDto = $scope.expenseItems;

        console.log($scope.request);

        var saveExpenseCall = expenseService.saveExpense($scope.request);
        saveExpenseCall.then(function success(d) {
            console.log("expense saved successfully");
            $scope.expenseItems = [];
            $scope.deletedExpenseItems = [];
            $window.location = window.actionUrls.saveSuccess;
        }, function error(e) {
            $scope.expenseItems.forEach(function (item) {
                item.expenseItemDate = formatDate(item.expenseItemDate, true);
            });
            console.log(e);
        });
    }

});