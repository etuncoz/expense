app.controller("expenseItemsController", function ($scope, expenseService, $window) {

    init();

    $scope.request = {};
    $scope.expenseItems = [];


    function init() {
        $scope.variables = { "totalAmount": 0 }
        setNewExpenseItem();
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
    function getTotal() {
        var total = 0;
        $scope.expenseItems.forEach(function (item) {
            var amount = item.amount;
            if (amount)
                total += amount;
        });
        $scope.variables.totalAmount = total;
    }

    function formatDate(objProp) {
        var dateString = objProp;
        var momentDateObj = moment(dateString, 'DD.MM.YYYY');
        var momentDateString = momentDateObj.format().split("+")[0];
        objProp = momentDateString;
        return objProp;
    }

    

    //$scope.onlyNumbers = /^\d+$/;

    //ADD EXPENSEITEM
    $scope.addExpenseItem = function () {
        $scope.expenseItems.push($scope.newExpenseItem);
        setNewExpenseItem();
        getTotal();
    }
    //SAVE EXPENSE
    $scope.saveExpense = function () {

        $scope.expenseItems.forEach(function (item) { //format expenseItem dates
            item.expenseItemDate = formatDate(item.expenseItemDate);
            console.log(item.createdDate);
        });
        console.log($scope.expenseItems);
        

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