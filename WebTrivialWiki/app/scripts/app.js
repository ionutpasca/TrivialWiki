'use strict';

App.module = angular.module('webTrivialWikiApp',
    ['ngAnimate', 'ngCookies', 'ngResource', 'ngRoute', 'ngSanitize', 'ngTouch'])
    .config(function($routeProvider) {
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

App.module.factory('responseErrorInterceptor', [
        '$q', '$location', function($q, $location) {

            var responseErrorMarker = {
                'responseError': function(rejection) {
                    if (rejection.status === 500 || rejection.status === 400) {
                        $location.path('/somethingWrong');
                    }
                }
            }
            return responseErrorMarker;
        }
    ])
    .config([
        '$httpProvider', function($httpProvider) {
            $httpProvider.interceptors.push('responseErrorInterceptor');
        }
    ]);
