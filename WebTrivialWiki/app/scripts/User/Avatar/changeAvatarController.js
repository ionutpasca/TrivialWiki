(function (angular) {
    'use strict';
    angular.module('webTrivialWikiApp')
        .controller('changeAvatarController', ['$scope', 'persistService', '$uibModalStack', 'changeAvatarService', 'cropper', 'bounds', function ($scope, persistService, $uibModalStack, changeAvatarService, cropper, bounds) {
            $scope.cropper = cropper;
            $scope.bounds = bounds;

            function disposeElements() {
                cropper = '';
                bounds = '';
            }

            $scope.uploadAvatar = function () {
                $scope.avatarIsSaving = true;
                var avatarAsBase64 = $scope.cropper.croppedImage.replace('data:image/png;base64,', '');
                changeAvatarService.changeAvatar(avatarAsBase64)
                .then(function () {
                    $scope.avatarIsSaving = false;
                        disposeElements();
                    $uibModalStack.dismissAll();
                });
            };

        }]);
}).call(this, this.angular);