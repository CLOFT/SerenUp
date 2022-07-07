setInterval(UpdateHour, 1000);
setInterval(UpdateDash, 2000);
setInterval(UpdateChart, 2000);
//setInterval(UpdateDashFake, 3000);
var values = [];
const months = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December'
]

var data = {
    labels: ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sanday"],
    datasets: [{
        label: "My week",
        backgroundColor: "rgba(255,99,132,0.2)",
        borderColor: "rgba(255,99,132,1)",
        borderWidth: 2,
        hoverBackgroundColor: "rgba(255,99,132,0.4)",
        hoverBorderColor: "rgba(255,99,132,1)",
        data: values,
    }]
};

var options = {
    maintainAspectRatio: false,
    scales: {
        y: {
            stacked: true,
            grid: {
                display: true,
                color: "rgba(255,99,132,0.2)"
            }
        },
        x: {
            grid: {
                display: false
            }
        }
    }
};

var ctx = document.getElementById('myChart').getContext('2d');
var chart = new Chart(ctx, {
    // The type of chart we want to create
    type: 'line',

    // The data for our dataset
    data,

    // Configuration options go here
    options
});



function UpdateHour() {
    const d = new Date();
    document.getElementById("dayOfWeek").innerHTML = "Day " + d.getDay();
    document.getElementById("week").innerHTML = "Week " + d.getDate();
    document.getElementById("day").innerHTML = d.getDate() + "th";
    var indexMounth = d.getMonth();
    document.getElementById("mounth").innerHTML = months[indexMounth];
    document.getElementById("year").innerHTML = d.getFullYear();
    //document.getElementById("clock").innerHTML = d.toLocaleString();
}

function UpdateDashFake() {
    document.getElementById("serendipity").innerHTML = Math.floor(Math.random() * 100) + " %";
    document.getElementById("heartRate").innerHTML = Math.floor(Math.random() * 200) + " bpm";
    document.getElementById("steps").innerHTML = Math.floor(Math.random() * 10000);
    document.getElementById("bloodSat").innerHTML = Math.floor(Math.random() * 110);
    document.getElementById("systolicPress").innerHTML = Math.floor(Math.random() * 200);
    document.getElementById("dyastolicPress").innerHTML = Math.floor(Math.random() * 100) + " mmHg";
    var battery = Math.floor(Math.random() * 100);
    document.getElementById("batteryValue").innerHTML = battery + " %";

    if (battery < 20) {
        document.getElementById("emptyBattery").classList.remove('d-none');
        document.getElementById("fullBattery").classList.add('d-none');
        document.getElementById("halfBattery").classList.add('d-none');

        document.getElementById("batteryValue").style.color = "rgb(224,123,130)";
        document.getElementById("batteryCard").classList.add('danger');
        document.getElementById("allertDiv").classList.remove('d-none');
    } else if (battery >= 20 && battery < 65) {
        document.getElementById("emptyBattery").classList.add('d-none');
        document.getElementById("fullBattery").classList.add('d-none');
        document.getElementById("halfBattery").classList.remove('d-none');

        document.getElementById("batteryValue").style.color = "white";
        document.getElementById("batteryCard").classList.remove('danger');
        document.getElementById("allertDiv").classList.add('d-none');
    } else if (battery >= 65) {
        document.getElementById("emptyBattery").classList.add('d-none');
        document.getElementById("fullBattery").classList.remove('d-none');
        document.getElementById("halfBattery").classList.add('d-none');

        document.getElementById("batteryValue").style.color = "white";
        document.getElementById("batteryCard").classList.remove('danger');
        document.getElementById("allertDiv").classList.add('d-none');
    }

    //document.getElementById("allarms").innerHTML = "Fallen detected";
}

