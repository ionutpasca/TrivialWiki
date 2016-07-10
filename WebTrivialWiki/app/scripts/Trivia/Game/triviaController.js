(function (angular, _) {
    'use strict';
    angular.module('triviaModule')
    .controller('triviaController', ['$scope', '$rootScope','triviaFactory', 'triviaService', 'persistService','$window','$mdDialog',
        function ($scope, $rootScope, triviaFactory, triviaService, persistService, $window, $mdDialog) {
        $scope.messages = [];
        $scope.triviaQuestions = [];
        $scope.text = "";
        $scope.skip = 0;
        $scope.responseToSend = '';
    
        function getActiveTables() {
            triviaService.getTriviaTables()
            .then(function (data) {
                $scope.tables = data;
            });
        }

        function getFriends() {
            triviaService.getUserFriends()
            .then(function (data) {
                    debugger;
                $scope.friends = data;
            });
        }

        function init() {
            getActiveTables();
            getFriends();
        }

        $scope.getCurrentUserName = function() {
            return persistService.readData('userName');
        };

        $scope.sendAnswer = function () {
            if ($scope.responseToSend === '') {
                return;
            }
            triviaService.sendAnswer($scope.responseToSend, $scope.getCurrentUserName())
            .then(function() {
                $scope.responseToSend = '';
            });
        };

        $scope.sendMessage = function () {
            if ($scope.messageToSend === undefined || $scope.messageToSend === '') {
                return;
            }
            
            triviaService.sendMessage($scope.messageToSend, $scope.getCurrentUserName())
            .then(function () {
                $scope.messageToSend = '';
            });
        };

        $scope.createNewTable = function() {
            $mdDialog.show({
                clickOutsideToClose: true,
                scope: $scope,
                preserveScope: true,
                templateUrl: 'scripts/Trivia/Game/createTable.html',
                controller: 'createTableController'
            });
        };

        $scope.addNewFriend = function() {
            $mdDialog.show({
                clickOutsideToClose: true,
                scope: $scope,
                preserveScope: true,
                templateUrl: 'scripts/Trivia/Game/addFriend.html',
                controller: 'addFriendController'
            })
            .finally(function() { getFriends(); } );
        };

        $scope.joinTable = function (table) {
            $window.location.href = '#/trivia/' + table.tableName;
            $window.location.reload();
        };

        $scope.senderIsTriviaBot = function(question) {
            return question.Sender === 'TriviaBot';
        };

        $scope.tableIsPublic = function(table) {
            return table.tableName === 'Public Table';
        };

        init();
    }]);
}).call(this, this.angular, this._);