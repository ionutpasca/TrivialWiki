(function (angular) {
    'use strict';
    angular.module('triviaModule')
    .controller('createTableController', ['$mdDialog', '$scope', 'triviaService','$window',
        function ($mdDialog, $scope, triviaService, $window) {

            function getTopics() {
                triviaService.getActiveTopics()
                .then(function (data) {
                    $scope.topics = data;
                });
            }

            function init() {
                getTopics();
            }

            init();

            $scope.querySearch = function(query) {
                return query ? $scope.topics.filter(createFilterFor(query)) : $scope.topics;
            };

            $scope.createFilterFor = function (query) {
                var lowercaseQuery = angular.lowercase(query);
                return function filterFn(state) {
                    return (state.value.indexOf(lowercaseQuery) === 0);
                };
            };

            $scope.createTable = function () {
                $scope.tableIsSaving = true;
                triviaService.createNewTable($scope.newTableName, $scope.selectedItem)
                .then(function() {
                    $scope.tableIsSaving = false;
                    $window.location.href = '#/trivia/' + $scope.newTableName;
                    $window.location.reload();
                });
            };
            
    }]);

}).call(this, this.angular);