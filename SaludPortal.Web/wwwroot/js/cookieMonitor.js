window.cookieMonitor = {
    checkCookies: function () {
        console.log('Checking cookies...');
        console.log('Document cookies:', document.cookie);
        
        let cookies = document.cookie.split(';').reduce((cookies, cookie) => {
            const [name, value] = cookie.split('=').map(c => c.trim());
            cookies[name] = value;
            return cookies;
        }, {});
        
        return cookies;
    },
    
    getCookie: function (name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
        return null;
    }
};