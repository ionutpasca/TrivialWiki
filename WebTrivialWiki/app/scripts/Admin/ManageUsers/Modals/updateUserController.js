(function(angular, _) {
    'use strict';

    angular.module('adminModule').controller('updateUserController',['$scope', '$uibModalInstance','userToUpdate', 'manageUsersService',
        function ($scope, $modalInstance, userToUpdate, usersService) {

            function getAllRoles() {
                usersService.getAllUserRoles()
                .then(function(data) {
                    $scope.roles = data;
                });
            }

            function init() {
                $scope.user = angular.copy(userToUpdate);
                getAllRoles();

                $scope.emailIsValid = true;
                $scope.pointsAreValid = true;
                $scope.emailExists = false;
            }   

            function validateEmail(email) {
                $scope.emailIsValid = usersService.validateEmail(email);
            }

            function validatePoints(points) {
                $scope.pointsAreValid = /^\d+$/.test(points);
            }

            $scope.updateUser = function () {
                if ($scope.emailIsInvalid) {
                    return;
                }
                if ($scope.user.email !== userToUpdate.email) {
                    usersService.emailExistsInDatabase($scope.user.email)
                        .then(function(data) {
                            $scope.emailExists = data;
                            if (data) {
                                return;
                            } 
                        });
                }
                $modalInstance.close($scope.user);
            };

            $scope.cancel = function () {
                $modalInstance.close(false);
            }

            $scope.$watch('user.email', function (newVal) {
                validateEmail(newVal);
                $scope.emailExists = false;
            });

            $scope.$watch('user.points', function (newVal) {
                $scope.pointsAreValid = true;
                validatePoints(newVal);
            });

            init();
        }]);

}).call(this, this.angular, this._);