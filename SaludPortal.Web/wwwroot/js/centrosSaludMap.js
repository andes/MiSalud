(function () {
    const instances = {};
    let googleMapsLoadingPromise = null;
    let markerClustererLoaded = false;

    function loadGoogleMaps(apiKey) {
        if (typeof window.google !== "undefined" && window.google.maps) {
            return Promise.resolve();
        }

        if (googleMapsLoadingPromise) {
            return googleMapsLoadingPromise;
        }

        googleMapsLoadingPromise = new Promise((resolve, reject) => {
            const existing = document.querySelector("script[data-gmaps='true']");
            if (existing) {
                existing.addEventListener("load", () => resolve(), { once: true });
                existing.addEventListener("error", () => reject(new Error("No se pudo cargar Google Maps.")), { once: true });
                return;
            }

            const script = document.createElement("script");
            script.src = `https://maps.googleapis.com/maps/api/js?key=${encodeURIComponent(apiKey)}`;
            script.async = true;
            script.defer = true;
            script.dataset.gmaps = "true";
            script.onload = () => resolve();
            script.onerror = () => reject(new Error("No se pudo cargar Google Maps."));
            document.head.appendChild(script);
        });

        return googleMapsLoadingPromise;
    }

    function loadMarkerClusterer() {
        if (markerClustererLoaded && window.markerClusterer) {
            return Promise.resolve();
        }

        return new Promise((resolve, reject) => {
            const script = document.createElement("script");
            script.src = "https://unpkg.com/@googlemaps/markerclusterer/dist/index.min.js";
            script.async = true;
            script.onload = () => { markerClustererLoaded = true; resolve(); };
            script.onerror = () => reject(new Error("No se pudo cargar MarkerClusterer."));
            document.head.appendChild(script);
        });
    }

    function toMarkerIcon(selected) {
        return {
            path: google.maps.SymbolPath.CIRCLE,
            scale: selected ? 10 : 8,
            fillColor: selected ? "#87b867" : "#00374c",
            fillOpacity: 1,
            strokeColor: selected ? "#00374c" : "#87b867",
            strokeWeight: selected ? 4 : 3
        };
    }

    function closeAllInfoWindows(instance) {
        Object.values(instance.markers).forEach((item) => {
            if (item.infoWindow) {
                item.infoWindow.close();
            }
        });
    }

    function normalizeCentros(centros) {
        if (!Array.isArray(centros)) {
            return [];
        }

        return centros
            .filter((c) => c && c.id && c.latitud != null && c.longitud != null)
            .map((c) => ({
                id: c.id,
                nombre: c.nombre || "Centro de Salud",
                direccion: c.direccion || "Sin direccion",
                region: c.region || "-",
                comunidad: c.comunidad || "-",
                complejidad: c.complejidad || "-",
                telefono: c.telefono || "-",
                origen: c.origen || "-",
                latitud: Number(c.latitud),
                longitud: Number(c.longitud)
            }))
            .filter((c) => Number.isFinite(c.latitud) && Number.isFinite(c.longitud));
    }

    // Renderer personalizado para los clusters con colores de la app
    function createClusterRenderer() {
        return {
            render({ count, position }) {
                const size = count < 10 ? 36 : count < 50 ? 44 : count < 200 ? 52 : 60;
                const svg = `<svg xmlns="http://www.w3.org/2000/svg" width="${size}" height="${size}" viewBox="0 0 ${size} ${size}">
                    <circle cx="${size / 2}" cy="${size / 2}" r="${size / 2 - 2}" fill="#00374c" fill-opacity="0.85" stroke="#87b867" stroke-width="3"/>
                    <text x="50%" y="52%" text-anchor="middle" dominant-baseline="middle" fill="#fff" font-family="Arial" font-size="${size < 44 ? 12 : 14}" font-weight="bold">${count}</text>
                </svg>`;

                return new google.maps.Marker({
                    position,
                    icon: {
                        url: "data:image/svg+xml;charset=UTF-8," + encodeURIComponent(svg),
                        scaledSize: new google.maps.Size(size, size),
                        anchor: new google.maps.Point(size / 2, size / 2)
                    },
                    zIndex: Number(google.maps.Marker.MAX_ZINDEX) + count
                });
            }
        };
    }

    function showUserLocation(instance, position) {
        const latLng = {
            lat: position.coords.latitude,
            lng: position.coords.longitude
        };

        if (!instance.userMarker) {
            instance.userMarker = new google.maps.Marker({
                position: latLng,
                map: instance.map,
                title: "Tu ubicacion",
                zIndex: Number(google.maps.Marker.MAX_ZINDEX) + 1000,
                icon: {
                    path: google.maps.SymbolPath.CIRCLE,
                    scale: 9,
                    fillColor: "#1a73e8",
                    fillOpacity: 1,
                    strokeColor: "#ffffff",
                    strokeWeight: 3
                }
            });
        } else {
            instance.userMarker.setPosition(latLng);
            instance.userMarker.setMap(instance.map);
        }

        instance.map.panTo(latLng);
        instance.map.setZoom(15);
    }

    function createUserLocationControl(instance) {
        const controlButton = document.createElement("button");
        controlButton.type = "button";
        controlButton.title = "Ir a mi ubicacion";
        controlButton.setAttribute("aria-label", "Ir a mi ubicacion");
        controlButton.innerHTML = "<span style='font-size:18px;line-height:1;'>◎</span>";
        controlButton.style.backgroundColor = "#fff";
        controlButton.style.border = "0";
        controlButton.style.borderRadius = "4px";
        controlButton.style.boxShadow = "0 2px 6px rgba(0,0,0,.3)";
        controlButton.style.cursor = "pointer";
        controlButton.style.margin = "10px";
        controlButton.style.padding = "0 12px";
        controlButton.style.height = "40px";
        controlButton.style.width = "40px";
        controlButton.style.display = "flex";
        controlButton.style.alignItems = "center";
        controlButton.style.justifyContent = "center";
        controlButton.style.color = "#00374c";

        controlButton.addEventListener("click", () => {
            if (!navigator.geolocation) {
                window.alert("Tu navegador no soporta geolocalizacion.");
                return;
            }

            controlButton.disabled = true;
            controlButton.style.opacity = "0.7";

            navigator.geolocation.getCurrentPosition(
                (position) => {
                    showUserLocation(instance, position);
                    controlButton.disabled = false;
                    controlButton.style.opacity = "1";
                },
                () => {
                    window.alert("No se pudo obtener tu ubicacion. Verifica los permisos del navegador.");
                    controlButton.disabled = false;
                    controlButton.style.opacity = "1";
                },
                {
                    enableHighAccuracy: true,
                    timeout: 10000,
                    maximumAge: 0
                }
            );
        });

        return controlButton;
    }

    function drawMarkers(instance, centros) {
        // Limpiar markers anteriores
        Object.values(instance.markers).forEach((item) => {
            item.marker.setMap(null);
            if (item.infoWindow) {
                item.infoWindow.close();
            }
        });
        instance.markers = {};

        // Limpiar clusterer anterior
        if (instance.clusterer) {
            instance.clusterer.clearMarkers();
            instance.clusterer = null;
        }

        const allGoogleMarkers = [];

        centros.forEach((centro) => {
            const marker = new google.maps.Marker({
                position: { lat: centro.latitud, lng: centro.longitud },
                title: centro.nombre,
                icon: toMarkerIcon(false)
            });

            const infoWindow = new google.maps.InfoWindow({
                content: `
                    <div style="font-family: Arial, sans-serif; color: #00374c; padding: 6px; max-width: 280px;">
                        <strong style="font-size: 14px;">${centro.nombre}</strong>
                        <p style="margin: 6px 0; font-size: 12px;">${centro.direccion}</p>
                        <p style="margin: 3px 0; font-size: 11px;"><strong>Comunidad:</strong> ${centro.comunidad}</p>
                        <p style="margin: 3px 0; font-size: 11px;"><strong>Complejidad:</strong> ${centro.complejidad}</p>
                        <p style="margin: 3px 0; font-size: 11px;"><strong>Telefono:</strong> ${centro.telefono}</p>
                        <p style="margin: 3px 0; font-size: 11px;"><strong>Origen:</strong> ${centro.origen}</p>
                    </div>
                `
            });

            marker.addListener("click", () => {
                closeAllInfoWindows(instance);
                infoWindow.open(instance.map, marker);
            });

            instance.markers[centro.id] = { marker, infoWindow };
            allGoogleMarkers.push(marker);
        });

        // Crear clusterer con los markers (NO se agregan al mapa directamente)
        if (window.markerClusterer && allGoogleMarkers.length > 0) {
            instance.clusterer = new markerClusterer.MarkerClusterer({
                map: instance.map,
                markers: allGoogleMarkers,
                renderer: createClusterRenderer()
            });
        } else {
            // Fallback sin clustering si la lib no cargó
            allGoogleMarkers.forEach((m) => m.setMap(instance.map));
        }

        if (centros.length > 0) {
            const bounds = new google.maps.LatLngBounds();
            centros.forEach((centro) => bounds.extend({ lat: centro.latitud, lng: centro.longitud }));
            instance.map.fitBounds(bounds);
        }
    }

    window.centrosSaludMap = {
        async init(elementId, apiKey, centerLat, centerLng, centros) {
            await loadGoogleMaps(apiKey);
            await loadMarkerClusterer();

            const element = document.getElementById(elementId);
            if (!element) {
                throw new Error(`No se encontro el contenedor del mapa: ${elementId}`);
            }

            const map = new google.maps.Map(element, {
                zoom: 12,
                center: { lat: Number(centerLat), lng: Number(centerLng) },
                mapTypeControl: true,
                fullscreenControl: true,
                streetViewControl: false
            });

            instances[elementId] = {
                map,
                markers: {},
                clusterer: null,
                userMarker: null
            };

            const userLocationControl = createUserLocationControl(instances[elementId]);
            map.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(userLocationControl);

            const normalized = normalizeCentros(centros);
            drawMarkers(instances[elementId], normalized);
        },

        setMarkers(elementId, centros) {
            const instance = instances[elementId];
            if (!instance) {
                return;
            }

            const normalized = normalizeCentros(centros);
            drawMarkers(instance, normalized);
        },

        focus(elementId, centroId) {
            const instance = instances[elementId];
            if (!instance || !instance.markers[centroId]) {
                return;
            }

            const markerData = instance.markers[centroId];

            Object.entries(instance.markers).forEach(([id, item]) => {
                item.marker.setIcon(toMarkerIcon(id === centroId));
            });

            closeAllInfoWindows(instance);
            markerData.infoWindow.open(instance.map, markerData.marker);
            instance.map.panTo(markerData.marker.getPosition());
            instance.map.setZoom(18);
        }
    };
})();
