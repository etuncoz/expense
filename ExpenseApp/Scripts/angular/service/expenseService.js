app.service("expenseService", function ($http) {

    //expenseitems 
    //this.getExpenseItems = function (expenseId) {
    //    return $http.get("/api/expenseitems/" + expenseId);
    //}

    //this.updateExpenseItem = function(expenseItemId,expenseItem) {
    //    return $http({
    //        method: "put",
    //        data: expenseItem,
    //        url: "/api/expenseitems/" + expenseItemId
    //    });
    //}
    //this.deleteExpenseItem = function(expenseItemId) {
    //    return $http.delete("/api/expenseitems/" + expenseItemId);
    //}

    //expenses
    this.getExpenseByUserId = function(request) { //for listing expenses on index page
        return $http({
            method:"post",
            data: request,
            url: window.actionUrls.getExpenseByUserId
        });
    }
    this.getExpenseItemByExpenseId = function (request) { //for saveexpense page
        return $http({
            method: "post",
            data: request,
            url: window.actionUrls.getExpenseItemByExpenseId
        });
    }
    this.saveExpense = function(request) {
        return $http({
            method: "post",
            data: request,
            url: window.actionUrls.saveExpense
        });
    }

    this.deleteExpense = function (request) {
        return $http({
            method: "post",
            data: request,
            url: window.actionUrls.deleteExpense
        });
    }
});