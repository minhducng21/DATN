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
        $('#addTask').modal();


    }

    // Test case
    $scope.openModalTestCase = () => {
        $scope.test = {};
        $('#addTestcase > .form-group').removeClass('is-filled');
        $scope.titleModal = 'Add Testcase'
        $scope.actionTestCase = 'Add';
        $('#addTestcase').modal();
        $('.fillId').addClass('is-filled');
    }
    $scope.tests = [];
    $scope.addTestCase = (test) => {
        if (test != null) {
            test.TestCaseId = $scope.lastestTestCaseId;
            $scope.tests.push(test);
            $scope.lastestTestCaseId = $scope.lastestTestCaseId + 1;
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
            //$('#addTestcase').modal();
            //$('.fillId').addClass('is-filled');
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
        $scope.test = test;
        $scope.lastestTestCaseId = test.TestCaseId;
        $('.form-group').addClass('is-filled');
        $scope.titleModal = 'Detail'
        $scope.actionTestCase = 'Edit';
        $('#addTestcase').modal();
        //$('.fillId').addClass('is-filled');
    }
    $scope.editTestCase = (test) => {
        for (var i = 0; i < $scope.tests.length; i++) {
            if ($scope.tests[i].TestCaseId == test.TestCaseId) {
                $scope.test[i] = Object.assign({}, test);
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

    $scope.addTask = (task, tests) => {
        $http({
            method: 'POST',
            url: '/Admin/Task/CreateTask',
            data: task
        }).then(function success(res) {
            if (res.data != 0) {
                for (var i = 0; i < tests.length; i++) {
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
            $('#addTask').modal();
            $('.form-group').addClass('is-filled');
        })
    }

    $scope.editTask = (task, tests) => {
        $http({
            method: 'POST',
            url: '/Admin/Task/Update',
            data: task
        }).then(function success(res) {
            if (res.data != 0) {
                //for (var i = 0; i < tests.length; i++) {
                //    tests[i].TaskId = res.data;
                //}
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


    $scope.test = function () {
        var html = `<div id="duplicate" class="row container">
                                                        <div class="form-group col-md-5 bmd-form-group">
                                                            <label class="bmd-label-floating">Name</label>
                                                            <input type="text" class="form-control" ng-model="task.InputName"></input>
                                                        </div>
                                                        <div class="form-group col-md-5">
                                                            <select class="form-control" ng-model="task.InputType" @*ng-options="type.DisplayName for type in dataTypes"*@>
                                                                <option selected value="">Choose input type</option>
                                                                <option ng-repeat="type in dataTypes">{{type.DisplayName}}</option>
                                                            </select>
                                                        </div>
                                                        <button type="submit" class="btn btn-danger" ng-click="removeDuplicate()">
                                                            <i class="glyphicon glyphicon-trash"></i>
                                                        </button>
                                                    </div>`;
        $('#parent').append($compile(html)($scope));
    }
}]);
