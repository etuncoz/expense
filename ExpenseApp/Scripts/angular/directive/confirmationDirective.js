app.directive('ngConfirmClick',
[
    function() {
        return {
            link: function(scope, element, attr) {
                var msg = "Selected expense will be deleted";
                var clickAction = attr.confirmedClick;
                element.bind('click',
                    function(event) {
                        if (window.confirm(msg)) {
                            scope.$eval(clickAction);
                        }
                    });
            }
        };
    }
]);