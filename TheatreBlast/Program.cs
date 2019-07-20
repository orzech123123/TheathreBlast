using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TheatreBlast
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var invokeWebRequestString =
                @"Invoke-WebRequest -Uri ""https://multikino.pl/data/getSeats"" -Method ""POST"" -Headers @{""path""=""/data/getSeats""; ""origin""=""https://multikino.pl""; ""accept-encoding""=""gzip, deflate, br""; ""accept-language""=""pl-PL,pl;q=0.9,en-US;q=0.8,en;q=0.7""; ""user-agent""=""Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.142 Safari/537.36""; ""accept""=""application/json, text/plain, */*""; ""referer""=""https://multikino.pl/kupbilet/7409/10/2/3875759""; ""authority""=""multikino.pl""; ""cookie""=""__cfduid=d800e5b428cccb4d0e84dc82c5ad038541563280653; ASP.NET_SessionId=pi2ktmxmrecd1floxj0oid1a; _gcl_au=1.1.1496278045.1563280655; _ga=GA1.2.1049950900.1563280655; scs=%7B%22t%22%3A1%7D; ins-mig-done=1; LastSelectedCinema=CinemaId=10&CityId=9&CinemaSitecoreId={39C0DA90-CCB9-4A6B-B804-DC8427260B9F}&CinemaSitecoreSecondaryMarket=True; PPC=true; G_ENABLED_IDPS=google; __RequestVerificationToken=xBw6MloTxx6OjoqlX-RRPeXwFcqG7tyhfYWMYfRgYM3ddn4fmtSN7EIIlMGt94jfBb1AbfkgFtMEyiPBfpX7bQRmXeY1; _gcl_aw=GCL.1563363047.EAIaIQobChMI75_3le274wIVlJAYCh2liA9TEAAYASAAEgLVePD_BwE; _gac_UA-3364491-9=1.1563363048.EAIaIQobChMI75_3le274wIVlJAYCh2liA9TEAAYASAAEgLVePD_BwE; .ASPXAUTH=71DB4D7D18E79D30939355BE0507058DC782DA73706F2547EA4EC04A49E019CD91F05C421191491AE8809ED23F6848F611A73F8616FAA461376120E4094B363F0BEEF6D1748221919FD3A45ABF3DB4E033B94DC3A934C07D549268B38D51F5CE22F5EFF4BE75487EC94EC71321B28CB19E6D39BDFC56C8DB84A5F8FA1AE4E3C2F2F071CC412D90BB599F8F74237A8C10DDF07454496B11AAF3B1BB9C89A3B69A7D9DB097; kobi_customer_new=0; kobi_customer_new_ext=M:0|G:0|1Y:0|HY:0|FY:0; salesOrderId=ins_unknown_a6qo0_1563372200; _gid=GA1.2.108940316.1563529458; __cfruid=7b3d5da821362881b3e883b78ef0dcaff5440a86-1563529455; current-currency=; ins-product-id=B8708ECD-B709-4633-8C5C-76BB416B191C; _gat_UA-3364491-9=1; ins-gaSSId=913d47c8-e512-9d61-1523-9c5844fa7c51_1563571462; __insp_wid=1965456342; __insp_slim=1563571465143; __insp_nv=true; __insp_targlpu=aHR0cHM6Ly9tdWx0aWtpbm8ucGwva3VwYmlsZXQvNzQwOQ%3D%3D; __insp_targlpt=V3liaWVyeiBraW5v; insdrSV=28; __insp_norec_sess=true; AWSALB=mwO4qYmtnvuQS68Ta0qov2/zv/IY8JWe6QE2esZYMl5H2eGCUgfNOD51AjdrwRl8C3gFkj0VNn9tmUVFYh9X02yg/+qp8c3X5onQup5FQoUPYfkVy2XC+eCA1dz6""; ""scheme""=""https""; ""method""=""POST""} -ContentType ""application/x-www-form-urlencoded"" -Body ""seance_id=3875759""";

            var xx =
                @"Invoke-WebRequest -Uri ""https://multikino.pl/data/getMovie"" -Method ""POST"" -Headers @{""path""=""/data/getMovie""; ""origin""=""https://multikino.pl""; ""accept-encoding""=""gzip, deflate, br""; ""accept-language""=""pl-PL,pl;q=0.9,en-US;q=0.8,en;q=0.7""; ""user-agent""=""Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.142 Safari/537.36""; ""accept""=""application/json, text/plain, */*""; ""referer""=""https://multikino.pl/kupbilet/7409""; ""authority""=""multikino.pl""; ""cookie""=""__cfduid=d800e5b428cccb4d0e84dc82c5ad038541563280653; ASP.NET_SessionId=pi2ktmxmrecd1floxj0oid1a; _gcl_au=1.1.1496278045.1563280655; _ga=GA1.2.1049950900.1563280655; scs=%7B%22t%22%3A1%7D; ins-mig-done=1; LastSelectedCinema=CinemaId=10&CityId=9&CinemaSitecoreId={39C0DA90-CCB9-4A6B-B804-DC8427260B9F}&CinemaSitecoreSecondaryMarket=True; PPC=true; G_ENABLED_IDPS=google; __RequestVerificationToken=xBw6MloTxx6OjoqlX-RRPeXwFcqG7tyhfYWMYfRgYM3ddn4fmtSN7EIIlMGt94jfBb1AbfkgFtMEyiPBfpX7bQRmXeY1; _gcl_aw=GCL.1563363047.EAIaIQobChMI75_3le274wIVlJAYCh2liA9TEAAYASAAEgLVePD_BwE; _gac_UA-3364491-9=1.1563363048.EAIaIQobChMI75_3le274wIVlJAYCh2liA9TEAAYASAAEgLVePD_BwE; .ASPXAUTH=71DB4D7D18E79D30939355BE0507058DC782DA73706F2547EA4EC04A49E019CD91F05C421191491AE8809ED23F6848F611A73F8616FAA461376120E4094B363F0BEEF6D1748221919FD3A45ABF3DB4E033B94DC3A934C07D549268B38D51F5CE22F5EFF4BE75487EC94EC71321B28CB19E6D39BDFC56C8DB84A5F8FA1AE4E3C2F2F071CC412D90BB599F8F74237A8C10DDF07454496B11AAF3B1BB9C89A3B69A7D9DB097; kobi_customer_new=0; kobi_customer_new_ext=M:0|G:0|1Y:0|HY:0|FY:0; salesOrderId=ins_unknown_a6qo0_1563372200; _gid=GA1.2.108940316.1563529458; __cfruid=7b3d5da821362881b3e883b78ef0dcaff5440a86-1563529455; ins-product-id=B8708ECD-B709-4633-8C5C-76BB416B191C; insdrSV=29; __insp_wid=1965456342; __insp_slim=1563576624961; __insp_nv=true; __insp_targlpu=aHR0cHM6Ly9tdWx0aWtpbm8ucGwva3VwYmlsZXQvNzQwOQ%3D%3D; __insp_targlpt=V3liaWVyeiBraW5v; __insp_norec_sess=true; AWSALB=Lrt6FLsMqPzCtHYfsOSXCs6syuKtKPEVVYXEJZNEVFU+U0IBWWdnbEKjq7iTtLIWQcDzEVLay3MIuFQfF37jWJbIU8dGPlFCNF3Me299o/BxrmMOvm7DYP5jl5ma""; ""scheme""=""https""; ""method""=""POST""} -ContentType ""application/x-www-form-urlencoded"" -Body ""film_id=7409&origin_url=%2F7409""";


            var ps = ParsePowershellCommand(invokeWebRequestString);
            var x = ps.Invoke();

            var ps2 = ParsePowershellCommand(xx);
            var x2 = ps2.Invoke();
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
