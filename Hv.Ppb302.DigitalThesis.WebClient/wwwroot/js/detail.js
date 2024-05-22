document.addEventListener('DOMContentLoaded', function () {
    const audioPlayer = document.getElementById('audio-player');
    audioPlayer.addEventListener('click', function (event) {
        event.preventDefault();
        const audioImg = document.getElementById('audio-img');
        console.log("test");

        if (audioPlayer.paused) {
            audioPlayer.play();
            audioImg.src = '/images/icons/stop_icon.png';
            console.log(audioImg.src);
        } else {
            audioPlayer.pause();
            audioPlayer.currentTime = 0;
            audioImg.src = '/images/icons/play_icon.png';
            console.log(audioImg.src);
        }
    });
});