(function() {
    'use strict';
    App.module.controller('proposeTopicController', ['$mdDialog', '$scope', 'topicsService', function ($mdDialog, $scope, topicsService) {
        function init() {
            $scope.topicIsInvalid = false;
        }
        init();

        $scope.proposeTopic = function () {
            $scope.topicIsSaving = true;
            topicsService.proposeTopic($scope.proposedTopic)
            .then(function() {
                $scope.topicIsSaving = false;
                $mdDialog.cancel();
            });
        };

        $scope.$watch('proposedTopic', function(newVal) {
            if (newVal.length > 19) {
                $scope.topicIsInvalid = true;
            } else {
                $scope.topicIsInvalid = false;
            }
        });
    }]);

}).call(this);