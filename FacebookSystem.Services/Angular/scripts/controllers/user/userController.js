'use strict';
Array.prototype.pushArray = function(arr) {
    this.push.apply(this, arr);
};

socialNetwork.controller('userController', function ($scope, $location, $http, $rootScope, $route,
                                                     $routeParams, $interval, authorizationService,
                                                     userService, postService, commentService, notifyService,
                                                     PAGE_SIZE) {

    // Authorization token
    $http.defaults.headers.common['Authorization'] = sessionStorage['access_token'];

    $scope.feedPageSize = PAGE_SIZE;
    $scope.wallPageSize = PAGE_SIZE;

    var Paths = {
        feedPath : '/users/feeds',
        wallPath : '/users/wall/',
        profilePath : '/users/profile'
    };

    $scope.userDetails = function () {
        if (authorizationService.isLoggedIn()) {
            if (sessionStorage['currentUsername'] && sessionStorage['currentUserprofilepic']) { // Optimize query to the database
                $scope.currentUsername = sessionStorage['currentUsername'];
                $scope.currentUserprofilepic = sessionStorage['currentUserprofilepic'];
            }else {
                authorizationService.getUserPreviewData()
                    .then(function (data) {
                        $scope.currentUsername = data.username;
                        if (data.profileImageData !== null) {
                            $scope.currentUserprofilepic = data.profileImageData;
                        }
                        $scope.hasPendingFriendRequest = data.hasPendingRequest;
                        sessionStorage['currentUsername'] = data.username;
                        if (data.profileImageData !== null) {
                            sessionStorage['currentUserprofilepic'] = data.profileImageData;
                        }
                    }, function (error) {
                        notifyService.showError('Connection error  ', error);
                    });
            }
        }
    };

    $scope.goToMyWall = function (username) {
        $location.path(Paths.wallPath + username);
    };

    $scope.goToUserWall = function (username) {
        $location.path(Paths.wallPath + username);
    };

    $rootScope.$on("$routeChangeSuccess", function(event, currentRoute, previousRoute) {
        $rootScope.title = currentRoute.title;
    });

    $scope.fillEditProfileData = function () {
        if ($location.path() === Paths.profilePath) {
            userService.getDataAboutMe()
                .then(function (data) {
                    $scope.profileData = data;
                }, function (err) {
                    notifyService.showError('Connection error ', err);
                });
        }
    };

    $scope.fileLoaded = function (ev) {
        var tgt = ev.target || window.event.srcElement,
            files = tgt.files;

        if (FileReader && files && files.length) {
            var fileReader = new FileReader();
            fileReader.onload = function () {

                $scope.loadedFile = fileReader.result;
                $scope.$apply();
            };
            fileReader.readAsDataURL(files[0]);
        } else {
            notifyService.showError('File not supported! ');
        }
    };

    $scope.editUserProfile = function (profileData) {
        userService.editProfile(profileData)
            .then(function (data) {
                $location.path(Paths.feedPath);
                $route.reload();
                authorizationService.clearUserTemporaryData();
                notifyService.showInfo(data.message);
            }, function (error) {
                notifyService.showError('Edit profile failed ', error);
            })
    };
    
    $scope.changePassword = function (userData) {
        userService.changeUserPassword(userData)
            .then(function (data) {
                $location.path(Paths.feedPath);
                notifyService.showInfo(data.message);
            }, function (error) {
                notifyService.showError('Change password failed ', error);
            })
    };

    $scope.getUserWall = function (username) {
        if (username) {
            authorizationService.getUserFullData(username)
                .then(function (userFullData) {
                    $scope.isCurrentUserFriend = userFullData.isFriend;
                    $scope.userWallData = userFullData;
                }, function (error) {
                    notifyService.showError('Connection to database error. Please try again. ');
                });
        }
    };

    $scope.getFriendsFriendsPreview = function (username) {
        if (username && $scope.isCurrentUserFriend) {
            authorizationService.getFriendsFriendsPreview(username)
                .then(function (friendsFriendsData) {
                    $scope.friendsFriends = friendsFriendsData.friends;
                }, function (error) {
                    notifyService.showError('Friend Connection to database error. Please try again. ');
                });
        }
    };

    $scope.getUserPendingFriendRequests = function () {
        if (authorizationService.isLoggedIn()) {
            authorizationService.getUserPreviewData()
                .then(function (data) {
                   $scope.hasPendingFriendRequest = data.hasPendingRequest;
                }, function (error) {
                    notifyService.showError('Connection to database error. Please try again. ');
                });
        }
    };

    $scope.hideUserRequestContainer = function () {
        $scope.isPendingRequestHovered = false;
    };

    $scope.getFriendRequests = function () {
        if ($scope.hasPendingFriendRequest) {
            userService.getFriendRequests()
                .then(function (data) {
                    $scope.pendingFriendRequestData = data;
                    $scope.isPendingRequestHovered = true;
                }, function (error) {
                    notifyService.showError('Connection to database error. Please try again. ');
                })
        }
    };

    $scope.acceptFriendRequest = function (request) {
        userService.approveFriendRequest(request.id)
            .then(function (data) {
                $route.reload();
                notifyService.showInfo('Successfully accepted friend request!');
            }, function (error) {
                notifyService.showError('Failed to accept friend. Please try again. ');
            });
    };

    $scope.declineFriendRequest = function (request) {
        userService.declineFriendRequest(request.id)
            .then(function (data) {
                $route.reload();
                notifyService.showInfo('Successfully declined friend request!');
            }, function (error) {
                notifyService.showError('Failed to decline friend request. Please try again. ');
            });
    };

    $scope.sendFriendRequest = function (request) {
        userService.sendFriendRequest(request.username)
            .then(function (data) {
                notifyService.showInfo(data.message);
            }, function (error) {
                notifyService.showError('Failed to send friend request. Please try again. ');
            });
    };
    
    $scope.getWallsPost = function (username, pageSize) {
        $scope.userOnMyWall = username;
        if (username) {
            if (pageSize) {
                authorizationService.getUserWallFeed(username, pageSize)
                    .then(function (wallFeedData) {
                        $scope.wallFeeds = wallFeedData;
                    }, function (error) {
                        notifyService.showError('Connection to database error. Please try again. ');
                    });
            }else {
                authorizationService.getUserWallFeed(username)
                    .then(function (wallFeedData) {
                        $scope.wallFeeds = wallFeedData;
                    }, function (error) {
                        notifyService.showError('Connection to database error. Please try again. ');
                    });
            }
        }
    };

    $scope.searchByUsersFullName = function (substringOfFullname) {
        authorizationService.searchUsersByName(substringOfFullname)
            .then(function (userNamesData) {
                $scope.userNames = userNamesData;
            }, function (error) {
                notifyService.showError('Connection to database error. Please try again. ');
            })
    };

    $scope.addCommentToPost = function (feed, commentContent) {
        commentService.addCommentToPost(feed, commentContent)
            .then(function (data) {
                $('#inputLarge').val('');
                feed.comments.unshift(data);
                notifyService.showInfo('Successfully added comment to post!');
            }, function (error) {
                notifyService.showError('Failed to add comment. Please try again. ');
            });
    };
    
    $scope.getCommentsByPostId = function (postId) {
        commentService.getPostComments(postId)
            .then(function (allCommentsForPostData) {
                $scope.allCommentsForPost = allCommentsForPostData;
            }, function (error) {
                notifyService.showError('Connection to database error. Please try again. ');
            })
    };

    $scope.likeComment = function (post, comment) {
        commentService.likeComment(post.id, comment.id)
            .then(function (data) {
                comment.liked = data.liked;
                comment.likesCount = data.likesCount;
                notifyService.showInfo('Successfully liked comment!');
            }, function (error) {
                notifyService.showError('Failed to like comment. Please try again. ');
            });
    };

    $scope.dislikeComment = function (post, comment) {
        commentService.dislikeComment(post.id, comment.id)
            .then(function (data) {
                comment.liked = data.liked;
                comment.likesCount = data.likesCount;
                notifyService.showInfo('Successfully liked comment!');
            }, function (error) {
                notifyService.showError('Failed to dislike comment. Please try again. ');
            });
    };

    $scope.previewUser = function (feed) {
        authorizationService.getUserPreviewDataByUsername(feed.author.username)
            .then(function (userData) {
                $scope.hoveredUserPreviewData = userData;
                $scope.isUserHovered = true;
                $scope.hoveredPostId = feed.id;
            }, function (error) {
                notifyService.showError('Connection error. Please reload the page and check your internet connection');
            })
    };

    $scope.stopUserPreview = function () {
        $scope.isUserHovered = false;
    };
    
    $scope.getNewsFeed = function (pageSize) {
        if ($location.path() === Paths.feedPath && authorizationService.isLoggedIn()) {
            if (pageSize) {
                userService.getNewsFeed(pageSize)
                    .then(function (feedsData) {
                        $scope.newsFeeds = feedsData;
                    }, function (error) {
                        notifyService.showError('Feed Connection error. Please reload the page and check your internet connection');
                    });
            }else {
                userService.getNewsFeed()
                    .then(function (feedsData) {
                        $scope.newsFeeds = feedsData;
                    }, function (error) {
                        notifyService.showError('Feed Connection error. Please reload the page and check your internet connection');
                    });
            }
        }
    };

    $scope.showMore = function () {
        $scope.feedPageSize = ($scope.feedPageSize * 2) - 1;
        $scope.getNewsFeed(Math.ceil($scope.feedPageSize));
    };

    $scope.showLess = function () {
        $scope.feedPageSize = $scope.feedPageSize / 2;
        $scope.getNewsFeed(Math.ceil($scope.feedPageSize));
    };

    $scope.showMoreFeedsOnWall = function () {
        $scope.wallPageSize = ($scope.wallPageSize * 2) - 1;
        $scope.getWallsPost($routeParams.username, Math.ceil($scope.wallPageSize));
    };

    $scope.showLessFeedsOnWall = function () {
        $scope.wallPageSize = $scope.wallPageSize / 2;
        $scope.getWallsPost($routeParams.username, Math.ceil($scope.wallPageSize));
    };

    $scope.showEditBox = function (postId) {
        $rootScope.isEditActivated = !$rootScope.isEditActivated;
        $scope.editPostId = postId;
    };

    $scope.showAllComments = function (post) {
        commentService.getPostComments(post.id)
            .then(function (allPostsData) {
                var newPostsArray = allPostsData.slice(3);
                post.comments.pushArray(newPostsArray);
                $scope.areAllCommentsShown = true;
                $scope.postId = post.id;
            }, function (error) {
                notifyService.showError('Connection to the database failed. Please try again later.');
            })
    };

    // Function calls
    if (sessionStorage['access_token']) {
        if (sessionStorage['username'] !== $routeParams.username) {
            $scope.getFriendsFriendsPreview($routeParams.username);
        }

        $scope.getNewsFeed();
        $scope.userDetails();
        $scope.fillEditProfileData();
        $scope.getUserWall($routeParams.username);
        $scope.getWallsPost($routeParams.username);
        $interval(function () {
            if (sessionStorage['access_token']) {
                $scope.getUserPendingFriendRequests();
            }
        }, 5000);

        // Add page auto-refresh
    }

});