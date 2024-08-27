// Audioplayer
const audioButton = document.getElementById('audio-icon');
if (audioButton) {
    const audioPlayer = document.getElementById('audio-player');
    const audioImagePlay = document.getElementById('audio-img-play');
    const audioImageStop = document.getElementById('audio-img-stop');

    // Add click event listener to the button
    audioButton.addEventListener('click', function () {
        // Check if audio is currently playing
        audioButton.classList.toggle("audio-icon-bckg-active")

        if (audioPlayer.paused) {
            // If paused, play the audio
            audioPlayer.play();
            audioImagePlay.style.display = "none";
            audioImageStop.style.display = "flex";
        } else {
            // If playing, pause the audio and reset the time to 0
            audioPlayer.pause();
            audioPlayer.currentTime = 0;
            audioImagePlay.style.display = "flex";
            audioImageStop.style.display = "none";
        }
    });
}

// PDF download
const pdfDownload = document.getElementById('pdf-download-btn');
if (pdfDownload) {
    document.getElementById('pdf-download-btn').addEventListener('click', async function () {
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

//Text reset
document.addEventListener('DOMContentLoaded', function () {
    var textContainer = document.querySelector('.text-container');
    var textResetBtn = document.getElementById('text-reset-btn');

    textContainer.addEventListener('scroll', function () {
        if (textContainer.scrollTop > 0) {
            textResetBtn.classList.remove('hidden');
            textResetBtn.classList.add('visible');
        } else {
            textResetBtn.classList.remove('visible');
            textResetBtn.classList.add('hidden');
        }
    });

    textResetBtn.addEventListener('click', function () {
        textContainer.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });
});

// Navigationmenu
document.addEventListener('DOMContentLoaded', function () {
    const joditHTML = editor.getEditorValue();

    // Parse the HTML string to create a DOM structure
    const parser = new DOMParser();
    const doc = parser.parseFromString(joditHTML, 'text/html');

    // Store the text content and tag name of each heading element in a new list
    const headingList = [];

    // Function to recursively collect headings in order
    function collectHeadings(element) {
        if (element.tagName === 'H1' || element.tagName === 'H2') {
            const id = `heading-${headingList.length}`;
            element.id = id;
            headingList.push({ tag: element.tagName.toLowerCase(), text: element.textContent, id: id });
        }
        Array.from(element.children).forEach(child => collectHeadings(child));
    }
    collectHeadings(doc.body);

    // Render the parsed HTML content into a container on the page
    const editorContainer = document.getElementById('editor-container');
    if (editorContainer) {
        editorContainer.innerHTML = doc.body.innerHTML;
    }

    const navmenu = document.getElementById('navmenu');
    if (navmenu) {
        for (let i = 0; i < headingList.length; i++) {
            const navmenuItem = document.createElement('a');
            navmenuItem.href = `#${headingList[i].id}`;
            navmenuItem.id = `nav-${headingList[i].id}`;
            if (headingList[i].tag === 'h1') {
                navmenuItem.className = 'navmenu-title';
            } else if (headingList[i].tag === 'h2') {
                navmenuItem.className = 'navmenu-subtitle';
            }
            navmenuItem.textContent = headingList[i].text;
            navmenu.appendChild(navmenuItem);
        }
    }

    // Highlight the current section in the navmenu
    const observerOptions = {
        root: null,
        rootMargin: '0px',
        threshold: 0.1
    };

    const observerCallback = (entries) => {
        entries.forEach(entry => {
            const navItem = document.getElementById(`nav-${entry.target.id}`);
            if (entry.isIntersecting) {
                navItem.classList.add('active');
            } else {
                navItem.classList.remove('active');
            }
        });
    };

    const observer = new IntersectionObserver(observerCallback, observerOptions);

    headingList.forEach(heading => {
        const target = document.getElementById(heading.id);
        if (target) {
            observer.observe(target);
        }
    });
});

// Info sidebar minimize
document.addEventListener('DOMContentLoaded', function () {
    const infoMinimizeBtn = document.getElementById('info-minimize-btn');
    const infoMinimizeImg = document.getElementById('info-minimize-img');
    let isRotated = false;

    infoMinimizeBtn.addEventListener('click', function () {
        const infoContainer = document.getElementById('info-container');

        if (infoContainer) {
            infoContainer.classList.toggle('minimized');
        }

        isRotated = !isRotated;
        infoMinimizeImg.style.transform = isRotated ? 'rotate(180deg)' : 'rotate(0deg)';
    });
});
