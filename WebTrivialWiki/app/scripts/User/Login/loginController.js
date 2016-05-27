(function (_) {
    'use strict';

    App.module.controller('loginController', ['$scope', 'loginService', '$location', 'persistService', '$uibModal','$uibModalInstance',
            function ($scope, loginService, $location, persistService, $modal, $modalInstance) {
        
        function init() {
            $scope.credentialsAreInvalid = false;
        }

        $scope.login = function () {
            if ($scope.username === "" || $scope.username === undefined
                || $scope.password === "" || $scope.password === undefined) {
                $scope.credentialsAreInvalid = true;
                return;
            }

            var params = {
                Username: $scope.username,
                Password: $scope.password
            };
            loginService.login(params)
                .then(function(data) {
                    _.map(data, function(value, key) {
                        persistService.storeData(key, value);
                    });
                    persistService.storeData('isLoggedIn', true);
                    $location.path('/');
                }, function() {
                    //ERROR
                });
        };
        init();

        $scope.$watch('username',function() {
            $scope.credentialsAreInvalid = false;
        });
        $scope.$watch('password', function () {
            $scope.credentialsAreInvalid = false;
        });
    }]);
}).call(this, this._);