(function (angular, _) {
    'use strict';
    angular.module('triviaModule')
    .controller('triviaController', ['$scope', '$rootScope', 'commonService',
        'signalRFactory', 'chatService', 'persistService', function ($scope, $rootScope,commonService, signalR, chatService, persistService) {
        $scope.messages = [];
        $scope.text = "";
        $scope.skip = 0;

        function init() {
            chatService.getMessages($scope.skip)
            .then(function(data) {
                _.each(data, function (message) {
                    $scope.messages.push(message);
                });
            });
        }

        signalR.on('addMessage', function (message) {
            $scope.messages.push(message);
        });

        $scope.sendMessage = function () {
            if ($scope.message === undefined || $scope.message === '') {
                return;
            }
            var UserName = persistService.readData('userName');
            var Message = $scope.message;
            chatService.sendMessage({ Message, UserName})
            .then(function() {
                //something
            });
        };

        init();
    }]);
}).call(this, this.angular, this._);