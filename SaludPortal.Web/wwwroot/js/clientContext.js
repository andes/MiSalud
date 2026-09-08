window.saludClientContext = {
    getDeviceContext: function () {
        const ua = navigator.userAgent || "";
        const platform = navigator.userAgentData?.platform || navigator.platform || "";

        return {
            userAgent: ua,
            browser: detectBrowser(ua),
            osName: detectOsName(ua, platform),
            osVersion: detectOsVersion(ua),
            deviceType: detectDeviceType(ua)
        };
    },

    tryGetGeolocation: function () {
        return new Promise((resolve) => {
            if (!navigator.geolocation) {
                resolve(null);
                return;
            }

            navigator.geolocation.getCurrentPosition(
                (position) => {
                    resolve({
                        latitude: Number(position.coords.latitude),
                        longitude: Number(position.coords.longitude)
                    });
                },
                () => resolve(null),
                {
                    enableHighAccuracy: false,
                    timeout: 8000,
                    maximumAge: 300000
                });
        });
    }
};

function detectBrowser(ua) {
    if (ua.includes("Edg/")) return "Edge";
    if (ua.includes("OPR/") || ua.includes("Opera")) return "Opera";
    if (ua.includes("Firefox/")) return "Firefox";
    if (ua.includes("Chrome/")) return "Chrome";
    if (ua.includes("Safari/")) return "Safari";
    return "Unknown";
}

function detectOsName(ua, platform) {
    if (ua.includes("Windows")) return "Windows";
    if (ua.includes("Android")) return "Android";
    if (ua.includes("iPhone") || ua.includes("iPad")) return "iOS";
    if (ua.includes("Mac OS X") || platform === "MacIntel") return "macOS";
    if (ua.includes("Linux") || platform.includes("Linux")) return "Linux";
    return "Unknown";
}

function detectOsVersion(ua) {
    const win = ua.match(/Windows NT ([0-9.]+)/);
    if (win) return win[1];

    const android = ua.match(/Android ([0-9.]+)/);
    if (android) return android[1];

    const ios = ua.match(/OS ([0-9_]+)/);
    if (ios) return ios[1].replace(/_/g, ".");

    const mac = ua.match(/Mac OS X ([0-9_]+)/);
    if (mac) return mac[1].replace(/_/g, ".");

    return null;
}

function detectDeviceType(ua) {
    if (ua.includes("iPad") || ua.includes("Tablet")) return "Tablet";
    if (ua.includes("Mobi") || ua.includes("Android")) return "Mobile";
    return "Desktop";
}
