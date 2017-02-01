app.controller("expenseItemsControllerDeneme", function ($scope, expenseService) {

    var currentExpenseId = window.expenseId,
        maxExpenseItemId = window.maxExpenseItemId+1;

    init();

    function init() { //get expenseItems ..
        $scope.variables = { "totalAmount": 0 }
        setNewExpenseItem();
        getExpenseItems(currentExpenseId);
    }   

    function getTotal () {
        var total = 0;
        var items = $scope.expenseItems;
        for(var item in items) // incele angular.foreach
        {
            var amount = items[item].amount;
            if (amount) {
                total += amount;
            }

        }
        $scope.variables.totalAmount = total;
    }

    function formatDate(objProp) {
        var dateString = objProp;
        var momentDateObj = moment(dateString, 'DD.MM.YYYY');
        var momentDateString = momentDateObj.format().split("+")[0];
        objProp = momentDateString;
        return objProp;
    }

    function setNewExpenseItem() {
        $scope.newExpenseItem = {
            id: maxExpenseItemId,
            expenseId: currentExpenseId,
            amount: '',
            description: '',
            expenseItemDate: ''
        }
    }

    function getExpenseItems(expenseId) {
        var serviceCall = expenseService.getExpenseItems(expenseId);
        serviceCall.then(function success(d) {
            //Format upcoming dates
            var dateString = "";
            for (var i = 0; i < d.data.length; i++)  {
                d.data[i].amount = parseInt(d.data[i].amount);
                dateString = d.data[i].expenseItemDate.split("T")[0];
                var momentDateObj = moment(dateString, 'YYYY-MM-DD');
                var momentDateString = momentDateObj.format('DD.MM.YYYY');
                d.data[i].expenseItemDate = momentDateString;
                dateString = "";
            }
            $scope.expenseItems = d.data;
            console.log($scope.expenseItems);
            getTotal();

        }, function error(e) {
            //clear newexpenseitem obj
            setNewExpenseItem();
            alert("Error: " + e);
        });
    }
    $scope.addExpenseItem = function () {
        //format dates to be sent

        console.log($scope.newExpenseItem.expenseItemDate);
        $scope.newExpenseItem.expenseItemDate = formatDate($scope.newExpenseItem.expenseItemDate);

        $scope.expenseItems.push($scope.newExpenseItem);

        console.log($scope.newExpenseItem);
        console.log($scope.expenseItems);

        var serviceCall = expenseService.saveExpenseItem($scope.newExpenseItem);
        serviceCall.then(function success(d) {
            maxExpenseItemId++;
            console.log("item saved successfully");
            init();

        }, function error(e) {
            init();
            alert("Error: " + e);
        });
    }

    $scope.updateExpenseItem = function (id) {
        var itemToBeUpdated = $scope.expenseItems.filter(function (e) {
            return e.id === id;
        })[0];

        itemToBeUpdated.expenseItemDate = formatDate(itemToBeUpdated.expenseItemDate);
        
        console.log("updateitem: ");
        console.log(itemToBeUpdated);

        var serviceCall = expenseService.updateExpenseItem(id, itemToBeUpdated);
        serviceCall.then(function success(d) {
                console.log("item updated successfully");
                init();
            },
            function error(e) {
                alert("Error: " + e);
                console.log("item update failed: "+e);
            });
    }
    $scope.deleteExpenseItem = function(id) {
        var serviceCall = expenseService.deleteExpenseItem(id);
        serviceCall.then(function success(d) {
            console.log("item updated deleted");
            init();
        },
            function error(e) {
                alert("Error: " + e);
                console.log("item delete failed: " + e);
            });

    }



});