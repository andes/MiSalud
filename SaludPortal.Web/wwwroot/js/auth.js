window.authApi = (function () {

    async function login(data) {
        const resp = await fetch('api/auth/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include'
            , body: JSON.stringify(data)
        });
        return await parse(resp);
    }

    async function logout() {
        try {
            const resp = await fetch('api/auth/logout', {
                method: 'POST',
                credentials: 'include'
            });
            if (resp.ok) {
                expireCookie('MiSalud');
                window.location.replace('/login');
            } else {
                console.warn('Logout falló', resp.status);
            }
        } catch (e) {
            console.error('Error en logout', e);
        }
    }

    function expireCookie(name) {
        document.cookie = name + '=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT; SameSite=Lax;';
    }

    async function parse(resp) {
        let raw = await resp.text();
        let data = null;
        try { data = raw ? JSON.parse(raw) : null; } catch { data = { message: raw }; }
        return { ok: resp.ok, status: resp.status, data, raw };
    }

    return { login, logout };

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