(function () {
    'use strict';

    var serviceId = 'entityManagerFactory';
    angular.module('app').factory(serviceId, ['breeze', 'config', 'model', emFactory]);

    function emFactory(breeze, config, model) {
        breeze.config.initializeAdapterInstance('modelLibrary', 'backingStore', true);
        breeze.NamingConvention.camelCase.setAsDefault();

        new breeze.ValidationOptions({ validateOnAttach: false }).setAsDefault();

        var serviceName = config.remoteServiceName;
        var metadataStore = createMetadataStore();


        var provider = {
            metadataStore: metadataStore,
            newManager: newManager
        };

        return provider;


        function createMetadataStore() {
            var store = new breeze.MetadataStore();
            model.configureMetadataStore(store);
            return store;
        }

        function newManager() {
            var mgr = new breeze.EntityManager({
                serviceName: serviceName,
                metadataStore: metadataStore
            });
            return mgr;
        }
    }
})();