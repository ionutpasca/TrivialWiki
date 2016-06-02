(function(angular) {
    'use strict';

    angular.module('webTrivialWikiApp').controller('changePasswordController', ['userService', '$uibModalInstance', '$scope','userName',
    function (userService, $modalInstance, $scope, userName) {

        function passwordsMatch() {
            if ($scope.repeatedNewPassword === $scope.newPassword &&
                $scope.repeatedNewPassword !== undefined &&
                $scope.repeatedNewPassword !== '') {
                return true;
            }
            return false;
        }

        $scope.cancel = function () {
            $modalInstance.close(false);
        }

        $scope.changePassword = function () {
            if ($scope.newPassword === $scope.oldPassword || !passwordsMatch()){
                $scope.oldAndNewAreEqual = true;
                return;
            }
            $scope.paswordIsChanging = true;
            userService.changePassword(userName, $scope.oldPassword, $scope.newPassword)
            .then(function() {
            $scope.paswordIsChanging = false;
                $scope.cancel();
            });
        };

        $scope.$watch('repeatedNewPassword', function(newValue) {
            $scope.invalidNewPassword = !passwordsMatch();
            if (newValue === $scope.newPassword) {
                $scope.invalidNewPassword = false;
            }
        });
    }]);

}).call(this, this.angular);