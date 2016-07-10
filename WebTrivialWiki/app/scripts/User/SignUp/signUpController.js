(function (_) {
    'use strict';

    App.module.controller('signUpController', ['$scope', 'signUpService','persistService', 'loginService', '$location', '$uibModalStack','$window',
                function ($scope, signUpService, persistService,loginService, $location, $modalInstance, $window) {

        $scope.passwordsDontMatch = function () {
            var pass = $scope.password ? $scope.password : '';
            var repeatedPass = $scope.repeatedPassword ? $scope.repeatedPassword : '';
            if (pass !== '' && repeatedPass !== '') {
                return pass !== repeatedPass;
            }
            return false;
        };

        $scope.registerNewAccount = function () {
            $scope.accountIsSaving = true;
            if ($scope.passwordsDontMatch()) {
                return;
            }
            var newUser = {
                UserName: $scope.username,
                Password: $scope.password,
                Email: $scope.email,
            };
            var params = {
                Username: $scope.username,
                Password: $scope.password
            };
            signUpService.registerNewUser(newUser)
            .then(function () {
                loginService.login(params)
                .then(function(data) {
                    $modalInstance.dismissAll();

                    _.map(data, function (value, key) {
                        persistService.storeData(key, value);
                    });
                    persistService.storeData('isLoggedIn', true);
                    $modalInstance.dismissAll();
                    $scope.accountIsSaving = false;

                    $window.location.reload();
                });
            });
        };

    }]);

}).call(this, this._);