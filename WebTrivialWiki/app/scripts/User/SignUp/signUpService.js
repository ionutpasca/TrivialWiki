(function () {

    'use strict';

    App.module.service('signUpService', ['$http', '$q', function ($http, $q) {

        this.registerNewUser = function (params) {
            var def = $q.defer();

            $http({
                url: App.url + '/addNewUser',
                method: 'POST',
                params: params
            })
            .success(function (data) {
                def.resolve(data);
            })
            .error(function (data, status) {
                def.reject({ status: status });
            });

            return def.promise;
        }

    }]);

}).call(this);