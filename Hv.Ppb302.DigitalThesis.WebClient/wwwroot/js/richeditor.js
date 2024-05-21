
var editorDiv = document.getElementById('editor');
if (editorDiv) {
    const quill = new Quill('#editor', {
        theme: 'bubble',
    });
    quill.enable(false);
}


const FontAttributor = Quill.import('attributors/class/font');
FontAttributor.whitelist = [
    'lora'
];
Quill.register(FontAttributor, true);

const toolbarOptions = [
    ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
    ['blockquote', 'code-block'],
    ['link', 'image', 'video', 'formula'],

    [{ 'header': 1 }, { 'header': 2 }],               // custom button values
    [{ 'list': 'ordered' }, { 'list': 'bullet' }, { 'list': 'check' }],
    [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
    [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
    [{ 'direction': 'rtl' }],                         // text direction

    [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
    [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

    [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
    [{ 'font': [] }],
    [{ 'align': [] }],

    ['clean']                                         // remove formatting button
];


var editorDiv2 = document.getElementById('editortest');
if (editorDiv2) {
    const quilleditor = new Quill('#editortest', {
        theme: 'snow',
        modules: {
            syntax: true,
            toolbar: toolbarOptions,
            imageResize: {
                displaySize: true
            }
        }
    });
    function getAndDisplayHTML() {
        var htmlContent = quilleditor.root.innerHTML;
        document.getElementById('htmlOutput').innerText = htmlContent;
    }

    let inputElement = document.getElementById('hiddeninput')
    quilleditor.on('text-change', function () {
        // sets the value of the hidden input to
        // the editor content in Delta format
        inputElement.value = quilleditor.root.innerHTML;

        // you can alternatively use
        // inputElement.value = quill.root.innerHTML
        // if you want the data as HTML
    });
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



function copyFunction() {
    // Get the text field
    var copyText = document.getElementById("fileurl");

    var selectedTag = copyText.getAttribute('data-tags'); // For mobile devices

    // Copy the text inside the text field
    navigator.clipboard.writeText(selectedTag);

}

const playButton = document.getElementById('audio-icon');

if (playButton) {
    const audioPlayer = document.getElementById('audio-player');
    // Add click event listener to the button
    playButton.addEventListener('click', function () {
        // Check if audio is currently playing
        if (audioPlayer.paused) {
            // If paused, play the audio
            audioPlayer.play();
        } else {
            // If playing, pause the audio
            audioPlayer.pause();
            audioPlayer.currentTime = 0;
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

