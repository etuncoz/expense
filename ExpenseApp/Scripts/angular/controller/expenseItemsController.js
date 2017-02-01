app.controller("expenseItemsController", function ($scope, expenseService,$window,$q) {

    init();

    function init() { //get expenseItems ..
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
        $scope.expenseItems.forEach(function(item) {
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

    $scope.expense = {};
    $scope.expenseItems = [];

    //$scope.onlyNumbers = /^\d+$/;

    //ADD EXPENSEITEM
    $scope.addExpenseItem = function () {
        $scope.expenseItems.push($scope.newExpenseItem);
        setNewExpenseItem();
        getTotal();    
    }
    //SAVE EXPENSE
    $scope.saveExpense = function () {
        //expense objesini createexpense controlünde olustur
        $scope.expense = {
            "id": maxExpenseId,
            "userId": 1,
            "totalAmount": $scope.variables.totalAmount,
            "createdDate": moment().format('DD.MM.YYYY').split("+")[0]
        };

        var saveExpenseCall = expenseService.saveExpense($scope.expense);
        saveExpenseCall.then(function success(d) {
                console.log("expense saved successfully");
            },
            function error(e) {
                alert("Error: " + e);
                console.log(e);
                return;
            });
        //Bunlara gerek yok sadece expenseItems objesini paslayacagiz(IEnum)
        var promiseArray = [];

        $scope.expenseItems.forEach(function (item, i) {
            item.expenseId = $scope.expense.id;
            item.expenseItemDate = formatDate(item.expenseItemDate);
            var saveExpenseItemCall = expenseService.saveExpenseItem(item);
            promiseArray.push(saveExpenseItemCall);
        });

        $q.all(promiseArray).then(function () {
            console.log(promiseArray);
            $scope.expenseItems = {};
            init();
        });
        
        //$window.location = "/employee/index";
    }
});