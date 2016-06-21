(function(angular) {

    angular.module('adminModule')
    .controller('manageTopicsController', ['$scope', 'manageTopicsService','growl', function ($scope, manageTopicsService, growl) {

        function init() {
            $scope.topicsAreLoading = true;
            manageTopicsService.getProposedTopics()
            .then(function (data) {
                $scope.proposedTopics = data;
                $scope.topicsAreLoading = false;
            });
        }

        $scope.processTopic= function(topic) {
            $scope.topicIsProcessing = true;
            manageTopicsService.processTopic(topic.topicName)
            .then(function() {
                $scope.topicIsProcessing = false;
                growl.success(topic.topicName + " was processed !");
            });
        };

        init();

    }]);
}).call(this, this.angular);