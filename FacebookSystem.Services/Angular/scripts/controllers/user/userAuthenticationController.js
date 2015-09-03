'use strict';

socialNetwork.controller('userAuthenticationController', function ($scope, $location, $http, $route, authorizationService, notifyService) {

    // Authorization token
    $http.defaults.headers.common['Authorization'] = sessionStorage['access_token'];

    $scope.go = function (path) {
        $location.path(path);
    };

    $scope.loginUser = function (loginData) {
        authorizationService.login(loginData)
            .then(function (data) {
                authorizationService.setUserCredentials(data);
                $location.path('/users/feeds');
                notifyService.showInfo('Login successfully');
            }, function (error) {
                notifyService.showError('Login failed ', error);
            });
    };

    $scope.registerUser = function (registerData) {
        authorizationService.register(registerData)
            .then(function (data) {
                authorizationService.setUserCredentials(data);
                $location.path('/users/feeds');
            }, function (error) {
                notifyService.showError('Register failed. Username/email already taken.', error);
            });
    };

    $scope.logoutUser = function () {
        authorizationService.logout()
            .then(function (data) {
                authorizationService.clearUserCredentials();
                authorizationService.clearUserTemporaryData();
                sessionStorage.clear();
                $location.path('/');
                window.location.reload();
                notifyService.showInfo('Successfully logged out!');
            }, function (err) {
                notifyService.showError('Logout failed ', error);
            })
    };
});