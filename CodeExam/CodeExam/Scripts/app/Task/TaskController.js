app.controller('TaskController', ['$scope', '$http', '$compile', function ($scope, $http, $compile) {

    //Paging
    $scope.ItemPaging = {};
    $scope.ItemPaging.maxSize = 3;
    $scope.ItemPaging.total = 0;
    $scope.ItemPaging.numPerPage = 5;
    $scope.ItemPaging.currentPage = 1;
    $scope.childsPageChanged = () => {
        getAllTask();
    }

    $scope.Point = [100, 200, 300, 400, 500, 600, 700, 800, 900, 1000];
    $scope.Level = ['Easy', 'Medium', 'Hard'];

    $scope.modalFunc = function () {
        //Get Lastest testcaseId;
        getLastestTestCaseId();
        $scope.modalTitle = 'Add Task';
        $scope.action = 'Add';
        $scope.tests = [];
        $('.form-group').removeClass('is-filled');
        $scope.task = {};
        $scope.dataInput = [{ InputID: 0, InputName: 'arg1', InputType: '' }];

        $scope.testCases = [];
        var testCase = {
            Input: [],
            Output: ''
        };
        $scope.testCases.push(testCase);

        $('#addTask').modal();
    }

    // Test case
    $scope.openModalTestCase = () => {
        
        $('#addTestcase > .form-group').removeClass('is-filled');
        $scope.titleModal = 'Add Testcase';
        $scope.actionTestCase = 'Add';
        $('#addTestcase').modal();
        $('.fillId').addClass('is-filled');
    }
    $scope.tests = [];
    $scope.addTestCase = (testCases) => {
        if (testCases != null) {
            var testCase = {
                Input: [],
                Output: testCases.Output
            };
            for (var i = 0; i < testCases.length; i++) {
                testCase.Input.push(testCases[i].Input);
            }
            $scope.tests.push(testCase);

            $('.modal').css('overflow-x', 'hidden');
            $('.modal').css('overflow-y', 'auto');
            $('#addTestcase').modal('toggle');
        }


    }
    function getLastestTestCaseId() {
        $http({
            method: 'GET',
            url: '/Admin/Task/GetLastestTestCaseId'
        }).then(function success(res) {
            $scope.lastestTestCaseId = res.data;
        });
    }
    $scope.deleteTestCase = (id) => {
        for (var i = 0; i < $scope.tests.length; i++) {
            if ($scope.tests[i].TestCaseId == id) {
                $scope.tests.splice(i, 1);
            }
        }
    }
    $scope.detailTestCase = (test) => {
        $scope.testCases = [];
        for (var i = 0; i < test.Input.length; i++) {
            testCase = { Input: test.Input[i] };
            $scope.testCases.TaskId = test.TaskId;
            $scope.testCases.TestCaseId = test.TestCaseId;
            $scope.testCases.Output = test.Output;
            $scope.testCases.push(testCase);
        }
        
        $('.form-group').addClass('is-filled');
        $scope.titleModal = 'Detail';
        $scope.actionTestCase = 'Edit';
        $('#addTestcase').modal();
    }
    $scope.editTestCase = (test) => {
        var inputTestCase = [];
        for (var i = 0; i < test.length; i++) {
            inputTestCase.push(test[i].Input);
        }
        test.Input = inputTestCase;

        for (var i = 0; i < $scope.tests.length; i++) {
            if ($scope.tests[i].TestCaseId == test.TestCaseId) {
                $scope.tests[i] = Object.assign({}, test);
            }
        }
        $('.modal').css('overflow-x', 'hidden');
        $('.modal').css('overflow-y', 'auto');
        $('#addTestcase').modal('toggle');
    }

    getAllTask();
    function getAllTask() {
        var data = {};
        data.page = $scope.ItemPaging.currentPage;
        data.pageSize = $scope.ItemPaging.numPerPage;
        $http({
            method: 'GET',
            url: '/Admin/Task/GetAll',
            params: data
        }).then(function success(res) {
            $scope.tasks = res.data.results;
            $scope.ItemPaging.total = res.data.count;
        });
    }

    $scope.dataInput = [{ InputID: 0, InputName: 'arg1', InputType: '' }];
    // Create a ele input
    $scope.add = function () {
        var input = { InputID: $scope.dataInput.length, InputName: '', InputType: '' };
        $scope.dataInput.push(input);

        $scope.testCases = [];
        for (var i = 0; i < $scope.dataInput.length; i++) {
            var testCase = { TestCaseID: i + 1, Input: '' };
            $scope.testCases.push(testCase);
        }
    };


    $scope.addTask = (task, tests) => {
        var inputName = '';
        for (var i = 0; i < $scope.dataInput.length; i++) {
            inputName += $scope.dataInput[i].InputName + ":" + $scope.dataInput[i].InputType + ";";
        }
        task.Input = inputName;

        $http({
            method: 'POST',
            url: '/Admin/Task/CreateTask',
            data: task
        }).then(function success(res) {
            if (res.data != 0) {
                for (var i = 0; i < tests.length; i++) {
                    inputTest = '';
                    for (var j = 0; j < tests[i].Input.length; j++) {
                        inputTest += tests[i].Input[j] + ";";
                    }
                    tests[i].Input = inputTest;
                    tests[i].TaskId = res.data;
                }
                $http({
                    method: 'POST',
                    url: '/Admin/Task/CreateTestCase',
                    data: tests
                }).then(function success(res) {
                    if (res.data == 0) {
                        getAllTask();
                        $scope.tests = [];
                        $('#addTask').modal('toggle');
                    }
                })
            }
        });
    }

    //Remove element Input
    $scope.delete = function (id) {
        for (var i = 0; i < $scope.dataInput.length; i++) {
            if ($scope.dataInput[i].InputID == id) {
                $scope.dataInput.splice(i, 1);
                $scope.testCases.splice(i, 1);
            }
        }
    }

    $scope.detailTask = (id) => {
        $scope.modalTitle = 'Edit Task';
        $scope.action = 'Edit';
        $http({
            method: 'GET',
            url: '/Admin/Task/GetTaskById',
            params: { id }
        }).then(function success(res) {
            $scope.task = res.data.task;
            $scope.tests = res.data.tests;

            testCaseName = [];
            for (var i = 0; i < $scope.tests.length; i++) {
                testCaseName = $scope.tests[i].Input.split(';');
                var inputRes = [];
                for (var j = 0; j < testCaseName.length; j++) {
                    if (testCaseName[j] != '') {
                        inputRes.push(testCaseName[j]);
                    }
                }
                $scope.tests[i].Input = inputRes;
            }

            var arrInput = $scope.task.Input.split(';');

            $scope.dataInput = [];
            for (var i = 0; i < arrInput.length; i++) {
                if (arrInput[i] != "") {
                    var input = {};
                    input.InputName = arrInput[i].split(":")[0];
                    input.InputType = arrInput[i].split(":")[1];

                    $scope.dataInput.push(input);
                }
            }

            $('#addTask').modal();
            $('.form-group').addClass('is-filled');
        });
    }

    $scope.editTask = (task, tests) => {
        var inputName = '';
        for (var i = 0; i < $scope.dataInput.length; i++) {
            inputName += $scope.dataInput[i].InputName + ":" + $scope.dataInput[i].InputType + ";";
        }
        task.Input = inputName;

        $http({
            method: 'POST',
            url: '/Admin/Task/Update',
            data: task
        }).then(function success(res) {
            if (res.data != 0) {

                for (var i = 0; i < tests.length; i++) {
                    inputTest = '';
                    for (var j = 0; j < tests[i].Input.length; j++) {
                        inputTest += tests[i].Input[j] + ";";
                    }
                    tests[i].Input = inputTest;
                    //tests[i].TaskId = res.data;
                }

                $http({
                    method: 'POST',
                    url: '/Admin/Task/EditTestCase',
                    data: tests
                }).then(function success(res) {
                    if (res.data == 0) {
                        $('#addTask').modal('toggle');
                        getAllTask();
                    }
                });


            }
        });
    }

    $scope.confirmDelete = (id) => {
        $('#delete').modal();
        $scope.taskId = id;
    }

    $scope.deleteTask = () => {
        id = $scope.taskId;
        $http({
            method: 'POST',
            url: '/Admin/Task/Delete',
            params: { id }
        }).then(function success(res) {
            if (res.data == 0) {
                $('#delete').modal('toggle');
                getAllTask();
            }
        })
    }

    getDataType();
    $scope.dataTypes = [];
    function getDataType() {
        $http({
            method: 'GET',
            url: '/Task/GetDataType'
        }).then(function success(res) {
            $scope.dataTypes = res.data;
        });
    }

    $scope.indexInput = 0;
    $scope.addInput = function () {
        var original = document.getElementById(`duplicate${$scope.indexInput}`);
        original.childNodes[3].onclick = removeElement;
        var clone = original.cloneNode(true);

        var currentIndexInput = $scope.indexInput;

        clone.id = `duplicate${++$scope.indexInput}`;
        clone.childNodes[5].onclick = $scope.addInput;
        clone.childNodes[3].disabled = false;

        clone.childNodes[3].onclick = removeElement;

        original.parentNode.appendChild(clone);

        var preDuplicate = document.getElementById(`duplicate${currentIndexInput}`);
        $scope.child = document.getElementById(`add-input`);
        preDuplicate.removeChild($scope.child);

        if (currentIndexInput == 0) {
            original.childNodes[3].disabled = false;
        }
    }

    function removeElement(e) {
        if ($scope.indexInput == 1) {
            e.currentTarget.disabled = true;
        }
        if (e.currentTarget.parentNode.childNodes.length == 9) {
            var preElement = document.getElementById(`duplicate${--$scope.indexInput}`);
            preElement.append($scope.child);
        }
        e.currentTarget.parentNode.remove();
    }

    $scope.save = function () {

    }

}]);
