// Write your JavaScript code.
(function () {
    function showAlert(alert) {
        const alertContainer = $('.alert-container');

        const alertElement = $(`<div class="alert alert-${alert.type} alert-dismissible" role="alert">` +
            '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
            `<strong>${alert.title}</strong> ${alert.body}` +
            '</div>');

        alertContainer.append(alertElement);
        alertElement.alert();
    }

    $(document).ajaxComplete((event, xhr) => {
        if (xhr.getResponseHeader('x-alert-type')) {
            const alert = {
                type: xhr.getResponseHeader('x-alert-type'),
                title: xhr.getResponseHeader('x-alert-title'),
                body: xhr.getResponseHeader('x-alert-body')
            }

            showAlert(alert);
        }
    });
})();


var DblAmericano = $("#DoubleAmericano").data("value");
var SwtLatte = $("#SweetLatte").data("value");
var FltWhite = $("#FlatWhite").data("value");

var CoffeeBean = $("#CoffeeBean").data("value");
var Sugar = $("#Sugar").data("value");
var Milk = $("#Milk").data("value");

var BeanBag = $("#BeanBag").data("value");
var SugarPack = $("#SugarPack").data("value");
var MilkCarton = $("#MilkCarton").data("value");

window.onload = function () {
    CanvasJS.addColorSet("coffeeSet", [
        '#873600',
        '#E67E22',
        '#F5CBA7']);

    var piechart = new CanvasJS.Chart("pieChart", {
        exportEnabled: true,
        animationEnabled: true,
        colorSet: "coffeeSet",
        title: {
            text: "Distribution of Orders"
        },
        legend: {
            cursor: "pointer",
            itemclick: explodePie
        },
        data: [{
            type: "pie",
            showInLegend: true,
            toolTipContent: "{name}: <strong>{y}</strong>",
            indexLabel: "{name} - {y}",
            dataPoints: [
                { y: DblAmericano, name: "Double Americano" },
                { y: SwtLatte, name: "Sweet Latte" },
                { y: FltWhite, name: "Flat White" }]
        }]
    });

    var barchartunit = new CanvasJS.Chart("barChartUnit", {
        exportEnabled: true,
        animationEnabled: true,
        colorSet: "coffeeSet",
        title: {
            text: "Ingredients Stock"
        },
        axisX: {
            interval: 1
        },
        axisY2: {
            interlacedColor: "rgba(1,77,101,.2)",
            gridColor: "rgba(1,77,101,.1)",
            title: "by number of units",
            interval: 5
        },
        data: [{
            type: "bar",
            name: "companies",
            axisYType: "secondary",
            color: "#873600",
            dataPoints: [
                { y: Milk, label: "Milk" },
                { y: Sugar, label: "Sugar" },
                { y: CoffeeBean, label: "Coffee Bean" }
            ]
        }]
    });

    var barchartcontainer = new CanvasJS.Chart("barChartContainer", {
        exportEnabled: true,
        animationEnabled: true,
        colorSet: "coffeeSet",
        title: {
            text: "Ingredients Stock"
        },
        axisX: {
            interval: 1
        },
        axisY2: {
            interlacedColor: "rgba(1,77,101,.2)",
            gridColor: "rgba(1,77,101,.1)",
            title: "by number of container",
            interval: 1
        },
        data: [{
            type: "bar",
            name: "companies",
            axisYType: "secondary",
            color: "#873600",
            dataPoints: [
                { y: MilkCarton, label: "Milk Cartons" },
                { y: SugarPack, label: "Packs of Sugar" },
                { y: BeanBag, label: "Bags of Coffee Bean" }
            ]
        }]
    });

    piechart.render();
    barchartunit.render();
    barchartcontainer.render();
}

function explodePie(e) {
    if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
        e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
    } else {
        e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
    }
    e.piechart.render();
}

