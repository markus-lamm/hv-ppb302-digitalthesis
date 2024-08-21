const { UIForm, UIInput, UIButton, UITextArea } = Jodit.modules;

Jodit.defaultOptions.controls.videonopause = {
    icon: 'video',
    popup: (editor, current, control, close) => {
        videoNoPauseform = new UIForm(editor, [
            new UIInput(editor, {
                name: 'videoNoPauseUrl',
                placeholder: 'Enter video URL...',
                autofocus: false,
                label: 'Video URL:'
            }),
            new UIButton(editor, {
                text: 'Insert no pause video',
                status: 'primary',
                variant: 'primary'
            }).onAction(() => {
                form.submit();
            })
        ]), closePopWindow = () => {
            editor.s.focus();
            editor.s.restore();
            close.__closePopup();
        };


        videoNoPauseform.onSubmit(() => { })
        return videoNoPauseform;
    },
    tooltip: "Insert No pausable video"
}

Jodit.defaultOptions.controls.video = {
    popup: (editor, current, control, close) => {
        const videoform = new UIForm(editor, [
            new UIInput(editor, {
                name: 'videoUrl',
                placeholder: 'Enter video URL...',
                autofocus: false,
                label: 'Video URL:'
            }),
            new UIButton(editor, {
                text: 'Insert Video',
                status: 'primary',
                variant: 'primary'
            }).onAction(() => {
                videoform.submit();
            })
        ]), closePopWindow = () => {
            editor.s.focus();
            editor.s.restore();
            close.__closePopup();
        };

        videoform.onSubmit((data) => {
            const iframetag = `<iframe src="${data.videoUrl}" title="description" width="304px" height="154px" data-control="true"></iframe>`;

            editor.selection.insertHTML(iframetag);
            closePopWindow();
        })

        return videoform;
    },
    tooltip: "Insert normal video"
};

Jodit.defaultOptions.controls.footnoteButton = {
    iconURL: "https://informatik13.ei.hv.se/DigitalThesis/images/icons/superscript.png",
    popup: function (editor, current, control, close) {
        const form = new UIForm(editor, [
            new UITextArea(editor, {
                name: 'linkText',
                placeholder: 'Enter link text...',
                autofocus: true,
                label: 'Text: *',
                required: true
            }),
            new UIInput(editor, {
                name: 'linkURL',
                placeholder: 'Enter link URL...',
                autofocus: false,
                label: 'URL:'
            }),
            new UIButton(editor, {
                text: 'Insert Footnote',
                status: 'primary',
                variant: 'primary'
            }).onAction(() => {
                form.submit();
            })
        ]), closePopWindow = () => {
            editor.s.focus();
            editor.s.restore();
            close.__closePopup();
        };

        form.onSubmit(() => {
            // Attempt to retrieve the input element from form.elements
            const LinkTextElement = form.elements.find(
                element => element.state && element.state.name === 'linkText'
            );
            const LinkUrlElement = form.elements.find(
                element => element.state && element.state.name === 'linkURL'
            );

            if (LinkTextElement && LinkUrlElement) {
                // Safely access the value
                const linkText = LinkTextElement.state.value || '';
                const linkURL = LinkUrlElement.state.value || '';
                let footnoteText = linkText;

                const existingFootnotes = editor.editor.querySelectorAll('a[href^="#_ftnref"]');
                const footnoteNumber = existingFootnotes.length + 1;


                if (linkURL) {
                    footnoteText = `<strong><a href="${linkURL}" target="_blank" title="${linkText}">${linkText}</a></strong>`;
                }

                const footnoteId = `ftn${footnoteNumber}`;
                const footnoteRefId = `_ftnref${footnoteNumber}`;

                const footnoteMarker = `<a href="#${footnoteId}" name="${footnoteRefId}" title=""><span class="MsoFootnoteReference" style="vertical-align: super;"><span style="font-size: 15px; line-height: 107%; font-family: Aptos, sans-serif; vertical-align: super;">${footnoteNumber}</span></span></a>&nbsp;`;

                const footnoteContent = `
                        <div id="${footnoteId}">
                            <p class="MsoFootnoteText" style="margin: 0px; font-size: 13px; font-family: Aptos, sans-serif;">
                                <a href="#${footnoteRefId}" name="${footnoteId}" title="">
                                    <span class="MsoFootnoteReference" style="vertical-align: super;">
                                        <span style="font-size: 13px; line-height: 107%; font-family: Aptos, sans-serif;">[${footnoteNumber}]</span>
                                    </span></a>&nbsp;&nbsp;&nbsp;${footnoteText}
                            </p>
                        </div>`;

                // Insert marker at cursor position
                editor.selection.insertHTML(footnoteMarker);

                // Insert the separator line if it's the first footnote
                if (footnoteNumber === 1) {
                    editor.value += `<br clear="all"><hr id="footnote-separator" align="left" size="1" width="33%">`;
                }

                // Append footnote content to the end
                editor.value += footnoteContent;
                
            } else {
                console.error('Footnote input field not found or state is undefined.');
            }
            closePopWindow();
        })
        return form;
    },
    tooltip: "Insert Footnote"
};


var editorDiv = document.getElementById('editor');
if (editorDiv) {
    var editor = new Jodit('#editor', {
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
        buttons: [...Jodit.defaultOptions.buttons, 'footnoteButton', 'videonopause'],
        
    });

    let inputElement = document.getElementById('hiddeninput')
    editor.events.on('change', e => {

        inputElement.value = editor.getEditorValue();
    })
}

document.addEventListener('DOMContentLoaded', function () {
    // Select all video elements with data-control="true"
    var iframes = document.querySelectorAll('iframe[data-control="true"]');

        iframes.forEach((iframe) => {
            iframe.onload = function () {
                var video = iframe.contentWindow.document.querySelector("video");

                if (video) {
                    video.addEventListener('pause', function () {
                        video.currentTime = 0;  // Reset video to the beginning
                        console.log("here")
                    });
                } else {
                    console.error("No video element found inside the iframe.");
                }
            };
        })
    

});

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
            audioImage.src = "https://informatik13.ei.hv.se/DigitalThesis/images/icons/stop.png"
        } else {
            // If playing, pause the audio
            audioPlayer.pause();
            audioPlayer.currentTime = 0;
            audioImage.src = "https://informatik13.ei.hv.se/DigitalThesis/images/icons/play.png"
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

