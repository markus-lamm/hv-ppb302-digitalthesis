//Mosaics
function hideMosaics() {
    var mosaics = document.querySelectorAll('.mosaic-container .mosaic-float .mosaic');
    mosaics.forEach(function (mosaic) {
        mosaic.style.display = "none";
    });
}

function showMosaics() {
    var mosaics = document.querySelectorAll('.mosaic-container .mosaic-float .mosaic');
    mosaics.forEach(function (mosaic) {
        mosaic.style.display = "";
    });
}

$(document).ready(function () {
    $(".mosaic").click(function () {
        var mosaicId = $(this).attr("id");
        window.location.href = "/Home/Mosaics?mosaicId=" + mosaicId;
    });
});

//Filter sidebar
function openNav() {
    document.getElementById("mySidebar").style.width = "250px";
    document.getElementById("main").style.marginLeft = "250px";
    let btn = document.querySelector('.openbtn');
    btn.classList.add('hide');
}

function closeNav() {
    document.getElementById("mySidebar").style.width = "0";
    document.getElementById("main").style.marginLeft = "0";

    let btn = document.querySelector('.openbtn');
    btn.classList.remove('hide');
}