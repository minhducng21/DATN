app.controller('SidebarController', ['$scope', '$http', function ($scope, $http) {
   // getCurrentUser();
    function getCurrentUser(){
        $http({
            method: 'GET',
            url: '/Sidebar/GetCurrentUser'
        }).then(function success(res) {
            $scope.currentUser = res.data;
        })
    }
}]);