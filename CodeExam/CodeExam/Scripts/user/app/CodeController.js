userApp.controller('CodeController', ['$scope', '$http', '$location', function ($scope, $http, $location) {

    var id = $location.absUrl().split('=')[1];
    if (id == undefined) {
        id = $location.absUrl().split('/')[5];
    }
    var Input = [];
    getTaskById(id);
    function getTaskById(id) {
        $http({
            method: 'GET',
            url: '/Direction//GetTaskById?id=' + id
        }).then(function success(res) {
            $scope.task = res.data.task;
            $scope.testCases = res.data.listTestCases;

            // handle task
            var arrInput = $scope.task.Input.split(';');
            for (var i = 0; i < arrInput.length; i++) {
                if (arrInput[i] != "") {
                    Input[i] = { InputName: arrInput[i].split(':')[0], InputType: arrInput[i].split(':')[1] };
                   
                }
            }
            $scope.task.Input = Input;

            // handle test case
            for (var i = 0; i < $scope.testCases.length; i++) {
                var arrTestCase = $scope.testCases[i].Input.split(';');
                    $scope.testCases[i].Input = []

                for (var j = 0; j < arrTestCase.length; j++) {
                    if (arrTestCase[j] != "") {
                        $scope.testCases[i].Input.push(encodeURIComponent(arrTestCase[j]));
                    }
                }
            }
        })
    }

    $scope.collapseTestCase = function (index) {
        if (typeof $scope.collapseIndex === "number" && $scope.collapseIndex != index + 1) {
            $(`#collapse${$scope.collapseIndex} div`).first().removeClass('in');
        }
        $scope.collapseIndex = index + 1;
        $(`#collapse${index + 1}`).attr("href", `#testcase${ index + 1 }`)
    }

}])