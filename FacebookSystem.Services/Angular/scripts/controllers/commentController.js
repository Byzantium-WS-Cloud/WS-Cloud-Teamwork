'use strict';

socialNetwork.controller('commentController', function ($scope, $location, $http, commentService, notifyService) {
    // Authorization token
    $http.defaults.headers.common['Authorization'] = sessionStorage['access_token'];

    $scope.deleteComment = function (feed, comment) {
        if (confirm('Are you sure you want to delete this comment?')) {
            commentService.deleteComment(feed.id, comment.id)
                .then(function (data) {
                    var commentIndex = feed.comments.indexOf(comment);
                    feed.comments.splice(commentIndex, 1);
                    notifyService.showInfo('Successfully deleted comment!');
                }, function (error) {
                    console.log(error);
                })
        }
    }
});