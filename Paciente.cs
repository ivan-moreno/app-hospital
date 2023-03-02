namespace AppHospital;

internal class Paciente : Persona
{
    public int DiasIngresado { get; set; } = 1;

    public Paciente(
        string nombre,
        string dni,
        int edad)
        : base(nombre, dni, edad) { }

    public override string ToString()
    {
        return "(Paciente) " + base.ToString();
    }
}
