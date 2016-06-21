(function (angular) {
    'use strict';

    angular.module('adminModule')
       .service('manageTopicsService', ['$q', '$http', function($q, $http) {

        this.getProposedTopics = function () {
            var def = $q.defer();

            $http.get(App.url + '/getProposedTopics')
            .success(function (data) {
                def.resolve(data);
            })
            .error(function (data) {
                def.reject(data);
            });

            return def.promise;
        }

        this.processTopic = function (topicName) {
            var def = $q.defer();

            $http.post(App.url + '/processTopic/' + topicName)
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