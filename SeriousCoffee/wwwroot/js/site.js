angular.module('seriousCoffeeModule', ['datatables', 'chart.js'])

    .config(['ChartJsProvider', function (ChartJsProvider) {
        ChartJsProvider.setOptions({
            responsive: true,
            borderWidth: 1,
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true,
                        stepSize: 1
                    }
                }]
            }
        });
    }])

    .controller('OrderCoffeeController', ['$rootScope', '$scope', '$http', function ($rootScope, $scope, $http) {

        var orderCoffee = this;

        orderCoffee.orderCoffee = function () {
            $http({
                method: 'POST',
                url: "/api/CoffeeOrder",
                data: {
                    "Id": parseInt(orderCoffee.coffeeOrderId)
                }
            }).then(function (response) {
                orderCoffee.coffeeOrderId = '';
                $rootScope.$emit("updateOrderHistory", {});
                $rootScope.$emit("updateStocksBarChart", {});
                $scope.orderCoffeeForm.$setPristine();
                alert("Your drink is now ready!");
            }, function (response) {
                alert("Ooops, unable to make this drink :(");
            });
        };

        $http({
            method: 'GET',
            url: "/api/coffee"
        }).then(function (response) {
            $scope.orderCoffee.coffeeOptions = response.data;
        });

    }]).controller('OrderHistoryController', ['$rootScope', '$scope', '$http', 'DTOptionsBuilder', 'DTColumnDefBuilder', function ($rootScope, $scope, $http, DTOptionsBuilder, DTColumnDefBuilder) {

        var orderHistory = this;
        orderHistory.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(3)
            .withPaginationType('simple')
            .withOption('lengthChange', false);

        updateOrderHistory(function (response) {
            $scope.orderHistory.coffeeOrders = response.data;

            var coffeeDistribution = generateCoffeeDistribution(response.data);
            var labels = Object.keys(coffeeDistribution);
            var data = Object.values(coffeeDistribution);

            $scope.labels = labels;
            $scope.data = data;
            $scope.$on('chart-create', function (evt, chart) {
                $scope.coffeDistributionChart = chart;
                var gradientStroke = produceGradientStroke(chart.chart.ctx, labels.length);
                chart.chart.config.data.datasets[0].backgroundColor = gradientStroke;
                chart.chart.config.data.datasets[0].borderColor = gradientStroke;
                chart.chart.config.data.datasets[0].hoverBackgroundColor = gradientStroke;
                chart.chart.config.data.datasets[0].hoverBorderColor = gradientStroke;
                chart.update();
            });
        });

        function generateCoffeeDistribution(coffeeOrders) {
            var coffeeDistribution = {};
                $.each(coffeeOrders, function (index, coffeeOrder) {
                    if (!coffeeDistribution[coffeeOrder.coffeeName])
                        coffeeDistribution[coffeeOrder.coffeeName] = 0;
                    coffeeDistribution[coffeeOrder.coffeeName] = coffeeDistribution[coffeeOrder.coffeeName] + 1;
                });
            return coffeeDistribution;
            }

        $rootScope.$on("updateOrderHistory", function () {
            updateOrderHistory(function (response) {
                $scope.orderHistory.coffeeOrders = response.data;   
                var coffeeDistribution = generateCoffeeDistribution(response.data);
                $scope.labels = Object.keys(coffeeDistribution);
                $scope.data = Object.values(coffeeDistribution);
            });
        });

        function updateOrderHistory(successCallback) {
            $http({
                method: 'GET',
                url: "/api/coffeeOrder"
            }).then(successCallback);  
        }    
        
    }]).controller("StocksController", ['$scope', '$rootScope', '$http', function ($scope, $rootScope, $http) {

        var stocks = this;

        stocks.addStocks = function () {
            $http({
                method: 'POST',
                url: "/api/Stock",
                data: {
                    "IngredientId": parseInt(stocks.ingredientId),
                    "ContainerCount": parseInt(stocks.containerCount)
                }
            }).then(function (response) {
                stocks.ingredientId = '';
                stocks.containerCount = '';
                updateStocksBarChart();
                $scope.addStockForm.$setPristine();                
                alert("Stocks has been added.");
            }, function (response) {
                alert("Ooops, unable to add more stocks.");
            });
        };


        $http({
            method: 'GET',
            url: "/api/Ingredient"
        }).then(function (response) {
            var ingredients = new Array();
            $.each(response.data, function (index, ingredient) {
                ingredients.push(ingredient.name);
            });

            $scope.stocks.ingredientOptions = response.data;  
            $scope.labels = ingredients;  

            $scope.$on('chart-create', function (evt, chart) {
                $scope.stocksChart = chart;
                var gradientStroke = produceGradientStroke(chart.chart.ctx, ingredients.length);
                chart.chart.config.data.datasets[0].backgroundColor = gradientStroke;
                chart.chart.config.data.datasets[0].borderColor = gradientStroke;
                chart.chart.config.data.datasets[0].hoverBackgroundColor = gradientStroke;
                chart.chart.config.data.datasets[0].hoverBorderColor = gradientStroke;
                chart.update();      
            });
            var container = new Array();
            getStocks(0, response.data, container);
            $scope.data = container;
   
        }); 

        $rootScope.$on("updateStocksBarChart", function () {
            updateStocksBarChart();
        });

        function updateStocksBarChart() {
            getStocks(0, $scope.stocks.ingredientOptions, new Array(), function (container) {
                $scope.stocksChart.data.datasets[0].data = container;
                $scope.stocksChart.update();
            });
        }

        function getStocks(index, ingredients, container, callback) {
            if (index >= ingredients.length) {
                if (callback)
                    callback(container);
                return
            }
            var ingredient = ingredients[index];
            $http({
                method: 'GET',
                url: "/api/Stock/" + ingredient.id 
            }).then(function (response) {
                container.push(response.data); 
                getStocks(++index, ingredients, container, callback);
            }); 
        }
    }]);

$.fn.dataTable.moment('dd/MM/yyyy HH:mm');

function produceGradientStroke(context, variationSize) {
    var gradientColors = ["#f44336", "#e91e63", "#9c27b0", "#673ab7", "#3f51b5", "#2196f3", "#03a9f4", "#00bcd4", "#009688", "#4caf50", "#8bc34a", "#cddc39", "#ffeb3b", "#ffc107", "#ff9800"]
    var gradientStroke = context.createLinearGradient(0, 0, ((variationSize + gradientColors.length) /2) * 200, 0);
    $.each(gradientColors, function (index, color) {
        gradientStroke.addColorStop(index != 0 ? index / gradientColors.length : 0, color);
    });
    return gradientStroke;
}