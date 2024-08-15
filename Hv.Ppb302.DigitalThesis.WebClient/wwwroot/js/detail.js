document.addEventListener('DOMContentLoaded', function () {
    const audioPlayer = document.getElementById('audio-player');
    const audioImg = document.getElementById('audio-img');

    audioPlayer.addEventListener('click', function (event) {
        event.preventDefault();
        if (audioPlayer.paused) {
            audioPlayer.play();
            audioImg.src = '/images/icons/stop_icon.png';
        } else {
            audioPlayer.pause();
            audioPlayer.currentTime = 0;
            audioImg.src = '/images/icons/play_icon.png';
        }
    });

    audioPlayer.addEventListener('ended', function () {
        audioImg.src = 'https://informatik13.ei.hv.se/digitalthesis/images/icons/play_icon.png';
    });
});

setTimeout(function () {
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
    const contentContainer = document.getElementById('content-container');
    if (contentContainer) {
        contentContainer.innerHTML = doc.body.innerHTML;
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
}, 100);
