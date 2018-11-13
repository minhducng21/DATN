app.controller('SidebarController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
    getControllerForCurrentRole();
    function getControllerForCurrentRole(){
        $http({
            method: 'GET',
            url: '/Sidebar/GetController'
        }).then(function success(res) {
            $scope.ctrls = res.data;
        })
    }

    $scope.getClass = function (path) {
        return (window.location.pathname.split('/')[2] === path) ? 'active' : '';
    }
}]);