(function (angular, _) {
    'use strict';
    angular.module('triviaModule')
    .controller('triviaTableController', ['$scope', 'triviaFactory', 'triviaService', '$routeParams','persistService','$interval',
        function ($scope, triviaFactory, triviaService, $routeParams, persistService, $interval) {

        function getTableTopic(tableName) {
            triviaService.getTableTopic(tableName)
                .then(function(data) {
                    $scope.tableTopic = data;
                });
        }

        function getUsersFromTable(tableName) {
            triviaService.getTableUsers(tableName)
                .then(function(data) {
                    $scope.usersOnTable = data;
                });
        }

        function resetMessages() {
            $scope.messages = [];
        }

        function increaseDeterminateValue() {
            $interval(function () {
                $scope.determinateValue += 1;
                if ($scope.determinateValue === 100) {
                    $scope.newQuestionIsLoading = false;
                    return;
                }
            }, 50, 0, true);
        }

        function init() {
            $scope.connectedUsersAreLoading = true;
            $scope.noQuestion = true;
            $scope.newQuestionIsLoading = false;
            $scope.determinateValue = 0;

            $scope.tableName = $routeParams.tableName;
            $scope.connectedUsers = [];
            $scope.currentQuestion = '';


            getTableTopic($scope.tableName);
            getUsersFromTable($scope.tableName);
            resetMessages();
        }
        init();

        var triviaProxy = triviaFactory($scope.tableName);

        triviaProxy.on('addQuestion', function (res) {
            if ($scope.currentQuestion === '') {
                $scope.currentQuestion = res;
            } else {
                $scope.nextQuestion = res;
            }
        });

        triviaProxy.on('addMessage', function (res) {
            $scope.messages.push(res);
        });

        triviaProxy.on('sendConnectedUsers', function (res) {
            $scope.connectedUsers = res;
        });
        
        triviaProxy.on('correctAnswer', function (res) {
            $scope.messages.push(res);
            $scope.newQuestionIsLoading = true;
            increaseDeterminateValue();
        });

        $scope.getCurrentUserName = function () {
            return persistService.readData('userName');
        };

        $scope.sendAnswer = function () {
            if ($scope.responseToSend === '') {
                return;
            }
            var response = angular.copy($scope.responseToSend);
            $scope.responseToSend = '';

            triviaService.sendAnswer(response, $scope.getCurrentUserName())
            .then(function () {
                
            });
        };

        $scope.senderIsTriviaBot = function (message) {
            return message.Sender === 'TriviaBot';
        }

        $scope.$watch('connectedUsers', function(connUsers) {
            if (connUsers.length > 0) {
                $scope.connectedUsersAreLoading = false;
            }
        });

        $scope.$watch('currentQuestion', function(question) {
            if (question !== '') {
                $scope.noQuestion = false;
            }
        });
    }]);
}).call(this, this.angular, this._);