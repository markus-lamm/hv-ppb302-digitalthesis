﻿//JODIT PLUGINS
const { UIForm, UIInput, UIButton, UITextArea } = Jodit.modules;

// Audio player
Jodit.defaultOptions.controls.audioplay = {
    iconURL: "/digitalthesis/images/icons/sound.png",
    popup: (editor, current, control, close) => {
        audioForm = new UIForm(editor, [
            new UIInput(editor, {
                name: "audiourl",
                placeholder: "Enter audio URL...",
                autofocus: false,
                label: "Audio URL:",
                required: true
            }),
            new UIInput(editor, {
                name: "Width",
                placeholder: "Enter width for the audio player ",
                autofocus: false,
                label: "Video width: (Normal size is 300)"
            }),
            new UIButton(editor, {
                text: "Insert audio player",
                status: "primary",
                variant: "primary"
            }).onAction(() => {
                audioForm.submit();
            })
        ]), closePopWindow = () => {
            editor.s.focus();
            editor.s.restore();
            close.__closePopup();
            }; 

        audioForm.onSubmit(data => {
            const width = data.Width !== "" ? `${data.Width}px` : "300px";
            const audiotag = `<audio controls style="width: ${width}""><source src="${data.audiourl}">Your browser does not support the audio element.</audio>`;
            editor.selection.insertHTML(audiotag);
            closePopWindow();

        })
        return audioForm;
    },
    tooltip: "Insert audio player"
}

// Video player (without pause)
Jodit.defaultOptions.controls.videonopause = {
    iconURL: "/digitalthesis/images/icons/replay.png",
    popup: (editor, current, control, close) => {
        videoNoPauseform = new UIForm(editor, [
            new UIInput(editor, {
                name: "videoNoPauseUrl",
                placeholder: "Enter video URL...",
                autofocus: false,
                label: "Video URL:",
                required: true
            }),
            new UIInput(editor, {
                name: "Width",
                placeholder: "Enter width for the video ",
                autofocus: false,
                label: "Video width: (Normal size is 304)"
            }),
            new UIButton(editor, {
                text: "Insert no pause video",
                status: "primary",
                variant: "primary"
            }).onAction(() => {
                videoNoPauseform.submit();
            })
        ]), closePopWindow = () => {
            editor.s.focus();
            editor.s.restore();
            close.__closePopup();
        };

        videoNoPauseform.onSubmit((data) => {
            const width = data.Width !== "" ? `${data.Width}px` : "304px";
            const height = data.Width !== "" ? `${data.Width * 0.56}px` : "171px"; 
            const iframetag = `<video id="videoIframe" src="${data.videoNoPauseUrl}" title="description" width="${width}" height="${height}" controls data-control="true"></video>`;
            editor.selection.insertHTML(iframetag);
            closePopWindow();
        })
        return videoNoPauseform;
    },
    tooltip: "Insert No pausable video"
}

// Video player (with pause)
Jodit.defaultOptions.controls.video = {
    popup: (editor, current, control, close) => {
        const videoform = new UIForm(editor, [
            new UIInput(editor, {
                name: "videoUrl",
                placeholder: "Enter video URL...",
                autofocus: false,
                label: "Video URL:",
                required: true
            }),
            new UIButton(editor, {
                text: "Insert Video",
                status: "primary",
                variant: "primary"
            }).onAction(() => {
                videoform.submit();
            })
        ]), closePopWindow = () => {
            editor.s.focus();
            editor.s.restore();
            close.__closePopup();
        };

        videoform.onSubmit((data) => {
            const iframetag =
                `<iframe id="videoIframe" src="${data.videoUrl
                    }" title="description" width="304px" height="154px"></iframe>`;
            editor.selection.insertHTML(iframetag);
            closePopWindow();
        });
        return videoform;
    },
    tooltip: "Insert normal video"
};

