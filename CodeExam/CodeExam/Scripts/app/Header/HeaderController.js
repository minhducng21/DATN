app.controller('HeaderController', ['$scope', '$http', '$window', function ($scope, $http, $window) {
    getCurrentUser();
    function getCurrentUser() {
        $http({
            method: 'GET',
            url: '/Header/GetCurrentUser'
        }).then(function success(res) {
            $scope.currentUser = res.data;
        });
    }

    $scope.logOut = () => {
        $http({
            method: 'GET',
            url: 'http://localhost:12595/Login/LogOut'
        }).then(function success(res) {
            if (res.data == 1) {
                $window.location.href = '/Login';
            }
        })
    }
}]);