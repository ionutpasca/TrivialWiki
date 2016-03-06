"use strict";

App.module.config(function ($routeProvider) {

    $routeProvider.when('/login', {
        templateUrl: 'templates/login.html',
        controller:'loginController'
    })
    .otherwise({
        redirectTo: '/home'
    });

});