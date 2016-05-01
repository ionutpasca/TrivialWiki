(function(angular) {
    'use strict';

    angular.module('adminModule')
    .service('manageUsersService',['$q','$http', function($q, $http) {
          
        this.addNewUserInDatabase = function (user) {
            var def = $q.defer();

            $http({
                url: App.url + '/addNewUser',
                method: 'POST',
                params: user
            })
            .success(function (data) {
                def.resolve(data);
            })
            .error(function (error) {
                def.reject(error);
            });
            return def.promise;
        }

        this.getUserBatch = function(pageNumber) {
            var def = $q.defer();
            $http.get(App.url + '/getUserBatch/' + pageNumber)
                .success(function (data) {
                    def.resolve(data);
                })
                .error(function (error) {
                    def.reject(error);
                });
            return def.promise;
        }

        this.removeUser = function(username) {
            var def = $q.defer();

            $http.post(App.url + '/removeUser/' + username)
                .success(function(data) {
                    def.resolve(data);
                })
                .error(function(error) {
                    def.reject(error);
                });
            return def.promise;
        }

        this.updateUser = function(user) {
            var def = $q.defer();

            $http({
                url: App.url + '/updateUser',
                method: 'POST',
                params: user
            })
            .success(function (data) {
                def.resolve(data);
            })
            .error(function (error) {
                def.reject(error);
            });
            return def.promise;
        }

        this.emailExistsInDatabase = function(email) {
            var def = $q.defer();

            $http.get(App.url + '/emailExists/' + email)
            .success(function(data) {
                def.resolve(data);
            })
            .error(function(error) {
                def.reject(error);
            });
            return def.promise;
        }

        this.usernameExistsInDatabase = function (username) {
            var def = $q.defer();

            $http.get(App.url + '/usernameExists/' + username)
            .success(function (data) {
                def.resolve(data);
            })
            .error(function (error) {
                def.reject(error);
            });
            return def.promise;
        }

        this.getAllUserRoles = function() {
            var def = $q.defer();

            $http.get(App.url + '/roles')
            .success(function(data) {
                def.resolve(data);
            })
            .error(function(data) {
                def.reject(data);
            });
            return def.promise;
        }

        this.getNumberOfUsers = function() {
            var def = $q.defer();

            $http.get(App.url + '/getNumberOfUsers')
            .success(function (data) {
                def.resolve(data);
            })
            .error(function (data) {
                def.reject(data);
            });
            return def.promise;
        }

        this.validateEmail = function (email) {
            var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        };

    }]);

}).call(this, this.angular);