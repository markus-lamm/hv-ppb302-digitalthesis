//JODIT PLUGINS
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
            const height = data.Width * 0.56;
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

                const footnoteId = `ftn${footnoteNumber}`;
                const footnoteRefId = `_ftnref${footnoteNumber}`;

                const footnoteMarker =
                    `<a href="#${footnoteId}" name="${footnoteRefId
                        }" title=""><span class="MsoFootnoteReference" style="vertical-align: super;"><span style="font-size: 15px; line-height: 107%; font-family: Aptos, sans-serif; vertical-align: super;">${
                        footnoteNumber}</span></span></a>&nbsp;`;

                const footnoteContent = `
                        <div id="${footnoteId}">
                            <p class="MsoFootnoteText" style="margin: 0px; font-size: 13px; font-family: Aptos, sans-serif;">
                                <a href="#${footnoteRefId}" name="${footnoteId}" title="">
                                    <span class="MsoFootnoteReference" style="vertical-align: super;">
                                        <span style="font-size: 13px; line-height: 107%; font-family: Aptos, sans-serif;">[${
                    footnoteNumber}]</span>
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
            scrollOffset: 0
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

        const hrElement = hrElements[hrElements.length - 1].parentElement;
        // Get the parent <div> of the <hr>
        const parentDiv = hrElement.closest('div');

        // Get all previous elements before the <div>
        const previousElements = getAllPreviousElements(parentDiv);
        previousElements.forEach(element => {
            parentDiv.appendChild(element); // Append each element individually
        });

        // Log the elements before the <div>
        console.log(previousElements);
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
            allHrs.remove();
            
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


            const ftnelement = tempPastedDiv.querySelector(`a[href^="#_ftnref${originalNumber}"]`)
            

            ftnelement.href = "#_ftnref" + footnoteNumber;
            ftnelement.id = "_ftn" + footnoteNumber;
            ftnelement.textContent = `[${footnoteNumber}]`;
            

            footnoteNumber++;
        });

        editor.selection.insertHTML(tempPastedDiv.innerHTML);

        return false;
    });

    editor.events.on('change', () => {
        const ftnrefElements = editor.editor.querySelectorAll('[href^="#_ftn"]:not([href^="#_ftnref"])');
        const k = editor.editor;

        // Step 1: Gather all reference and footnote elements
        const references = editor.editor.querySelectorAll('[href^="#_ftn"]:not([href^="#_ftnref"])');
        const footnotes = editor.editor.querySelectorAll('[href^="#_ftnref"]');

        // Step 2: Extract relevant data from the references and footnotes
        const refData = Array.from(references).map((ref) => ({
            element: ref,
            id: ref.id,
            href: ref.getAttribute('href')
        }));

        const ftnData = Array.from(footnotes).map((ftn) => ({
            element: ftn,
            id: ftn.id,
            href: ftn.getAttribute('href')
        }));

        // Step 3: Sort the reference and footnote elements chronologically by their order in the document
        refData.sort((a, b) => a.element.compareDocumentPosition(b.element) & Node.DOCUMENT_POSITION_FOLLOWING ? -1 : 1);
        ftnData.sort((a, b) => a.element.compareDocumentPosition(b.element) & Node.DOCUMENT_POSITION_FOLLOWING ? -1 : 1);

        // Step 4: Reassign the reference and footnote IDs in order
        refData.forEach((ref, index) => {
            const newIndex = index + 1; // Assign new ID starting from 1
            const oldHref = ref.href;

            // Update the reference ID and href
            ref.element.id = `_ftnref${newIndex}`;
            ref.element.href = `#_ftn${newIndex}`;

            // Find the corresponding footnote and update its ID and href
            const ftn = ftnData.find(ftn => ftn.href === oldHref);
            if (ftn) {
                ftn.element.id = `_ftn${newIndex}`;
                ftn.element.href = `#_ftnref${newIndex}`;
            }
        });

        // Loop through each <a> tag and update href and id

        //ftnrefElements.forEach(function (element, index) {
        //    const match = element.getAttribute('href').match(/_ftn(\d+)/);
        //    // New number based on the order in the document (starting from 1)
        //    var footnoteNumber = index + 1;
            
        //    let originalNumber;
        //    if (match) {
        //        originalNumber = parseInt(match[1], 10);
        //    }

        //    element.href = "#_ftn" + footnoteNumber;
        //    element.id = "_ftnref" + footnoteNumber;
        //    element.textContent = `[${footnoteNumber}]`;


        //    //const ftnelement = k.querySelector(`a[href^="#_ftnref${originalNumber}"]`)
        //    //console.log(ftnelement+  " Numbe:" +footnoteNumber)
        //    //ftnelement.href = "#_ftnref" + footnoteNumber;
        //    //ftnelement.id = "_ftn" + footnoteNumber;
        //    //ftnelement.textContent = `[${footnoteNumber}]`;
        //});

    });
    editor.events.on('processHTML', function (e, value) {
        
        /*console.log(Jodit.modules.Helpers.cleanFromWord(value))*/

        const d = Jodit.modules.Helpers.cleanFromWord(value)
        debugger;
        return ;
    });
    //editor.events.on('change', function (e, value, texts) {
    ////    // Cleaned HTML and existing editor content
    ////    const cleanedHtml = Jodit.modules.Helpers.cleanFromWord(value);
    ////    const editorHTML = editor.getEditorValue();

    ////    // Create a temp div to parse the pasted content
    ////    const tempPastedDiv = document.createElement('div');
    ////    tempPastedDiv.innerHTML = cleanedHtml;

    ////    // Query for footnotes in the pasted content
    ////    const footnoteReferences = Array.from(tempPastedDiv.querySelectorAll('[href^="#_ftn"]:not([href^="#_ftnref"])'));

    ////    // Get the current selection range in the editor
    ////    let range = editor.selection.sel?.getRangeAt(0);

    ////    // Variables to hold the references before and after the cursor
    ////    let footnotesBeforeCursor = [];
    ////    let footnotesAfterCursor = [];

    ////    if (range) {
    ////        let currentContainer = range.startContainer;
    ////        let startOffset = range.startOffset;

    ////        // *** Part 1: Get footnote references before the cursor ***

    ////        let beforeRange = document.createRange();
    ////        beforeRange.setStart(editor.editor.firstChild, 0);  // Start from the beginning of the editor
    ////        beforeRange.setEnd(currentContainer, startOffset);  // End at the cursor position

    ////        let contentBeforeCursor = beforeRange.cloneContents();
    ////        let beforeContainerDiv = document.createElement('div');
    ////        beforeContainerDiv.appendChild(contentBeforeCursor);

    ////        // Get footnote references before the cursor
    ////        footnotesBeforeCursor = Array.from(beforeContainerDiv.querySelectorAll('[href^="#_ftn"]:not([href^="#_ftnref"])'));

    ////        // *** Part 2: Get footnote references after the cursor ***

    ////        let afterRange = document.createRange();
    ////        afterRange.setStart(currentContainer, startOffset); // Start from cursor position
    ////        afterRange.setEndAfter(editor.editor.lastChild);    // End after the last element

    ////        let contentAfterCursor = afterRange.cloneContents();
    ////        let afterContainerDiv = document.createElement('div');
    ////        afterContainerDiv.appendChild(contentAfterCursor);

    ////        // Get footnote references after the cursor
    ////        footnotesAfterCursor = Array.from(afterContainerDiv.querySelectorAll('[href^="#_ftn"]:not([href^="#_ftnref"])'));
    ////    }

    ////    // Find the last matching footnote reference before the cursor that matches with pasted content
    ////    for (let i = footnotesBeforeCursor.length - 1; i >= footnoteReferences.length; i--) {
    ////        let footnoteBefore = footnotesBeforeCursor[i];

    ////        // Find if the footnote matches one from the pasted content
    ////        const matchingFootnote = footnoteReferences.find(footnoteRef => footnoteBefore.getAttribute('href') === footnoteRef.getAttribute('href'));

    ////        if (matchingFootnote) {
    ////            // Remove the last matching footnote reference from footnotesBeforeCursor
    ////            footnoteBefore.remove();
    ////            break; // Stop after removing the last match
    ////        }
    ////    }

    ////    // Log results
    ////    console.log('Remaining Footnote References Before Cursor:', footnotesBeforeCursor);
    ////    console.log('Footnote References After Cursor:', footnotesAfterCursor);

    ////    // Get the current selection range in the editor
    ////     // Get the start position of the selection

    ////    //if (markerPosition !== null) {
    ////    //    // Retrieve the editor's value using the appropriate method
    ////    //    const editorContent = editor.value || editor.getEditorValue(); // Adjust this based on your editor API

    ////    //    // Extract the text before the marker position
    ////    //    const textBeforeMarker = editorContent.substring(0, markerPosition);

    ////    //    console.log('Text before ', textBeforeMarker);
    ////    //    // Extract existing footnote numbers from the text before the marker
    ////    //    const existingFootnoteNumbers = Array.from(textBeforeMarker.matchAll(/_ftn(\d+)/g)).map(match => parseInt(match[1], 10));

    ////    //    // Determine the highest footnote number in the existing text
    ////    //    const highestFootnoteNumber = existingFootnoteNumbers.length > 0 ? Math.max(...existingFootnoteNumbers) : 0;

    ////    //    // Update the footnote references in the pasted content
    ////    //    footnoteReferences.forEach((footnote, index) => {
    ////    //        const match = footnote.getAttribute('href').match(/_ftn(\d+)/);
    ////    //        if (match) {
    ////    //            const originalNumber = parseInt(match[1], 10);
    ////    //            const newNumber = highestFootnoteNumber + index + 1; // Continue numbering from the highest existing number
    ////    //            const updatedHref = footnote.getAttribute('href').replace(/_ftn\d+/, `_ftn${newNumber}`);
    ////    //            footnote.setAttribute('href', updatedHref);

    ////    //            // Optionally, update the inner text or other relevant attributes
    ////    //            // footnote.innerHTML = `Footnote ${newNumber}`; // Example for updating text
    ////    //        }
    ////    //    });

            
    ////    //} else {
    ////    //    console.log('No marker position found. Cannot update footnote references.');
    ////    //}


    //    // Get the cleaned HTML content after paste
    //    var cleanedContent = editor.getEditorValue();
    //    const allHrs = editor.editor.querySelectorAll('hr');

    //    // Check if there are any <hr> elements
    //    if (allHrs.length > 0) {
    //        // Get the last <hr> element
    //        const lastHr = allHrs[allHrs.length - 1];

    //        // Remove all <hr> elements except the last one
    //        allHrs.forEach(hr => {
    //            if (hr !== lastHr) {
    //                hr.remove();
    //            }
    //        });
    //    }

    //    // Set the id attributes for <a> elements with href matching #_ftnref and #_ftn
    //    const ftnElements = editor.editor.querySelectorAll('a[href^="#_ftn"]');
    //    const ftnrefElements = editor.editor.querySelectorAll('a[href^="#_ftnref"]');
    //    const footnoteNumber = ftnrefElements.length + 1;

    //    ftnElements.forEach(el => {
    //        const match = el.href.match(/#_ftn(\d+)/);
    //        if (match) {
    //            const number = match[1];
    //            // Set the id to "_ftnref" + number
    //            el.id = `_ftnref${number}`;
    //        }
    //    });

        
    //    ftnrefElements.forEach(el => {
    //        const match = el.href.match(/#_ftnref(\d+)/);
    //        if (match) {
    //            const number = match[1];
    //            // Set the id to "_ftn" + number
    //            el.id = `_ftn${number}`;
    //        }
    //    });

    //    // Select all <a> elements with href starting with "#_ftnref"
    //    const linksWithFtnref = editor.editor.querySelectorAll('div > p > a[href^="#_ftnref"]');
    //    console.log()
    //    // Collect the closest ancestor <div> for each <a> element
    //    const divsWithFtnref = Array.from(linksWithFtnref).map(link => link.closest('div'));
    //    const lastHrDiv = allHrs[allHrs.length - 1].parentElement;
    //    divsWithFtnref.forEach(div => {
    //        lastHrDiv.appendChild(div);
    //    });


    //});

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
