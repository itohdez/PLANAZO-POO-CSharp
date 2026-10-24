using System;
using System.Collections.Generic;
using Planazo.Modelos;
using Planazo.Servicios;

namespace Planazo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var gestor = new GestorReservas();
            bool salir = false;

            while (!salir)
            {
                MostrarMenu();
                string? entrada = Console.ReadLine();
                Console.WriteLine();

                try
                {
                    switch (entrada?.Trim())
                    {
                        case "1":
                            VerEventosDisponibles(gestor);
                            break;
                        case "2":
                            ConsultarEvento(gestor);
                            break;
                        case "3":
                            ReservarEntradas(gestor);
                            break;
                        case "4":
                            VerReservasRealizadas(gestor);
                            break;
                        case "5":
                            CancelarReserva(gestor);
                            break;
                        case "6":
                            VerResumenReservas(gestor);
                            break;
                        case "7":
                            ConsultarReservasPorUsuario(gestor);
                            break;
                        case "0":
                            salir = true;
                            EscribirExito("¡Gracias por usar PLANAZO! Hasta pronto.");
                            break;
                        default:
                            EscribirError("[ADVERTENCIA] Opción no válida. Por favor seleccione una opción entre 0 y 7.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    EscribirError($"[ERROR] {ex.Message}");
                }

                if (!salir)
                {
                    Console.WriteLine();
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            }
        }

        private static void MostrarMenu()
        {
            EscribirTitulo("================================");
            EscribirTitulo("        PLANAZO - EVENTOS");
            EscribirTitulo("================================");
            Console.WriteLine();
            Console.WriteLine("1. Ver eventos disponibles");
            Console.WriteLine("2. Consultar evento");
            Console.WriteLine("3. Reservar entradas");
            Console.WriteLine("4. Ver reservas realizadas");
            Console.WriteLine("5. Cancelar una reserva");
            Console.WriteLine("6. Ver resumen de reservas");
            Console.WriteLine("7. Consultar reservas por usuario (Reto 2)");
            Console.WriteLine("0. Salir");
            Console.WriteLine();
            Console.Write("Seleccione una opción: ");
        }

        private static void VerEventosDisponibles(GestorReservas gestor)
        {
            EscribirTitulo("--- LISTADO DE EVENTOS DISPONIBLES ---");
            foreach (var evento in gestor.ObtenerEventos())
            {
                Console.WriteLine(evento.ObtenerDetalle());
            }
        }

        private static void ConsultarEvento(GestorReservas gestor)
        {
            EscribirTitulo("--- CONSULTAR EVENTO POR ID ---");
            Console.Write("Ingrese el ID del evento: ");
            if (!int.TryParse(Console.ReadLine(), out int idEvento))
                throw new FormatException("El ID del evento debe ser un número entero válido.");

            Evento evento = gestor.ConsultarEventoPorId(idEvento);
            Console.WriteLine();
            Console.WriteLine("Información detallada del evento:");
            Console.WriteLine(evento.ObtenerDetalle());
        }

        private static void ReservarEntradas(GestorReservas gestor)
        {
            EscribirTitulo("--- RESERVAR ENTRADAS ---");
            Console.Write("Ingrese el documento del cliente: ");
            string documento = Console.ReadLine() ?? string.Empty;

            Console.Write("Ingrese el nombre completo del cliente: ");
            string nombre = Console.ReadLine() ?? string.Empty;

            Console.Write("Ingrese el ID del evento que desea reservar: ");
            if (!int.TryParse(Console.ReadLine(), out int idEvento))
                throw new FormatException("El ID del evento debe ser un número entero válido.");

            Console.Write("Ingrese la cantidad de entradas: ");
            if (!int.TryParse(Console.ReadLine(), out int cantidad))
                throw new FormatException("La cantidad de entradas debe ser un número entero válido.");

            Reserva reserva = gestor.CrearReserva(documento, nombre, idEvento, cantidad);
            Console.WriteLine();
            EscribirExito("[ÉXITO] ¡Reserva registrada exitosamente!");
            Console.WriteLine(reserva);
        }

        private static void VerReservasRealizadas(GestorReservas gestor)
        {
            EscribirTitulo("--- HISTORIAL DE RESERVAS REALIZADAS ---");
            var reservas = gestor.ObtenerReservas();
            if (reservas.Count == 0)
            {
                Console.WriteLine("Aún no se han registrado reservas en el sistema.");
                return;
            }

            foreach (var reserva in reservas)
            {
                Console.WriteLine(reserva);
            }
        }

        private static void CancelarReserva(GestorReservas gestor)
        {
            EscribirTitulo("--- CANCELAR UNA RESERVA ---");
            Console.Write("Ingrese el ID de la reserva a cancelar: ");
            if (!int.TryParse(Console.ReadLine(), out int idReserva))
                throw new FormatException("El ID de la reserva debe ser un número entero válido.");

            Reserva reservaCancelada = gestor.CancelarReserva(idReserva);
            Console.WriteLine();
            EscribirExito("[ÉXITO] Reserva cancelada correctamente. Los cupos han sido devueltos al evento.");
            Console.WriteLine(reservaCancelada);
        }

        private static void VerResumenReservas(GestorReservas gestor)
        {
            EscribirTitulo("--- RESUMEN GENERAL DE RESERVAS ACTIVAS ---");
            var resumen = gestor.ObtenerResumenReservas();
            Console.WriteLine($"Total de reservas activas   : {resumen.ReservasActivas}");
            Console.WriteLine($"Total de entradas reservadas: {resumen.EntradasReservadas}");
            Console.WriteLine($"Valor total recaudado       : ${resumen.ValorTotalActivo:N0}");
        }

        private static void ConsultarReservasPorUsuario(GestorReservas gestor)
        {
            EscribirTitulo("--- CONSULTAR RESERVAS POR USUARIO ---");
            Console.Write("Ingrese el documento o nombre del usuario: ");
            string criterio = Console.ReadLine() ?? string.Empty;

            List<Reserva> reservasUsuario = gestor.ConsultarReservasPorUsuario(criterio);
            if (reservasUsuario.Count == 0)
            {
                Console.WriteLine($"No se encontraron reservas asociadas al usuario '{criterio}'.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"Reservas encontradas ({reservasUsuario.Count}):");
            foreach (var reserva in reservasUsuario)
            {
                Console.WriteLine(reserva);
            }
        }

        private static void EscribirTitulo(string texto)
        {
            var colorAnterior = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(texto);
            Console.ForegroundColor = colorAnterior;
        }

        private static void EscribirExito(string texto)
        {
            var colorAnterior = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(texto);
            Console.ForegroundColor = colorAnterior;
        }

        private static void EscribirError(string texto)
        {
            var colorAnterior = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(texto);
            Console.ForegroundColor = colorAnterior;
        }
    }
}