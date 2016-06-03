(function (angular, $) {
    'use strict';

    App.module.run(['$rootScope', '$window', function ($rootScope, $window) {
        $rootScope.$on('$routeChangeSuccess', function (ev, data) {

            function addResizeClasses() {
                $('.headerContainer').addClass('smaller');
                $('.navigation-bar').addClass('smaller');
                $('.wiki-logo-image').addClass('smaller');
                $('.wikiTitle').addClass('smaller');
                $('.settings-dropdown').addClass('smaller');
            }

            function removeResizeClasses() {
                $('.headerContainer').removeClass('smaller1');
                $('.headerContainer').removeClass('smaller');
                $('.navigation-bar').removeClass('smaller');
                $('.wiki-logo-image').removeClass('smaller');
                $('.wikiTitle').removeClass('smaller');
                $('.settings-dropdown').removeClass('smaller');
            }

            if (data.$$route && data.$$route.originalPath === '/') {
                if ($('.smaller1') !== []) {
                    removeResizeClasses();
                }
                angular.element($window).bind('scroll', function () {
                    if (this.pageYOffset >= 100) {
                        $('.headerContainer').removeClass('smaller1');
                        addResizeClasses();
                    } else {
                        removeResizeClasses();
                    }
                });
            } else {
                angular.element($window).unbind('scroll');
                addResizeClasses();
                $('.headerContainer').addClass('smaller1');
            }
        });
    }]);
}).call(this, this.angular, this.$);