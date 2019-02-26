/*
The MIT License (MIT)

Copyright (c) 2014 Lee Prosser

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
(function (factory) {
    // Module systems magic dance.

    if (typeof require === "function" && typeof exports === "object" && typeof module === "object") {
        // CommonJS or Node: hard-coded dependency on "knockout"
        factory(require("knockout"), require("chart"), exports);
    } else if (typeof define === "function" && define["amd"]) {
        // AMD anonymous module with hard-coded dependency on "knockout"
        define(["knockout", "chart", "exports"], factory);
    } else {
        // <script> tag: use the global `ko` object, attaching a `mapping` property
        factory(ko, Chart);
    }
}
(function (ko, Chart, exports) {

    ko.observableGroup = function(observables) {
        var observableManager = {};
        var throttle = 0;
        var throttleTimeout;

        observableManager.throttle = function(duration) {
            throttle = duration;
            return observableManager;
        };

        observableManager.subscribe = function(handler) {
            function throttledHandler(val) {
                if(throttle > 0) {
                    if(!throttleTimeout) {
                        throttleTimeout = setTimeout(function() {
                            throttleTimeout = undefined;
                            handler(val);
                        }, throttle);
                    }
                }
                else
                { handler(val); }
            }

            for(var i = 0; i < observables.length; i++)
            { observables[i].subscribe(throttledHandler); }

            return observableManager;
        };

        return observableManager;
    };

    var getType = function(obj) {
        if ((obj) && (typeof (obj) === "object") && (obj.constructor == (new Date).constructor)) return "date";
        return typeof obj;
    };

    var getSubscribables = function(model) {
        var subscribables = [];
        scanForObservablesIn(model, subscribables);
        return subscribables;
    };

    var scanForObservablesIn = function(model, subscribables){
        for (var parameter in model)
        {
            var typeOfData = getType(model[parameter]);
            switch(typeOfData)
            {
                case "object": { scanForObservablesIn(model[parameter], subscribables); } break;
                case "array":
                {
                    var underlyingArray = model[parameter]();
                    underlyingArray.forEach(function(entry, index){ scanForObservablesIn(underlyingArray[index], subscribables); });
                }
                break;

                default:
                {
                    if(ko.isComputed(model[parameter]) || ko.isObservable(model[parameter]))
                    { subscribables.push(model[parameter]); }
                }
                break;
            }
        }
    };

    ko.bindingHandlers.chart = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var allBindings = allBindingsAccessor();
            var chartBinding = allBindings.chart;
            var activeChart;
            var chartData;

            var createChart = function() {
                var chartType = ko.unwrap(chartBinding.type);
                var data = ko.toJS(chartBinding.data);
                var options = ko.toJS(chartBinding.options);

                chartData = {
                    type: chartType,
                    data: data,
                    options: options
                };

                activeChart = new Chart(element, chartData);
            };

            var refreshChart = function() {
                chartData.data = ko.toJS(chartBinding.data);
                activeChart.update();
                activeChart.resize();
            };

            var subscribeToChanges = function() {
                var throttleAmount = ko.unwrap(chartBinding.options.throttle) || 100;
                var dataSubscribables = getSubscribables(chartBinding.data);
                console.log("found obs", dataSubscribables);

                ko.observableGroup(dataSubscribables)
                    .throttle(throttleAmount)
                    .subscribe(refreshChart);
            };

            createChart();

            if(chartBinding.options && chartBinding.options.observeChanges)
            { subscribeToChanges(); }
        }
    };

}));