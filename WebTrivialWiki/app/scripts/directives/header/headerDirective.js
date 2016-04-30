(function() {
    'use strict';

    App.module.directive('headerMenu', function() {
        return{
            restrict: 'E',
            templateUrl: 'scripts/directives/header/headerMenu.tmpl.html',
            controller:'headerController'
        };
    });

}).call(this);