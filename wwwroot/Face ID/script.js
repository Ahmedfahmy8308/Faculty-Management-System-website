const video = document.getElementById("video");

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
        console.log('Error fetching labeled face descriptions:', error);
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
        const apiUrl = '/api/GetUserName';
        const data = {
            result: result.toString(),
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
                throw new Error('Failed to fetch user role');
            }

            const role = await response.text();
            console.log( role);

            switch (role) {
                case 'affaire':
                    window.location.href = '../Affaire/index';
                    break;
                case 'doctor':
                    window.location.href = '../Doctor/index';
                    break;
                case 'Student':
                    window.location.href = '../Student/index';
                    break;
                case 'Unknown user':
                    console.error('Unknown role:', role);
                default:
                    console.error('Unknown role:', role);

            }
        } catch (error) {
            console.error('Error during API call:', error);
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