using System.Text;

namespace AppHospital;

internal class Hospital
{
    public string Nombre { get; }
    public int NumeroDeMedicos => medicos.Count;
    public int NumeroDePacientes => pacientes.Count;
    private readonly List<Medico> medicos = new();
    private readonly List<Paciente> pacientes = new();

    public Hospital(string nombre)
    {
        Nombre = nombre;
    }

    public void AnadirMedico(Medico medico)
    {
        medicos.Add(medico);
    }

    public void AsignarPaciente(Paciente paciente, Medico medico)
    {
        pacientes.Add(paciente);
        medico.Pacientes.Add(paciente);
    }

    public void AsignarPaciente(Paciente paciente, int indiceMedico)
    {
        AsignarPaciente(paciente, medicos[indiceMedico]);
    }

    public void EliminarPaciente(Paciente paciente)
    {
        medicos.Where(medico => medico.Pacientes.Contains(paciente)).First().Pacientes.Remove(paciente);
        pacientes.Remove(paciente);
    }

    public void EliminarPaciente(int indicePaciente)
    {
        EliminarPaciente(pacientes[indicePaciente]);
    }

    public Medico MedicoPorIndice(int indice)
    {
        return medicos[indice];
    }

    public string ListarMedicos()
    {
        StringBuilder stringBuilder = new();

        for (int i = 0; i < medicos.Count; i++)
            stringBuilder.Append($"\n ({i + 1}) {medicos[i].Nombre}");

        return stringBuilder.ToString();
    }

    public string ListarPacientes()
    {
        StringBuilder stringBuilder = new();

        for (int i = 0; i < pacientes.Count; i++)
            stringBuilder.Append($"\n ({i + 1}) {pacientes[i].Nombre}");

        return stringBuilder.ToString();
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append($"Hospital '{Nombre}'"
            + $"\n\nMédicos: ({medicos.Count})");

        foreach (Medico medico in medicos)
            stringBuilder.Append("\n· " + medico);

        stringBuilder.Append($"\n\nPacientes: ({pacientes.Count})");

        foreach (Paciente paciente in pacientes)
            stringBuilder.Append("\n· " + paciente);

        return stringBuilder.ToString();
    }
}
