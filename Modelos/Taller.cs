using System;

namespace Planazo.Modelos
{
    /// <summary>
    /// Evento de tipo Taller: aplica un descuento del 10% sobre el precio base para incentivar el aprendizaje.
    /// </summary>
    public class Taller : Evento
    {
        private const decimal PorcentajeDescuento = 0.10m;

        public string Instructor { get; private set; }

        public override string TipoEvento => "Taller (-10%)";

        public Taller(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima, string instructor = "Instructor Asignado")
            : base(id, nombre, fecha, precioBase, capacidadMaxima)
        {
            Instructor = string.IsNullOrWhiteSpace(instructor) ? "Instructor Asignado" : instructor.Trim();
        }

        public override decimal CalcularPrecioFinal()
        {
            return PrecioBase * (1 - PorcentajeDescuento);
        }

        public override string ObtenerDetalle()
        {
            return $"{base.ObtenerDetalle()} | Instructor: {Instructor}";
        }
    }
}
