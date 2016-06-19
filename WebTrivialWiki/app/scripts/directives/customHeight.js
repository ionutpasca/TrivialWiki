(function() {
    'use strict';
    App.module.directive('customHeight', function () {
        return {
            restrict: 'A',

            link: function(scope, elem, attrs) {
                var headerHeight = attrs.customHeight ? attrs.customHeight : 0;
                elem.css('height', headerHeight + 'px');
            }
        };
    });

}).call(this);