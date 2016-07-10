(function() {
    'use strict';
    App.module.directive('dropdownPreventClose', function () {
        return {
            restrict: 'A',
            link: function(scope, element) {
                element.on('click', function(e) {
                    e.stopPropagation();
                });
            }
        };
    });

}).call(this);