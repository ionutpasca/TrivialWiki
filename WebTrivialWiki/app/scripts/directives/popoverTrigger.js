(function() {
    'use strict';
    App.module.directive('popoverTrivver', function () {
        return {
            link: function (scope, element) {
                element.on('click', function () {
                    element.addClass('trigger');
                });
            }
        };
    });
}).call(this);