(function (angular, _) {
    'use strict';
    angular.module('triviaModule')
    .controller('triviaController', ['$scope', '$rootScope','triviaFactory', 'triviaService', 'persistService','$location',
         function ($scope, $rootScope, triviaFactory, triviaService, persistService,$location) {
        $scope.messages = [];
        $scope.triviaQuestions = [];
        $scope.text = "";
        $scope.skip = 0;
        $scope.responseToSend = '';

        var triviaTableProxy;
        //function initializeTriviaChat() {
        //    $scope.messagesAreLoading = true;

        //    triviaService.getMessages($scope.skip)
        //    .then(function (data) {
        //        _.each(data, function (message) {
        //            $scope.messages.push(message);
        //        });
        //        $scope.messagesAreLoading = false;
        //    });
        //}

        //function intializeTriviaHistory() {
        //    $scope.triviaHistoryIsLoading = true;

        //    triviaService.getTriviaHistory()
        //    .then(function (data) {
        //        _.each(data, function (message) {
        //            var historyQuestion = {
        //                MessageText: message.messageText,
        //                Sender: message.sender
        //            };
        //            $scope.triviaQuestions.unshift(historyQuestion);
        //        });
        //        $scope.triviaHistoryIsLoading = false;
        //    });
        //}


        function getActiveTables() {
            triviaService.getTriviaTables()
            .then(function (data) {
                $scope.tables = data;
            });
        }

        function init() {
            getActiveTables();
            //initializeTriviaChat();
            //intializeTriviaHistory();
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


        $scope.joinTable = function (table) {
            $location.url('/trivia/' + table.tableName);
        }

        $scope.senderIsTriviaBot = function(question) {
            return question.Sender === 'TriviaBot';
        }

        init();
    }]);
}).call(this, this.angular, this._);