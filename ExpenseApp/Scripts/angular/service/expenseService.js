app.service("expenseService", function ($http) {

    //expenseitems 
    this.getExpenseItems = function (expenseId) {
        return $http.get("/api/expenseitems/" + expenseId);
    }
    this.saveExpenseItem = function (expenseItem) {
        return $http({
            method:"post",
            data:expenseItem,
            url:"/api/expenseitems"
        }); 
    }
    this.updateExpenseItem = function(expenseItemId,expenseItem) {
        return $http({
            method: "put",
            data: expenseItem,
            url: "/api/expenseitems/" + expenseItemId
        });
    }
    this.deleteExpenseItem = function(expenseItemId) {
        return $http.delete("/api/expenseitems/" + expenseItemId);
    }

    //expenses
    this.getExpenses = function() {
        return $http.get("/api/expenses/");
    }
    this.saveExpense = function (expense) {
        return $http({
            method: "post",
            data: expense,
            url: "/api/expenses"
        });
    }
});