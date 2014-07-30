(function () {
    'use strict';

    var serviceId = 'signalR';

    // TODO: replace app with your module name
    angular.module('app').factory(serviceId,
        ['$rootScope', 'common', 'config', 'datacontext', signalR]);

    function signalR($rootScope, common, config, datacontext) {

        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(serviceId);
        var logError = getLogFn(serviceId, 'error');
        var logSuccess = getLogFn(serviceId, 'success');
        var primePromise;
        var $q = common.$q;

        var proxy = null;
        var url = 'signalr';
        var qs = { "tname": "WebDashboard" };
        var service = {
            init: init,
            updateSetting: updateSetting
        };

        return service;

        function init() {
            if (primePromise) return primePromise;

            primePromise = $q.all([connect()])
                .then(success);
            return primePromise;

            function success() {
                log('Connected to signalR hub');
            }
        }

        function connect() {
            $.connection.hub.url = url;

            proxy = $.connection.settings;

            proxy.client.settingsRefreshed = function (data) {
                log('settingsRefreshed fired', data, true);
                datacontext.signalRChanged(data);
            };

            $.connection.hub.qs = qs;
            $.connection.hub.logging = true;
            $.connection.hub.error(failedConnection);

            return $q.when($.connection.hub.start()
                            .done(startDone)
                            .fail(startFailure));

        }

        function updateSetting(setting) {
            proxy.invoke('setSetting', setting);
        }

        function startDone() {
            $.connection.hub.logging = true;
            // seems to be a bug in CORs signalR client library that
            // the URL host in the connection object is not passed through to the hub
            $.connection.hub.url = url;
            logSuccess('Connection Established.', true);
        }

        function startFailure(error) {
            var msg = config.appErrorPrefix + 'Connection failed: ';
            error.message = msg;
            logError(msg, error);
            throw error;
        }

        function failedConnection(error) {
            var msg = config.appErrorPrefix + 'Error occured on the hub.' + error.message;
            logError(msg, error);
            throw error;
        }
    }
})();