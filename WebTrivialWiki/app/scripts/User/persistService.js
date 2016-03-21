(function(angular) {
    'use strict';

    App.module.service('persistService', [
        '$cookieStore', '$window', function($cookieStore, $window) {

            var hasLocalStorage = Boolean($window.localStorage),
                hasSessionStorage = Boolean($window.sessionStorage);

            function storeData(key, value) {
                if (hasLocalStorage) {
                    $window.localStorage[key] = angular.toJson(value);
                    return;
                }
                if (hasSessionStorage) {
                    $window.sessionStorage[key] = angular.toJson(value);
                }
                $cookieStore.put(key, value);
            }

            function readData(key) {
                if (hasLocalStorage) {
                    return angular.fromJson($window.localStorage[key]);
                }
                if (hasSessionStorage) {
                    return angular.fromJson($window.sessionStorage[key]);
                }
                return $cookieStore.get(key);
            }

            function removeData(key) {
                if (hasLocalStorage) {
                    if (angular.isFunction($window.localStorage.removeItem)) {
                        $window.localStorage.removeItem(key);
                        return;
                    }
                    delete $window.localStorage[key];
                    return;
                }
                if (hasSessionStorage) {
                    if (angular.isFunction($window.sessionStorage.removeItem)) {
                        $window.sessionStorage.removeItem(key);
                        return;
                    }
                    delete $window.sessionStorage[key];
                    return;
                }
                $cookieStore.remove(key);
            }

            function storeSessionData(key, value) {
                if (hasSessionStorage) {
                    $window.sessionStorage[key] = angular.toJson(value);
                    return;
                }
                $cookieStore.put(key, value);
            }

            function readSessionData(key) {
                if (hasSessionStorage) {
                    return angular.fromJson($window.sessionStorage[key]);
                }
                return $cookieStore.get(key);
            }

            function removeSessionData(key) {
                if (hasSessionStorage) {
                    if (angular.isFunction($window.sessionStorage.removeItem)) {
                        $window.sessionStorage.removeItem(key);
                        return;
                    }
                    delete $window.sessionStorage[key];
                    return;
                }
                $cookieStore.remove(key);
            }

            this.storeData = storeData;
            this.readData = readData;
            this.removeData = removeData;
            this.storeSessionData = storeSessionData;
            this.readSessionData = readSessionData;
            this.removeSessionData = removeSessionData;
        }
    ]);

}).call(this, this.angular);