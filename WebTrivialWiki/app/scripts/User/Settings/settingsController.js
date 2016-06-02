(function (angular) {
    'use strict';

    angular.module('webTrivialWikiApp')
        .controller('settingsController', ['$scope', 'persistService','FileUploader','$uibModal', function ($scope, persistService, FileUploader,$uibModal) {

            $scope.cropper = {};
            $scope.cropper.sourceImage = null;
            $scope.cropper.croppedImage = null;
            $scope.bounds = {};
            $scope.bounds.left = 0;
            $scope.bounds.right = 0;
            $scope.bounds.top = 0;
            $scope.bounds.bottom = 0;

            //var uploader = $scope.uploader = new FileUploader({
            //    url: App.url + 'changeAvatar',
            //    headers: {
            //        'Authorization': getSecurityToken()
            //    }
            //});

            //uploader.filters.push({
            //    name: 'customFilter',
            //    fn: function (item /*{File|FileLikeObject}*/, options) {
            //        return this.queue.length < 10;
            //    }
            //});

            $scope.getCurrentUserName = function () {
                return persistService.readData('userName');
            };

            $scope.openChangeAvatarModal = function () {
                $uibModal.open({
                    templateUrl: 'scripts/User/Settings/ChangeAvatar/changeAvatarModal.html',
                    controller: 'changeAvatarController',
                    resolve: {
                        cropper: function () {
                            return $scope.cropper;
                        },
                        bounds: function() {
                            return $scope.bounds;
                        }
                    }
                });
            };

            $scope.triggerUpload = function () {
                var fileUploader = angular.element('#fileInput');
                fileUploader.trigger('click');
            };

            $scope.userHasAvatar = function() {
                //$scope.avatar = persistService.readData('avatar');
                //return $scope.avatar !== null;
            };

            $scope.signOut = function () {
                persistService.clearLocalStorage();
            };

            //uploader.onAfterAddingFile = function () {
            //    uploader.queue[0].upload();
            //};

            $scope.$watch('cropper.sourceImage', function (newVal) {
                if (newVal !== null) {
                    $scope.openChangeAvatarModal();
                }
            });
            
        }]);
}).call(this, this.angular);