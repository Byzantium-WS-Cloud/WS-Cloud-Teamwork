'use strict';

socialNetwork.controller('feedsController', function ($scope, $http, $location, $route, userService, authorizationService, notifyService) {

    $http.defaults.headers.common['Authorization'] = sessionStorage['access_token'];

    var Paths = {
        feedPath : '/users/feeds',
        wallPath : '/users/wall/',
        profilePath : '/users/profile',
        friendsList: '/users/friendslist',
        myWall: '/users/wall/'+$scope.currentUsername
    };

    $scope.getMyFriends = function () {
        if ($location.path() === Paths.feedPath ||
            $location.path() === Paths.friendsList ||
            $location.path() === Paths.myWall) {
            userService.getOwnFriends()
                .then(function (allFriendsData) {
                    $scope.ownFriendsCollection = allFriendsData;
                }, function (error) {
                    notifyService.showError('Connection to the database failed. Please try again later.');
                });
        }
    };

    $scope.stopFriendPreview = function () {
        $scope.isFriendHovered = false;
    };

    $scope.previewFriend = function (friend) {
        authorizationService.getUserPreviewDataByUsername(friend.username)
            .then(function (userData) {
                $scope.hoveredUserPreviewData = userData;
                $scope.isFriendHovered = true;
                $scope.hoveredPostId = friend.id;
            }, function (error) {
                notifyService.showError('Connection to the database failed. Please try again later.');
            })
    };

    // Functions calls
    if (authorizationService.isLoggedIn()) {
        $scope.getMyFriends();

    }
});