(function (angular, _) {
    'use strict';
    angular.module('triviaModule')
    .controller('triviaController', ['$scope', '$rootScope',
        'signalRFactory', 'chatService', 'persistService', function ($scope, $rootScope, signalR, chatService, persistService) {
        $scope.messages = [];
        $scope.text = "";
        $scope.skip = 0;

        function init() {
            chatService.getMessages($scope.skip)
            .then(function (data) {
                _.each(data, function (message) {
                    $scope.messages.push(message);
                });
            });
        }

        signalR.on('addMessage', function (msg) {
            var newMessage = {
                userName: msg.UserName,
                message: msg.Message
            };
            $scope.messages.unshift(newMessage);
        });

        $scope.getCurrentUserName = function() {
            return persistService.readData('userName');
        };

        $scope.sendMessage = function () {
            if ($scope.messageToSend === undefined || $scope.messageToSend === '') {
                return;
            }
            var UserName = $scope.getCurrentUserName();
            var Message = $scope.messageToSend;
            chatService.sendMessage({ Message, UserName})
            .then(function () {
                $scope.messageToSend = null;
            });
        };

        init();
    }]);
}).call(this, this.angular, this._);