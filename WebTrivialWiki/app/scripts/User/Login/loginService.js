(function () {
    'use strict';

    App.module.service('loginService', ['$http', '$q', function ($http, $q) {
        this.login = function (params) {
            var def = $q.defer();

            $http({
                url: App.url + '/login',
                method: 'GET',
                params: params
            })
            .success(function (data) {
                    debugger;
                var user = {
                    userName: data.UserName,
                    securityToken: data.SecurityToken,
                    role: data.Role,
                    firstName: data.FirstName,
                    lastName: data.LastName,
                    avatar: data.Avatar,
                    rank: data.Rank,
                    email: data.Email
                };

                def.resolve(user);
            })
            .error(function (data, status) {
                def.reject({ status: status });
            });

            return def.promise;
        };

    }]);
}).call(this);