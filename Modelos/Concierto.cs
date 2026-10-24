using System;

namespace Planazo.Modelos
{
    /// <summary>
    /// Evento de tipo Concierto: aumenta el precio base en un 15% por cargos de producción y servicio.
    /// </summary>
    public class Concierto : Evento
    {
        private const decimal PorcentajeRecargo = 0.15m;

        public string Artista { get; private set; }

        public override string TipoEvento => "Concierto (+15%)";

        public Concierto(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima, string artista = "Artista Invitado")
            : base(id, nombre, fecha, precioBase, capacidadMaxima)
        {
            Artista = string.IsNullOrWhiteSpace(artista) ? "Artista Invitado" : artista.Trim();
        }

        public override decimal CalcularPrecioFinal()
        {
            return PrecioBase * (1 + PorcentajeRecargo);
        }

        public override string ObtenerDetalle()
        {
            return $"{base.ObtenerDetalle()} | Artista: {Artista}";
        }
    }
}
