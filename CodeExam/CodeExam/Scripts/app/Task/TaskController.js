app.controller('TaskController', ['$scope', '$http', function ($scope, $http) {

    $scope.modalFunc = function() {
        $('#addTask').modal();
    }
}]);
