(function (angular) {
    'use strict';

    angular.module('topicsModule', ['ngRoute', 'ngMaterial'])
    .config(function ($routeProvider) {
        $routeProvider
            .when('/topics',
            {
                templateUrl: 'scripts/Topics/topics.tmpl.html',
                controller: 'topicsController'
            });
    });


}).call(this, this.angular);