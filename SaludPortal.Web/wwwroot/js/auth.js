window.authApi = (function () {

    const PENDING_CONSENT_KEY = 'PendingConsentimientoPCI';
    const EVALUACION_CACHE_KEY_PREFIX = 'ConsentimientoPCIEvaluacion:';
    const AVISO_NO_ELEGIBLE_KEY_PREFIX = 'AvisoCuidar65Entendido:';

    function evaluacionCacheKey(pacienteId) {
        return EVALUACION_CACHE_KEY_PREFIX + pacienteId;
    }

    function limpiarCacheEvaluacionConsentimiento() {
        const keysToRemove = [];
        for (let i = 0; i < sessionStorage.length; i++) {
            const key = sessionStorage.key(i);
            if (key && key.startsWith(EVALUACION_CACHE_KEY_PREFIX)) {
                keysToRemove.push(key);
            }
        }
        keysToRemove.forEach(key => sessionStorage.removeItem(key));
    }

    function marcarConsentimientoPendiente() {
        limpiarCacheEvaluacionConsentimiento();
        sessionStorage.setItem(PENDING_CONSENT_KEY, '1');
    }

    function tieneConsentimientoPendiente() {
        return sessionStorage.getItem(PENDING_CONSENT_KEY) === '1';
    }

    function limpiarConsentimientoPendiente() {
        sessionStorage.removeItem(PENDING_CONSENT_KEY);
        limpiarCacheEvaluacionConsentimiento();
    }

    function resolverConsentimientoPendiente(pacienteId) {
        sessionStorage.removeItem(PENDING_CONSENT_KEY);
        if (pacienteId) {
            sessionStorage.removeItem(evaluacionCacheKey(pacienteId));
        }
    }

    function obtenerEvaluacionConsentimientoCacheada(pacienteId) {
        if (!pacienteId) {
            return null;
        }

        const raw = sessionStorage.getItem(evaluacionCacheKey(pacienteId));
        if (!raw) {
            return null;
        }

        try {
            return JSON.parse(raw);
        } catch {
            sessionStorage.removeItem(evaluacionCacheKey(pacienteId));
            return null;
        }
    }

    function guardarEvaluacionConsentimientoCacheada(pacienteId, evaluacion) {
        if (!pacienteId || !evaluacion) {
            return;
        }

        sessionStorage.setItem(evaluacionCacheKey(pacienteId), JSON.stringify(evaluacion));
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
        tieneConsentimientoPendiente,
        resolverConsentimientoPendiente,
        obtenerEvaluacionConsentimientoCacheada,
        guardarEvaluacionConsentimientoCacheada,
        fueAvisoNoElegibleEntendido,
        marcarAvisoNoElegibleEntendido
    };

})();
