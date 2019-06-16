using System;
using VMS.TPS.Common.Model.API;
using YamlDotNet.Serialization;

namespace ExportPatientData
{
public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.Error.WriteLine("Missing parameters: patient-id");
            return;
        }

        using (var app = Application.CreateApplication(null, null))
        {
            var patientId = args[0];
            var patient = app.OpenPatientById(patientId);

            var serializer = new SerializerBuilder()
                .EmitDefaults()
                .WithAttributeOverride<Structure>(e => e.SegmentVolume, new YamlIgnoreAttribute())
                .WithAttributeOverride<Structure>(e => e.MeshGeometry, new YamlIgnoreAttribute())
                .WithAttributeOverride<Isodose>(e => e.MeshGeometry, new YamlIgnoreAttribute())
                .WithAttributeOverride<ControlPoint>(e => e.LeafPositions, new YamlIgnoreAttribute())
                .Build();

            var patientString = serializer.Serialize(patient);
            Console.WriteLine(patientString);
        }
    }
}
}
