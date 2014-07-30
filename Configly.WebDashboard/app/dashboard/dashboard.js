(function () {
    'use strict';
    var controllerId = 'dashboard';
    angular.module('app').controller(controllerId, ['common', 'datacontext', dashboard]);

    function dashboard(common, datacontext) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var vm = this;
        vm.settingCount = 0;
        vm.connectionCount = 0;
       
        vm.title = 'Dashboard';

        activate();

        function activate() {
            var promises = [getSettingCount(), getConnectionCount()];
            common.activateController(promises, controllerId)
                .then(function () { log('Activated Dashboard View'); });
        }

        function getSettingCount() {
            return datacontext.getSettingCount().then(function (data) {
                return vm.settingCount = data;
            });
        }
        function getConnectionCount() {
            return datacontext.getConnectionCount().then(function (data) {
                return vm.connectionCount = data;
            });
        }

       
    }
})();