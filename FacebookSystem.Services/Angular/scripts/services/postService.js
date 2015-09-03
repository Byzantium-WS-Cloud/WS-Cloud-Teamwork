'use strict';

socialNetwork.factory('postService', function (BASE_URL, mainRequesterService) {
    var serviceUrl = BASE_URL + '/posts';
    var serviceRequester = {};

    serviceRequester.addPost = function (postRawData) {
        var post = {
            "postContent":postRawData.postContent ,
            "username":postRawData.username
        };
        return mainRequesterService.postRequest(serviceUrl, post);
    };
    
    serviceRequester.likePost = function (postId) {
        return mainRequesterService.postRequest(serviceUrl + '/' + postId + '/likes');
    };

    serviceRequester.dislikePost = function (postId) {
        return mainRequesterService.deleteRequest(serviceUrl + '/' + postId + '/likes');
    };

    serviceRequester.deletePostById = function (postId) {
        return mainRequesterService.deleteRequest(serviceUrl + '/'+postId);
    };

    serviceRequester.editPostById = function (post) {
        var postData = {
            "postContent": post.postContent
        };

        return mainRequesterService.putRequest(serviceUrl + '/' + post.id, postData);
    };

    return serviceRequester;
});