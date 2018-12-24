var userApp = angular.module('userApp', ['ui.bootstrap', 'ui.select2', 'ngRoute', 'ngSanitize']);

var a = function ($routeProvider) {
    $routeProvider
        .when("/Direction/Code/:id", { controller: 'CodeController' });
};
userApp.config(a);

userApp.filter('to_trusted', ['$sce', function ($sce) {
    return function (text) {
        var doc = new DOMParser().parseFromString(text, 'text/html');
        var rval = doc.documentElement.textContent;
        return $sce.trustAsHtml(rval);
    };
}]);