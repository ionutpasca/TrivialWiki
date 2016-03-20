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
        .otherwise({
            redirectTo: '/'
        });
});
