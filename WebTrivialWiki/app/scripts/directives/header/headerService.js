(function() {
    'use strict';

    App.module.service('headerService', ['$http', '$q', function ($http, $q) {

        this.getNotifications = function () {
            var def = $q.defer();

            $http.get(App.url + '/notifications')
            .success(function (data) {
                def.resolve(data);
            })
            .error(function (data) {
                def.reject(data);
            });

            return def.promise;
        };

        this.markNotificationAsSeen = function (notificationId) {
            var def = $q.defer();

            $http.post(App.url + '/markNotificationsAsSeen/' + notificationId)
            .success(function (data) {
                def.resolve(data);
            })
            .error(function (data) {
                def.reject(data);
            });

            return def.promise;
        }

    }]);


}).call(this);