

var data = {
    labels: ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sanday"],
    datasets: [{
        label: "My week",
        backgroundColor: "rgba(255,99,132,0.2)",
        borderColor: "rgba(255,99,132,1)",
        borderWidth: 2,
        hoverBackgroundColor: "rgba(255,99,132,0.4)",
        hoverBorderColor: "rgba(255,99,132,1)",
        data: [65, 59, 20, 81, 56, 55, 40],
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



setInterval(UpdateHour, 1000);
//setInterval(UpdateDash, 2000);
setInterval(UpdateDashFake, 2000);

function UpdateHour() {
    const d = new Date();
    document.getElementById("clock").innerHTML = d.toLocaleString();
}


function UpdateDashFake() {
    document.getElementById("serendipity").innerHTML = Math.floor(Math.random() * 100) + " %";
    document.getElementById("heartRate").innerHTML = Math.floor(Math.random() * 200) + " bpm";
    document.getElementById("steps").innerHTML = Math.floor(Math.random() * 10000);
    document.getElementById("bloodSat").innerHTML = Math.floor(Math.random() * 110);
    document.getElementById("systolicPress").innerHTML = Math.floor(Math.random() * 200);
    document.getElementById("dyastolicPress").innerHTML = Math.floor(Math.random() * 100) + " mmHg";
    var battery = Math.floor(Math.random() * 100);

    if (battery < 20) {
        document.getElementById("battery").style.color = "rgb(240, 119, 110)";
        document.getElementById("batteryCard").classList.add('danger');
        document.getElementById("allertDiv").classList.remove('d-none');
    } else {
        document.getElementById("battery").style.color = "white";
        document.getElementById("batteryCard").classList.remove('danger');
        document.getElementById("allertDiv").classList.add('d-none');
    }
    document.getElementById("battery").innerHTML = battery + " %";

    // document.getElementById("allarms").innerHTML = "Fallen detected";
}

function UpdateDash() {
    const xhr = new XMLHttpRequest()
    //open a get request with the remote server URL
    xhr.open("GET", "https://world.openfoodfacts.org/category/pastas/1.json")
    //send the Http request
    xhr.send()

    //EVENT HANDLERS

    //triggered when the response is completed
    xhr.onload = function () {
        if (xhr.status === 200) {
            //parse JSON datax`x
            data = JSON.parse(xhr.responseText)
            console.log(data.count)
            console.log(data.products)
            // Here update value of the dash
            document.getElementById("serendipity").innerHTML = Math.floor(Math.random() * 100);
            document.getElementById("heartRate").innerHTML = Math.floor(Math.random() * 200);
            document.getElementById("steps").innerHTML = Math.floor(Math.random() * 10000);
            document.getElementById("bloodSat").innerHTML = Math.floor(Math.random() * 110);
            document.getElementById("SystolicPress").innerHTML = Math.floor(Math.random() * 200);
            document.getElementById("DyastolicPress").innerHTML = Math.floor(Math.random() * 100);
            document.getElementById("battery").innerHTML = Math.floor(Math.random() * 100);
            document.getElementById("allarms").innerHTML = Math.floor(Math.random() * 10);
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
            console.log(`${e.loaded} B of ${e.total} B loaded!`)
        } else {
            console.log(`${e.loaded} B loaded!`)
        }
    }
}