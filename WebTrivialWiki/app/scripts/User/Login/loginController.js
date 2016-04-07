(function () {
    'use strict';

    App.module.controller('loginController', ['$scope', 'loginService', '$location', 'persistService', '$uibModal','$uibModalInstance',
            function ($scope, loginService, $location, persistService, $modal, $modalInstance) {

        $scope.login = function () {
            var params = {
                Username: $scope.username,
                Password: $scope.password
            };
            loginService.login(params)
            .then(function (data) {
                _.map(data, function (value, key) {
                    persistService.storeData(key, value);
                });
                persistService.storeData('isLoggedIn', true);
                $location.path('/');
            }, function () {
                //ERROR
            });
        }

        $scope.openRegisterModal = function () {
            $modalInstance.close();

            $modal.open({
                templateUrl: 'scripts/User/SignUp/signUpModal.tmpl.html',
                controller: 'signUpController',
                size: 'sm'
            });
        }

        $scope.loginWithFacebook = function () {

            FB.login(function (response) {
                if (response.authResponse) {
                    console.log(response);
                    //statusChangeCallback(response);
                    FB.api('/me', { fields: 'email' }, function (response) {
                        //console.log(JSON.stringify(response));
                        console.log(response);
                    });
                }
            });
        }
    }]);
}).call(this);