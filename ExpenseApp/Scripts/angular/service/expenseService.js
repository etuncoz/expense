app.service("expenseService", function ($http) {
    //login
    this.login = function (request) {
        return $http({
            method: "post",
            data: request,
            url: window.actionUrls.loginPage
        });
    }

    //this.checkUserActivity = function () {
    //    return $http.get(window.userUrls.checkUserActivity);
    //}

    //expenses
    this.getExpenseByUserId = function (request) { //for employee/index
        return $http({
            method: "post",
            data: request,
            url: window.actionUrls.getExpenseByUserId
        });
    }
    this.getExpenseByActionId = function (request) {//for manager/index
        return $http({
            method:"post",
            data:request,
            url:window.actionUrls.getExpenseByActionId
        });
    }

    this.getExpenseItemByExpenseId = function (request) { //for employee/saveexpense
        return $http({
            method: "post",
            data: request,
            url: window.actionUrls.getExpenseItemByExpenseId
        });
    }

    this.sendExpenseForApproval = function (request) { // employee sends expense to management
        return $http({
            method: "post",
            data: request,
            url: window.actionUrls.sendExpenseForApproval
        });
    }

    this.approveOrRejectExpense = function (request) {
        return $http({
            method: "post",
            data: request,
            url: window.actionUrls.approveOrRejectExpense
        });
    }

    this.payExpense = function (request) {
        return $http({
            method: "post",
            data: request,
            url: window.actionUrls.payExpense
        });
    }

    this.saveExpense = function (request) {
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

    this.getCurrentExpenseStatus = function (request) {
        return $http({
            method: "post",
            data: request,
            url: window.actionUrls.getCurrentExpenseStatus
        });
    }

    this.getRejectReason = function (request) {
        return $http({
            method: "post",
            data: request,
            url: window.actionUrls.getRejectReason
        });
    }
});