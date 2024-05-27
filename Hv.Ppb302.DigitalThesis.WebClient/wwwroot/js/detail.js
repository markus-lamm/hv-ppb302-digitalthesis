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
