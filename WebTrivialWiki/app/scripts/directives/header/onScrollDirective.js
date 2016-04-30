(function(angular, $) {
    'use strict';

    App.module.directive('onScroll', ['$window', function($window) {
        return function() {
            angular.element($window).bind('scroll', function() {
                if (this.pageYOffset >= 100) {
                    $('.headerContainer').addClass('smaller');
                    $('.navigation-bar').addClass('smaller');
                    $('.wiki-logo-image').addClass('smaller');
                    $('.wikiTitle').addClass('smaller');
                } else {
                    $('.headerContainer').removeClass('smaller');
                    $('.navigation-bar').removeClass('smaller');
                    $('.wiki-logo-image').removeClass('smaller');
                    $('.wikiTitle').removeClass('smaller');
                }
            });
        }
    }]);

}).call(this, this.angular, this.jQuery);