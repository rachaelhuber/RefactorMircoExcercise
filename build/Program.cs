namespace build
{
    using System;
    using System.IO;
    using System.Linq;
    using static Bullseye.Targets;
    using static SimpleExec.Command;
    using static System.IO.Path;

    internal class Program
    {
        public const string ArtifactsDir = "artifacts";

        private const string Clean = "clean";
        private const string DotNetBuild = "dotnet-build";
        private const string DotNetTest = "dotnet-test";
        private const string DotNetPack = "dotnet-pack";

        static void Main(string[] args)
        {
            var artifactsPath = Combine(Directory.GetCurrentDirectory(), "artifacts");
            var nugetDirectory = Combine(artifactsPath, "nuget");
            
            Target(Clean, () =>
            {
                if (Directory.Exists(artifactsPath))
                {
                    var filesToDelete = Directory
                        .GetFiles(artifactsPath, "*.*", SearchOption.AllDirectories)
                        .Where(f => !f.EndsWith(".gitignore"));
                    foreach (var file in filesToDelete)
                    {
                        Console.WriteLine($"Deleting file {file}");
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.Delete(file);
                    }

                    var directoriesToDelete = Directory.GetDirectories(artifactsPath);
                    foreach (var directory in directoriesToDelete)
                    {
                        Console.WriteLine($"Deleting directory {directory}");
                        Directory.Delete(directory, true);
                    }
                }
            });

            Target(DotNetBuild, DependsOn(Clean), () =>
            {
                Run("dotnet", "build src/TDDMicroExercises.sln -c Release");
            });

            Target(DotNetTest, DependsOn(DotNetBuild), () =>
            {
                Run("dotnet",
                    "test tests/TirePressureMonitoringSystem.Tests/TirePressureMonitoringSystem.Tests.csproj -c Release " +
                    $"-r {artifactsPath} -l trx --verbosity=normal");
            });

            Target(DotNetPack, DependsOn(DotNetBuild), () =>
            {
                var projectsToPack = new[]
                {
                    "TirePressureMonitoringSystem",
                };

                // Ensure output directory exists
                Directory.CreateDirectory(nugetDirectory);

                foreach (var project in projectsToPack)
                {
                    Run("dotnet", $"pack src/{project}/{project}.csproj -c Release -o {nugetDirectory}");
                }
            });

            Target("default", DependsOn(DotNetBuild, DotNetTest, DotNetPack));

            RunTargetsAndExit(args);
        }
    }
}
