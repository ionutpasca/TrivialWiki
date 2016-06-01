(function(angular) {
    'use strict';
    angular.module('triviaModule').service('commonService',['$q','$http',function($q, $http) {
        
        this.getUserAvatar = function(username) {
            var def = $q.defer();

            $http.get(App.url + 'avatar/' + username)
            .success(function(data) {
                    def.resolve(data);
            }).error(function(data) {
                def.reject(data);
            });

            return def.promise;
        }
    }]);

}).call(this, this.angular);