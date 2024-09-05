function checkScreenSize() {
    const sizeGuard = document.querySelector('#size-guard');
    if (window.innerWidth < 768) {
        sizeGuard.style.display = 'flex';
    } else {
        sizeGuard.style.display = 'none';
    }
}

checkScreenSize();

window.addEventListener('resize', checkScreenSize);