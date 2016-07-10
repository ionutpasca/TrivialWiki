(function (angular) {
    'use strict';
    angular.module('triviaModule')
    .controller('addFriendController', ['$mdDialog', '$scope', 'triviaService', '$window',
        function ($mdDialog, $scope, triviaService) {

            $scope.addFriend = function () {
                $scope.requestIsPending = true;
                triviaService.addNewFriend($scope.friendName)
                .then(function () {
                    $scope.friendName = '';
                    $scope.requestIsPending = false;
                    $mdDialog.cancel();
                });
            };

        }]);

}).call(this, this.angular);