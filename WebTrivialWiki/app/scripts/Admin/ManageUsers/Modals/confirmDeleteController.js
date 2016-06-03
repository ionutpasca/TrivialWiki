(function(angular) {
    'use strict';

    angular.module('adminModule')
    .controller('confirmDeleteController', ['$scope', '$uibModalInstance', 'userName', function ($scope, $modalInstance, userName) {
        $scope.username = userName;

        $scope.delete = function () {
            $modalInstance.close(true);
        };
        $scope.cancel = function () {
            $modalInstance.close(false);
        }
    }]);

}).call(this,this.angular)