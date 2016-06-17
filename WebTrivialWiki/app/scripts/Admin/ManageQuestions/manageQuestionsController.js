(function(angular, _) {

    angular.module('adminModule')
    .controller('manageQuestionsController', ['$scope', 'manageQuestionsService', 'growl','$mdToast', function($scope, questionsService, growl, $mdToast) {
        
        var toastPosition = {
            bottom: false,
            top: true,
            left: false,
            right: true
        };

        function getToastPosition() {
            return Object.keys($scope.toastPosition)
                .filter(function(pos) { return $scope.toastPosition[pos]; })
                .join(' ');
        }

        function showToast(text) {
            var pinTo = getToastPosition();
         
            $mdToast.show(
                $mdToast.simple()
                .textContent(text)
                .position(pinTo)
                .hideDelay(3000)
            );
        }

        function init() {
            //questionsService.getTopicNames()
            //.then(function (data) {
            //    $scope.topicNames = data;
            //    $scope.topicsAreLoading = false;
            //});
            $scope.questionsType = 0;
            $scope.selectedQuestions = [];
            $scope.toastPosition = angular.extend({}, toastPosition);

            $scope.topicsAreLoading = true;
            questionsService.getTopicsWithoutQuestions()
            .then(function (data) {
                $scope.inactiveTopics = data;
                $scope.topicsAreLoading = false;

            });
        }

        function getInactiveQuestions(topic) {
            questionsService.getInactiveQuestionsForTopic(topic)
           .then(function (data) {
               $scope.questions = data;
               $scope.questionsAreLoading = false;
           });
        }

        function getActiveQuestions(topic) {
            questionsService.getActiveQuestionsForTopic(topic)
            .then(function (data) {
                $scope.questions = data;
                $scope.questionsAreLoading = false;
            });
        }

        function refreshQuestions(topic) {
            $scope.questionsAreLoading = true;
            if ($scope.invalidQuestionsAreSelected()) {
                getInactiveQuestions(topic);
            } else {
                getActiveQuestions(topic);
            }
        }

        $scope.selectQuestion = function (question) {
            var questionToAdd =
                {
                    id: question.questionId,
                    question: question.question,
                    answer: question.answer
                };
            var quest = _.find($scope.selectedQuestions, function(q) {
                return q.question === questionToAdd.question;
            });
            if (quest !== undefined)
            {
                _.remove($scope.selectedQuestions, function(q) {
                    return q.question === questionToAdd.question;
                });
            }
            else
            {
                $scope.selectedQuestions.push(questionToAdd);
            }
        };

        $scope.questionIsSelected = function(question) {
            var found = _.find($scope.selectedQuestions, function(q) {
                return q.question === question.question;
            });
            return found !== undefined;
        };

        $scope.clearSelected = function() {
            $scope.selectedQuestions = [];
        };

        $scope.updateQuestions = function () {
            $scope.questionsAreLoading = true;

            var questions = _.map($scope.selectedQuestions, function(q) {
                return {
                    id: q.id,
                    question: q.question
                }
            });
            questionsService.updateQuestions(questions)
           .then(function () {
                showToast('Questions were updated!');
               //growl.success('Questions were updated!');
               $scope.selectedQuestions = [];

               refreshQuestions($scope.topicName);
           });
        };

        $scope.deleteQuestions = function () {
            $scope.questionsAreLoading = true;

            var questions = _.map($scope.selectedQuestions, function (q) {
                return q.id;
            });
            questionsService.deleteQuestions(questions)
            .then(function () {
                growl.success('Questions were deleted!');
                $scope.selectedQuestions = [];

                refreshQuestions($scope.topicName);
            });
        };

        $scope.enableTopic = function() {
            questionsService.enableTopic($scope.topicName)
            .then(function() {
                growl.success('Topic '+$scope.topicName + ' was eabled');
            });
        };

        $scope.invalidQuestionsAreSelected = function () {
            return $scope.questionsType === 0;
        }

        $scope.selectQuestionType = function (value) {
            $scope.questionsType = value;
            $scope.clearSelected();
            refreshQuestions($scope.topicName);
        };

        $scope.$watch('topicName', function (newVal) {
            if (newVal !== null && newVal !== undefined) {
                $scope.questionsAreLoading = true;
                refreshQuestions(newVal);
            }
        });

        init();
    }]);
}).call(this, this.angular, this._);