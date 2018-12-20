userApp.controller('CodeController', ['$scope', '$http', '$location', function ($scope, $http, $location) {

    var id = $location.absUrl().split('=')[1];
    if (id == undefined) {
        id = $location.absUrl().split('/')[5];
    }

    $('.lds-ring').hide();

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

            $('.description').html($scope.task.TaskDescription);
        })
    }

    $scope.collapseTestCase = function (index) {
        if (typeof $scope.collapseIndex === "number" && $scope.collapseIndex != index + 1) {
            $(`#collapse${$scope.collapseIndex} div`).first().removeClass('in');
        }
        $scope.collapseIndex = index + 1;
        $(`#collapse${index + 1}`).attr("href", `#testcase${index + 1}`)
    }

    $scope.clickRun = function () {
        $('.lds-ring').show();
        $('.ts').hide();
        $http({
            method: 'POST',
            url: '/Compiler/GenFileAndRun',
            data: {
                source: editor.getValue(), taskId: $scope.task.TaskId, language: $('#select-language').val()
            }
        }).then(function success(res) {
            if (res.data.isSuccess) {
                for (var i = 0; i < res.data.detail.length; i++) {
                    if (res.data.detail[i].CompareExpection) {
                        $(`#collapse${i + 1} i`).first().removeAttr('hidden');
                        $(`#collapse${i + 1} i`).last().attr('hidden', 'true');
                    }
                    else {
                        $(`#collapse${i + 1} i`).last().removeAttr('hidden');
                        $(`#collapse${i + 1} i`).first().attr('hidden', 'true');
                    }
                }
                $('.lds-ring').hide();
                $('.ts').show();
            }
        })
    }



}])