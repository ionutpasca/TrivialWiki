"use strict";

App.module.service('loginService', ['$http', '$q', function ($http, $q) {
    this.login = function (params) {
        var def = $q.defer();

        $http({
            url: App.url + '/login',
            method: 'GET',
            params: params
        })
        .success(function (data) {
            var user = {
                userName: data.userName,
                securityToken: data.securityToken,
                roles: data.roles,
                firstName: data.firstName,
                lastName: data.lastName,
                avatar: data.avatar,
                rank: data.rank,
                email: data.email
            };

            def.resolve(user);
        })
        .error(function (data, status) {
            def.reject({ status: status });
        });

        return def.promise;
    };

}]);