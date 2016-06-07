(function(angular) {
    'use strict';

    angular.module('triviaModule')
    .service('triviaService', ['$q', '$http', function ($q, $http) {

        this.getMessages = function(skip) {
            var def = $q.defer();
            $http.get(App.url + '/getMessages/' + skip)
                .success(function(data) {
                    def.resolve(data);
                })
                .error(function(data) {
                    def.reject(data);
                });

            return def.promise;
        };

        this.sendMessage = function(message, sender) {
            var def = $q.defer();
            var param = {
                Message: message,
                UserName: sender
            }
            $http({
                url: App.url + '/addMessage',
                method: 'POST',
                params: param
            })
           .success(function (data) {
               def.resolve(data);
           })
           .error(function (data) {
               def.reject(data);
           });

            return def.promise;
        };

        this.getTriviaHistory = function () {
            var def = $q.defer();
            $http.get(App.url + '/getLastQuestions')
                .success(function (data) {
                    def.resolve(data);
                })
                .error(function (data) {
                    def.reject(data);
                });

            return def.promise;
        };

        this.sendAnswer = function(message, sender) {
            var def = $q.defer();
            var param = {
                messageText: message,
                Sender: sender
            }
            $http({
                url: App.url + '/addResponse',
                method: 'POST',
                params: param
            })
           .success(function (data) {
               def.resolve(data);
           })
           .error(function (data) {
               def.reject(data);
           });

            return def.promise;
        }

    }]);
}).call(this, this.angular);