using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.PowerShell.Commands;
using Newtonsoft.Json;
using TheatreBlast.Responses;

namespace TheatreBlast
{
    class Program
    {
        static void Main(string[] args)
        {
            var cinemasRequestStr = ReadFile("TheatreBlast.Requests.WhatsOnV2GetCinemas.ps1");
            var cinemasPs = ParsePowershellCommand(cinemasRequestStr);
            var cinemasPsResponse = cinemasPs.Invoke();
            var cinemasJson = ((BasicHtmlWebResponseObject)cinemasPsResponse.First().BaseObject).Content;
            var cinemas = JsonConvert.DeserializeObject<WhatsOnV2GetCinemasResponse>(cinemasJson);

            //TODO blast!
        }

        private static string ReadFile(string path)
        {
            string file;
            var assembly = typeof(Program).GetTypeInfo().Assembly;

            using (var stream = assembly.GetManifestResourceStream(path))
            {
                using (var reader = new StreamReader(stream))
                {
                    file = reader.ReadToEnd();
                }
            }

            return file;
        }

        private static PowerShell ParsePowershellCommand(string requestString)
        {
            var parametersParts = CommandLineToArgs(requestString);

            var ps = PowerShell.Create();

            string lastParameterName = null;
            var hashTableParts = new List<string>();
            var hashTableCollectingInProgess = false;
            foreach (var parameterPart in parametersParts)
            {
                if (parametersParts.First() == parameterPart)
                {
                    ps.AddCommand(parameterPart);
                    continue;
                }

                if (hashTableCollectingInProgess)
                {
                    hashTableParts.Add(parameterPart);

                    if (parameterPart.EndsWith("}"))
                    {
                        var hashTable = GetHashTable(hashTableParts);
                        ps.AddParameter(lastParameterName, hashTable);

                        hashTableParts.Clear();
                        hashTableCollectingInProgess = false;
                    }

                    continue;
                }

                if (parameterPart.StartsWith("-"))
                {
                    lastParameterName = parameterPart.Replace("-", string.Empty);
                    continue;
                }

                if (parameterPart.StartsWith("@{"))
                {
                    hashTableParts.Add(parameterPart);
                    hashTableCollectingInProgess = true;
                    continue;
                }

                ps.AddParameter(lastParameterName, parameterPart);
            }

            return ps;
        }

        private static Hashtable GetHashTable(IList<string> hashTableParts)
        {
            hashTableParts[0] = hashTableParts[0].Replace("@{", string.Empty);
            for (var i = 0; i < hashTableParts.Count; i++)
            {
                hashTableParts[i] = hashTableParts[i].Remove(hashTableParts[i].Length - 1);
            }

            var hashTable = new Hashtable();
            var hashTableDictionary = hashTableParts
                .Select(part => part.Split(new[] {'='}))
                .Select(group => new
                {
                    Key = @group.First(),
                    Value = string.Join('=', @group.Skip(1))
                });
            hashTableDictionary.ToList().ForEach(kv => hashTable.Add(kv.Key, kv.Value));

            return hashTable;
        }

        [DllImport("shell32.dll", SetLastError = true)]
        static extern IntPtr CommandLineToArgvW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

        private static string[] CommandLineToArgs(string commandLine)
        {
            int argc;
            var argv = CommandLineToArgvW(commandLine, out argc);
            if (argv == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception();
            try
            {
                var args = new string[argc];
                for (var i = 0; i < args.Length; i++)
                {
                    var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p);
                }

                return args;
            }
            finally
            {
                Marshal.FreeHGlobal(argv);
            }
        }
    }
}
