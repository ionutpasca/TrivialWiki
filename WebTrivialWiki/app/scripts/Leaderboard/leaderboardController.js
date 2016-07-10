(function (angular) {
    'use strict';

    angular.module('leaderboardModule')
        .controller('leaderboardController', ['leaderboardService', '$scope', function (leaderboardService, $scope) {
            function setFirstThreeUsersOrder(users) {
                var aux = users[0];
                users[0] = users[1];
                users[1] = aux;
                users[0].position = 2;
                users[1].position = 1;
                users[2].position = 3;
                return users;
            }

            function init() {
                $scope.pageNumber = 1;

                leaderboardService.getFirstThreeUsers()
                .then(function (data) {
                    var firstThreeUsers = angular.copy(data);
                    $scope.firstThree = setFirstThreeUsersOrder(firstThreeUsers);
                });

                leaderboardService.getLeaderboardUsers($scope.pageNumber)
                .then(function (data) {
                    $scope.leaderboardUsers = data;
                });
            }

            init();
        }]);

}).call(this, this.angular);