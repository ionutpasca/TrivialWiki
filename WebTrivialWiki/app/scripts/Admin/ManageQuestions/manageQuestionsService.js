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
                $http.get(App.url + '/inactiveTopics')
               .success(function (data) {
                   def.resolve(data);
               })
               .error(function (data) {
                   def.reject(data);
               });

                return def.promise;
            }

            this.getInactiveQuestionsForTopic = function(topic) {
                var def = $q.defer();
                $http.get(App.url + '/inactiveQuestions/' + topic)
               .success(function (data) {
                   def.resolve(data);
               })
               .error(function (data) {
                   def.reject(data);
               });

                return def.promise;
            }

            this.getActiveQuestionsForTopic = function (topic) {
                var def = $q.defer();
                $http.get(App.url + '/activeQuestions/' + topic)
               .success(function (data) {
                   def.resolve(data);
               })
               .error(function (data) {
                   def.reject(data);
               });

                return def.promise;
            }

            this.updateQuestions = function(questions) {
                var def = $q.defer();

                $http({
                    url: App.url + '/updateQuestions',
                    method: 'POST',
                    data: questions
                })
                .success(function (data) {
                    def.resolve(data);
                })
                .error(function (data) {
                    def.reject(data);
                });

                return def.promise;
            }

            this.deleteQuestions = function(questions) {
                var def = $q.defer();

                $http({
                    url: App.url + '/deleteQuestions',
                    method: "POST",
                    data: questions
                })
                .success(function (data) {
                    def.resolve(data);
                })
                .error(function(data) {
                    def.reject(data);
                });

                return def.promise;
            }

            this.enableTopic = function(topicName) {
                var def = $q.defer();

                $http.post(App.url + '/enableTopic/' + topicName)
                .success(function(data) {
                    def.resolve(data);
                }).error(function(data) {
                    def.reject(data);
                });

                return def.promise;
            }

        }]);
}).call(this, this.angular);