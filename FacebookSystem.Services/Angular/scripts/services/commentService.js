'use strict';

socialNetwork.factory('commentService', function (BASE_URL, mainRequesterService) {
    var serviceUrl = BASE_URL + '/posts/';
    var serviceRequester = {};
    
    serviceRequester.addCommentToPost = function (feed, commentContent) {
        var postId = feed.id;
        var comment = {
            "Content": commentContent
        };

        return mainRequesterService.postRequest(serviceUrl + postId + '/comments', comment);
    };

    serviceRequester.getPostComments = function (postId) {
        return mainRequesterService.getRequest(serviceUrl + postId + '/comments', {});
    };

    serviceRequester.likeComment = function (postId, commentId) {
        var url = BASE_URL + '/comments/' + commentId + '/likes';
        return mainRequesterService.postRequest(url, {});
    };

    serviceRequester.dislikeComment = function (postId, commentId) {
        var url = BASE_URL + '/comments/' + commentId + '/likes';
        return mainRequesterService.deleteRequest(url, {});
    };

    serviceRequester.deleteComment = function (postId, commentId) {
        var url = serviceUrl + postId + '/comments/' + commentId;

        return mainRequesterService.deleteRequest(url, {});
    };

    return serviceRequester;    
});