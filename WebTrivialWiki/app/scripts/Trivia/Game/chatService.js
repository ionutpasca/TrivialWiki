(function(angular) {
    'use strict';

    angular.module('triviaModule')
    .service('chatService', ['$q', '$http', function ($q, $http) {

        this.sendMessage = function(message) {
            var def = $q.defer();
            $http({
                url: App.url + '/addMessage',
                method: 'POST',
                params: message
            })
           .success(function (data) {
               def.resolve(data);
           })
           .error(function (data) {
               def.reject(data);
           });

            return def.promise;
        };

    }]);
}).call(this, this.angular);