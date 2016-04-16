(function () {
    'use strict';

    App.module = angular.module('webTrivialWikiApp',
        ['ngAnimate', 'ngCookies', 'ngResource', 'ngRoute', 'ngSanitize', 'ngTouch', 'ui.bootstrap'])
        .config(function ($routeProvider) {
            $routeProvider
            .when('/', {
                templateUrl: 'views/about.html',
                controller: 'AboutCtrl'
            })
            .when('/login', {
                templateUrl: 'views/User/login.html',
                controller: 'loginController'
            })
            .when('/register', {
                templateUrl: 'views/User/signUp.html',
                controller: 'signUpController'
            })
            .when('/somethingWrong', {
                templateUrl: 'views/somethingWrong.html'
            })
            .otherwise({
                redirectTo: '/'
            });
        });

    App.module.factory('responseErrorInterceptor', ['$q', '$location', function ($q, $location) {
            var responseErrorMarker = {
                'responseError': function (rejection) {
                    if (rejection.status === 500 || rejection.status === 400) {
                        $location.path('/somethingWrong');
                    }
                }
            };
            return responseErrorMarker;
        }])
        .config([
            '$httpProvider', function ($httpProvider) {
                $httpProvider.interceptors.push('responseErrorInterceptor');
            }
        ]);


    window.fbAsyncInit = function () {
        FB.init({
            appId: '674147732724867',
            cookie: true,
            xfbml: true,
            version: 'v2.5'
        });
    };

    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) { return; }
        js = d.createElement(s); js.id = id;
        js.src = '//connect.facebook.net/en_US/sdk.js';
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

}).call(this);