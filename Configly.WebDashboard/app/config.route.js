(function () {
    'use strict';

    var app = angular.module('app');

    // Collect the routes
    app.constant('routes', getRoutes());
    
    // Configure the routes and route resolvers
    app.config(['$routeProvider', 'routes', routeConfigurator]);
    function routeConfigurator($routeProvider, routes) {

        routes.forEach(function (r) {
            setRoute(r.url, r.config);
        });
        $routeProvider.otherwise({ redirectTo: '/' });


        function setRoute(url, definition) {
            // Sets resolvers for all of the routes
            // by extending any existing resolvers (or creating a new one).
            definition.resolve = angular.extend(definition.resolve || {}, {
                prime: prime
            });
            $routeProvider.when(url, definition);
            return $routeProvider;
        }

    }

    prime.$inject = ['datacontext'];
    function prime(dc) { return dc.prime(); }

    // Define the routes 
    function getRoutes() {
        return [
            {
                url: '/',
                config: {
                    templateUrl: 'app/dashboard/dashboard.html',
                    title: 'Dashboard',
                    settings: {
                        nav: 1,
                        content: '<i class="fa fa-dashboard"></i> Dashboard'
                    }
                }
            },
            {
                url: '/settings',
                config: {
                    title: 'Settings',
                    templateUrl: 'app/setting/settings.html',
                    settings: {
                        nav: 2,
                        content: '<i class="fa fa-cogs"></i> Settings'
                    }
                }
            },
            {
                url: '/setting/:id',
                config: {
                    templateUrl: 'app/setting/settingdetail.html',
                    title: 'setting',
                    settings: {}
                }
            },
            {
                url: '/connections',
                config: {
                    title: 'Connections',
                    templateUrl: 'app/connection/connections.html',
                    settings: {
                        nav: 2,
                        content: '<i class="fa fa-chain"></i> Connections'
                    }
                }
            },
            {
                url: '/clientapi',
                config: {
                    title: 'Client Api',
                    templateUrl: 'app/clientapi/clientapi.html',
                    settings: {
                        nav: 2,
                        content: '<i class="fa fa-question-circle"></i> Client API'
                    }
                }
            },
             {
                 url: '/property/history/:id',
                 config: {
                     templateUrl: 'app/property/history.html',
                     title: 'Property History',
                     settings: {}
                 }
             },
              {
                  url: '/property/override/:id',
                  config: {
                      templateUrl: 'app/property/override.html',
                      title: 'Override Property',
                      settings: {}
                  }
              }
        ];
    }
})();