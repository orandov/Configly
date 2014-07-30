(function () {
    'use strict';

    var controllerId = 'connections';

    angular.module('app').controller(controllerId,
        ['$location', 'common', 'config', 'datacontext', connections]);

    function connections($location, common, config, datacontext) {
        var vm = this;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var keyCodes = config.keyCodes;
        vm.title = 'Connections';
        vm.connections = [];
        vm.filteredConnections = [];
        vm.search = search;
        vm.connectionSearch = '';
        vm.refresh = refresh;
       // vm.gotoSetting = gotoSetting;

        activate();

        function activate() {
            var promises = [getConnections()];
            common.activateController(promises, controllerId)
                .then(function () { log('Activated Connections View'); });
        }

        function getConnections(forceRefresh) {
            return datacontext.getConnections(forceRefresh).then(function (data) {
                vm.connections = data;
                applyFilter();
                return vm.connections;
            });
        };

        function refresh() { getConnections(true); }

        function search($event) {
            if ($event.keyCode === keyCodes.esc) {
                vm.connectionSearch = '';
            }
            applyFilter();
        }

        function applyFilter() {
            vm.filteredConnections = vm.connections.filter(connectionFilter);
        }

        function connectionFilter(connection) {
            var isMatch = vm.connectionSearch
                ? common.textContains(connection.machineName, vm.connectionSearch) || common.textContains(connection.t, vm.connectionSearch)
                : true;
            return isMatch;
        }

        //function gotoSetting(setting) {
        //    if (setting && setting.settingId) {
        //        $location.path('/setting/' + setting.settingId);
        //    }
        //}

    }
})();
