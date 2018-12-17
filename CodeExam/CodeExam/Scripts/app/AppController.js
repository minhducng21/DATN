var app = angular.module('app', ['ui.bootstrap', 'ui.select2']).directive('ckEditor', function () {
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
});