(function (angular, moment) {
    'use strict';

    angular.module('webTrivialWikiApp')
        .controller('settingsController', ['$scope', 'userService', 'persistService', 'FileUploader', '$uibModal', function ($scope,userService, persistService, FileUploader, $uibModal) {

            $scope.cropper = {};
            $scope.cropper.sourceImage = null;
            $scope.cropper.croppedImage = null;
            $scope.bounds = {};
            $scope.bounds.left = 0;
            $scope.bounds.right = 0;
            $scope.bounds.top = 0;
            $scope.bounds.bottom = 0;

            function init() {
                $scope.userName = persistService.readData('userName');
                $scope.userRank = persistService.readData('rank');
                $scope.userEmail = persistService.readData('email');

                userService.getUserPoints($scope.userName)
                .then(function(data) {
                    $scope.userPoints = data;
                });

                userService.getAccountCreationDate($scope.userName)
                .then(function (data) {
                    $scope.accountCreationDate = moment(data).format('LL');
                });
            }

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

            $scope.openChangeAvatarModal = function () {
                $uibModal.open({
                    templateUrl: 'scripts/User/Avatar/changeAvatarModal.html',
                    controller: 'changeAvatarController',
                    windowClass: 'change-avatar-modal',
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

            $scope.openChangePasswordModal = function() {
                $uibModal.open({
                    templateUrl: 'scripts/User/Settings/changePasswordModal.html',
                    controller: 'changePasswordController',
                    windowClass: 'change-password-modal',
                    resolve: {
                        userName: function() {
                             return $scope.userName;
                        }
                    }
                });
            };

            $scope.triggerUpload = function () {
                var fileUploader = angular.element('#fileInput');
                fileUploader.trigger('click');
            };

            $scope.signOut = function () {
                persistService.clearLocalStorage();
            };

            $scope.$watch('cropper.sourceImage', function (newVal) {
                if (newVal !== null) {
                    $scope.openChangeAvatarModal();
                }
            });

            init();
        }]);
}).call(this, this.angular, this.moment);