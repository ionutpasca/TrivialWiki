(function (angular) {
    'use strict';

    angular.module('leaderboardModule', ['ngRoute'])
    .config(function ($routeProvider) {
        $routeProvider
            .when('/leaderboards',
            {
                templateUrl: 'scripts/Leaderboard/leaderboard.html',
                controller: 'leaderboardController'
            });
    });


}).call(this, this.angular);