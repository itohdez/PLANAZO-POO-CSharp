using System;
using System.Collections.Generic;
using System.Linq;
using Planazo.Modelos;

namespace Planazo.Servicios
{
    /// <summary>
    /// Servicio encargado de administrar la colección de eventos, usuarios y reservas de PLANAZO.
    /// </summary>
    public class GestorReservas
    {
        private readonly List<Evento> _eventos;
        private readonly List<Reserva> _reservas;
        private readonly List<Usuario> _usuarios;
        private int _siguienteIdReserva;

        public GestorReservas()
        {
            _eventos = new List<Evento>();
            _reservas = new List<Reserva>();
            _usuarios = new List<Usuario>();
            _siguienteIdReserva = 1;
            CargarEventosIniciales();
        }

        /// <summary>
        /// Inicializa el sistema con al menos 6 eventos registrados (2 películas, 2 conciertos, 2 talleres)
        /// más 1 festival del Reto Adicional 1.
        /// </summary>
        private void CargarEventosIniciales()
        {
            // 2 Funciones de Cine (mantienen precio base)
            _eventos.Add(new Cine(1, "Dune: Parte Dos - Función IMAX", new DateTime(2026, 11, 10, 19, 30, 0), 25000m, 40, "Sala IMAX 1", "+12 años"));
            _eventos.Add(new Cine(2, "Spider-Man: Beyond the Spider-Verse", new DateTime(2026, 11, 12, 16, 0, 0), 18000m, 50, "Sala MacroXE", "Todo Público"));

            // 2 Conciertos (+15% sobre el precio base; ej. $50.000 -> $57.500)
            _eventos.Add(new Concierto(3, "Noche de Rock Sinfónico", new DateTime(2026, 11, 15, 20, 0, 0), 50000m, 100, "Banda Filarmónica & Kraken Tribute"));
            _eventos.Add(new Concierto(4, "Tour Tropical Caribeño", new DateTime(2026, 11, 20, 21, 0, 0), 80000m, 150, "Carlos Vives & Invitados"));

            // 2 Talleres (-10% de descuento sobre el precio base)
            _eventos.Add(new Taller(5, "Taller de Arquitectura Limpia en C# y .NET", new DateTime(2026, 11, 18, 9, 0, 0), 40000m, 25, "Ing. Valentina Morales"));
            _eventos.Add(new Taller(6, "Taller Práctico de Fotografía Urbana", new DateTime(2026, 11, 22, 14, 0, 0), 30000m, 20, "Mtro. Sebastián Gómez"));

            // Reto Adicional 1 (+0,2): Nuevo tipo de evento (Festival: +20% + $5.000 manilla Cashless)
            _eventos.Add(new Festival(7, "Festival Estéreo Planazo 2026", new DateTime(2026, 12, 5, 13, 0, 0), 100000m, 200, 3));
        }

        public IReadOnlyList<Evento> ObtenerEventos() => _eventos.AsReadOnly();

        public IReadOnlyList<Reserva> ObtenerReservas() => _reservas.AsReadOnly();

        public IReadOnlyList<Usuario> ObtenerUsuarios() => _usuarios.AsReadOnly();

        public Evento ConsultarEventoPorId(int idEvento)
        {
            var evento = _eventos.FirstOrDefault(e => e.Id == idEvento);
            if (evento == null)
                throw new KeyNotFoundException($"No existe ningún evento registrado con el identificador #{idEvento}.");
            return evento;
        }

        public Usuario ObtenerORegistrarUsuario(string documento, string nombre, string correo = "sin-correo@planazo.co")
        {
            if (string.IsNullOrWhiteSpace(documento))
                throw new ArgumentException("El documento del usuario es obligatorio.");

            var docLimpio = documento.Trim();
            var existente = _usuarios.FirstOrDefault(u => u.Documento.Equals(docLimpio, StringComparison.OrdinalIgnoreCase));
            if (existente != null)
                return existente;

            var nuevoUsuario = new Usuario(docLimpio, nombre, correo);
            _usuarios.Add(nuevoUsuario);
            return nuevoUsuario;
        }

        public Reserva CrearReserva(string documentoUsuario, string nombrePersona, int idEvento, int cantidadEntradas, string correo = "sin-correo@planazo.co")
        {
            var evento = ConsultarEventoPorId(idEvento);
            var usuario = ObtenerORegistrarUsuario(documentoUsuario, nombrePersona, correo);
            var nuevaReserva = new Reserva(_siguienteIdReserva, usuario, evento, cantidadEntradas);

            _reservas.Add(nuevaReserva);
            _siguienteIdReserva++;
            return nuevaReserva;
        }

        public Reserva CancelarReserva(int idReserva)
        {
            var reserva = _reservas.FirstOrDefault(r => r.IdReserva == idReserva);
            if (reserva == null)
                throw new KeyNotFoundException($"No se encontró ninguna reserva con el identificador #{idReserva}.");

            reserva.Cancelar();
            return reserva;
        }

        /// <summary>
        /// Reto Adicional 2: Consultar todas las reservas asociadas a un usuario (por documento o nombre).
        /// </summary>
        public List<Reserva> ConsultarReservasPorUsuario(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
                throw new ArgumentException("Debe ingresar un documento o nombre válido para buscar.");

            string filtro = criterio.Trim();
            return _reservas
                .Where(r => r.Usuario.Documento.Equals(filtro, StringComparison.OrdinalIgnoreCase) ||
                            r.NombrePersona.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public (int ReservasActivas, int EntradasReservadas, decimal ValorTotalActivo) ObtenerResumenReservas()
        {
            var activas = _reservas.Where(r => r.Estado == EstadoReserva.Activa).ToList();
            int cantidadReservasActivas = activas.Count;
            int cantidadEntradasReservadas = activas.Sum(r => r.CantidadEntradas);
            decimal valorTotal = activas.Sum(r => r.ValorTotal);

            return (cantidadReservasActivas, cantidadEntradasReservadas, valorTotal);
        }
    }
}
