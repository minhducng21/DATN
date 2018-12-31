userApp.controller('LeaderBoardController', ['$http', '$scope', function ($http, $scope) {

    //Paging
    $scope.ItemPaging = {};
    $scope.ItemPaging.maxSize = 5;
    $scope.ItemPaging.total = 0;
    $scope.ItemPaging.numPerPage = 10;
    $scope.ItemPaging.currentPage = 1;

    $scope.leaderBoards = [];
    $scope.languages = [];
    $scope.language = {};

    getLanguage();

   function getLanguage() {
        $http({
            method: 'GET',
            url: '/Leader/GetLanguage'
        }).then(function success(res) {
            $scope.languages = res.data;
            $scope.language = $scope.languages[1];
        })
    }

    $scope.getLeaderBoard = function (id) {
        $scope.languageId = id;
        var data = {};
        data.id = id;
        data.page = $scope.ItemPaging.currentPage;
        data.pageSize = $scope.ItemPaging.numPerPage;
        $http({
            method: 'GET',
            url: '/Leader/GetLeaderBoardByLanguageId',
            params: data
        }).then(function success(res) {
            $scope.leaderBoards = res.data.results;
            $scope.ItemPaging.total = res.data.count;
        });
    }
}]);