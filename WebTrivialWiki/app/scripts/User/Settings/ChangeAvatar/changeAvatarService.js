(function (angular, $) {
    'use strict';
    angular.module('webTrivialWikiApp')
        .service('changeAvatarService', ['$http', '$q', function ($http, $q) {

            this.changeAvatar = function(avatar) {
                var def = $q.defer();
                $http({
                    url: App.url + '/changeAvatarAsBase64',
                    method: 'POST',
                    data: avatar
                })
                .success(function (data) {
                    def.resolve(data);
                })
                .error(function (data) {
                    def.reject(data);
                });

                return def.promise;
            }

        }]);
}).call(this, this.angular, this.$);