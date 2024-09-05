const { UIForm, UIInput, UIButton, UITextArea } = Jodit.modules;

Jodit.defaultOptions.controls.audioplay = {
    iconURL: '/images/icons/sound.png',
    popup: (editor, current, control, close) => {
        audioForm = new UIForm(editor, [
            new UIInput(editor, {
                name: 'audiourl',
                placeholder: 'Enter audio URL...',
                autofocus: false,
                label: 'Audio URL:',
                required: true
            }),
            new UIInput(editor, {
                name: 'Width',
                placeholder: 'Enter width for the audio player ',
                autofocus: false,
                label: 'Video width: (Normal size is 300)'
            }),
            new UIButton(editor, {
                text: 'Insert audio player',
                status: 'primary',
                variant: 'primary'
            }).onAction(() => {
                audioForm.submit();
            })
        ]), closePopWindow = () => {
            editor.s.focus();
            editor.s.restore();
            close.__closePopup();
            }; 

        audioForm.onSubmit(data => {
            const width = data.Width !== "" ? `${data.Width}px` : '300px';
            const audiotag = `<audio controls style="width: ${width}""><source src="${data.audiourl}">Your browser does not support the audio element.</audio>`;
            editor.selection.insertHTML(audiotag);
            closePopWindow();

        })
        return audioForm;
    },
    tooltip: 'Insert audio player'
    
}

Jodit.defaultOptions.controls.videonopause = {
    iconURL: '/images/icons/replay.png',
    popup: (editor, current, control, close) => {
        videoNoPauseform = new UIForm(editor, [
            new UIInput(editor, {
                name: 'videoNoPauseUrl',
                placeholder: 'Enter video URL...',
                autofocus: false,
                label: 'Video URL:',
                required: true
            }),
            new UIInput(editor, {
                name: 'Width',
                placeholder: 'Enter width for the video ',
                autofocus: false,
                label: 'Video width: (Normal size is 304)'
            }),
            new UIButton(editor, {
                text: 'Insert no pause video',
                status: 'primary',
                variant: 'primary'
            }).onAction(() => {
                videoNoPauseform.submit();
            })
        ]), closePopWindow = () => {
            editor.s.focus();
            editor.s.restore();
            close.__closePopup();
        };


        videoNoPauseform.onSubmit((data) => {
            const width = data.Width !== "" ? `${data.Width}px` : '304px';
            const height = data.Width * 0.56;
            const iframetag = `<video id="videoIframe" src="${data.videoNoPauseUrl}" title="description" width="${width}" height="${height}" controls data-control="true"></video>`;
            editor.selection.insertHTML(iframetag);
            closePopWindow();
        })
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
                label: 'Video URL:',
                required: true
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
            const iframetag = `<iframe id="videoIframe" src="${data.videoUrl}" title="description" width="304px" height="154px"></iframe>`;
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
        "allowResizeY": false,
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
        buttons: [...Jodit.defaultOptions.buttons, 'footnoteButton', 'videonopause','audioplay'],
        
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
}

function copyFunction(fileurl) {
    // Get the text field
    navigator.clipboard.writeText(fileurl);
}

document.addEventListener("DOMContentLoaded", function (event) {
    var editorDivss = document.getElementById('editor-container');
    var videos = document.querySelectorAll('video[data-control="true"]');
    var editAdminContainer = document.getElementById('editor-layout-id');

    const popup = document.getElementById('popup');

    if (editAdminContainer) {
        const sup = editAdminContainer.querySelectorAll('sup');
        sup.forEach(foot => {
            foot.style.zIndex = '222'
            foot.addEventListener('click', () => { popup.style.display = 'block'; })
            foot.addEventListener('mouseleave', () => { popup.style.display = 'none'; })
        })
        console.log(sup);
    }
    if (editorDivss) {
        const images = editorDivss.querySelectorAll('img');
        mediumZoom(images, {
            margin: 24,
            background: '#365f9390',
            scrollOffset: 0,
        });
    }

    if (videos) {
        videos.forEach(function (video) {
            video.addEventListener('pause', function () {
                video.currentTime = 0;
            });
        });
    }

});