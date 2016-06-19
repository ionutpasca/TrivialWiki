(function (moment, _) {
    'use strict';

    App.module.controller('headerController', ['$uibModal', '$scope', '$timeout', 'headerService', 'persistService', '$location',
        '$mdSidenav','notificationsHubFactory',
        function ($uibModal, $scope, $timeout, headerService, persistService, $location, $mdSidenav, notificationHub) {

        var notificationsProxy = notificationHub();

        notificationsProxy.on('notify', function (res) {
            var notification = {
                id: res.Id,
                sender: res.Sender,
                notificationText: res.NotificationText,
                notificationDate: res.NotificationDate,
                seen: res.Seen
            };
            $scope.notifications.unshift(notification);
            $scope.unseenNotifications = getNumberOfUnseenNotifications();
        });

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

        function getNumberOfUnseenNotifications() {
            return _.filter($scope.notifications, function(not) {
                return not.seen === false;
            }).length;
        };

        function init() {
            $scope.userIsLoggedIn = persistService.readData('isLoggedIn');
            if ($scope.userIsLoggedIn === '' || $scope.userIsLoggedIn === undefined) {
                return;
            }
            $scope.rank = persistService.readData('rank');
            $scope.userName = persistService.readData('userName');
            $scope.userRole = persistService.readData('role');
            headerService.getNotifications()
            .then(function (data) {
                $scope.notifications = [];

                _.each(data, function(not) {
                    $scope.notifications.unshift(not);
                });
                $scope.unseenNotifications = getNumberOfUnseenNotifications();
            });
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

        $scope.openAdmin = function () {
            $location.url('/admin');
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

        $scope.openTopics = function() {
            $location.url('/topics');
        };

        $scope.toggleRight = function () {
            $mdSidenav('right').toggle();
        };

        $scope.markNotificationAsSeen = function (notification) {
            notification.seen = true;

            headerService.markNotificationAsSeen(notification.id)
            .then(function() {
                $scope.unseenNotifications = getNumberOfUnseenNotifications();
            });
        };

        $scope.getNiceDateTime = function(date) {
            return moment(date).format('LL');
        };

        $scope.addTest = function() {
            $scope.customPending = $scope.customPending + 1;
        };

        }]).controller('RightCtrl', function ($scope, $timeout, $mdSidenav) {
            $scope.close = function () {
                $mdSidenav('right').close()
                  .then(function () {
                  });
            };
        });
}).call(this, this.moment, this._);