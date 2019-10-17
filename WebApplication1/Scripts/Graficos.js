var chart = new CanvasJS.Chart("chartContainer", {
    theme: "light2",
    animationEnabled: true,
    title: {
        text: "Saldos Mensais"
    },
    subtitles: [
        { text: "Intervalo de 12 meses" }
    ],
    axisX: {
        valueFormatString: "MMM",
        labelAngle: -75
    },
    axisY: {
        valueFormatString: "R$#,###.#0",
    },
    data: [
        {
            indexLabel: "{y} ",
            yValueFormatString: "R$#,###.#0",
            type: "column", //change type to bar, line, area, pie, etc
            dataPoints: dataPoints1,
        }
    ]
});
chart.render();