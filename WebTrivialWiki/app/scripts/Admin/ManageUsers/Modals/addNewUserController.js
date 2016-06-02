(function (angular) {
    'use strict';

    angular.module('adminModule').controller('addNewUserController', ['$scope', 'manageUsersService', '$uibModalInstance',
    function ($scope, usersService, $modalInstance) {

        function init() {
            $scope.user = {
                username: '',
                email: '',
                password: ''
            };
            $scope.usernameExists = false;
            $scope.emailIsValid = true;
            $scope.emailExists = false;
        }

        function validateEmail(email) {
            $scope.emailIsValid = usersService.validateEmail(email);
        }

        $scope.saveUser = function () {
            if (!$scope.emailIsValid) {
                return;
            }

            usersService.emailExistsInDatabase($scope.user.email)
             .then(function (data) {
                 $scope.emailExists = data;
                 if (data) return;
             })
            .then(function () {
                usersService.usernameExistsInDatabase($scope.user.username)
                .then(function (data) {
                    $scope.usernameExists = data;
                    if (data) return;
                })
                .then(function () {
                    if (!$scope.usernameExists && !$scope.emailExists) {
                        $modalInstance.close($scope.user);
                    }
                });
            });
        };

        $scope.cancel = function () {
            $modalInstance.close(false);
        }

        $scope.$watch('user.email', function (newVal) {
            validateEmail(newVal);
            $scope.emailExists = false;
        });

        $scope.$watch('user.username', function (newVal) {
            $scope.usernameExists = false;
        });

        init();
    }]);

}).call(this, this.angular);