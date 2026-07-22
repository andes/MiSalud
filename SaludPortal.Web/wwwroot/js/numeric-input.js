window.numericInput = {
    preventNonNumeric: function (event) {
        if (event.ctrlKey || event.metaKey || event.altKey) {
            return;
        }

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
    },

    sanitizeDigitsInput: function (event) {
        const input = event.target;
        if (!input) {
            return;
        }

        const raw = input.value || '';
        const selectionStart = input.selectionStart ?? raw.length;
        const maxLength = input.maxLength > 0 ? input.maxLength : null;

        let digitsBeforeCaret = 0;
        for (let i = 0; i < Math.min(selectionStart, raw.length); i++) {
            if (/\d/.test(raw[i])) {
                digitsBeforeCaret++;
            }
        }

        let cleaned = raw.replace(/\D/g, '');
        if (maxLength !== null) {
            cleaned = cleaned.slice(0, maxLength);
            digitsBeforeCaret = Math.min(digitsBeforeCaret, cleaned.length);
        }

        if (cleaned !== raw) {
            input.value = cleaned;
            input.setSelectionRange(digitsBeforeCaret, digitsBeforeCaret);
        }
    },

    // Capture phase: corre antes del @oninput de Blazor, sin carrera por SignalR.
    attachSanitize: function (element) {
        if (!element || element.dataset.numericSanitize === '1') {
            return;
        }
        element.dataset.numericSanitize = '1';
        element.addEventListener('input', function (e) {
            window.numericInput.sanitizeDigitsInput(e);
        }, true);
    },

    pasteDigitsOnly: function (event) {
        event.preventDefault();

        const input = event.target;
        const pasted = (event.clipboardData || window.clipboardData).getData('text') || '';
        const digits = pasted.replace(/\D/g, '');
        if (!digits) {
            return;
        }

        const maxLength = input.maxLength > 0 ? input.maxLength : null;
        const start = input.selectionStart ?? input.value.length;
        const end = input.selectionEnd ?? input.value.length;
        const before = input.value.slice(0, start).replace(/\D/g, '');
        const after = input.value.slice(end).replace(/\D/g, '');
        let next = before + digits + after;

        if (maxLength !== null) {
            next = next.slice(0, maxLength);
        }

        const caret = Math.min(before.length + digits.length, next.length);
        input.value = next;
        input.setSelectionRange(caret, caret);
        input.dispatchEvent(new Event('input', { bubbles: true }));
    },

    setCaret: function (element, position) {
        if (!element || typeof element.setSelectionRange !== 'function') {
            return;
        }

        // No robar el foco (p. ej. al hacer click en "Crear Cuenta").
        if (document.activeElement !== element) {
            return;
        }

        const max = element.value ? element.value.length : 0;
        const caret = Math.max(0, Math.min(position, max));
        element.setSelectionRange(caret, caret);
    }
};
