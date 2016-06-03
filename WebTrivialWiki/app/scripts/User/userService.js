(function () {
    'use strict';

    App.module.service('userService', ['$q','$http', function ($q, $http) {

        this.getAccountCreationDate = function(username) {
            var def = $q.defer();

            $http.get(App.url + 'accountCreationDate/' + username)
            .success(function(data) {
                def.resolve(data);
            })
            .error(function(data) {
                def.reject(data);
            });
            return def.promise;
        };

        this.getUserPoints = function(username) {
            var def = $q.defer();

            $http.get(App.url + 'userPoints/' + username)
            .success(function (data) {
                def.resolve(data);
            })
            .error(function (data) {
                def.reject(data);
            });
            return def.promise;
        };

        this.changePassword = function(username, oldPass, newPass) {
            var def = $q.defer();

            $http.post(App.url + 'changePassword/' + username + '/' + oldPass + '/' + newPass)
            .success(function (data) {
                def.resolve(data);
            })
            .error(function (data) {
                def.reject(data);
            });
            return def.promise;
        };
    }]);
}).call(this);