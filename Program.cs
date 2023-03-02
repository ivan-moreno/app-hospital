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

    /// <summary>
    /// Informa al usuario de qué puede realizar y qué tecla debe pulsar para realizarlo.
    /// </summary>
    private static void AbrirMenuHospital()
    {
        EscribirLinea($"¡Bienvenido al hospital '{hospital.Nombre}'!", color: ConsoleColor.Yellow);

        while (enEjecucion)
        {
            EscribirLinea("\n1. Añadir médico" +
                "\n2. Añadir paciente" +
                "\n3. Lista de médicos" +
                "\n4. Lista de pacientes de un médico" +
                "\n5. Lista de personas en el hospital" +
                "\n6. Eliminar paciente" +
                "\n7. Cerrar aplicación", color: ConsoleColor.Cyan);

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

        EscribirLinea(
            mensaje: string.Format(mensajeMedicoRegistrado, medico.Nombre),
            esperarTecla: true,
            color: ConsoleColor.Green);
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

        EscribirLinea(
            mensaje: string.Format(mensajePacienteRegistrado, paciente.Nombre),
            esperarTecla: true,
            color: ConsoleColor.Yellow);

        int indiceMedico = PedirIndiceMedico();
        hospital.AsignarPaciente(paciente, indiceMedico);

        EscribirLinea(
            mensaje: string.Format(mensajePacienteAsignado, paciente.Nombre),
            esperarTecla: true,
            color: ConsoleColor.Green);
    }

    private static void ListarMedicos()
    {
        if (!HayMedicos())
            return;

        EscribirLinea(hospital.ListarMedicos(), esperarTecla: true);
    }

    private static void ListarPacientesDeMedico()
    {
        if (!HayMedicos())
            return;

        int indiceMedico = PedirIndiceMedico();
        Medico medico = hospital.MedicoPorIndice(indiceMedico);
        EscribirLinea(medico.ToString(), esperarTecla: true);
    }

    private static void ListarPersonas()
    {
        EscribirLinea(hospital.ToString(), esperarTecla: true);
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

    /// <summary>
    /// Muestra una lista de los médicos y pide al usuario que elija uno de ellos.
    /// </summary>
    /// <returns>
    /// El índice del médico en la lista del hospital. (<see cref="Hospital.medicos"/>)
    /// </returns>
    private static int PedirIndiceMedico()
    {
        return PedirIndice(hospital.ListarMedicos(), hospital.NumeroDeMedicos);
    }

    /// <summary>
    /// Muestra una lista de los pacientes y pide al usuario que elija uno de ellos.
    /// </summary>
    /// <returns>
    /// El índice del paciente en la lista del hospital. (<see cref="Hospital.pacientes"/>)
    /// </returns>
    private static int PedirIndicePaciente()
    {
        return PedirIndice(hospital.ListarPacientes(), hospital.NumeroDePacientes);
    }

    /// <summary>
    /// Muestra una <paramref name="lista"/> de opciones y devuelve
    /// el índice que el usuario haya presionado.
    /// </summary>
    /// <param name="lista">El texto que representa la lista de opciones.</param>
    /// <param name="numeroMax">El número de elementos en la lista.</param>
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

    /// <summary>
    /// Informa al usuario de qué teclas puede presionar y la devuelve si válida.
    /// </summary>
    /// <param name="mensaje">El mensaje a mostrar, usado para informar al usuario sobre qué debe introducir.</param>
    /// <param name="caracteresValidos">Una secuencia de los carácteres que el usuario puede pulsar.</param>
    /// <returns>El carácter asociado a la tecla que el usuario ha pulsado.</returns>
    private static char PedirCaracter(
        string mensaje = "Introduce una letra: ",
        params char[] caracteresValidos)
    {
        char caracter;

        EscribirLinea(mensaje, false, ConsoleColor.White);

        do
            caracter = char.ToLower(Console.ReadKey().KeyChar);
        while (!caracteresValidos.Contains(caracter));

        return caracter;
    }

    /// <summary>
    /// Informa al usuario de que debe escribir un texto y lo devuelve si tiene una longitud válida.
    /// </summary>
    /// <param name="minLetras">La longitud mínima del texto a escribir, inclusivo.</param>
    /// <param name="maxLetras">La longitud máxima del texto a escribir, inclusivo.</param>
    /// <param name="mensaje">El mensaje a mostrar, usado para informar al usuario sobre qué debe introducir.</param>
    /// <returns>El texto que el usuario ha escrito.</returns>
    private static string PedirTexto(
        int minLetras = 0,
        int maxLetras = int.MaxValue,
        string mensaje = "Escribe: ")
    {
        string texto;

        EscribirLinea(mensaje, false, ConsoleColor.White);

        do
            texto = Console.ReadLine() ?? string.Empty;
        while (texto.Length < minLetras || texto.Length > maxLetras);

        return texto;
    }

    /// <summary>
    /// Informa al usuario de que debe escribir un número y lo devuelve si está dentro del rango.
    /// </summary>
    /// <param name="min">El número mínimo a introducir, inclusivo.</param>
    /// <param name="max">El número máximo a introducir, inclusivo.</param>
    /// <param name="mensaje">El mensaje a mostrar, usado para informar al usuario sobre qué debe introducir.</param>
    /// <returns>El número que el usuario ha escrito.</returns>
    private static int PedirNumero(
        int min = int.MinValue,
        int max = int.MaxValue,
        string mensaje = "Introduce un número: ")
    {
        int numero;

        EscribirLinea(mensaje, false, ConsoleColor.White);

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

    /// <summary>
    /// Muestra un <paramref name="mensaje"/> en la consola, sin terminar en una nueva línea.
    /// </summary>
    /// <param name="mensaje">El contenido del mensaje.</param>
    /// <param name="esperarTecla">Determina si hace falta pulsar una tecla para continuar.</param>
    /// <param name="color">El color usado para el contenido del mensaje.</param>
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

    /// <summary>
    /// Muestra un <paramref name="mensaje"/> en la consola, y pasa a la siguiente línea.
    /// </summary>
    /// <param name="mensaje">El contenido del mensaje.</param>
    /// <param name="esperarTecla">Determina si hace falta pulsar una tecla para continuar.</param>
    /// <param name="color">El color usado para el contenido del mensaje.</param>
    private static void EscribirLinea(
        string mensaje,
        bool esperarTecla = false,
        ConsoleColor color = ConsoleColor.Gray)
    {
        Escribir(mensaje + "\n", esperarTecla, color);
    }
}