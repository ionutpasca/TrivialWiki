(function($) {
    'use strict';

    angular.module('triviaModule').factory('signalRFactory',['$rootScope', function($rootScope) {
        return {
            on: function(eventName, callback) {
                var connection = $.hubConnection();
                connection.url = 'http://localhost:4605/signalr';
                var triviaMessageHubProxy = connection.createHubProxy('chatHub');

                triviaMessageHubProxy.on(eventName,
                    function() {
                        var args = arguments;
                        $rootScope.$apply(function () {
                            callback.apply(triviaMessageHubProxy, args);
                        });
                    });

                connection.start().done(function() {});
            }
        };
    }]);
}).call(this, this.$);