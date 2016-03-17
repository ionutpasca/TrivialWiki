"use strict";

App.module.config(function ($stateProvider) {

    $stateProvider
    .state('login', {
        url: '/login',
        templateUrl: 'templates/login.html',
        controller: 'loginController',
        roles: []
    })

});