(function () {
    'use strict';

    App.module.controller('headerController', ['$uibModal', '$scope','$timeout', 'persistService', '$location','$mdSidenav',
        function ($uibModal, $scope, $timeout, persistService, $location, $mdSidenav) {

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

        function debounce(func, wait) {
            var timer;
            return function debounced() {
                var context = $scope,
                    args = Array.prototype.slice.call(arguments);
                $timeout.cancel(timer);
                timer = $timeout(function () {
                    timer = undefined;
                    func.apply(context, args);
                }, wait || 10);
            };
        }

        function buildDelayedToggler(navID) {
            return debounce(function () {
                $mdSidenav(navID)
                  .toggle()
                  .then(function () {
                  });
            }, 200);
        }

        $scope.toggleRight = buildDelayedToggler('right');
        $scope.isOpenRight = function () {
            return $mdSidenav('right').isOpen();
        };

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

        $scope.openLeaderboards = function() {
            $location.url('/leaderboards');
        };

        $scope.goToHomePage = function() {
            $location.url('/');
        };

        $scope.toggleRight = function () {
            $mdSidenav('right').toggle();
        };

        }]).controller('RightCtrl', function ($scope, $timeout, $mdSidenav) {
            $scope.close = function () {
                $mdSidenav('right').close()
                  .then(function () {
                  });
            };
        });
}).call(this);