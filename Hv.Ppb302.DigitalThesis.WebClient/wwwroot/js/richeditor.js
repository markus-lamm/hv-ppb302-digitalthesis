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

Jodit.defaultOptions.controls.popupnoteButton = {
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
            editor.s.synchronizeValues();
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

                const existingFootnotes = editor.editor.querySelectorAll('a[href^="#_ftnref"]');
                const footnoteNumber = existingFootnotes.length + 1;


                const footnoteId = `ftn${footnoteNumber}`;
                const footnoteRefId = `_ftnref${footnoteNumber}`;

                const footnoteMarker = `<a href="#${footnoteId}" name="${footnoteRefId}" title=""><span class="MsoFootnoteReference" style="vertical-align: super;"><span style="font-size: 15px; line-height: 107%; font-family: Aptos, sans-serif; vertical-align: super;"><sup>${footnoteNumber}</sup></span></span><div id="popup" class="popup"><div class="popup-content"><h2>Popup Title</h2><p>${linkText}</p></div></div></a>&nbsp;`;

                // Insert marker at cursor position
                editor.selection.insertHTML(footnoteMarker);

            } else {
                console.error('popup input field not found or state is undefined.');
            }
            closePopWindow();
        })
        return form;
    },
    tooltip: "Insert popup note"
};

Jodit.plugins.add('hoverPopupSup', function (editor) {
    function createPopup(content) {
        const popup = new Jodit.modules.Popup(editor);
        popup.setContent(content);
        return popup;
    }

    function showPopup(event, popup) {
        popup.open(() => ({
            left: event.clientX,
            top: event.clientY
        }));
    }

    function openEditDialog(target) {
        const dialog = new Jodit.modules.Dialog({
            title: 'Edit Popup Content',
            buttons: [
                Jodit.modules.Button(editor, {
                    text: 'Save',
                    action: function () {
                        const newContent = dialog.getContent().querySelector('textarea').value;
                        const newTitle = dialog.getContent().querySelector('input[name="popup-title"]').value;
                        const newType = dialog.getContent().querySelector('select[name="popup-type"]').value;

                        target.setAttribute('data-popup-content', newContent);
                        target.setAttribute('data-popup-title', newTitle);
                        target.setAttribute('data-popup-type', newType);

                        dialog.close();
                    }
                }),
                Jodit.modules.Button(editor, {
                    text: 'Cancel',
                    action: function () {
                        dialog.close();
                    }
                })
            ]
        });

        const content = document.createElement('div');

        const titleInput = document.createElement('input');
        titleInput.name = 'popup-title';
        titleInput.placeholder = 'Popup Title';
        titleInput.value = target.getAttribute('data-popup-title') || '';
        content.appendChild(titleInput);

        const textarea = document.createElement('textarea');
        textarea.value = target.getAttribute('data-popup-content');
        content.appendChild(textarea);

        const typeSelect = document.createElement('select');
        typeSelect.name = 'popup-type';
        const types = ['info', 'warning', 'error'];
        types.forEach(type => {
            const option = document.createElement('option');
            option.value = type;
            option.text = type.charAt(0).toUpperCase() + type.slice(1);
            if (type === target.getAttribute('data-popup-type')) {
                option.selected = true;
            }
            typeSelect.appendChild(option);
        });
        content.appendChild(typeSelect);

        dialog.setContent(content);
        dialog.open();
    }

    editor.events.on('afterInit', function () {
        editor.container.addEventListener('mouseover', function (event) {
            const target = event.target;
            if (target && target.tagName === 'SUP' && target.hasAttribute('data-popup-content')) {
                const content = target.getAttribute('data-popup-content');
                const popup = createPopup(content);
                showPopup(event, popup);
            }
        });

        editor.container.addEventListener('click', function (event) {
            const target = event.target;
            if (target && target.tagName === 'SUP' && target.hasAttribute('data-popup-content')) {
                openEditDialog(target);
            }
        });
    });

    editor.events.on('beforeDestruct', function () {
        editor.container.removeEventListener('mouseover', showPopup);
        editor.container.removeEventListener('click', openEditDialog);
    });
});




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
        buttons: [...Jodit.defaultOptions.buttons, 'footnoteButton', 'videonopause', 'audioplay', 'popupnoteButton'],
        
    });

    let inputElement = document.getElementById('hiddeninput')
    editor.events.on('change', e => {
        inputElement.value = editor.getEditorValue();
    //    addPopupNoteEventlistener();
    })
    editor.events.on('afterinit', e => {
    //    addPopupNoteEventlistener();
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

function addPopupNoteEventlistener() {
    var editAdminContainer = document.getElementById('editor-layout-id');
    const popup = document.getElementById('popup');

    if (editAdminContainer) {
        const sup = editAdminContainer.querySelectorAll('sup');

        sup.forEach(foot => {

            foot.addEventListener('mouseover', (event) => {
                popup.style.display = 'block';

                // Get mouse position
                const mouseX = event.clientX;
                const mouseY = event.clientY;

                // Get popup dimensions
                const popupWidth = popup.offsetWidth;
                const popupHeight = popup.offsetHeight;

                // Get viewport dimensions
                const viewportWidth = window.innerWidth;
                const viewportHeight = window.innerHeight;

                // Calculate the best position
                let top = mouseY;
                let left = mouseX;

                // Adjust position if the popup overflows the viewport
                if (mouseY + popupHeight > viewportHeight) {
                    top = mouseY - popupHeight; // Show above the mouse
                }

                if (mouseX + popupWidth > viewportWidth) {
                    left = mouseX - popupWidth; // Show to the left of the mouse
                }

                // Apply the calculated positions
                popup.style.top = `${top}px`;
                popup.style.left = `${left}px`;
            });
            foot.addEventListener('mouseout', () => {

                popup.style.display = 'none';
            })

            popup.addEventListener('mouseover', (event) => {
                popup.style.display = 'block';
            })
            popup.addEventListener('mouseout', (event) => {
                popup.style.display = 'none';
            })


        })
        console.log(sup);
    }
}

document.addEventListener("DOMContentLoaded", function (event) {
    var editorDivss = document.getElementById('editor-container');
    var videos = document.querySelectorAll('video[data-control="true"]');
   
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