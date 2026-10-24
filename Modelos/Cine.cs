using System;

namespace Planazo.Modelos
{
    /// <summary>
    /// Evento de tipo Cine: mantiene el precio base sin recargos ni descuentos.
    /// </summary>
    public class Cine : Evento
    {
        public string Sala { get; private set; }
        public string Clasificacion { get; private set; }

        public override string TipoEvento => "Cine";

        public Cine(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima, string sala = "Sala General", string clasificacion = "Todo Público")
            : base(id, nombre, fecha, precioBase, capacidadMaxima)
        {
            Sala = string.IsNullOrWhiteSpace(sala) ? "Sala General" : sala.Trim();
            Clasificacion = string.IsNullOrWhiteSpace(clasificacion) ? "Todo Público" : clasificacion.Trim();
        }

        public override decimal CalcularPrecioFinal()
        {
            return PrecioBase;
        }

        public override string ObtenerDetalle()
        {
            return $"{base.ObtenerDetalle()} | Sala: {Sala} ({Clasificacion})";
        }
    }
}
