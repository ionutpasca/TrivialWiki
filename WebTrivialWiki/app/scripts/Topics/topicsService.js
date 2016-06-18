(function () {
    'use strict';

    App.module.service('topicsService', ['$http', '$q', function ($http, $q) {

        this.getTopics = function () {
            var def = $q.defer();

            $http.get(App.url + 'detailedTopics')
            .success(function (data) {
                def.resolve(data);
            }).error(function (data) {
                def.reject(data);
            });
            return def.promise;
        };

        this.proposeTopic = function (topic) {
            var def = $q.defer();

            $http.post(App.url + 'proposeTopic/' + topic)
            .success(function (data) {
                def.resolve(data);
            }).error(function (data, status) {
                    debugger;
                def.reject(data);
            });
            return def.promise;
        };

    }]);
}).call(this);