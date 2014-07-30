(function () {
    'use strict';

    var controllerId = 'history';

    angular.module('app').controller(controllerId,
        ['$location', 'common', 'config', 'datacontext', '$window', history]);

    function history($location, common, config, datacontext, $window) {
        var vm = this;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var keyCodes = config.keyCodes;
        vm.title = 'History';
        vm.histories = [];
        vm.goBack = goBack;
        vm.refresh = refresh;

        activate();

        function activate() {
            var promises = [getPropHistory()];
            common.activateController(promises, controllerId)
                .then(function () { log('Activated history View'); });
        }

        function getPropHistory(forceRefresh) {
            return datacontext.getPropHistory(forceRefresh).then(function (data) {
                vm.histories = data;
                return vm.histories;
            });
        };

        function refresh() { getPropHistory(true); }

        function goBack() { $window.history.back(); }

    }
})();
