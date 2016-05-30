(function (angular, $) {
    'use strict';
    angular.module('webTrivialWikiApp')
        .service('changeAvatarService', ['$http', '$q', function ($http, $q) {

            this.changeAvatar = function(avatar) {
                var def = $q.defer();
                var img = JSON.stringify(avatar);
                //$.ajax({
                //    url: App.url + '/changeAvatarAsBase64',
                //    dataType: 'json',
                //    data: img,
                //    type: 'POST',
                //    success: function (data) {
                //        debugger;
                //        console.log(data);
                //    }
                //});

                $http({
                    url: App.url + '/changeAvatarAsBase64',
                    method: 'POST',
                    data: img
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