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

    angular.module('triviaModule').filter('reverse', function () {
        return function (items) {
            return items.slice().reverse();
        };
    });

}).call(this, this.angular);