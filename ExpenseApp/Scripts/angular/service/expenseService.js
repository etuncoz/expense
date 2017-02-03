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
    this.getExpenseByUserId = function(request) {
        return $http({
            method:"post",
            data: request,
            url: window.actionUrls.GetAllExpenses
        });
    }
    this.GetExpenseItemByExpenseId = function (request) {
        return $http({
            method: "post",
            data: request,
            url: window.actionUrls.GetExpenseItemByExpenseId
        });
    }
    this.saveExpense = function (expense) {
        return $http({
            method: "post",
            data: expense,
            url: window.actionUrls.SaveExpense
        });
    }
});