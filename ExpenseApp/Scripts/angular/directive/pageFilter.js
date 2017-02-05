app.filter('startFrom', function () {
    return function (input, start) {
        if (input) {
            start = 0 + start;
            return input.slice(start);
        }
        return [];
    }
});