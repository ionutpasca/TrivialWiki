(function (angular) {
    'use strict';

    angular.module('triviaModule', ['ngAnimate', 'ngRoute', 'ui.bootstrap', 'ngAnimate', 'luegg.directives'])
       .config(function ($routeProvider) {
           $routeProvider
               .when('/trivia/:tableName',
                {
                    templateUrl: 'scripts/Trivia/Game/triviaTable.html',
                    controller:'triviaTableController'
                })
               .when('/trivia',
               {
                   templateUrl: 'scripts/Trivia/Game/trivia.html',
                   controller: 'triviaController'
               });
       });

}).call(this, this.angular);