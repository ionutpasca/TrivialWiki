(function(angular) {
    'use strict';

    angular.module('adminModule', ['ngAnimate', 'ngRoute', 'ui.bootstrap', 'ngAnimate'])
        .config(function($routeProvider) {
            $routeProvider
                .when('/manageUsers',
                {
                    templateUrl: 'scripts/Admin/ManageUsers/manageUsers.tmpl.html',
                    controller: 'manageUsersController'
                });
        });
}).call(this, this.angular);