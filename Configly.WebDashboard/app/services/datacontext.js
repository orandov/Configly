(function () {
    'use strict';

    var serviceId = 'datacontext';
    angular.module('app').factory(serviceId, ['common', 'config', 'entityManagerFactory', 'model', datacontext]);

    function datacontext(common, config, emFactory, model) {

        var EntityQuery = breeze.EntityQuery;
        var getLogFn = common.logger.getLogFn;
        var entityNames = model.entityNames;
        var log = getLogFn(serviceId);
        var logError = getLogFn(serviceId, 'error');
        var logSuccess = getLogFn(serviceId, 'success');
        var manager = emFactory.newManager();
        var primePromise;
        var $q = common.$q;

        var storeMeta = {
            isLoaded: {
                settings: false,
                connections: false
            }
        };

        var service = {
            getConnections: getConnections,
            getSettings: getSettings,
            getSettingCount: getSettingCount,
            getConnectionCount: getConnectionCount,
            getPropHistory: getPropHistory,
            getEntityById: getEntityById,
            signalRChanged: signalRChanged,
            cancel: cancel,
            save: save,
            prime: prime
        };

        init();

        return service;

        function init() {
            setupEventForHasChangesChanged();
            setupEventForEntitiesChanged();
        }

        function getPropHistory(forceRemote) {
            forceRemote = true;
            var orderBy = 'timeStamp';
            var histories;

            //if (_areConnectionsLoaded() && !forceRemote) {
            //    connections = _getAllLocal(entityNames.connections, orderBy);
            //    return $q.when(connections);
            //}

            return EntityQuery.from('PropertyHistories')
                .select('user,timeStamp,oldValue,newValue.propertyId')
                .orderBy(orderBy)
                //.toType(entityNames.connection)
                .using(manager).execute()
                .then(querySucceeded, _queryFailed);

            function querySucceeded(data) {
                histories = data.results;
                //_areConnectionsLoaded(true);
                log('Retrieved [Property History] from remote data source', histories.length, true);
                return histories;
            }
        }

        function getConnections(forceRemote) {
            forceRemote = true;
            var orderBy = 'machineName';
            var connections;

            if (_areConnectionsLoaded() && !forceRemote) {
                connections = _getAllLocal(entityNames.connections, orderBy);
                return $q.when(connections);
            }

            return EntityQuery.from('Connections')
                .select('machineName,ip,timeStamp,scope,t')
                .orderBy(orderBy)
                //.toType(entityNames.connection)
                .using(manager).execute()
                .then(querySucceeded, _queryFailed);

            function querySucceeded(data) {
                connections = data.results;
                _areConnectionsLoaded(true);
                log('Retrieved [Connections] from remote data source', connections.length, true);
                return connections;
            }
        }

        function getSettings(forceRemote) {
            var orderBy = 'name';
            var settings;

            if (_areSettingsLoaded() && !forceRemote) {
                settings = _getAllLocal(entityNames.settings, orderBy);
                return $q.when(settings);
            }

            return EntityQuery.from('Settings')
                .select('settingId,name,properties')
                .orderBy(orderBy)
                .toType(entityNames.setting)
                .using(manager).execute()
                .then(querySucceeded, _queryFailed);

            function querySucceeded(data) {
                settings = data.results;
                _areSettingsLoaded(true);
                log('Retrieved [Setting] from remote data source', settings.length, true);
                return settings;
            }
        }

        function cancel() {
            if (manager.hasChanges()) {
                manager.rejectChanges();
                logSuccess('Canceled changes', null, true);
            }
        }

        function save() {

            return manager.saveChanges().then(saveSucceeded).catch(saveFailed);

            function saveSucceeded(result) {
                logSuccess('Saved data', result, true);
            }

            function saveFailed(error) {
                var msg = config.appErrorPrefix + 'Save failed: ' +
                    breeze.saveErrorMessageService.getErrorMessage(error);
                error.message = msg;
                logError(msg, error);
                throw error;
            }
        }

        function prime() {
            if (primePromise) return primePromise;

            primePromise = $q.all([getSettings(true)])
                 //.then(extendMetadata)
                .then(success);
            return primePromise;

            function success() {
                // setLookups();
                log('Primed the data');
            }
        }

        function getSettingCount() {
            if (_areSettingsLoaded()) {
                return $q.when(_getLocalEntityCount(entityNames.settings));
            }
            // settings aren't loaded; ask the server for a count.
            return EntityQuery.from('Settings')
                .take(0).inlineCount()
                .using(manager).execute()
                .then(_getInlineCount);
        }

        function getConnectionCount() {
            if (_areConnectionsLoaded()) {
                return $q.when(_getLocalEntityCount(entityNames.connections));
            }
            // settings aren't loaded; ask the server for a count.
            return EntityQuery.from('Connections')
                .take(0).inlineCount()
                .using(manager).execute()
                .then(_getInlineCount);
        }

        function _getInlineCount(data) { return data.inlineCount; }

        function _getAllLocal(resource, ordering, predicate) {
            return EntityQuery.from(resource)
                .orderBy(ordering)
                 .where(predicate)
                .using(manager)
                .executeLocally();
        }

        function getEntityById(entityName, id, forceRemote) {
            if (!forceRemote) {
                // Check cache first (synchronous)
                var entity = manager.getEntityByKey(entityName, id);
                if (entity && !entity.isPartial) {
                    log('Retrieved [' + entityName + '] id:' + id + ' from cache.', entity, true);
                    if (entity.entityAspect.entityState.isDeleted()) {
                        entity = null; // hide session marked-for-delete
                    }
                    // Should not need to call $apply because it is synchronous
                    return $q.when(entity);
                }
            }

            // It was not found in cache, so let's query for it.
            // fetchEntityByKey will go remote because 
            // 3rd parm is false/undefined. 
            return manager.fetchEntityByKey(entityName, id)
                .then(querySucceeded).catch(_queryFailed);

            function querySucceeded(data) {
                entity = data.entity;
                if (!entity) {
                    log('Could not find [' + entityName + '] id:' + id, null, true);
                    return null;
                }
                entity.isPartial = false;
                log('Retrieved [' + entityName + '] id ' + entity.id + ' from remote data source', entity, true);

                return entity;
            }
        }

        function _getLocalEntityCount(resource) {
            var entities = EntityQuery.from(resource)
                .using(manager)
                .executeLocally();
            return entities.length;
        }

        function _queryFailed(error) {
            var msg = config.appErrorPrefix + 'Error retreiving data.' + error.message;
            logError(msg, error);
            throw error;
        }

        function _areSettingsLoaded(value) {
            return _areItemsLoaded('settings', value);
        }

        function _areConnectionsLoaded(value) {
            return _areItemsLoaded('connections', value);
        }

        function _areItemsLoaded(key, value) {
            if (value === undefined) {
                return storeMeta.isLoaded[key]; // get
            }
            return storeMeta.isLoaded[key] = value; // set
        }

        //#region Internal methods   

        function setupEventForEntitiesChanged() {
            // We use this for detecting changes of any kind so we can save them to local storage
            manager.entityChanged.subscribe(function (changeArgs) {
                if (changeArgs.entityAction === breeze.EntityAction.PropertyChange) {
                    interceptPropertyChange(changeArgs);
                    common.$broadcast(config.events.entitiesChanged, changeArgs);
                }
            });
        }

        function signalRChanged(data) {
            getSettings(true).then(function (data) {
                log('SigmalR refreshed the data', data);
            });
        }

        function setupEventForHasChangesChanged() {
            manager.hasChangesChanged.subscribe(function (eventArgs) {
                var data = { hasChanges: eventArgs.hasChanges };
                common.$broadcast(config.events.hasChangesChanged, data);
            });
        }

        // Forget certain changes by removing them from the entity's originalValues
        // This function becomes unnecessary if Breeze decides that
        // unmapped properties are not recorded in originalValues
        //
        // We do this so we can remove the and isPartial properties from
        // the originalValues of an entity. Otherwise, when the object's changes
        // are canceled these values will also reset: isPartial will go
        // from false to true, and force the controller to refetch the
        // entity from the server.
        // Ultimately, we do not want to track changes to these properties, 
        // so we remove them.        
        function interceptPropertyChange(changeArgs) {
            var changedProp = changeArgs.args.propertyName;
            if (changedProp === 'isPartial') {
                delete changeArgs.entity.entityAspect.originalValues[changedProp];
            }
        }
        //#endregion
    }
})();