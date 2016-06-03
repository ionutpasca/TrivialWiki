(function(angular, _) {

    angular.module('adminModule')
        .controller('manageUsersController', ['$scope', 'manageUsersService','growl', '$uibModal',
        function ($scope, usersService, growl, $modal) {

            
            function addNewUserInDatabase(user) {
                var payload = {
                    Username: user.username,
                    Email: user.email,
                    Password: user.password
                };

                usersService.addNewUserInDatabase(payload)
                .then(function() {
                    growl.success('User ' + user.username + ' was added.');
                    $scope.getUsersBatch();
                });
            }

            function updateUserInDatabase(user) {
                var payload = {
                    Username: user.username,
                    Email: user.email,
                    Role: user.role,
                    Points: user.points
                };

                usersService.updateUser(payload)
                .then(function () {
                    growl.success('User ' + user.username + ' was updated.');
                    $scope.getUsersBatch();
                });
            }

            function removeUserFromDatabase(user) {
                usersService.removeUser(user.username)
                .then(function () {
                    growl.success('User ' + user.username + ' deleted.');
                    $scope.getUsersBatch();
                });
            }

            $scope.getUsersBatch = function () {
                usersService.getUserBatch($scope.currentPage, $scope.queryString)
                .then(function (data) {
                    $scope.users = data.users;
                    $scope.numberOfUsers = data.totalNumberOfUsers;
                });
            };

            $scope.addNewUser = function () {
                var modalInstance = $modal.open({
                    templateUrl: 'scripts/Admin/ManageUsers/Modals/addNewUserModal.html',
                    controller: 'addNewUserController'
                });
                modalInstance.result.then(function (result) {
                    if (result) {
                        addNewUserInDatabase(result);
                    }
                });
            }

            $scope.updateUser = function(user) {
                var modalInstance = $modal.open({
                    templateUrl: 'scripts/Admin/ManageUsers/Modals/updateUserModal.html',
                    controller: 'updateUserController',
                    resolve: {
                        userToUpdate: function() {
                            return user;
                        }
                    }
                });

                modalInstance.result.then(function(result) {
                    if (result) {
                        updateUserInDatabase(result);
                    }
                });
            };

            $scope.removeUser = function (user) {
                var modalInstance = $modal.open({
                    templateUrl: 'scripts/Admin/ManageUsers/Modals/confirmDeleteModal.html',
                    controller: 'confirmDeleteController',
                    resolve: {
                        userName: function() {
                            return user.username;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    if (result) {
                        removeUserFromDatabase(user);
                    }
                });
            };

            $scope.pageChanged = function() {
                $scope.getUsersBatch();
            };

            function init() {
                $scope.queryString = '';
                $scope.currentPage = 1;
                $scope.getUsersBatch();
            }

            $scope.$watch('queryString', _.debounce(function (newQuery) {
                if (newQuery.length < 2 && newQuery !== '') {
                    return;
                }
                $scope.getUsersBatch();
            }, 300));

            init();
        }]);

}).call(this, this.angular, this._);