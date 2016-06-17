(function (_) {
    'use strict';

    App.module.controller('loginController', ['$scope', 'loginService', '$location', 'persistService', '$uibModalStack',
            function ($scope, loginService, $location, persistService, $uibModalStack) {
        
        function init() {
            $scope.credentialsAreInvalid = false;
        }

        $scope.login = function () {
            $scope.userIsLogging = true;
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
                .then(function (data) {
                    _.map(data, function(value, key) {
                        persistService.storeData(key, value);
                    });
                    persistService.storeData('isLoggedIn', true);
                    $uibModalStack.dismissAll();
                    $location.path('/');
                    $scope.userIsLogging = false;
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