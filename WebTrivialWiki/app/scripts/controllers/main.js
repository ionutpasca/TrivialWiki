(function () {
    'use strict';

    App.module.controller('MainCtrl', ['$uibModal','$scope', function ($uibModal, $scope) {

        $scope.openLoginModal = function () {
            $uibModal.open({
                templateUrl: 'scripts/User/Login/loginModal.tmpl.html',
                controller: 'loginController',
                size:'sm'
            });
        };

        $scope.openRegisterModal = function () {
            $uibModal.open({
                templateUrl: 'scripts/User/SignUp/signUpModal.tmpl.html',
                controller: 'signUpController',
                size: 'sm'
            });
        };

    }]);
}).call(this);