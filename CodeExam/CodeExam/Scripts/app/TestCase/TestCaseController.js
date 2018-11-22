app.controller('TestCaseController', ['$http', '$scope', function ($http, $scope) {

    $scope.openModal = () => {
        $scope.action = 'Add';
        $scope.titleModal = 'Add Testcase';
        $('#addTestcase').modal();
    }

    getAllTestCase();
    function getAllTestCase() {
        $http({
            method: 'GET',
            url: '/Admin/TestCase/GetAll'
        }).then(function success(res) {
            $scope.tests = res.data;
        })
    }

    $scope.getTask = () => {
        $http({
            method: 'GET',
            url: '/Admin/Task/GetAll'
        }).then(function success(res) {
            $scope.tasks = res.data;
        });
    }

    $scope.addTest = (test) => {

        $http({
            method: 'POST',
            url: '/Admin/TestCase/AddTestCase',
            data: test
        }).then(function success(res) {
            if (res.data == 0) {
                getAllTestCase();
            }
        })
    }
}]);