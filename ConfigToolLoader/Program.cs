using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace ConfigToolLoader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var Tool = new ConfigTool();
            if (!Tool.IsValid)
            {
                Console.ReadKey();
                return;
            }

            // [section] .stabstr
            // [+] readable
            Tool.PatchUint32(0x000002B4, 0x42000000);

            // URL 1
            Tool.PatchString(0x01235B40, "https://combo384.twmdzz.com/targa102/auto.php?");
            Tool.PatchUint32(0x00026BAC, 0x01642344);
            Tool.PatchUint32(0x0002AA1F, 0x01642344);

            // URL 2
            Tool.PatchString(0x01235B80, "https://combo384.twmdzz.com/targa102/");
            Tool.PatchUint32(0x0002702B, 0x01642384);
            Tool.PatchUint32(0x00027660, 0x01642384);
            Tool.PatchUint32(0x00029810, 0x01642384);
            Tool.PatchUint32(0x0002B924, 0x01642384);

            Tool.Execute();
        }
    }

    public class ConfigTool
    {
        private readonly string EXE = "ConfigTool.exe";
        private readonly string Hash = "75eb32ba75833bd0c4555911ce5c2cd51d4e3301f1e033f630abb6bd1e6a90f7";
        private static byte[] Buffer;
        public bool IsValid { get; set; }

        public ConfigTool()
        {
            if (!File.Exists(EXE))
            {
                Console.WriteLine($"{EXE} not found!");
                return;
            }

            Buffer = File.ReadAllBytes(EXE);

            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Buffer);
                var str = BitConverter.ToString(hash).Replace("-", "").ToLower();
                if (str != Hash)
                {
                    Console.WriteLine($"{EXE} checksum mismatch!");
                    return;
                }
            }

            Console.WriteLine("========================================");
            Console.WriteLine("       Combo384 ConfigTool Loader       ");
            Console.WriteLine("========================================");
            Console.WriteLine();
            IsValid = true;
        }

        public void PatchUint32(UInt32 offset, UInt32 content)
        {
            BitConverter.GetBytes(content).CopyTo(Buffer, offset);
            Console.WriteLine($"[+] 0x{offset.ToString("X8")} => 0x{content.ToString("X8")}");
        }

        public void PatchString(UInt32 offset, string content)
        {
            BitConverter.GetBytes(content.Length).CopyTo(Buffer, offset);
            Encoding.UTF8.GetBytes(content).CopyTo(Buffer, offset + 4);
            Console.WriteLine($"[+] 0x{offset.ToString("X8")} => 0x{content.Length.ToString("X8")}");
            Console.WriteLine($"[+] 0x{(offset + 4).ToString("X8")} => \"{content}\"");
        }

        public void Execute()
        {
            var temp = "temp.exe";
            File.WriteAllBytes(temp, Buffer);
            var p = Process.Start(temp);
            p.WaitForExit();
            Thread.Sleep(500);
            File.Delete(temp);
        }
    }
}
