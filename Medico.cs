using System.Text;

namespace AppHospital;

internal class Medico : Persona
{
    public float SalarioMensual { get; set; }
    public List<Paciente> Pacientes { get; } = new();

    public Medico(
        string nombre,
        string dni,
        int edad,
        float salarioMensual)
        : base(nombre, dni, edad)
    {
        SalarioMensual = salarioMensual;
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append("(Médico) "
            + base.ToString()
            + $", salario mensual de {SalarioMensual} euros"
            + $"\n  Tiene {Pacientes.Count} pacientes: ");

        foreach (Paciente paciente in Pacientes)
            stringBuilder.Append(paciente.Nombre + ", ");

        return stringBuilder.ToString();
    }
}
