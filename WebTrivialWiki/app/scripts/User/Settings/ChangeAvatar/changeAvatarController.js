(function (angular) {
    'use strict';
    angular.module('webTrivialWikiApp')
        .controller('changeAvatarController', ['$scope', 'persistService', '$uibModalStack', 'changeAvatarService', 'cropper', 'bounds', function ($scope, persistService, $uibModalStack, changeAvatarService, cropper, bounds) {
            $scope.cropper = cropper;
            $scope.bounds = bounds;

            $scope.uploadAvatar = function () {
                var avatarAsBase64 = $scope.cropper.croppedImage.replace('data:image/png;base64,', '');
                persistService.storeData('avatar', avatarAsBase64);
                changeAvatarService.changeAvatar(avatarAsBase64)
                .then(function() {
                    $uibModalStack.dismissAll();
                });
            };

        }]);
}).call(this, this.angular);