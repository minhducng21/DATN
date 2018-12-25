﻿userApp.controller('CodeController', ['$scope', '$http', '$location', function ($scope, $http, $location) {

    $scope.languages = [{ Name: 'Javascript', Value: 'js' }, { Name: 'C#', Value: 'csharp' }];

    var editor = CodeMirror(document.getElementById("codeeditor"), {
        mode: "javascript",
        theme: "abcdef",
        lineNumbers: true,
        viewportMargin: 50
    });

    var id = $location.absUrl().split('=')[1];
    if (id == undefined) {
        id = $location.absUrl().split('/')[5];
    }

    $('.lds-ring').hide();
    $('#console').hide();

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
            templateCode($scope.task.TaskId, 'js');
        })
    }


    function templateCode(taskId, language) {
        $http({
            method: 'GET',
            url: '/Compiler/GenerateTemplateCode',
            params: { taskId, language }
        }).then(function success(res) {
            //$('#codeeditor').html(res.data);
            editor.setValue(res.data);
        });
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

                $('#submit').attr('hidden', 'true');
            }
            else {
                $('#console').show();
                $('#menu2 p').html(res.data.errMsg);
                $('#menu2 p').show();
                $('.ts').show();
                $('.ts').removeClass('active');

                $('#testcase').removeClass('active');
                $('#console').addClass('active');
                $('#menu2').addClass('active');
                $('.lds-ring').hide();

                $('#submit').attr('hidden', 'true');
            }
        })
    }

    $scope.changeLanguage = function (language) {
        templateCode($scope.task.TaskId, language);
    }

    $scope.submit = function () {
        //$scope.currentPoint = $scope.task.Point;

        $('#menu2 p').hide();
        $('#testcase').removeClass('active');
        $('#console').show();
        $('#console').addClass('active');
        $('#menu2').addClass('active');
        $('#submit').removeAttr('hidden');
    }
}])