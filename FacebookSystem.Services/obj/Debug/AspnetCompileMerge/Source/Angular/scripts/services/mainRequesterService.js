'use strict';

socialNetwork.factory('mainRequesterService', function ($http, $q) {
    var baseService = {};

    baseService.postRequest = function (url, data) {
        var deffer = $q.defer();
        $http.post(url, data)
            .success(function (data) {
                deffer.resolve(data);
            })
            .error(function (error) {
                deffer.reject(error);
            });

        return deffer.promise;
    };
    
    baseService.getRequest = function (url, data) {
        var deffer = $q.defer();
        $http.get(url, data)
            .success(function (data) {
                deffer.resolve(data);
            })
            .error(function (error) {
                deffer.reject(error);
            });
        return deffer.promise;
    };

    baseService.putRequest = function (url, data) {
        var deffer = $q.defer();
        $http.put(url, data)
            .success(function (data) {
                deffer.resolve(data);
            })
            .error(function (error) {
                deffer.reject(error);
            });

        return deffer.promise;
    };

    baseService.deleteRequest = function (url, data) {
        var deffer = $q.defer();
        $http.delete(url, data)
            .success(function (data) {
                deffer.resolve(data);
            })
            .error(function (error) {
                deffer.reject(error);
            });

        return deffer.promise;
    };

    return baseService;
});