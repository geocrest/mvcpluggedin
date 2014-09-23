using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GetAssemblyVersion
{
    class Program
    {
        private static string FILEFLAG = "-f";
        private static string INFOFLAG = "-i";
        private static string REVFLAG = "-r";
        static int Main(string[] args)
        {

            try
            {
                // get required file path                
                if (args.Length == 0 || string.IsNullOrEmpty(args[0]))
                {
                    Console.WriteLine("Please specify a file path to an assembly as the first argument.");
                    Console.ReadLine();
                    return 1;
                }
                var assembly = args[0];
                
                if (!File.Exists(assembly))
                {
                    Console.WriteLine("File does not exist.");
                    Console.ReadLine();
                    return 1;
                }

                // get optional parameters
                VersionType type = VersionType.Assembly;
                bool includeRevision = false;
                if (args.Contains(FILEFLAG) && args.Contains(INFOFLAG))
                {
                    Console.WriteLine("Cannot specify both file and informational arguments.");
                    return 1;
                }
                if (args.Contains(FILEFLAG)) type = VersionType.File;
                if (args.Contains(INFOFLAG)) type = VersionType.Informational;
                if (args.Contains(REVFLAG)) includeRevision = true;
                switch (type)
                {
                    case VersionType.File:
                        Console.WriteLine(GetVersion(FileVersionInfo.GetVersionInfo(assembly).FileVersion, includeRevision));
                        return 0;
                    case VersionType.Informational:
                        Console.WriteLine(FileVersionInfo.GetVersionInfo(assembly).ProductVersion);
                        return 0;
                    default:
                        Console.WriteLine(GetVersion(AssemblyName.GetAssemblyName(assembly).Version.ToString(), includeRevision));
                        return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return 1;
            }
        }
        private static string GetVersion(string version, bool includeRevision)
        {
            Version vVersion = new Version(version);
            return vVersion.ToString(includeRevision ? 4 : 3);
        }
    }
    /// <summary>
    /// The version type to retrieve from the assembly
    /// </summary>
    enum VersionType
    {
        [Description("The .NET framework assembly version.")]
        Assembly = 0,
        [Description("The Windows file version.")]
        File = 1,
        [Description("The informational version that can contain modifiers such as -rc2")]
        Informational = 2
    }
}
