(function (angular, _) {
    'use strict';
    angular.module('triviaModule')
    .controller('triviaController', ['$scope', '$rootScope',
        'chatFactory', 'triviaFactory', 'chatService', 'persistService', function ($scope, $rootScope, chatFactory, triviaFactory, chatService, persistService) {
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

        chatFactory.on('addMessage', function (msg) {
            var newMessage = {
                userName: msg.UserName,
                message: msg.Message
            };
            $scope.messages.unshift(newMessage);
        });

        chatFactory.on('onConnected', function (res) {
            debugger;
        });

        chatFactory.on('someoneConnected', function (res) {
            debugger;
        });

        triviaFactory.on('addResponse', function(msg) {
            //var newResponse = {
            //    userName: msg.UserName,
            //    message: msg.Message
            //};
            debugger;
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