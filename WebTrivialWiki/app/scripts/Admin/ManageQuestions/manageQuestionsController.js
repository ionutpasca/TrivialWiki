(function(angular, _) {

    angular.module('adminModule')
    .controller('manageQuestionsController', ['$scope', 'manageQuestionsService', 'growl',function($scope, questionsService, growl) {
                
        function init() {
            questionsService.getTopicNames()
            .then(function (data) {
                $scope.topicNames = data;
            });

            questionsService.getTopicsWithoutQuestions()
            .then(function(data) {
                $scope.topicsWithoutQuestions = data;
            });
        }

        $scope.$watch('topicName', function (newVal) {
            if (newVal !== null && newVal !== undefined) {
                questionsService.getQuestionsForTopic(newVal)
                .then(function (data) {
                    $scope.questions = data;
                });
            }
        });

        init();
    }]);
}).call(this, this.angular, this._);