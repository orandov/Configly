(function () {
    'use strict';
    var controllerId = 'clientapi';
    angular.module('app').controller(controllerId, ['common', 'datacontext', clientapi]);

    function clientapi(common, datacontext) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var vm = this;

        vm.title = 'Client API';

        activate();

        function activate() {
            var promises = [];
            common.activateController(promises, controllerId)
                .then(function () { log('Client API View'); });
        }
    }
})();