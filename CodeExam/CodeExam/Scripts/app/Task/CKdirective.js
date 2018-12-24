app.directive('ck', ['$scope', '$http', function ($scope, $http) {
    return {
        require: '?ngModel',
        link: function (scope, ele, attr, ngModel) {
            var ck = CKEDITOR.replace(ele[0]);

            if (!ngModel) return;

            ck.on('pasteState', function () {
                scope.$apply(function () {
                    ngModel.$setViewValue(ck.getData());
                });
            });

            ngModel.$render = function (value) {
                ck.setData(ngModel.$viewValue);
            };
        }
    }; 
}]);