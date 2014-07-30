(function () {
    'use strict';

    var controllerId = 'override';

    angular.module('app').controller(controllerId,
        ['$location', 'common', 'config', 'datacontext', '$window', override]);

    function override($location, common, config, datacontext, $window) {
        var vm = this;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var keyCodes = config.keyCodes;
        vm.title = 'Override';
        vm.goBack = goBack;

        activate();

        function activate() {
            var promises = [];
            common.activateController(promises, controllerId)
                .then(function () { log('Activated Override View'); });
        }

        function goBack() { $window.history.back(); }

    }
})();
