from django.db import models

# Create your models here.
class QuestionSet(models.Model):
    QuestionText = models.CharField(max_length = 400)
    CorrectAnswer = models.CharField(max_length = 50)
    FillerAnswers = models.CharField(max_length = 200)
    UsageCount = models.IntegerField(default = 0)
    CorrectAnswerCount = models.IntegerField(default = 0)

class Topic(models.Model):
    Name = models.CharField(max_length=50)
    TopicQuestions = models.ManyToManyField(QuestionSet)

class RelatedTopic(models.Model):
    Name = models.CharField(max_length = 100)
    TopicId = models.ForeignKey(Topic)

class Statistics(models.Model):
    Wins = models.IntegerField(default = 0)
    GamesPlayed = models.IntegerField(default = 0)
    BestTopicId = models.ForeignKey(Topic, related_name='best_topic_Id')
    MostPlayedTopicId = models.ForeignKey(Topic, related_name='most_played_topic')
    WinsInARow = models.IntegerField(default = 0)

class Role(models.Model):
    Name = models.CharField(max_length= 50)

class User(models.Model):
    FirstName = models.CharField(max_length = 20)
    LastName = models.CharField(max_length = 20)
    Username = models.CharField(max_length = 30)
    Password = models.CharField(max_length = 50)
    SecurityToken = models.CharField(max_length = 50)
    Sex = models.CharField(max_length = 10)
    Email = models.EmailField()
    Avatar = models.BinaryField()
    RoleId = models.ForeignKey(Role)
    Points = models.IntegerField(default = 0)
    Rank = models.IntegerField()
    StatisticsId = models.ForeignKey(Statistics, default=None)
    Friends = models.ManyToManyField("self", blank=True, default=None)
    UserTopics = models.ManyToManyField(Topic, default=None)

class Achievement(models.Model):
    Name = models.CharField(max_length = 200)
    Threshold = models.IntegerField(default = 0)
    Icon = models.BinaryField()
    User = models.ManyToManyField(User)

class Match(models.Model):
    Loser = models.ForeignKey(User, related_name='loser')
    Winner = models.ForeignKey(User, related_name='winner')
    QuestionCount = models.IntegerField()
    LoserCorrectAnswers = models.IntegerField()
    WinnerCorrectAnswers = models.IntegerField()
    TopicId = models.ManyToManyField(Topic)

class ChatRoom(models.Model):
    MessageText = models.CharField(max_length = 500)
    UserId = models.ForeignKey(User)
    Timestamp = models.DateTimeField(auto_now_add = True)

class PrivateChat(models.Model):
    FirstUserId = models.ForeignKey(User, related_name = 'first_user')
    SecondUserId = models.ForeignKey(User, related_name = 'second_user')

class Message(models.Model):
    SenderId = models.ForeignKey(User)
    MessageText = models.CharField(max_length = 500)
    Timestamp = models.DateTimeField(auto_now_add = True)
    ChatId = models.ForeignKey(PrivateChat)










