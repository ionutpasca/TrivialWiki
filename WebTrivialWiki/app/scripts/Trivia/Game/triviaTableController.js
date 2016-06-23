(function (angular, _) {
    'use strict';
    angular.module('triviaModule')
    .controller('triviaTableController', ['$scope', 'triviaFactory', '$routeParams', function ($scope, triviaFactory, $routeParams) {

        function init() {
            $scope.connecting = true;
            $scope.tableName = $routeParams.tableName;

            $scope.connecting = false;
        }

        init();

        var triviaProxy = triviaFactory($scope.tableName);

        triviaProxy.on('addMessage', function (res) {
            $scope.currentQuestion = res;
        });


        

    }]);
}).call(this, this.angular, this._);