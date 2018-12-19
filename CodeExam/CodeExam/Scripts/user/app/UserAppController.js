var userApp = angular.module('userApp', ['ui.bootstrap', 'ui.select2', 'ngRoute']);

var a = function ($routeProvider) {
    $routeProvider
        .when("/Direction/Code/:id", { controller: 'CodeController' })
};
userApp.config(a);