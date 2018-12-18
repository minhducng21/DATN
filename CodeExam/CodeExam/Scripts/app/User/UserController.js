app.controller('UserController', ['$scope', '$http', function ($scope, $http) {
    //Paging
    $scope.ItemPaging = {};
    $scope.ItemPaging.maxSize = 3;
    $scope.ItemPaging.total = 0;
    $scope.ItemPaging.numPerPage = 5;
    $scope.ItemPaging.currentPage = 1;
    $scope.childsPageChanged = () => {
        getListUser();
    };
    $scope.action = {
        ADD: 1,
        EDIT: 2
    };


    $scope.user = [];
    $scope.message = '';
    $scope.titleModal = '';

    getListUser();

    $scope.modalFunc = () => {
        $scope.titleModal = 'Add user';
        $scope.isAction = $scope.action.ADD;
        $scope.user = {};
        $('.form-group').removeClass('is-filled');
        $('#addUser').modal();
    };

    $scope.getRole = () => {
        $http({
            method: 'GET',
            url: '/Admin/User/GetRole'
        }).then(function success(res) {
            $scope.roles = res.data;
        });
    };

    $scope.getUserById = id => {
        $http({
            method: 'GET',
            url: '/Admin/User/GetUserById?id=' + id
        }).then(function success(res) {
            $scope.user = res.data;
        });
        $scope.titleModal = "User Detail";
        $scope.isAction = $scope.action.EDIT;
        $('#rePass').hide();
        $('.form-group').addClass('is-filled');
        $('#addUser').modal();
    };

    $scope.editUser = user => {
        $http({
            method: 'POST',
            url: '/Admin/User/EditUser',
            data: user
        }).then((res) => {
            if (res.data == 0) {
                var isAssign = true;
                for (var i = 0; i < $scope.users.length; i++) {
                    if (isAssign) {
                        if ($scope.users[i].ID == user.ID) {
                            $scope.users[i] = Object.assign({}, user);
                            isAssign = false;
                        }
                    }
                }
                $scope.message = "Thay đổi thành công";
                $('#notify').modal();
                $('#addUser').modal('toggle');
            }
        });
    };

    function getListUser() {
        var data = {};
        data.page = $scope.ItemPaging.currentPage;
        data.pageSize = $scope.ItemPaging.numPerPage;

        $http({
            method: 'GET',
            url: '/Admin/User/GetUser',
            params: data
        }).then(function success(res) {
            $scope.users = res.data.results;
            $scope.ItemPaging.total = res.data.count;
        });
    }

    $scope.addUser = (user) => {
        var isExist = false;
        angular.forEach($scope.users, (value, key) => {
            if (isExist == false) {
                if (value.UserName == user.UserName) {
                    isExist = true;
                }
            }
        });
        if (isExist) {
            $scope.message = "Tài khoản đã tồn tại";
            //$('#addUser').modal('toggle');

            $('#notify').modal();
        }
        else {
            $http({
                method: 'POST',
                url: '/Admin/User/AddUser',
                params: user
            }).then(function success(res) {
                if (res.data == 0) {
                    $scope.message = "Thành công";
                    $('#addUser').modal('toggle');
                    $('#notify').modal();
                }
            });
        }
    };
        
    $scope.confirmDelete = (id) => {
        $scope.deleteUserId = id;
        $('#delete').modal();
    };
    $scope.deleteUser = () => {
        var id = $scope.deleteUserId;
        $http({
            method: 'POST',
            url: '/Admin/User/DeleteUser',
            data: { id }
        }).then(function success(res) {
            if (res.data == 0) {
                getListUser();
                $('#delete').modal('toggle');
            }
        });
    };
}]);