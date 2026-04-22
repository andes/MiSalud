(function () {
    const instances = {};

    function toMarkerIcon() {
        return L.icon({
            iconUrl: "/images/icon-marker-hospital.png",
            iconSize: [42, 42],
            iconAnchor: [21, 42],
            popupAnchor: [0, -42]
        });
    }

    function toPatientIcon() {
        return L.icon({
            iconUrl: "/images/icon-marker-persona.png",
            iconSize: [42, 42],
            iconAnchor: [21, 42],
            popupAnchor: [0, -42]
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

    function createClusterIconFunction() {
        return function (cluster) {
            const count = cluster.getChildCount();
            const size = count < 10 ? 36 : count < 50 ? 44 : count < 200 ? 52 : 60;
            const svg = `<svg xmlns="http://www.w3.org/2000/svg" width="${size}" height="${size}" viewBox="0 0 ${size} ${size}">
                <circle cx="${size / 2}" cy="${size / 2}" r="${size / 2 - 2}" fill="#0daca4" fill-opacity="0.85" stroke="#e9414e" stroke-width="3"/>
                <text x="50%" y="52%" text-anchor="middle" dominant-baseline="middle" fill="#fff" font-family="Arial" font-size="${size < 44 ? 12 : 14}" font-weight="bold">${count}</text>
            </svg>`;
            return L.divIcon({
                html: svg,
                className: "",
                iconSize: [size, size],
                iconAnchor: [size / 2, size / 2]
            });
        };
    }

    function closeAllPopups(instance) {
        Object.values(instance.markers).forEach((item) => {
            item.marker.closePopup();
        });
        if (instance.patientMarker) {
            instance.patientMarker.closePopup();
        }
    }

    function showUserLocation(instance, position) {
        const latLng = [position.coords.latitude, position.coords.longitude];

        if (!instance.userMarker) {
            instance.userMarker = L.circleMarker(latLng, {
                radius: 9,
                fillColor: "#1a73e8",
                fillOpacity: 1,
                color: "#ffffff",
                weight: 3
            }).addTo(instance.map);
        } else {
            instance.userMarker.setLatLng(latLng);
        }

        instance.map.setView(latLng, 15);
    }

    function createUserLocationControl(instance) {
        const UserLocationControl = L.Control.extend({
            options: { position: "bottomright" },
            onAdd() {
                const btn = L.DomUtil.create("button");
                btn.type = "button";
                btn.title = "Ir a mi ubicacion";
                btn.setAttribute("aria-label", "Ir a mi ubicacion");
                btn.innerHTML = "<span style='font-size:18px;line-height:1;'>\u25CE</span>";
                btn.style.backgroundColor = "#fff";
                btn.style.border = "0";
                btn.style.borderRadius = "4px";
                btn.style.boxShadow = "0 2px 6px rgba(0,0,0,.3)";
                btn.style.cursor = "pointer";
                btn.style.margin = "10px";
                btn.style.padding = "0 12px";
                btn.style.height = "40px";
                btn.style.width = "40px";
                btn.style.display = "flex";
                btn.style.alignItems = "center";
                btn.style.justifyContent = "center";
                btn.style.color = "#00374c";

                L.DomEvent.disableClickPropagation(btn);
                L.DomEvent.on(btn, "click", () => {
                    if (!navigator.geolocation) {
                        window.alert("Tu navegador no soporta geolocalizacion.");
                        return;
                    }

                    btn.disabled = true;
                    btn.style.opacity = "0.7";

                    navigator.geolocation.getCurrentPosition(
                        (position) => {
                            showUserLocation(instance, position);
                            btn.disabled = false;
                            btn.style.opacity = "1";
                        },
                        () => {
                            window.alert("No se pudo obtener tu ubicacion. Verifica los permisos del navegador.");
                            btn.disabled = false;
                            btn.style.opacity = "1";
                        },
                        { enableHighAccuracy: true, timeout: 10000, maximumAge: 0 }
                    );
                });

                return btn;
            }
        });

        return new UserLocationControl();
    }

    function drawMarkers(instance, centros) {
        // Limpiar markers anteriores
        Object.values(instance.markers).forEach((item) => {
            item.marker.remove();
        });
        instance.markers = {};

        // Limpiar clusterer anterior
        if (instance.clusterer) {
            instance.clusterer.clearLayers();
            instance.map.removeLayer(instance.clusterer);
            instance.clusterer = null;
        }

        const clusterer = L.markerClusterGroup({
            iconCreateFunction: createClusterIconFunction(),
            showCoverageOnHover: false
        });

        centros.forEach((centro) => {
            const marker = L.marker([centro.latitud, centro.longitud], {
                title: centro.nombre,
                icon: toMarkerIcon()
            });

            const popupContent = `
                <div style="font-family: Arial, sans-serif; color: #00374c; padding: 6px; max-width: 280px;">
                    <strong style="font-size: 14px;">${centro.nombre}</strong>
                    <p style="margin: 6px 0; font-size: 12px;">${centro.direccion}</p>
                    <p style="margin: 3px 0; font-size: 11px;"><strong>Comunidad:</strong> ${centro.comunidad}</p>
                    <p style="margin: 3px 0; font-size: 11px;"><strong>Complejidad:</strong> ${centro.complejidad}</p>
                    <p style="margin: 3px 0; font-size: 11px;"><strong>Telefono:</strong> ${centro.telefono}</p>
                    <p style="margin: 3px 0; font-size: 11px;"><strong>Origen:</strong> ${centro.origen}</p>
                </div>
            `;

            marker.bindPopup(popupContent);
            instance.markers[centro.id] = { marker };
            clusterer.addLayer(marker);
        });

        instance.map.addLayer(clusterer);
        instance.clusterer = clusterer;

        if (centros.length > 0) {
            const bounds = L.latLngBounds(centros.map((c) => [c.latitud, c.longitud]));
            instance.map.fitBounds(bounds, { padding: [30, 30] });
        }
    }

    function showPatientAddressMarker(instance, lat, lng, label) {
        const latLng = [Number(lat), Number(lng)];
        const popupContent = `<div style="font-family: Arial, sans-serif; color: #00374c; padding: 6px; max-width: 260px;"><strong style="font-size: 13px;">Tu direcci\u00F3n registrada</strong><p style="margin: 6px 0; font-size: 12px;">${label || ""}</p></div>`;

        if (!instance.patientMarker) {
            instance.patientMarker = L.marker(latLng, {
                title: "Tu direcci\u00F3n registrada",
                icon: toPatientIcon(),
                zIndexOffset: 500
            })
                .bindPopup(popupContent)
                .addTo(instance.map);
        } else {
            instance.patientMarker.setLatLng(latLng);
            instance.patientMarker.setPopupContent(popupContent);
            if (!instance.map.hasLayer(instance.patientMarker)) {
                instance.patientMarker.addTo(instance.map);
            }
        }
    }

    window.centrosSaludMap = {
        init(elementId, centerLat, centerLng, centros) {
            const element = document.getElementById(elementId);
            if (!element) {
                throw new Error(`No se encontro el contenedor del mapa: ${elementId}`);
            }

            const map = L.map(elementId).setView([Number(centerLat), Number(centerLng)], 12);

            L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
                attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
                maxZoom: 19
            }).addTo(map);

            instances[elementId] = {
                map,
                markers: {},
                clusterer: null,
                userMarker: null,
                patientMarker: null
            };

            const userLocationControl = createUserLocationControl(instances[elementId]);
            userLocationControl.addTo(map);

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
            closeAllPopups(instance);

            if (instance.clusterer) {
                instance.clusterer.zoomToShowLayer(markerData.marker, () => {
                    markerData.marker.openPopup();
                });
            } else {
                instance.map.setView(markerData.marker.getLatLng(), 18);
                markerData.marker.openPopup();
            }
        },

        showPatientAddress(elementId, lat, lng, label) {
            const instance = instances[elementId];
            if (!instance) {
                return;
            }

            showPatientAddressMarker(instance, lat, lng, label);
        }
    };
})();
