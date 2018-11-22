app.controller('RoleController', ['$scope', '$http', function ($scope, $http) {
    getRole();
    function getRole() {
        $http({
            method: 'GET',
            url: '/Admin/Role/GetRole'
        }).then(function success(res) {
            $scope.roles = res.data;
        })
    }

    function getController() {
        $http({
            method: 'GET',
            url: '/Admin/Role/GetController'
        }).then(function myfunction(res) {
            $scope.ctrls = res.data;
        })
    }

    $scope.getRoleById = function (id) {
        $http({
            method: 'GET',
            url: '/Admin/Role/GetControllerById',
            params: { id }
        }).then(function success(res) {
            $scope.ctrls = res.data.ctrls;
            $scope.role = res.data.role;
            $scope.modalTitle = 'Edit Role';
            $scope.action = 'Edit';
            $('#roleModal').modal();
        });
    }

    $scope.openModalRole = function () {
        getController();
        $scope.role = {};
        $scope.modalTitle = 'Add Role';
        $scope.action = 'Add';
        $('#roleModal').modal();
    }

    $scope.addRole = function (ctrls) {

        var controllers = [];
        angular.forEach(ctrls, function (val, key) {
            if (val.IsChecked) {
                controllers.push(val);
            }
        });
        var roleControllerViewModel =
            {
                RoleId: "",
                RoleName: "",
                ControllerViewModels: []
            };
        roleControllerViewModel.RoleName = $scope.role.RoleName;
        roleControllerViewModel.ControllerViewModels = controllers;

        $http({
            method: 'POST',
            url: '/Admin/Role/AddRole',
            data: roleControllerViewModel
        }).then(function success(res) {
            if (res.data == 0) {
                $('#roleModal').modal('toggle');
                getRole();
            }
        });
    }

    $scope.editRole = function (ctrls) {
        var RoleId = $scope.role.RoleId;
        var ControllerViewModels = [];
        angular.forEach(ctrls, function (val, key) {
            if (val.IsChecked == true) {
                ControllerViewModels.push(val);
            }
        });
        var roleControllerViewModel =
            {
                RoleId: "",
                RoleName: "",
                ControllerViewModels: []
            };
        roleControllerViewModel.RoleId = RoleId;
        roleControllerViewModel.RoleName = $scope.role.RoleName;
        roleControllerViewModel.ControllerViewModels = ControllerViewModels;
        $http({
            method: 'POST',
            url: '/Admin/Role/EditRole',
            data: roleControllerViewModel
        }).then(function success(res) {
            if (res.data == 0) {
                $('#roleModal').modal('toggle');
                getRole();
            }
        })
    }

    $scope.confirmDeleteRole = function (id) {
        $scope.deleteRoleId = id;
        $('#deleteRole').modal();
    }

    $scope.deleteRole = function () {
        var roleId = $scope.deleteRoleId;
        $http({
            method: 'POST',
            url: '/Admin/Role/DeleteRole',
            params: { roleId }
        }).then(function success(res) {
            if (res.data == 0) {
                $('#deleteRole').modal('toggle');
                getRole();
            }
        })
    }
}]);