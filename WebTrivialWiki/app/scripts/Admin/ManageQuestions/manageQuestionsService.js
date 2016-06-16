(function (angular) {
    'use strict';

    angular.module('adminModule')
        .service('manageQuestionsService', ['$q', '$http', function($q, $http) {
            
            this.getTopicNames = function() {
                var def = $q.defer();

                $http.get(App.url + '/topicNames')
                .success(function(data) {
                    def.resolve(data);
                })
                .error(function(data) {
                    def.reject(data);
                });

                return def.promise;
            }

            this.getTopicsWithoutQuestions = function() {
                var def = $q.defer();
                $http.get(App.url + '/topicsWithoutQuestions')
               .success(function (data) {
                   def.resolve(data);
               })
               .error(function (data) {
                   def.reject(data);
               });

                return def.promise;
            }

            this.getQuestionsForTopic = function(topic) {
                var def = $q.defer();
                $http.get(App.url + '/questions/' + topic)
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