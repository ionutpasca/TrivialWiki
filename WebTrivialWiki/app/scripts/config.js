'use strict';

App.module.service('APIInterceptor', ['$rootScope', function ($rootScope) {
    var service = this;

    service.request = function(config) {
        //var currentUser = userService.getCurrentUser();
        //var securityToken = currentUser ? currentUser.SecurityToken : null;

        //if (securityToken) {
        //    config.headers.authorization = securityToken;
        //}
        return config;
    };

    service.responseError = function(response) {
        if (response.status === 401) {
            //$rootScope.$broadcast('unauthorized');
        }
        return response;
    }
}]);