function UpdateDash() {
    const xhr = new XMLHttpRequest()

    xhr.open("GET", "https://clofttestapi.azurewebsites.net/CloftData")
    //send the Http request
    xhr.send()

    //triggered when the response is completed
    xhr.onload = function () {
        if (xhr.status === 200) {
            //parse JSON datax`x
            mesure = JSON.parse(xhr.responseText)

            document.getElementById("serendipity").innerHTML = mesure.serendipity + " %";
            document.getElementById("heartRate").innerHTML = mesure.heartbeat + " bpm";
            document.getElementById("steps").innerHTML = mesure.steps;
            document.getElementById("bloodSat").innerHTML = mesure.bloodSaturation;
            document.getElementById("systolicPress").innerHTML = mesure.systolicPressure;
            document.getElementById("dyastolicPress").innerHTML = mesure.dyastolicPressure + " mmHg";
            document.getElementById("batteryValue").innerHTML = mesure.battery + " %";
            var battery = mesure.battery;

            // if fall logic
            let fall = mesure.alarmFall;
            if (fall === true) {
                document.getElementById("allarms").innerHTML = "Fallen detected";
                document.getElementById("allarmCard").classList.add('warning');
                document.getElementById("allarmCard").classList.add('danger');
                document.getElementById("allarmCard").classList.remove('cardNormal');

            } else {
                document.getElementById("allarms").innerHTML = "";
                document.getElementById("allarmCard").classList.remove('warning');
                document.getElementById("allarmCard").classList.remove('danger');
                document.getElementById("allarmCard").classList.add('cardNormal');
            }

            // battery logic
            if (battery < 20) {
                document.getElementById("emptyBattery").classList.remove('d-none');
                document.getElementById("fullBattery").classList.add('d-none');
                document.getElementById("halfBattery").classList.add('d-none');

                document.getElementById("batteryValue").style.color = "rgb(224,123,130)";
                document.getElementById("batteryCard").classList.add('danger');
                document.getElementById("allertDiv").classList.remove('d-none');
            } else if (battery >= 20 && battery < 65) {
                document.getElementById("emptyBattery").classList.add('d-none');
                document.getElementById("fullBattery").classList.add('d-none');
                document.getElementById("halfBattery").classList.remove('d-none');

                document.getElementById("batteryValue").style.color = "white";
                document.getElementById("batteryCard").classList.remove('danger');
                document.getElementById("allertDiv").classList.add('d-none');
            } else if (battery >= 65) {
                document.getElementById("emptyBattery").classList.add('d-none');
                document.getElementById("fullBattery").classList.remove('d-none');
                document.getElementById("halfBattery").classList.add('d-none');

                document.getElementById("batteryValue").style.color = "white";
                document.getElementById("batteryCard").classList.remove('danger');
                document.getElementById("allertDiv").classList.add('d-none');
            }

        } else if (xhr.status === 404) {
            console.log("No records found")
        }
    }

    //triggered when a network-level error occurs with the request
    xhr.onerror = function () {
        console.log("Network error occurred")
    }

    //triggered periodically as the client receives data
    //used to monitor the progress of the request
    xhr.onprogress = function (e) {
        if (e.lengthComputable) {
            //console.log(`${e.loaded} B of ${e.total} B loaded!`)
        } else {
            //console.log(`${e.loaded} B loaded!`)
        }
    }
}

function UpdateChart() {
    const xhr = new XMLHttpRequest()

    xhr.open("GET", "https://clofttestapi.azurewebsites.net/CloftData/GetGraphData")
    //send the Http request
    xhr.send()

    //triggered when the response is completed
    xhr.onload = function () {
        if (xhr.status === 200) {
            //parse JSON datax`x
            var res = JSON.parse(xhr.responseText)

            values[0] = res.monday;
            values[1] = res.tuesday;
            values[2] = res.wednesday;
            values[3] = res.thursday;
            values[4] = res.friday;
            values[5] = res.saturday;
            values[6] = res.sunday;
            //console.log(values);

        } else if (xhr.status === 404) {
            console.log("No records found")
        }
    }

    //triggered when a network-level error occurs with the request
    xhr.onerror = function () {
        console.log("Network error occurred")
    }

    //triggered periodically as the client receives data
    //used to monitor the progress of the request
    xhr.onprogress = function (e) {
        if (e.lengthComputable) {
            //console.log(`${e.loaded} B of ${e.total} B loaded!`)
        } else {
            //console.log(`${e.loaded} B loaded!`)
        }
    }
}


