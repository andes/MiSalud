using System;

namespace SaludPortal.Application.Models;

public sealed class TurnosPorTipoPrestacion
    {
        public string TipoPrestacionConceptId { get; set; } = string.Empty;
        public string? TipoPrestacionTerm { get; set; }
        public List<Turno> Turnos { get; set; } = new List<Turno>();
    }
