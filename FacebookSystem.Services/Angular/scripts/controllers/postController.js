'use strict';

socialNetwork.controller('postController', function ($scope, $http, $route, $routeParams, $rootScope, postService, notifyService) {
    
    // Authorization token
    $http.defaults.headers.common['Authorization'] = sessionStorage['access_token'];
    
    $scope.deletePost = function (post) {
        if (confirm('Are you sure you want to delete this post?')) {
            postService.deletePostById(post.id)
                .then(function (data) {
                    var postIndex = $scope.wallFeeds.indexOf(post);
                    $scope.wallFeeds.splice(postIndex, 1);
                    notifyService.showInfo('Successfully deleted post!');
                }, function (error) {
                    notifyService.showError('Delete post failed ', error);
                })
        }
    };

    $scope.deleteFeedPost = function (post) {
        if (confirm('Are you sure you want to delete this post?')) {
            postService.deletePostById(post.id)
                .then(function (data) {
                    var postIndex = $scope.newsFeeds.indexOf(post);
                    $scope.newsFeeds.splice(postIndex, 1);
                    notifyService.showInfo('Successfully deleted post!');
                }, function (error) {
                    notifyService.showError('Delete post failed ', error);
                })
        }
    };
    $scope.addWallPost = function (postData) {
        postData['username'] = $routeParams.username;
        postService.addPost(postData)
            .then(function (data) {
                $route.reload();
                notifyService.showInfo('Successfully added post!');
            }, function (error) {
                notifyService.showError('Add post failed ', error);
            });
    };

    $scope.likePost = function (post) {
        postService.likePost(post.id)
            .then(function (data) {
                post.liked = data.liked;
                post.likesCount = data.likesCount;
                notifyService.showInfo('Successfully liked post!')
            }, function (error) {
                notifyService.showError('Like post failed ', error);
            })
    };

    $scope.dislikePost = function (post) {
        postService.dislikePost(post.id)
            .then(function (data) {
                post.liked = data.liked;
                post.likesCount = data.likesCount;
                notifyService.showInfo('Successfully disliked post!')
            }, function (error) {
                notifyService.showError('Dislike post failed ', error);
            })
    };

    $scope.editPost = function (post) {
        postService.editPostById(post)
            .then(function (data) {
                $rootScope.isEditActivated = false;
                notifyService.showInfo('Successfully disliked post!')
            }, function (error) {
                notifyService.showError('Dislike post failed ', error);
            });
    };
});