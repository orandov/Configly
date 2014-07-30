(function () {
    'use strict';

    var controllerId = 'settings';

    angular.module('app').controller(controllerId,
        ['$location', 'common', 'config', 'datacontext', settings]);

    function settings($location,common, config, datacontext) {
        var vm = this;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var keyCodes = config.keyCodes;
        vm.title = 'Settings';
        vm.settings = [];
        vm.filteredSettings = [];
        vm.search = search;
        vm.settingSearch = '';
        vm.refresh = refresh;
        vm.gotoSetting = gotoSetting;


        activate();


        function activate() {
            var promises = [getSettings()];
            common.activateController(promises, controllerId)
                .then(function () { log('Activated Settings View'); });
        }


        function getSettings(forceRefresh) {
            return datacontext.getSettings(forceRefresh).then(function (data) {
                vm.settings = data;
                applyFilter();
                return vm.settings ;

            });
        };

        function refresh() { getSettings(true); }

        function search($event) {
            if ($event.keyCode === keyCodes.esc) {
                vm.settingSearch = '';
            }
            applyFilter();
        }

        function applyFilter() {
            vm.filteredSettings = vm.settings.filter(settingFilter);
        }

        function settingFilter(setting) {
            var isMatch = vm.settingSearch
                ? common.textContains(setting.name, vm.settingSearch)
                : true;
            return isMatch;
        }

        function gotoSetting(setting) {
            if (setting && setting.settingId) {
                $location.path('/setting/' + setting.settingId);
            }
        }

    }
})();
