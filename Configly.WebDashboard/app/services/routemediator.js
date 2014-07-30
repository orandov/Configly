(function () {
    'use strict';

    var serviceId = 'routemediator';

    // TODO: replace app with your module name
    angular.module('app').factory(serviceId,
        ['$location','$rootScope','config','logger', routemediator]);

    function routemediator($location, $rootScope, config, logger) {
     
        var handleRouteChangeError = false;
        var service = {
            setRoutingHandlers: setRoutingHandlers
        };

        return service;

        function setRoutingHandlers() {
            updateDocTitle();
            handleRoutingErrors();
        }

        function handleRoutingErrors() {
            $rootScope.$on('$routeChangeError',
                function (event, current, previous, rejection) {
                    if (handleRouteChangeError) { return; }
                    handleRouteChangeError = true;
                    var msg = 'Error routing: ' + (current && current.name)
                        + '. ' + (rejection.msg || '');
                    logger.logWarning(msg, current, serviceId, true);
                    $location.path('/');
                });
        }

        function updateDocTitle() {
            $rootScope.$on('$routeChangeSuccess',
                function (event, current, previous) {
                    handleRouteChangeError = false;
                    var title = config.docTitle + ' ' + (current.title || '');
                    $rootScope.title = title;
                });
        }
    }
})();