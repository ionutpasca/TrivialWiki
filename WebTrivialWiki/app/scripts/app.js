'use strict';

App.module = angular.module('webTrivialWikiApp', ['ngAnimate', 'ngCookies', 'ngResource', 'ngRoute', 'ngSanitize', 'ngTouch'])
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
        .otherwise({
        redirectTo: '/'
        });
  });
