'use strict';

socialNetwork.factory('authorizationService', function (BASE_URL, PAGE_SIZE, mainRequesterService) {
    var serviceUrl = BASE_URL + '/users';
    var serviceRequester = {};

    serviceRequester.login = function (loginData) {
        return mainRequesterService.postRequest(serviceUrl + '/login', loginData);
    };

    serviceRequester.register = function (rawRegisterData) {
        var registerData = {
            "username": rawRegisterData.username,
            "password": rawRegisterData.password,
            "confirmPassword": rawRegisterData.confirmPassword,
            "name": rawRegisterData.name,
            "email": rawRegisterData.email,
            "gender": rawRegisterData.gender
        };
        return mainRequesterService.postRequest(serviceUrl + '/register', registerData);
    };

    serviceRequester.logout = function () {
        return mainRequesterService.postRequest(serviceUrl + '/logout', {});
    };

    serviceRequester.getUserPreviewData = function () {
        var currentUser = sessionStorage['username'];
        var url = serviceUrl + '/' + currentUser + '/preview';

        return mainRequesterService.getRequest(url, {});
    };

    serviceRequester.getUserPreviewDataByUsername = function (username) {
        return mainRequesterService.getRequest(serviceUrl + '/' + username + '/preview?limit=1');
    };

    serviceRequester.getUserFullData = function (username) {
        return mainRequesterService.getRequest(serviceUrl + '/' + username, {});
    };

    serviceRequester.getFriendsFriendsPreview = function (username) {
        var url = serviceUrl + '/' + username + '/friends/preview';
        return mainRequesterService.getRequest(url, {});
    };

    serviceRequester.setUserCredentials = function (userCredentials) {
        sessionStorage['username'] = userCredentials.userName;
        sessionStorage['access_token'] = 'bearer ' + userCredentials.access_token;
    };

    serviceRequester.getUserWallFeed = function (username, pageSize) {
        var url;
        if (pageSize) {
            url = serviceUrl + '/' + username + '/wall?StartPostId=&PageSize='+pageSize;
        }else {
            url = serviceUrl + '/' + username + '/wall?StartPostId=&PageSize='+PAGE_SIZE;
        }

        return mainRequesterService.getRequest(url, {});
    };

    serviceRequester.searchUsersByName = function (fullName) {
        var url = serviceUrl + '/search?searchTerm=' + fullName;
        return mainRequesterService.getRequest(url, {});
    };

    serviceRequester.clearUserCredentials = function () {
        if (sessionStorage.hasOwnProperty('username')) {
            sessionStorage.removeItem('username');
        }
        if (sessionStorage.hasOwnProperty('access_token')) {
            sessionStorage.removeItem('access_token');
        }
    };

    serviceRequester.clearUserTemporaryData = function () {
        if (sessionStorage.hasOwnProperty('currentUserprofilepic')) {
            sessionStorage.removeItem('currentUserprofilepic');
        }
        if (sessionStorage.hasOwnProperty('currentUsername')) {
            sessionStorage.removeItem('currentUsername');
        }
    };

    serviceRequester.GetHeaders = function() {
        return {
            Authorization: "Bearer " + sessionStorage['access_token']
        };
    };

    serviceRequester.isLoggedIn = function () {
        if (sessionStorage['access_token']) {
            return true;
        }

        return false;
    };

    return serviceRequester;
});