
var editorDiv = document.getElementById('editor');
if (editorDiv) {
    var editor = new Jodit('#editor', {
        autofocus: true,
        toolbar: false,
        readonly: true,
        "showCharsCounter": false,
        "showWordsCounter": false,
        "showXPathInStatusbar": false,
        className: 'previeweditor',
        height: '100%',
        width: '100%',
        "allowResizeY": false
    });
}

var editorDiv2 = document.getElementById('editortest');
if (editorDiv2) {
    var editor = new Jodit('#editortest', {
        autofocus: true,
        "defaultFontSizePoints": "pt",
        "minHeight": 900,
        "maxHeight": 900,
        "uploader": {
            "insertImageAsBase64URI": true
        },
    });

    let inputElement = document.getElementById('hiddeninput')
    editor.events.on('change', e => {

        inputElement.value = editor.getEditorValue();
    })
}

var input = document.getElementById('Becomings')
if (input) {
    var tagify = new Tagify(input, {
        dropdown: {
            enabled: 0
        }
    })
    var selectedTag = input.getAttribute('data-tags');

    input.value = '';
    // Convert the string of tags into an array
    var tagsArray = selectedTag.split(',');

    tagsArray.forEach(function (tag) {
        // Get the value of each tag
        tagify.addTags([tag.trim()]);
    });
}



function copyFunction(fileurl) {
    // Get the text field

    navigator.clipboard.writeText(fileurl);

}

const playButton = document.getElementById('audio-icon');

if (playButton) {
    const audioPlayer = document.getElementById('audio-player');
    const audioImage = document.getElementById('audio-img');

    // Add click event listener to the button
    playButton.addEventListener('click', function () {
        // Check if audio is currently playing
        if (audioPlayer.paused) {
            // If paused, play the audio
            audioPlayer.play();
            audioImage.src = "https://informatik13.ei.hv.se/DigitalThesis/images/icons/stop_icon.png"
        } else {
            // If playing, pause the audio
            audioPlayer.pause();
            audioPlayer.currentTime = 0;
            audioImage.src = "https://informatik13.ei.hv.se/DigitalThesis/images/icons/play_icon.png"
        }
    });
}


const savebtns = document.getElementById('saveBtn');

if (savebtns) {
    document.getElementById('saveBtn').addEventListener('click', async function () {
        // URL of the PDF to be downloaded

        var selectedTag = this.getAttribute('data-tags');

        const pdfUrl = selectedTag;  // Replace with your actual file URL

        try {
            // Check if the browser supports the File System Access API
            if (!window.showSaveFilePicker) {
                alert('showSaveFilePicker API is not supported in this browser.');
                return;
            }

            // Fetch the PDF file from the URL
            const response = await fetch(pdfUrl);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const pdfBlob = await response.blob();

            // Open the save file picker
            const fileHandle = await window.showSaveFilePicker({
                suggestedName: 'downloaded.pdf', // Default file name
                types: [
                    {
                        description: 'PDF Files',
                        accept: {
                            'application/pdf': ['.pdf'],
                        },
                    },
                ],
            });

            // Create a writable stream to the file
            const writableStream = await fileHandle.createWritable();

            // Write the PDF blob to the file
            await writableStream.write(pdfBlob);

            // Close the file and save the changes
            await writableStream.close();
        } catch (error) {
            console.error('Error saving file:', error);
        }
    });
}

