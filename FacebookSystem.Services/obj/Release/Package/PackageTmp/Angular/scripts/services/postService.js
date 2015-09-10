'use strict';

socialNetwork.factory('postService', function (BASE_URL, mainRequesterService) {
    var serviceUrl = BASE_URL + '/posts';
    var serviceRequester = {};

    serviceRequester.addPost = function (postRawData) {
        var post = {
            "Content": postRawData.postContent,
            "WallOwnerUsername" : postRawData.username
        };
        return mainRequesterService.postRequest(serviceUrl + '/CreatePost', post);
    };
    
    serviceRequester.likePost = function (postId) {
        return mainRequesterService.postRequest(serviceUrl + '/' + postId + '/like');
    };

    serviceRequester.dislikePost = function (postId) {
        return mainRequesterService.deleteRequest(serviceUrl + '/' + postId + '/dislike');
    };

    serviceRequester.deletePostById = function (postId) {
        return mainRequesterService.deleteRequest(serviceUrl + '/' + postId + '/Delete');
    };

    serviceRequester.editPostById = function (post) {
        var postData = {
            "postContent": post.postContent
        };

        return mainRequesterService.putRequest(serviceUrl + '/' + post.id, postData);
    };

    return serviceRequester;
});