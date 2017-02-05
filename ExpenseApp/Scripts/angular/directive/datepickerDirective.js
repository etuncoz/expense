app.directive('datepicker', function () {
    return {
        require: 'ngModel',
        maxDate: '+1m',
        link: function (scope, el, attr, ngModel) {
            $(el).datepicker({
                prevText: '<i class="fa fa-chevron-left"></i>',
                nextText: '<i class="fa fa-chevron-right"></i>',
                dateFormat:'dd.mm.yy',
                onSelect: function (dateText) {
                    scope.$apply(function () {
                        ngModel.$setViewValue(dateText);
                    });
                }
            });
        }
    };
});