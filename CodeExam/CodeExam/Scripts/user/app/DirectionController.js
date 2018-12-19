userApp.controller('DirectionController', ['$scope', '$http', function ($scope, $http) {
    //Paging
    $scope.ItemPaging = {};
    $scope.ItemPaging.maxSize = 3;
    $scope.ItemPaging.total = 0;
    $scope.ItemPaging.numPerPage = 9;
    $scope.ItemPaging.currentPage = 1;
    $scope.childsPageChanged = () => {
        getListTask();
    }

    getListTask();
    function getListTask() {
        var data = {};
        data.page = $scope.ItemPaging.currentPage;
        data.pageSize = $scope.ItemPaging.numPerPage;

        $http({
            method: 'GET',
            url: '/Direction/GetAllTask',
            params: data
        }).then(function success(res) {
            $scope.TaskList = res.data.listTasks;
            $scope.ItemPaging.total = res.data.count;
        });
    }
}]);