(function () {
    'use strict';

    var serviceId = 'model';

    
    angular.module('app').factory(serviceId,  model);

    function model() {


        var entityNames = {
            property: 'Property',
            setting: 'Setting',
            settings: 'Settings',
            connections: 'Connections',
            connection: 'Connection'
        };

        var service = {
            configureMetadataStore: configureMetadataStore,
            entityNames: entityNames
        };

        return service;

    
        function configureMetadataStore(metadataStore) {
            registerSetting(metadataStore);
        }

        function registerSetting(metadataStore) {
            metadataStore.registerEntityTypeCtor('Setting', Setting);
            //metadataStore.registerEntityTypeCtor('Connection', Connection);

            function Setting() { }
            //function Connection() { }

            Object.defineProperty(Setting.prototype, 'scopes', {
                get: function () {

                    var scopes = [];
                    this.properties.forEach(function(property) {
                        if (property.scope) {
                            scopes.push(property.scope);
                        }
                    });

                    var res = scopes.filter(function (value, index, self) {
                        return self.indexOf(value) === index;
                    });
                    return res.join(', ');

                },
               
            });
        }

    }
})();