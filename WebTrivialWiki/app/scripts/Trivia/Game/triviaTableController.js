(function (angular, _) {
    'use strict';
    angular.module('triviaModule')
    .controller('triviaTableController', ['$scope', 'triviaFactory', 'triviaService', '$routeParams', 'persistService', '$interval', '$window',
        function ($scope, triviaFactory, triviaService, $routeParams, persistService, $interval, $window) {

        $scope.tableName = $routeParams.tableName;
        var triviaProxy = triviaFactory($scope.tableName);

        function getTableTopic(tableName) {
            triviaService.getTableTopic(tableName)
                .then(function(data) {
                    $scope.tableTopic = data;
                });
        }

        function getUsersFromTable(tableName) {
            triviaService.getTableUsers(tableName)
                .then(function (data) {
                    $scope.connectedUsers = data;
                });
        }

        function getQuestion(tableName) {
            triviaService.getCurrentQuestion(tableName)
               .then(function (data) {
                    if ($scope.currentQuestion === '') {
                        $scope.currentQuestion = data;
                    }
                });
        }

        function resetMessages() {
            $scope.messages = [];
        }

        function increaseDeterminateValue() {
            $scope.determinateValue = 0;
            if ($scope.determinateValue !== 0) {
                $scope.determinateValue = 0;
            } else
            {
                $interval(function () {
                    $scope.determinateValue += 1;
                    if ($scope.determinateValue === 100) {
                        $scope.currentQuestion = $scope.nextQuestion;
                        resetMessages();
                        $scope.newQuestionIsLoading = false;
                        return;
                    }
                }, 50, 0, true);
            }
        }

        function init() {
            $scope.connectedUsersAreLoading = true;
            $scope.noQuestion = true;
            $scope.newQuestionIsLoading = false;

            

            $scope.connectedUsers = [];
            $scope.currentQuestion = '';

            getTableTopic($scope.tableName);
            getUsersFromTable($scope.tableName);
            getQuestion($scope.tableName);
            resetMessages();
        }
        init();


        triviaProxy.on('addQuestion', function (res) {
            var question = {
                messageText: res.MessageText,
                sender: res.Sender
            };
            if ($scope.currentQuestion === '') {
                $scope.currentQuestion = question;
            } else {
                $scope.nextQuestion = question;
                $scope.determinateValue = 0;

                if ($scope.newQuestionIsLoading) {
                    increaseDeterminateValue();
                }
            }
        });

        triviaProxy.on('addMessage', function (res) {
            $scope.messages.push(res);
        });

        triviaProxy.on('sendConnectedUsers', function (res) {
            $scope.connectedUsers = res;
        });

        triviaProxy.on('newUserConnected', function (res) {
            $scope.connectedUsers.push(res);
        });
        
        triviaProxy.on('correctAnswer', function (res) {
            $scope.messages.push(res);
            $scope.newQuestionIsLoading = true;
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

        $scope.getNextQuestion = function () {
            $scope.newQuestionIsLoading = true;
            $scope.determinateValue = 0;

            triviaService.getNextQuestion()
            .then(function() {});
        };

        $scope.senderIsTriviaBot = function (message) {
            return message.Sender === 'TriviaBot';
        }

        $scope.leaveTable= function() {
            $window.location.href = '#/trivia';
            $window.location.reload();
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