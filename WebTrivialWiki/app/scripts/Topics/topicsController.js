(function() {
    'use strict';

    App.module.controller('topicsController', ['$scope', 'topicsService', '$mdDialog','$mdMedia', function ($scope, topicsService, $mdDialog, $mdMedia) {
        $scope.imagePath = 'https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Reign_of_the_Superman.jpg/300px-Reign_of_the_Superman.jpg';
        $scope.imagePath1 = 'https://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Lambda_lc.svg/300px-Lambda_lc.svg.png';

        function initializeTopics(topics) {
            for (var i = 0; i < topics.length; i += 3) {
                $scope.firstTopicsBatch.push(topics[i]);
                if (topics[i+1] < topics.length) {
                    $scope.secondTopicsBatch.push(topics[i + 1]);
                }
                if (topics[i + 2] < topics.length) {
                    $scope.thirdTopicsBatch.push(topics[i + 2]);
                }
            }
        }

        $scope.test = [1, 2, 3,4,5,6,7];
        function init() {
            $scope.topicsAreLoading = true;
            $scope.firstTopicsBatch = [];
            $scope.secondTopicsBatch = [];
            $scope.thirdTopicsBatch = [];


            topicsService.getTopics()
            .then(function(data) {
                initializeTopics(data);
                $scope.topicsAreLoading = false;
            });
        }

        $scope.showAdvanced = function (event) {
            $mdDialog.show({
                clickOutsideToClose: true,
                scope: $scope,
                preserveScope: true,
                templateUrl: 'scripts/Topics/proposeTopic.html',
                controller: 'proposeTopicController'
            });
        };

        init();
    }]);
}).call(this);