const { UIForm, UIInput, UIButton } = Jodit.modules;

Jodit.defaultOptions.controls.footnoteButton = {
    iconURL: "/images/icons/superscript.png",
    popup: function (editor, current, control, close) {
        const form = new UIForm(editor, [
            new UIInput(editor, {
                name: 'linkText',
                placeholder: 'Enter link text...',
                autofocus: true,
                label: 'Text:',
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
        ]).onSubmit(() => {

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

                const existingFootnotes = editor.editor.querySelectorAll('a[href^="#ftn"]');
                const footnoteNumber = existingFootnotes.length + 1;


                if (linkURL) {
                    footnoteText = `<strong><a href="${linkURL}" target="_blank" title="${linkText}">${linkText}</a></strong>`;
                }

                const footnoteId = `ftn${footnoteNumber}`;
                const footnoteRefId = `_ftnref${footnoteNumber}`;

                const footnoteMarker = `<a href="#${footnoteId}" name="${footnoteRefId}" title=""><span class="MsoFootnoteReference" style="vertical-align: super;"><span style="font-size: 15px; line-height: 107%; font-family: Aptos, sans-serif;">[${footnoteNumber}]</span></span></a>&nbsp;`;

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
        buttons: [...Jodit.defaultOptions.buttons, 'footnoteButton']
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
