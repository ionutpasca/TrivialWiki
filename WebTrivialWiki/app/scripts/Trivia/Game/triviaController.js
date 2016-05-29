(function (angular) {
    'use strict';
    angular.module('triviaModule')
    .controller('triviaController', ['$scope', '$rootScope', 'signalRFactory', 'chatService','persistService', function ($scope, $rootScope, signalR, chatService, persistService) {
        $scope.messages = [];
        $scope.text = "";

        signalR.on('addMessage', function (message) {
            $scope.messages.push(message);
        });

        $scope.sendMessage = function () {
            var UserName = persistService.readData('userName');
            var Message = $scope.message;
            chatService.sendMessage({ Message, UserName})
            .then(function() {
                //something
            });
        };
    }]);
}).call(this, this.angular);