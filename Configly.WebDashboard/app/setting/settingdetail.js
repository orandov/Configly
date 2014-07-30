(function () {
    'use strict';

    var controllerId = 'settingdetail';

    angular.module('app').controller(controllerId,
        ['$location', '$scope', '$routeParams', '$window',
            'common', 'config', 'datacontext', 'model','signalR', settingdetail]);

    function settingdetail($location, $scope, $routeParams, $window,
        common, config, datacontext, model, signalR) {
        var vm = this;
        var logError = common.logger.getLogFn(controllerId, 'error');
        var logWarning = common.logger.getLogFn(controllerId, 'warn');
        var $q = common.$q;
        var entityName = model.entityNames.setting;
        vm.cancel = cancel;
        vm.goBack = goBack;
        vm.hasChanges = false;
        vm.isSaving = false;
        vm.save = save;
        vm.setting = undefined;
        vm.settings = [];

        Object.defineProperty(vm, 'canSave', { get: canSave });

        activate();

        function activate() {
            onDestroy();
            onHasChanges();
            common.activateController([getRequestedSetting()], controllerId)
                .then(onEveryChange);
        }

     

        function cancel() {
            datacontext.cancel();
          
            if (vm.setting.entityAspect.entityState.isDetached()) {
                gotoSettings();
            }
        }

        function gotoSettings() { $location.path('/settings'); }

        function canSave() { return vm.hasChanges && !vm.isSaving; }

        function getRequestedSetting() {
            var val = $routeParams.id;

            return datacontext.getEntityById(entityName, val)
            .then(function (data) {
                if (data) {
                    vm.setting = data.entity || data;
                } else {
                    logWarning('Could not find setting id = ' + val);
                    gotoSettings();
                }
            })
            .catch(function (error) {
                logError('Error while getting setting id = ' + val + "; " + error);
                gotoSettings();
            });
        }

        function goBack() { $window.history.back(); }

     

        function onDestroy() {
            $scope.$on('$destroy', function () {
             
                datacontext.cancel();
            });
        }

        function onHasChanges() {
            $scope.$on(config.events.hasChangesChanged,
                function(event, data) {
                     vm.hasChanges = data.hasChanges;
                });
        }

       
        function onEveryChange() {
            $scope.$on(config.events.entitiesChanged, function(event, data) {
                
            });
        }
       

        function save() {
            if (!canSave()) { return $q.when(null); } // Must return a promise

            vm.isSaving = true;
            return datacontext.save().then(function (saveResult) {
                vm.isSaving = false;
                

            }).catch(function (error) {
                vm.isSaving = false;
            });
        }

       
    }
})();
