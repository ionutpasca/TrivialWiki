﻿'use strict';

App.module.controller('loginController', ['$scope', 'loginService', '$window', '$location', 'persistService', function ($scope, loginService, $window, $location, persistService) {
    $scope.login = function () {
        persistService.storeData(1, 1);

        var params = {
            Username: $scope.username,
            Password: $scope.password
        };
        loginService.login(params)
        .then(function (data) {
            _.map(data, function (value, key) {
                $window.localStorage[key] = value;
            });
            $window.localStorage.isLoggedId = true;
            debugger;
            $location.path('/');
            }, function () {
            //ERROR
        });
    }

    $scope.logOut = function() {
        $window.localStorage.clear();
        $window.localStorage.isLoggedId = false;
    }

    $scope.registerNewAccount = function () {
        $location.path('/register');
    }

    function statusChangeCallback(response) {
        if (response.status === 'connected') {
            FB.api('/me', { fields: 'email,name,picture' }, function (response) {
                debugger;
            });
        } else if (response.status === 'not_authorized') {
            debugger;
        } else {
            debugger;
        }
    }

    $scope.loginWithFacebook = function () {

        FB.login(function (response) {
            if (response.authResponse) {
                console.log(response);
                statusChangeCallback(response);
                //FB.api('/me', { fields: 'email,name' }, function (response) {
                //    console.log(JSON.stringify(response));
                //});
            }
        }, { scope: 'email' });
    }
}]);