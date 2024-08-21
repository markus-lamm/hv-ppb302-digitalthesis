const { UIForm, UIInput, UIButton, UITextArea } = Jodit.modules;

Jodit.defaultOptions.controls.videonopause = {
    iconURL: '/images/icons/replay.png',
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
                videoNoPauseform.submit();
            })
        ]), closePopWindow = () => {
            editor.s.focus();
            editor.s.restore();
            close.__closePopup();
        };


        videoNoPauseform.onSubmit((data) => {
            const iframetag = `<video id="videoIframe" src="${data.videoNoPauseUrl}" title="description" width="304px" height="154px" controls data-control="true"></video>`;
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
    var videos = document.querySelectorAll('video[data-control="true"]');
    videos.forEach(function (video) {
        video.addEventListener('pause', function () {
            video.currentTime = 0;
        });
    });
});

var input = document.getElementById('Becomings')
if (input) {
    var tagify = new Tagify(input, {
        dropdown: {
            enabled: 0
        }
    })
    var selectedTag = input.getAttribute('data-tags');

    // Convert the string of tags into an array
    if (selectedTag) {
        var tagsArray = selectedTag.split(',');
        tagsArray.forEach(function (tag) {
            // Get the value of each tag
            tagify.addTags([tag.trim()]);
        });
    }

}

function copyFunction(fileurl) {
    // Get the text field
    navigator.clipboard.writeText(fileurl);
}
