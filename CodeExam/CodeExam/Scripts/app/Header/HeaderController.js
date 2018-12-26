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

    $scope.openProfileModal = () => {
        $('.form-group').addClass('is-filled');
        $('#profile').modal();
    }

    $scope.updateUser = (obj) => {
        $http({
            method: 'POST',
            url: '/Header/EditUser',
            params: obj
        }).then(function success(res) {
            if (res.data == 0) {
                $('#profile').modal('toggle');
            }
        })
    }

    $scope.logOut = () => {
        $http({
            method: 'GET',
            url: 'https://localhost:44371/Login/LogOut'
        }).then(function success(res) {
            if (res.data == 1) {
                $window.location.href = '/Login';
            }
        })
    }
}]);