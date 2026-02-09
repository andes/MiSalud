window.numericInput = {
    preventNonNumeric: function (event) {
        const key = event.key;
        // Permitir teclas de control
        if (key === 'Backspace' || key === 'Delete' || key === 'Tab' ||
            key === 'ArrowLeft' || key === 'ArrowRight' || key === 'Home' || key === 'End') {
            return;
        }
        // Prevenir si no es dígito
        if (!/^\d$/.test(key)) {
            event.preventDefault();
        }
    }
};