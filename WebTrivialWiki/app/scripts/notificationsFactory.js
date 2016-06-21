(function(angular, $) {
    'use strict';

    App.module
    .factory('notificationsHubFactory', ['$rootScope', function ($rootScope) {
        //return {
        //    on: function (eventName, callback) {
        //        var connection = $.hubConnection();
        //        connection.url = 'http://localhost:4605/signalr';
        //        var triviaMessageHubProxy = connection.createHubProxy('notificationsHub');

        //        triviaMessageHubProxy.on(eventName,
        //            function () {
        //                var args = arguments;
        //                $rootScope.$apply(function () {
        //                    callback.apply(triviaMessageHubProxy, args);
        //                });
        //            });

        //        $.signalR.connectionState = { connecting: 0, connected: 1, reconnecting: 2, disconnected: 4 };

        //        if (connection.state === 4) {
        //            //if ($.connection.hub && $.connection.hub.state === $.signalR.connectionState.disconnected) {
        //            connection.start().done(function() {});
        //        }
        //        //}
        //    }
        //};
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