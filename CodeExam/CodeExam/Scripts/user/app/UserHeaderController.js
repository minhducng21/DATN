userApp.controller('UserHeaderController', ['$http', '$scope', '$window', function ($http, $scope, $window) {
    getCurrentUser();
    function getCurrentUser() {
        $http({
            method: 'GET',
            url: '/UserHeader/GetCurrentUser'
        }).then(function success(res) {
                $scope.user = res.data;
        });
    }

    $scope.openProfile = function () {
        $('#profileUser').modal();
    }

    $scope.updateUser = function (user) {
        if (user.Password == user.RePassword) {
            $http({
                method: 'POST',
                url: '/UserHeader/UpdateUser',
                data: user
            }).then(function success(res) {
                if (res.data == 0) {
                    $window.location.href = '/Login';
                }
                else {
                    $('#profile').modal('toggle');
                }
            });
        } else {
            $scope.message = 'Mật khẩu không khớp';
        }
    }

    $scope.logOut = function() {
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