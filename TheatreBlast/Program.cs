using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.InteropServices;
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

            LoopOverCinemas(cinemas);
        }

        private static void LoopOverCinemas(WhatsOnV2GetCinemasResponse cinemas)
        {
            var date = DateTime.Now.ToString("dd-MM-yyyy");
            Console.WriteLine($"Date: {date}");

            foreach (var cinema in cinemas.WhatsOnCinemas)
            {
                var moviesInCinemaRequestStr = ReadFile("TheatreBlast.Requests.WhatsOnV2Alphabetic.ps1");
                moviesInCinemaRequestStr = string.Format(moviesInCinemaRequestStr, cinema.CinemaId, date);

                var moviesInCinemaPs = ParsePowershellCommand(moviesInCinemaRequestStr);
                var moviesInCinemaPsResponse = moviesInCinemaPs.Invoke();
                var moviesInCinemaJson = ((BasicHtmlWebResponseObject)moviesInCinemaPsResponse.First().BaseObject).Content;
                var moviesInCinema = JsonConvert.DeserializeObject<WhatsOnV2AlphabeticResponse>(moviesInCinemaJson);

                Console.WriteLine($"Cinema/City: {cinema.CinemaName} ({cinema.CinemaId})");
                LoopOverSeancesInCinema(moviesInCinema);
            }
        }

        private static void LoopOverSeancesInCinema(WhatsOnV2AlphabeticResponse moviesInCinema)
        {
            foreach (var movie in moviesInCinema.WhatsOnAlphabeticFilms)
            {
                Console.WriteLine($"   Movie: {movie.Title} ({movie.FilmId})");

                foreach (var whatsOn1 in movie.WhatsOnAlphabeticCinemas)
                {
                    foreach (var whatsOn2 in whatsOn1.WhatsOnAlphabeticCinemas)
                    {
                        foreach (var schedule in whatsOn2.WhatsOnAlphabeticShedules)
                        {
                            Console.WriteLine($"      Schedule: {schedule.Time}, {schedule.VersionTitle} ({schedule.BookingLink})");

                            ProceedSeance(schedule);
                        }
                    }
                }
            }
        }

        private static void ProceedSeance(WhatsOnAlphabeticShedulesEntry schedule)
        {
            var seanceId = schedule.BookingLink.Split(new[] {'/'}).SkipLast(1).Last();
//
//            if (seanceId != "3897255")
//            {
//                return;
//            }

            var seanceRequestStr = ReadFile("TheatreBlast.Requests.GetSeats.ps1");
            seanceRequestStr = string.Format(seanceRequestStr, seanceId);

            var seancePs = ParsePowershellCommand(seanceRequestStr);
            var seancePsResponse = seancePs.Invoke();
            var seanceJson = ((BasicHtmlWebResponseObject)seancePsResponse.First().BaseObject).Content;
            var seance = JsonConvert.DeserializeObject<GetSeatsResponse>(seanceJson);

            DrawSeanceRoom(seance);
        }

        private static void DrawSeanceRoom(GetSeatsResponse seance)
        {
            Console.SetCursorPosition(0, Console.CursorTop);

            var initZeroX = Console.CursorLeft;
            var initZeroY = Console.CursorTop;

            var seats = seance.Rows
                .SelectMany(row => row.Seats)
                .Select(seat => new
                {
                    seat.X,
                    seat.Y,
                    seat.IsBlocked,
                    seat.IsIdGreatherThanMinusOne,
                    seat.IsReserved
                });

            foreach (var seat in seats)
            {
                Console.SetCursorPosition(initZeroX + seat.X, initZeroY + seat.Y);

                if (seat.X == 6 && seat.Y == 0)
                {

                }

                if (seat.IsBlocked && !seat.IsIdGreatherThanMinusOne) //wylaczone
                {
                    Console.Write("x");
                }
                else
                {
                    if (seat.IsReserved)
                    {
                        Console.Write("o"); 
                    }
                    else
                    {
                        Console.Write("-");
                    }
                }
            }

            Console.SetCursorPosition(0, initZeroY + seats.Max(point => point.Y) + 1);
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
                        var hashTable = BuildHashTable(hashTableParts);
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

        private static Hashtable BuildHashTable(IList<string> hashTableParts)
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
