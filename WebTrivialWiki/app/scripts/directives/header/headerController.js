(function () {
    'use strict';

    App.module.controller('headerController', ['$uibModal', '$scope', 'persistService', '$location',
        function ($uibModal, $scope, persistService, $location) {

        $scope.openLoginModal = function () {
            $uibModal.open({
                templateUrl: 'scripts/User/Login/loginModal.tmpl.html',
                controller: 'loginController',
                size: 'sm'
            });
        };

        $scope.openRegisterModal = function () {
            $uibModal.open({
                templateUrl: 'scripts/User/SignUp/signUpModal.tmpl.html',
                controller: 'signUpController',
                size: 'sm'
            });
        };

        function init() {
            $scope.userIsLoggedIn = persistService.readData('isLoggedIn');
            if ($scope.userIsLoggedIn === '' || $scope.userIsLoggedIn === undefined) {
                return;
            }
            $scope.rank = persistService.readData('rank');
            $scope.userName = persistService.readData('userName');
            $scope.userRole = persistService.readData('role');
        }
        init();

        $scope.signOut = function () {
            persistService.clearLocalStorage();
        };

        $scope.goToSettings = function () {
            $location.url('/settings');
        };

        $scope.openUserManagement = function () {
            $location.url('/manageUsers');
        };

        $scope.openTrivia = function() {
            $location.url('/trivia');
        };
    }]);
}).call(this);