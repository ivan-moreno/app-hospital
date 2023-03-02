namespace AppHospital;

internal sealed class Program
{
    private const string
        mensajeDatosMedico = "Por favor, rellene los siguientes datos del médico.",
        mensajeMedicoRegistrado = "El Dr./Dra. {0} ha sido registrado.",
        mensajeDatosPaciente = "Por favor, rellene los siguientes datos del paciente.",
        mensajePacienteRegistrado = "El paciente {0} ha sido registrado. Asígnele un médico.",
        mensajePacienteAsignado = "El paciente {0} ha sido asignado correctamente.",
        mensajePacienteEliminado = "El paciente ha sido eliminado correctamente.",
        mensajeNoHayMedicos = "No hay ningún médico en el hospital. Añade uno primero.",
        mensajeNoHayPacientes = "No hay ningún paciente en el hospital. Añade uno primero.";

    private static readonly Hospital hospital = new("Fundació S-plai");
    private static bool enEjecucion = true;

    private static void Main()
    {
        AbrirMenuHospital();
    }

    private static void AbrirMenuHospital()
    {


        while (enEjecucion)
        {
            EscribirLinea("\n1. Añadir médico" +
                "\n2. Añadir paciente" +
                "\n3. Lista de médicos" +
                "\n4. Lista de pacientes de un médico" +
                "\n5. Lista de personas en el hospital" +
                "\n6. Eliminar paciente" +
                "\n7. Cerrar aplicación", true, ConsoleColor.Cyan);

            char letra = PedirCaracter("Introduce un número: ", '1', '2', '3', '4', '5', '6', '7');
            Console.WriteLine();

            switch (letra)
            {
                case '1':
                    AnadirMedico();
                    break;

                case '2':
                    AnadirPaciente();
                    break;

                case '3':
                    ListarMedicos();
                    break;

                case '4':
                    ListarPacientesDeMedico();
                    break;

                case '5':
                    ListarPersonas();
                    break;

                case '6':
                    EliminarPaciente();
                    break;

                case '7':
                    CerrarAplicacion();
                    break;
            }
        }
    }

    private static void AnadirMedico()
    {
        EscribirLinea(mensajeDatosMedico, true, ConsoleColor.Yellow);

        Medico medico = new(
            nombre: PedirTexto(2, 60, "Nombre: "),
            dni: PedirTexto(9, 9, "DNI: "),
            edad: PedirNumero(18, 120, "Edad: "),
            salarioMensual: PedirNumero(0, 9999, "Salario mensual: "));

        hospital.AnadirMedico(medico);

        EscribirLinea(string.Format(mensajeMedicoRegistrado, medico.Nombre), true, ConsoleColor.Green);
    }

    private static void AnadirPaciente()
    {
        if (!HayMedicos())
            return;

        EscribirLinea(mensajeDatosPaciente, true, ConsoleColor.Yellow);

        Paciente paciente = new(
            nombre: PedirTexto(2, 60, "Nombre: "),
            dni: PedirTexto(9, 9, "DNI: "),
            edad: PedirNumero(18, 120, "Edad: "));

        EscribirLinea(string.Format(mensajePacienteRegistrado, paciente.Nombre), true, ConsoleColor.Yellow);

        int indiceMedico = PedirIndiceMedico();
        hospital.AsignarPaciente(paciente, indiceMedico);

        EscribirLinea(string.Format(mensajePacienteAsignado, paciente.Nombre), true, ConsoleColor.Green);
    }

    private static void ListarMedicos()
    {
        if (!HayMedicos())
            return;

        EscribirLinea(hospital.ListarMedicos());
    }

    private static void ListarPacientesDeMedico()
    {
        if (!HayMedicos())
            return;

        int indiceMedico = PedirIndiceMedico();
        Medico medico = hospital.MedicoPorIndice(indiceMedico);
        Console.WriteLine(medico);
    }

    private static void ListarPersonas()
    {
        Console.WriteLine(hospital);
    }

