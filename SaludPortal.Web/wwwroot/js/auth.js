window.authApi = (function () {

    const PENDING_CONSENT_KEY = 'PendingConsentimientoPCI';
    const AVISO_NO_ELEGIBLE_KEY_PREFIX = 'AvisoCuidar65Entendido:';

    function marcarConsentimientoPendiente() {
        sessionStorage.setItem(PENDING_CONSENT_KEY, '1');
    }

    function consumirConsentimientoPendiente() {
        const value = sessionStorage.getItem(PENDING_CONSENT_KEY);
        if (value) {
            sessionStorage.removeItem(PENDING_CONSENT_KEY);
        }
        return value === '1';
    }

    function limpiarConsentimientoPendiente() {
        sessionStorage.removeItem(PENDING_CONSENT_KEY);
    }

    function avisoNoElegibleKey(pacienteId) {
        return AVISO_NO_ELEGIBLE_KEY_PREFIX + pacienteId;
    }

    function fueAvisoNoElegibleEntendido(pacienteId) {
        if (!pacienteId) {
            return false;
        }
        return localStorage.getItem(avisoNoElegibleKey(pacienteId)) === '1';
    }

    function marcarAvisoNoElegibleEntendido(pacienteId) {
        if (!pacienteId) {
            return;
        }
        localStorage.setItem(avisoNoElegibleKey(pacienteId), '1');
    }

    async function login(data) {
        const resp = await fetch('api/auth/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include'
            , body: JSON.stringify(data)
        });
        const result = await parse(resp);
        if (result.ok) {
            marcarConsentimientoPendiente();
        }
        return result;
    }

    async function logout() {
        try {
            const resp = await fetch('api/auth/logout', {
                method: 'POST',
                credentials: 'include'
            });
            limpiarConsentimientoPendiente();
            if (resp.ok) {
                expireCookie('MiSalud');
                window.location.replace('/login');
            } else {
                console.warn('Logout fallù', resp.status);
            }
        } catch (e) {
            console.error('Error en logout', e);
        }
    }

    function expireCookie(name) {
        document.cookie = name + '=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT; SameSite=Lax;';
    }

    async function crearContrasenia(data) {
        const resp = await fetch('api/auth/crear-contrasenia', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include'
            , body: JSON.stringify(data)
        });
        const result = await parse(resp);
        if (result.ok) {
            marcarConsentimientoPendiente();
        }
        return result;
    }

    async function parse(resp) {
        let raw = await resp.text();
        let data = null;
        try { data = raw ? JSON.parse(raw) : null; } catch { data = { message: raw }; }
        return { ok: resp.ok, status: resp.status, data, raw };
    }

    return {
        login,
        logout,
        crearContrasenia,
        consumirConsentimientoPendiente,
        fueAvisoNoElegibleEntendido,
        marcarAvisoNoElegibleEntendido
    };

})();


//window.loginApi = {
//    login: async function (data) {
//        const resp = await fetch('api/auth/login', {
//            method: 'POST',
//            headers: { 'Content-Type': 'application/json' },
//            body: JSON.stringify(data),
//            credentials: 'include'
//        });
//        let text = await resp.text();
//        let json;
//        try { json = text ? JSON.parse(text) : null; } catch { json = { message: text }; }
//        return { ok: resp.ok, status: resp.status, data: json ?? {}, raw: text };
//    }
//};