(function(angular) {
    'use strict';

    angular.module('adminModule')
    .directive('stopEvent', function() {
            return {
                restrict: 'A',
                link: function(scope, element, attr) {
                    if (attr && attr.stopEvent) {
                        element.bind(attr.stopEvent, function(e) {
                            e.stopPropagation();
                        });
                    }
                }
            };
        });

}).call(this, this.angular);