(function () {
    'use strict';

    angular.module('triviaModule', ['ngAnimate', 'ngRoute', 'ui.bootstrap', 'ngAnimate'])
       .config(function ($routeProvider) {
           $routeProvider
               .when('/trivia',
               {
                   templateUrl: 'scripts/Trivia/Game/trivia.html',
                   controller: 'triviaController'
               });
       });

}).call(this);