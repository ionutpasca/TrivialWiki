(function () {
    'use strict';

    App.module.controller('signUpController', ['$scope', 'signUpService', '$location', '$uibModalInstance',
                function ($scope, signUpService, $location, $modalInstance) {

        $scope.passwordsDontMatch = function () {
            var pass = $scope.password ? $scope.password : '';
            var repeatedPass = $scope.repeatedPassword ? $scope.repeatedPassword : '';
            if (pass !== '' && repeatedPass !== '') {
                return pass !== repeatedPass;
            }
            return false;
        };

        $scope.registerNewAccount = function () {
            if ($scope.passwordsDontMatch()) {
                return;
            }
            var newUser = {
                UserName: $scope.username,
                Password: $scope.password,
                Email: $scope.email,
            };
            signUpService.registerNewUser(newUser)
            .then(function () {
                $location.path('/login');
            });
        };

        $scope.closeRegisterModal = function () {
            $modalInstance.close();
        }

    }]);

}).call(this);