// Footnote
Jodit.defaultOptions.controls.footnoteButton = {
    iconURL: "/digitalthesis/images/icons/superscript.png",
    popup: function (editor, current, control, close) {
        const form = new UIForm(editor, [
            new UITextArea(editor, {
                name: "linkText",
                placeholder: "Enter link text...",
                autofocus: true,
                label: "Text: *",
                required: true
            }),
            new UIInput(editor, {
                name: "linkURL",
                placeholder: "Enter link URL...",
                autofocus: false,
                label: "URL:"
            }),
            new UIButton(editor, {
                text: "Insert Footnote",
                status: "primary",
                variant: "primary"
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
            const linkTextElement = form.elements.find(
                element => element.state && element.state.name === "linkText"
            );
            const linkUrlElement = form.elements.find(
                element => element.state && element.state.name === "linkURL"
            );

            if (linkTextElement && linkUrlElement) {
                // Safely access the value
                const linkText = linkTextElement.state.value || "";
                const linkUrl = linkUrlElement.state.value || "";
                let footnoteText = linkText;

                const existingFootnotes = editor.editor.querySelectorAll('a[href^="#_ftnref"]');
                const footnoteNumber = existingFootnotes.length + 1;


                if (linkUrl) {
                    footnoteText =
                        `<strong><a href="${linkUrl}" target="_blank" title="${linkText}">${linkText}</a></strong>`;
                }

                const footnoteId = `_ftn${footnoteNumber}`;
                const footnoteRefId = `_ftnref${footnoteNumber}`;

                const footnoteMarker = `<a style="color: rgb(0, 0, 255);" href="#${footnoteId}" id="${footnoteRefId}">[${footnoteNumber}]</a>`;

                const footnoteContent = `<div><p><a datainsert="true" style="color: rgb(0, 0, 255);" href="#${footnoteRefId}" id="${footnoteId}">[${footnoteNumber}]</a> ${footnoteText}</p></div>`;

                // Insert marker at cursor position
                editor.selection.insertHTML(footnoteMarker);

                // Insert the separator line if it's the first footnote
                if (footnoteNumber === 1) {
                    editor.value += `<br clear="all"><hr id="footnote-separator" align="left" size="1" width="33%">`;
                }

                const tempDiv = document.createElement('div');
                tempDiv.innerHTML = footnoteContent;
                const footnoteElement = tempDiv.firstChild;

                // Select the <hr> element
                const hrElements = editor.editor.querySelectorAll('hr');

                const hrElement = hrElements[hrElements.length - 1].parentElement;
                // Get the parent <div> of the <hr>
                const parentDiv = hrElement.closest('div');

                parentDiv.appendChild(footnoteElement); // Append each element individually
                editor.synchronizeValues();

            } else {
                console.error("Footnote input field not found or state is undefined.");
            }
            closePopWindow();
        });
        return form;
    },
    tooltip: "Insert Footnote"
};

// JODIT EDITOR
var editorDiv = document.querySelector("#editor");
if (editorDiv) {
    var editor = new Jodit("#editor", {
        toolbar: false,
        readonly: true,
        "showCharsCounter": false,
        "showWordsCounter": false,
        "showXPathInStatusbar": false,
        className: "previeweditor",
        height: "100%",
        width: "100%",
        "allowResizeY": false
    });
}

document.addEventListener("DOMContentLoaded", (event) => {
    var editorDivss = document.querySelector("#editor-container");
    var videos = document.querySelectorAll('video[data-control="true"]');
   
    if (editorDivss) {
        const images = editorDivss.querySelectorAll("img");

        mediumZoom(images, {
            margin: 24,
            background: "#365f9390",
            scrollOffset: 0,
            zIndex: 9999
        });
    }

    if (videos) {
        videos.forEach(function (video) {
            video.addEventListener("pause", () => {
                video.currentTime = 0;
            });
        });
    }
});

var editorDiv2 = document.querySelector("#editortest");
if (editorDiv2) {
    var editor = new Jodit("#editortest", {
        autofocus: true,
        "defaultFontSizePoints": "pt",
        "minHeight": 900,
        "maxHeight": 900,
        "uploader": {
            "insertImageAsBase64URI": true
        },
        buttons: [...Jodit.defaultOptions.buttons, "footnoteButton", "videonopause", "audioplay"],
        defaultActionOnPasteFromWord: Jodit.constants.INSERT_AS_TEXT,
        defaultActionOnPaste: Jodit.constants.INSERT_AS_HTML,
        askBeforePasteFromWord: false,
        askBeforePasteHTML: true,
        processPasteHTML: false
    });
    let selection = null;
    let markerPosition = null;

    //editor.events.on('paste', (event) => {

    //    selection = editor.selection.current();
    //    markerPosition = selection;
    //    event.preventDefault();
    //})

    editor.events.on('afterPaste', function (e) {
        console.log("afterpaste")
        // Function to collect all elements (including nested ones) before the <div>
        function getAllPreviousElements(element) {
            const elements = [];
            const regex = /^#_ftnref\d+$/; // Regex to match href starting with #_ftnref followed by numbers

            let sibling = element.previousElementSibling;
            while (sibling) {
                // Check for <a> elements in the sibling
                const anchors = sibling.querySelectorAll('a[href]'); // Use querySelectorAll to get all <a> elements

                anchors.forEach(anchor => {
                    if (regex.test(anchor.getAttribute('href'))) {
                        // Add the parent <div> of the anchor to the array
                        const parentDiv = anchor.closest('div'); // Get the closest parent <div>
                        if (parentDiv) {
                            elements.push(parentDiv); // Add the parent <div> to the array
                        }
                    }
                });

                // Also check the children of the sibling
                sibling.querySelectorAll('*').forEach(child => {
                    const anchor = child.querySelector('a[href]');
                    if (anchor && regex.test(anchor.getAttribute('href'))) {
                        // Add the parent <div> of the anchor to the array
                        const parentDiv = anchor.closest('div'); // Get the closest parent <div>
                        if (parentDiv) {
                            elements.push(parentDiv); // Add the parent <div> to the array
                        }
                    }
                });

                // Move to the next previous sibling
                sibling = sibling.previousElementSibling;
            }

            return elements;
        }

        // Select the <hr> element
        const hrElements = editor.editor.querySelectorAll('hr');
        if (hrElements.length > 0) {
            const hrElement = hrElements[hrElements.length - 1].parentElement;
            // Get the parent <div> of the <hr>
            const parentDiv = hrElement.closest('div');

            // Get all previous elements before the <div>
            const previousElements = getAllPreviousElements(parentDiv);
            previousElements.forEach(element => {
                parentDiv.appendChild(element); // Append each element individually
            });
            
        }

        // Log the elements before the <div>
        
        e.preventDefault();
        return false;
    });
    editor.events.on('beforePaste', function (e, value) {
        const clipboardData = e.clipboardData || window.clipboardData;
        const pastedData = clipboardData.getData('text/html')
        const cleanHTML = Jodit.modules.Helpers.cleanFromWord(pastedData);

        const existingFootnotes = editor.editor.querySelectorAll('a[href^="#_ftnref"]');
        var footnoteNumber = existingFootnotes.length + 1;
        const tempPastedDiv = document.createElement('div');
        tempPastedDiv.innerHTML = cleanHTML;

        if (footnoteNumber != 1) {
            const allHrs = tempPastedDiv.querySelector('hr');
            if (allHrs) {
                const parent = tempPastedDiv.querySelector('hr').closest('div');
                if (parent) {
                    allHrs.remove();
                    const grandparent = parent.parentNode;

                    // Move all child elements of the parentDiv to the grandparent
                    while (parent.firstChild) {
                        grandparent.insertBefore(parent.firstChild, parent);
                    }

                    // Remove the parentDiv from the DOM
                    grandparent.removeChild(parent);
                }
            }
        }
        var elftn = tempPastedDiv.querySelectorAll('a[href^="#_ftnref"]');
        var elftnreg = tempPastedDiv.querySelectorAll('[href^="#_ftn"]:not([href^="#_ftnref"])');
        elftnreg.forEach((element) => {
            const match = element.getAttribute('href').match(/_ftn(\d+)/);
            let originalNumber;
            if (match) {
                originalNumber = parseInt(match[1], 10);
            }
            element.href = "#_ftn" + footnoteNumber;
            element.id = "_ftnref" + footnoteNumber;
            element.textContent = `[${footnoteNumber}]`;
            element.style.color = 'rgb(0, 0, 255)';


            const ftnelement = tempPastedDiv.querySelector(`a[href^="#_ftnref${originalNumber}"]`)


            ftnelement.href = "#_ftnref" + footnoteNumber;
            ftnelement.id = "_ftn" + footnoteNumber;
            ftnelement.textContent = `[${footnoteNumber}]`;
            ftnelement.style.color = 'rgb(0, 0, 255)';

            footnoteNumber++;
        });
        
        editor.selection.insertHTML(tempPastedDiv.innerHTML);

        return false;
    });

    document.addEventListener('change', () => {
        console.log("changed")
        const references = Array.from(editor.editor.querySelectorAll('[href^="#_ftn"]:not([href^="#_ftnref"])'));
        const footnotes = Array.from(editor.editor.querySelectorAll('[href^="#_ftnref"]'));

        // Step 2: Extract relevant data from the references and footnotes
        const refData = references.map((ref) => ({
            element: ref,
            id: ref.id,
            href: ref.getAttribute('href')
        }));

        const ftnData = footnotes.map((ftn) => ({
            element: ftn,
            id: ftn.id,
            href: ftn.getAttribute('href'),
            datainsert: ftn.getAttribute('datainsert')
        }));

        // Step 3: Sort the reference and footnote elements chronologically by their order in the document
        refData.sort((a, b) => a.element.sourceIndex - b.element.sourceIndex);
        ftnData.sort((a, b) => a.element.sourceIndex - b.element.sourceIndex);

        // Step 4: Reassign the reference and footnote IDs in order
        refData.forEach((ref, index) => {
            const newIndex = index + 1; // Assign new ID starting from 1
            const oldHref = ref.href.replace('#', '');
            const ftn = ftnData.find(ftn => ftn.id === oldHref || ftn.datainsert === "true");
            if (ftn) {
                // Update the reference ID and href
                ref.element.id = `_ftnref${newIndex}`;
                ref.element.href = `#_ftn${newIndex}`;
                ref.element.textContent = `[${newIndex}]`;
                ref.element.style.color = 'rgb(0, 0, 255)';

                // Find the corresponding footnote and update its ID and href
                const ftn = ftnData.find(ftn => ftn.id === oldHref || ftn.datainsert === "true");
                if (ftn) {
                    ftn.element.id = `_ftn${newIndex}`; // Update footnote ID
                    ftn.element.href = `#_ftnref${newIndex}`; // Update footnote href
                    ftn.element.textContent = `[${newIndex}]`;
                    ftn.element.setAttribute('datainsert', 'false');
                    ftn.element.style.color = 'rgb(0, 0, 255)';
                }
            }


        });
    })
    editor.events.on('change', () => {
 

    });
    editor.events.on('processHTML', function (e, value) {
        
        /*console.log(Jodit.modules.Helpers.cleanFromWord(value))*/

        const d = Jodit.modules.Helpers.cleanFromWord(value)
        debugger;
        return ;
    });


    let inputElement = document.querySelector("#hiddeninput");
    editor.events.on("change",
        e => {
            inputElement.value = editor.getEditorValue();
        });
}

var input = document.querySelector("#Becomings");
if (input) {
    var tagify = new Tagify(input,
    {
        dropdown: {
            enabled: 0
        }
    });
}

function copyFunction(fileurl) {
    // Get the text field
    navigator.clipboard.writeText(fileurl);
}


