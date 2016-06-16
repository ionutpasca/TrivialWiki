(function(angular) {
    'use strict';

    angular.module('adminModule', ['ngAnimate', 'ngRoute', 'ui.bootstrap', 'ngAnimate', 'ngMaterial'])
        .config(function($routeProvider) {
            $routeProvider
                .when('/manageQuestions',
                {
                    templateUrl: 'scripts/Admin/ManageQuestions/manageQuestions.html',
                    controller: 'manageQuestionsController'
                })
                .when('/manageUsers',
                {
                    templateUrl: 'scripts/Admin/ManageUsers/manageUsers.tmpl.html',
                    controller: 'manageUsersController'
                });
        });
}).call(this, this.angular);