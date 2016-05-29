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

        $scope.userIsLoggedIn = function () {
            return persistService.readData('isLoggedIn') === true &&
                persistService.readData('userName') !== undefined &&
                persistService.readData('securityToken') !== undefined;
        };

        $scope.getProfilePicture = function() {
            return persistService.readData('avatar');
        };

        $scope.userHasAvatar = function() {
            $scope.avatar = persistService.readData('avatar');
            return $scope.avatar !== null;
        };

        $scope.getUserName = function() {
            return persistService.readData('userName');
        };

        $scope.getRank = function () {
            $scope.rank = persistService.readData('rank');
            return $scope.rank;
        };

        $scope.signOut = function() {
            persistService.clearLocalStorage();
        };

        $scope.goToSettings = function () {
            $location.url('/settings');
        };
    }]);
}).call(this);