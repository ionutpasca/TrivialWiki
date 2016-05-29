(function(angular) {
    angular.module('webTrivialWikiApp')
        .controller('settingsController', ['$scope', 'persistService','FileUploader', function ($scope, persistService, FileUploader) {
            var uploader = $scope.uploader = new FileUploader({
                url: App.url + 'changeAvatar',
                headers: {
                    'Authorization': getSecurityToken()
                }
            });

            uploader.filters.push({
                name: 'customFilter',
                fn: function (item /*{File|FileLikeObject}*/, options) {
                    return this.queue.length < 10;
                }
            });

            function getSecurityToken() {
                return persistService.readData('securityToken');
            }

            $scope.triggerUpload = function() {
                var fileUploader = angular.element('#fileInput');
                fileUploader.trigger('click');
            };

            $scope.userHasAvatar = function() {
                $scope.avatar = persistService.readData('avatar');
                return $scope.avatar !== null;
            };

            $scope.signOut = function () {
                persistService.clearLocalStorage();
            };

            uploader.onAfterAddingFile = function () {
                uploader.queue[0].upload();
            };
    }]);
}).call(this, this.angular);