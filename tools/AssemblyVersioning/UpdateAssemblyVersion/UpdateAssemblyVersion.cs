using System;
using System.IO;

namespace UpdateAssemblyVersion
{
    class UpdateAssemblyVersion
    {
        static void Main(string[] args)
        {
            try
            {
                bool bMajor = false;
                int iMajor = 0;
                bool bMinor = false;
                int iMinor = 0;
                bool bBuild = false;
                int iBuild = 0;
                bool bRevision = false;
                int iRevision = 0;

                String sFileName = null;

                for (int x = 0; x < args.Length; x++)
                {
                    if (args[x].Contains("-m"))
                        bMinor = true;
                    else if (args[x].Contains("-M"))
                        bMajor = true;
                    else if (args[x].Contains("-b"))
                        bBuild = true;
                    else if (args[x].Contains("-r"))
                        bRevision = true;
                    else if (args[x].Contains("-f"))
                    {
                        sFileName = args[x + 1]; //get file location                    
                        x++; //skip file location
                    }
                    else
                        Console.WriteLine("incorrect options entered");
                }
                string sFileContents = string.Empty;
                using (var sr = new StreamReader(sFileName.Replace("\"", "/")))
                {
                    sFileContents = sr.ReadToEnd();                    
                }
                String sOriginal = sFileContents.Remove(0, sFileContents.IndexOf("GlobalFileVersion", 0));
                sOriginal = sOriginal.Remove(0, sOriginal.IndexOf("\"") + 1);
                sOriginal = sOriginal.Remove(sOriginal.IndexOf("\""), sOriginal.Length - sOriginal.IndexOf("\""));

                Version vTemp = new Version(sOriginal);

                if (bMajor)
                    iMajor = vTemp.Major + 1;
                else
                    iMajor = vTemp.Major;
                if (bMinor)
                    iMinor = vTemp.Minor + 1;
                else
                    iMinor = vTemp.Minor;
                if (bBuild)
                    iBuild = GetBuild();
                else
                    iBuild = vTemp.Build;
                if (bRevision)
                    iRevision = GetRevision();
                else
                    iRevision = vTemp.Revision;

                Version vVersion = new Version(iMajor, iMinor, iBuild, iRevision);
                sFileContents = sFileContents.Replace("GlobalFileVersion = \"" + vTemp.ToString() + "\"", "GlobalFileVersion = \"" + vVersion.ToString() + "\"");
                                
                using (var fs = new FileStream(sFileName, FileMode.Truncate))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.Write(sFileContents);
                    }
                }
                Console.WriteLine("******************************************************************");
                Console.WriteLine(string.Format("Updated assembly file version to {0}", vVersion.ToString()));
                Console.WriteLine("******************************************************************");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
        
        static int GetBuild()
        {
            //var now = DateTime.Now;           
            //string buildStr = now.ToString("yyMdd");
            //return int.Parse(buildStr, System.Globalization.NumberStyles.None);
            var start = new DateTime(2000, 1, 1);
            var now = DateTime.Now;           
            var offset = now.Subtract(start).TotalDays;
            return Convert.ToInt32(offset);
        }
        static int GetRevision()
        {
            //var now = DateTime.Now;
            //string revStr = now.ToString("Hmm");
            //return int.Parse(revStr, System.Globalization.NumberStyles.None);
            var now = DateTime.Now;           
            var start = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);           
            var offset = now.Subtract(start).TotalSeconds / 2;          
            return Convert.ToInt32(offset);
        }
    }
}