    private static void EliminarPaciente()
    {
        if (!HayPacientes())
            return;

        int indicePaciente = PedirIndicePaciente();
        hospital.EliminarPaciente(indicePaciente);
        EscribirLinea(mensajePacienteEliminado, true, ConsoleColor.Green);
    }

    private static void CerrarAplicacion()
    {
        enEjecucion = false;
    }

    private static int PedirIndiceMedico()
    {
        return PedirIndice(hospital.ListarMedicos(), hospital.NumeroDeMedicos);
    }

    private static int PedirIndicePaciente()
    {
        return PedirIndice(hospital.ListarPacientes(), hospital.NumeroDePacientes);
    }

    private static int PedirIndice(string lista, int numeroMax)
    {
        EscribirLinea(lista);

        return PedirNumero(1, numeroMax) - 1;
    }

    /// <summary>
    /// Comprueba si hay al menos 1 médico en el hospital.
    /// De lo contrario, muestra un mensaje sobre ello.
    /// </summary>
    private static bool HayMedicos()
    {
        return HayPersonas(hospital.NumeroDeMedicos, mensajeNoHayMedicos);
    }

    /// <summary>
    /// Comprueba si hay al menos 1 paciente en el hospital.
    /// De lo contrario, muestra un mensaje sobre ello.
    /// </summary>
    private static bool HayPacientes()
    {
        return HayPersonas(hospital.NumeroDePacientes, mensajeNoHayPacientes);
    }

    /// <summary>
    /// Comprueba si <paramref name="cantidad"/> es mayor a 0.
    /// De lo contrario, muestra un mensaje sobre ello.
    /// </summary>
    /// <param name="cantidad">La cantidad de personas a comprobar.</param>
    /// <param name="mensajeNoHay">El mensaje que se mostrará si <paramref name="cantidad"/> es 0.</param>
    /// <returns><see langword="true"/> si <paramref name="cantidad"/> es mayor a 0.</returns>
    private static bool HayPersonas(int cantidad, string mensajeNoHay)
    {
        if (cantidad == 0)
        {
            EscribirLinea(mensajeNoHay, true, ConsoleColor.Red);
            return false;
        }

        return true;
    }

    private static char PedirCaracter(
        string mensaje = "Introduce una letra: ",
        params char[] caracteresValidos)
    {
        char caracter;

        Escribir(mensaje, false, ConsoleColor.White);

        do
            caracter = char.ToLower(Console.ReadKey().KeyChar);
        while (!caracteresValidos.Contains(caracter));

        return caracter;
    }

    private static string PedirTexto(
        int minLetras = 0,
        int maxLetras = int.MaxValue,
        string mensaje = "Escribe: ")
    {
        string texto;

        Escribir(mensaje, false, ConsoleColor.White);

        do
            texto = Console.ReadLine() ?? string.Empty;
        while (texto.Length < minLetras || texto.Length > maxLetras);

        return texto;
    }

    private static int PedirNumero(
        int min = int.MinValue,
        int max = int.MaxValue,
        string mensaje = "Introduce un número: ")
    {
        int numero;

        Escribir(mensaje, false, ConsoleColor.White);

        do
        {
            string texto = Console.ReadLine() ?? string.Empty;

            if (int.TryParse(texto, out numero)
                && numero >= min
                && numero <= max)
                break;
        }
        while (true);

        return numero;
    }

    private static void Escribir(
        string mensaje,
        bool esperarTecla = false,
        ConsoleColor color = ConsoleColor.Gray)
    {
        Console.ForegroundColor = color;
        Console.Write(mensaje);
        Console.ResetColor();

        if (esperarTecla)
            Console.ReadKey(true);
    }

    private static void EscribirLinea(
        string mensaje,
        bool esperarTecla = false,
        ConsoleColor color = ConsoleColor.Gray)
    {
        Escribir(mensaje + "\n", esperarTecla, color);
    }
}