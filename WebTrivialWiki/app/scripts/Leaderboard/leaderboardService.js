(function(angular) {
    'use strict';

    angular.module('leaderboardModule')
    .service('leaderboardService',['$q', '$http', function($q, $http) {
            
        this.getFirstThreeUsers = function() {
            var def = $q.defer();

            $http.get(App.url + 'leaderBoard/firstThree')
            .success(function(data) {
                def.resolve(data);
            }).error(function(data) {
                def.reject(data);
            });
            return def.promise;
        };

        this.getLeaderboardUsers = function(pageNumber) {
            var def = $q.defer();

            $http.get( App.url + 'leaderBoard/' + pageNumber)
            .success(function (data) {
                def.resolve(data);
            }).error(function (data) {
                def.reject(data);
            });
            return def.promise;
        };

    }]);

}).call(this, this.angular);