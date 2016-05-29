(function(angular) {
    angular.module('webTrivialWikiApp')
        .controller('settingsController', ['$scope', 'persistService', function ($scope, persistService) {

            $scope.userHasAvatar = function() {
                $scope.avatar = persistService.readData('avatar');
                return $scope.avatar !== null;
            };

            $scope.signOut = function () {
                persistService.clearLocalStorage();
            };
    }]);
}).call(this, this.angular);