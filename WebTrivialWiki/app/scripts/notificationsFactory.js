(function(angular, $) {
    'use strict';

    App.module
    .factory('notificationsHubFactory', ['$rootScope', function ($rootScope) {
        function notificationsFactory() {

            var connection = $.hubConnection();
            connection.url = 'http://localhost:4605/signalr';
            var proxy = connection.createHubProxy('notificationsHub');

            connection.start().done(function() {});

            return {
                on: function(eventName, callback) {
                    proxy.on(eventName,
                        function(result) {
                            $rootScope.$apply(function() {
                                if (callback) {
                                    callback(result);
                                }
                            });
                        });
                },
                invoke: function(methodName, callback) {
                    proxy.invoke(methodName)
                        .done(function(result) {
                            $rootScope.$apply(function() {
                                if (callback) {
                                    callback(result);
                                }
                            });
                        });
                }
            };
        }

        return notificationsFactory;
    }]);
}).call(this, this.angular, this.$);