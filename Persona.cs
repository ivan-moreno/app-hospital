namespace AppHospital;

internal class Persona
{
    public string Nombre { get; }
    public string Dni { get; }
    public int Edad { get; set; }

    public Persona(string nombre, string dni, int edad)
    {
        Nombre = nombre;
        Dni = dni;
        Edad = edad;
    }

    public override string ToString()
    {
        return $"{Nombre}, con DNI {Dni} y {Edad} años";
    }
}
