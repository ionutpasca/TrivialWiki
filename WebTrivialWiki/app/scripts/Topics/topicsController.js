(function(_) {
    'use strict';

    App.module.controller('topicsController', ['$scope', 'topicsService', '$mdDialog', function ($scope, topicsService, $mdDialog) {
        $scope.imagePath = 'https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Reign_of_the_Superman.jpg/300px-Reign_of_the_Superman.jpg';
        $scope.imagePath1 = 'https://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Lambda_lc.svg/300px-Lambda_lc.svg.png';

        function getImageHeight(imageUrl) {
            var tmpImg = new Image();
            tmpImg.src = imageUrl;
            return tmpImg.height;
        }

        function init() {
            $scope.topicsAreLoading = true;
            $scope.topics = [];

            topicsService.getTopics()
            .then(function (data) {
                _.each(data, function (topic) {
                    topic.imageHeight = getImageHeight(topic.thumbnailUrl);
                    $scope.topics.push(topic);
                });
                $scope.topicsAreLoading = false;
            });
        }

        $scope.showAdvanced = function () {
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
}).call(this, this._);