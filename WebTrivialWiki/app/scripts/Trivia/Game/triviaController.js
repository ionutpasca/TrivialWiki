(function (angular, _) {
    'use strict';
    angular.module('triviaModule')
    .controller('triviaController', ['$scope', '$rootScope',
        'chatFactory', 'triviaFactory', 'triviaService', 'persistService', function ($scope, $rootScope, chatFactory, triviaFactory, triviaService, persistService) {
        $scope.messages = [];
        $scope.triviaQuestions = [];
        $scope.text = "";
        $scope.skip = 0;
        $scope.responseToSend = '';

        function initializeTriviaChat() {
            $scope.messagesAreLoading = true;

            triviaService.getMessages($scope.skip)
            .then(function (data) {
                _.each(data, function (message) {
                    $scope.messages.push(message);
                });
                $scope.messagesAreLoading = false;
            });
        }

        function intializeTriviaHistory() {
            $scope.triviaHistoryIsLoading = true;

            triviaService.getTriviaHistory()
            .then(function (data) {
                _.each(data, function (message) {
                    var historyQuestion = {
                        MessageText: message.messageText,
                        Sender: message.sender
                    };
                    $scope.triviaQuestions.unshift(historyQuestion);
                });
                $scope.triviaHistoryIsLoading = false;
            });
        }

        function init() {
            initializeTriviaChat();
            intializeTriviaHistory();
        }

        triviaFactory.on('addMessage', function (question) {
            $scope.triviaQuestions.push(question);
        });

        chatFactory.on('addMessage', function (msg) {
            var newMessage = {
                userName: msg.UserName,
                message: msg.Message
            };
            $scope.messages.unshift(newMessage);
        });

        chatFactory.on('onConnected', function (res) {
        });

        chatFactory.on('someoneConnected', function (res) {
        });

        triviaFactory.on('addResponse', function(msg) {
            //var newResponse = {
            //    userName: msg.UserName,
            //    message: msg.Message
            //};
        });

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

        $scope.senderIsTriviaBot = function(question) {
            return question.Sender === 'TriviaBot';
        }

        $scope.addNewTopic = function () {
            triviaService.addNewTopic('Superman')
                .then(function() {
                    debugger;
                });
        }

        init();
    }]);
}).call(this, this.angular, this._);