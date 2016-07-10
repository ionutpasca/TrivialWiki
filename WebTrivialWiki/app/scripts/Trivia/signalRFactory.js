(function(angular, $) {
    'use strict';

    angular.module('triviaModule')
    //.factory('chatFactory', ['$rootScope', function ($rootScope) {
    //    function chatFactory() {

    //        var connection = $.hubConnection();
    //        connection.url = 'http://localhost:4605/signalr';
    //        var proxy = connection.createHubProxy('chatHub');

    //        connection.start().done(function () { });

    //        return {
    //            on: function (eventName, callback) {
    //                proxy.on(eventName,
    //                function (result) {
    //                    $rootScope.$apply(function () {
    //                        if (callback) {
    //                            callback(result);
    //                        }
    //                    });
    //                });
    //            },
    //            invoke: function (methodName, callback) {
    //                proxy.invoke(methodName)
    //                .done(function (result) {
    //                    $rootScope.$apply(function () {
    //                        if (callback) {
    //                            callback(result);
    //                        }
    //                    });
    //                });
    //            }
    //        };
    //    }

    //    return chatFactory;
    //}])
    .factory('triviaFactory',['$rootScope', function($rootScope) {
        function triviaFactory(tableName) {
            var connection = $.hubConnection();
            connection.url = 'http://localhost:4605/signalr';
            connection.qs = { 'tableName': tableName };

            var proxy = connection.createHubProxy('triviaHub');

            connection.start().done(function () { });

            return {
                on: function (eventName, callback) {
                    proxy.on(eventName,
                    function (result) {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback(result);
                            }
                        });
                    });
                },
                invoke: function (methodName, callback) {
                    proxy.invoke(methodName)
                    .done(function (result) {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback(result);
                            }
                        });
                    });
                }
            };
        }

        return triviaFactory;
    }]);
}).call(this,this.angular, this.$);