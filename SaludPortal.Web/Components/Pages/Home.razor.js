export function initHomeBannerBlur(selector) {
    const img = document.querySelector(selector);
    if (!img) return { dispose() { } };

    const maxBlurPx = 14;      // Máximo desenfoque
    const startPx = 0;         // Umbral inicial (px de scroll para empezar)
    const rangePx = 600;       // Rango de píxeles de scroll para llegar al máximo (ajusta)

    function computeBlur(scrollTop) {
        if (scrollTop <= startPx) return 0;
        const rel = Math.min((scrollTop - startPx) / rangePx, 1);
        return +(rel * maxBlurPx).toFixed(2);
    }

    let ticking = false;
    function onScroll() {
        if (ticking) return;
        ticking = true;
        requestAnimationFrame(() => {
            const blur = computeBlur(window.scrollY);
            img.style.setProperty('--blur', blur + 'px');
            ticking = false;
        });
    }

    window.addEventListener('scroll', onScroll, { passive: true });
    // Inicial
    img.style.setProperty('--blur', '0px');

    return {
        dispose() {
            window.removeEventListener('scroll', onScroll);
        }
    };
}