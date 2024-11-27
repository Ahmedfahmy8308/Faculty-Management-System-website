const video = document.getElementById("video");
const Lecnum = document.getElementById("Lecnum").innerHTML;
const Coursecode = document.getElementById("CourseCode").innerHTML;
var atee = 0;
Promise.all([
    faceapi.nets.ssdMobilenetv1.loadFromUri("../../Face ID/mod"),
    faceapi.nets.faceRecognitionNet.loadFromUri("../../Face ID/mod"),
    faceapi.nets.faceLandmark68Net.loadFromUri("../../Face ID/mod"),
]).then(startWebcam);

function startWebcam() {
    navigator.mediaDevices
        .getUserMedia({
            video: true,
            audio: false,
        })
        .then((stream) => {
            video.srcObject = stream;
        })
        .catch((error) => {
            console.error(error);
        });
}

async function getLabeledFaceDescriptions() {
    try {
        const labelsResponse = await fetch('/api/folders');
        const labelsData = await labelsResponse.json();
        const labels = labelsData; 
        console.log(labels);

        return Promise.all(
            labels.map(async (label) => {
                const descriptions = [];
                for (let i = 1; i <= 1; i++) {
                    const img = await faceapi.fetchImage(`../\\Face ID\\labels\\${label}\\${i}.jpg`);
                    const detections = await faceapi.detectSingleFace(img).withFaceLandmarks().withFaceDescriptor();
                    descriptions.push(detections.descriptor);
                }
                return new faceapi.LabeledFaceDescriptors(label, descriptions);
            })
        );
    } catch (error) {
        console.error('Error fetching labeled face descriptions:', error);
        throw error;
    }
}

video.addEventListener("play", async () => {
    const labeledFaceDescriptors = await getLabeledFaceDescriptions();
    const faceMatcher = new faceapi.FaceMatcher(labeledFaceDescriptors);

    const canvas = faceapi.createCanvasFromMedia(video);
    document.body.append(canvas);

    const displaySize = { width: video.width, height: video.height };
    faceapi.matchDimensions(canvas, displaySize);

    const callApi = async (result, box) => {
        const apiUrl = '/api/GetUserNameAtendance';
        const data = {
            result: result.toString(),
            CourseCode: Coursecode.toString(),
            LecNum: Lecnum.toString(),
        };

        try {
            const response = await fetch(apiUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });

            if (!response.ok) {
                throw new Error('Failed to fetch user Data');
            }

            const responseData = await response.text(); // Get the response as text

            if (responseData === "False") {
                // Handle the "False" response
                // For example, display an error message or take appropriate action
                console.log("Response is False");
                return;
            }

            // Parse the response as JSON
            const parsedResponse = JSON.parse(responseData);
            console.log(parsedResponse)
            
            const FirstName = parsedResponse.firstName;
            const LastName = parsedResponse.lastName;
            const Level = parsedResponse.level;
            const HasCourse = parsedResponse.hasCourse;

            var table = document.getElementById("schedule");
            var newRow = table.insertRow();
            var cells = [];
            for (var i = 0; i < 5; i++) {
                cells[i] = newRow.insertCell(i);
            }
            cells[0].innerHTML = ++atee;
            cells[1].innerHTML = FirstName;
            cells[2].innerHTML = LastName;
            cells[3].innerHTML = Level;
            cells[4].innerHTML = HasCourse;


        } catch (error) {
            // Handle error
            console.error('Error:', error.message);
        }

        

            
        
    };

    setInterval(async () => {
        const detections = await faceapi.detectAllFaces(video).withFaceLandmarks().withFaceDescriptors();
        const resizedDetections = faceapi.resizeResults(detections, displaySize);
        canvas.getContext("2d").clearRect(0, 0, canvas.width, canvas.height);
        const results = resizedDetections.map((d) => { return faceMatcher.findBestMatch(d.descriptor); });

        results.forEach(async (result, i) => {
            const box = resizedDetections[i].detection.box;
            const drawBox = new faceapi.draw.DrawBox(box, { label: result });
            drawBox.draw(canvas);

            await callApi(result, box);
        });
    }, 100);
});


async function CancelDataApi() {
    console.log("CancelDataApi");
    try {
        // Make an AJAX request to the controller endpoint

        const data = {
            CourseCode: Coursecode.toString(),
            LecNum: Lecnum.toString(),
        };

        const response = await fetch('/api/CancelAttendance', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        else {
            console.log("Attendance Cancelled");
            SaveDataApi();
        }

    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
    }
}

async function SaveDataApi() {


    window.location.href = '../../Doctor/AttendanceManagement';
    return;

}

