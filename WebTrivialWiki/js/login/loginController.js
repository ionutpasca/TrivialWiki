"use strict";

App.module.controller('loginController', ['$scope','loginService', '$window', function ($scope, loginService, $window) {
    $scope.login = function () {
        var params = {
            Username: $scope.username,
            Password: $scope.password
        };

        loginService.login(params)
        .then(function (data) {
            _.map(data, function (value, key) {
                $window.localStorage[key] = value;
            });
        }, function(){
            //ERROR
        })
    }
}]);