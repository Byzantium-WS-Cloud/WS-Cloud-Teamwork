'use strict';

socialNetwork.factory('userService', function (BASE_URL, PAGE_SIZE, mainRequesterService) {
    var serviceUrl = BASE_URL + '/me';
    var serviceRequester = {};

    serviceRequester.editProfile = function (newData) {
        var profileData = {
            "name": newData.name,
            "email": newData.email,
            "gender": newData.gender,
            "profileImageData": newData.profileImageData,
            "coverImageData": newData.coverImageData
        };

        return mainRequesterService.putRequest(serviceUrl, profileData);
    };
    
    serviceRequester.getDataAboutMe = function () {
        return mainRequesterService.getRequest(serviceUrl, {});
    };
    
    serviceRequester.changeUserPassword = function (userData) {
        var newUserPassword = {
            "oldPassword": userData.oldPassword,
            "newPassword": userData.newPassword,
            "confirmPassword": userData.confirmPassword
        };

        return mainRequesterService.putRequest(serviceUrl + '/ChangePassword', newUserPassword);
    };
    
    serviceRequester.getOwnFriends = function () {
        return mainRequesterService.getRequest(serviceUrl + '/friends', {});
    };

    serviceRequester.getFriendRequests = function () {
        return mainRequesterService.getRequest(serviceUrl + '/requests', {});
    };

    serviceRequester.approveFriendRequest = function (requestId) {
        return mainRequesterService.putRequest(serviceUrl + '/requests/' + requestId + '?status=approved', {});
    };

    serviceRequester.declineFriendRequest = function (requestId) {
        return mainRequesterService.putRequest(serviceUrl + '/requests/' + requestId + '?status=rejected', {});
    };

    serviceRequester.sendFriendRequest = function (username) {
        return mainRequesterService.postRequest(serviceUrl + '/requests/' + username);
    };
    
    serviceRequester.getNewsFeed = function (pageSize) {
        var url;
        if (pageSize) {
            url = serviceUrl + '/feed?StartPostId=&PageSize='+pageSize;
        }else {
            url = serviceUrl + '/feed?StartPostId=&PageSize='+PAGE_SIZE;
        }

        return mainRequesterService.getRequest(url, {});
    };

    return serviceRequester;
});