document.addEventListener('DOMContentLoaded', function () {
    // Select all content-options-btn elements
    const buttons = document.querySelectorAll('.content-options-btn');

    buttons.forEach(button => {
        button.addEventListener('click', function (event) {
            // Prevent the default action to stop the browser from navigating away
            event.preventDefault();

            // Determine which button was clicked
            //const isPdfButton = event.target.querySelector('.pdf_icon');
            const isAudioButton = event.target.querySelector('#audio-icon');

            //if (isPdfButton) {
            //    // Trigger the download of the PDF file
            //    window.location.href = event.target.dataset.tags;
            //}
            if (isAudioButton) {
                // Play the audio
                const audioPlayer = document.getElementById('audio-player');
                if (audioPlayer.paused) {
                    audioPlayer.play();
                }
                else {
                    audioPlayer.pause();
                    // Reset the audio player to the beginning
                    audioPlayer.currentTime = 0;
                }
            }
        });
    });
});
