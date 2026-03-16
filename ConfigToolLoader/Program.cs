using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace ConfigToolLoader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConfigTool tool = new();

            if (tool.IsValid)
                tool.Patch();

            Console.ReadKey();
        }
    }

    public class ConfigTool
    {
        private readonly string fileName = "ConfigTool.exe";

        private readonly string fileHash = "75eb32ba75833bd0c4555911ce5c2cd51d4e3301f1e033f630abb6bd1e6a90f7";

        private Process process;

        public bool IsValid => process != null && !process.HasExited;

        public ConfigTool()
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"{fileName} not found!");
                return;
            }

            using (var fs = File.OpenRead(fileName))
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(fs);
                var str = BitConverter.ToString(hash).Replace("-", "").ToLower();
                if (str != fileHash)
                {
                    Console.WriteLine($"{fileHash} checksum mismatch!");
                    return;
                }
            }

            process = new()
            {
                StartInfo = new(fileName)
                {
                    UseShellExecute = false
                }
            };
            process.Start();
        }

        public void Patch()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("       Combo384 ConfigTool Loader       ");
            Console.WriteLine("========================================\n");

            var addr = AllocMemory(0x1000);
            var main = process.MainModule.BaseAddress;

            WriteString(addr + 0x00, "https://combo384.twmdzz.com/targa102/auto.php?");
            WriteString(addr + 0x80, "https://combo384.twmdzz.com/targa102/");

            WriteInt32(main + 0x000277AB + 1, (int)addr + 0x04);
            WriteInt32(main + 0x0002B61E + 1, (int)addr + 0x04);

            WriteInt32(main + 0x00027C2A + 1, (int)addr + 0x84);
            WriteInt32(main + 0x0002825F + 1, (int)addr + 0x84);
            WriteInt32(main + 0x0002A40F + 1, (int)addr + 0x84);
            WriteInt32(main + 0x0002C523 + 1, (int)addr + 0x84);
        }

        private nint AllocMemory(uint size)
        {
            return VirtualAllocEx(process.Handle, IntPtr.Zero, size, MEM_COMMIT, PAGE_READWRITE);
        }

        private void WriteInt32(nint address, int content)
        {
            var buff = BitConverter.GetBytes(content);
            WriteProcessMemory(process.Handle, address, buff, (uint)buff.Length, out uint _);
            Console.WriteLine($"[+] 0x{address.ToString("X8")} => 0x{content.ToString("X8")}");
        }

        private void WriteString(nint address, string content)
        {
            WriteInt32(address, content.Length);

            address += 4;
            var buff = Encoding.UTF8.GetBytes(content);
            WriteProcessMemory(process.Handle, address, buff, (uint)buff.Length, out uint _);
            Console.WriteLine($"[+] 0x{address.ToString("X8")} => \"{content}\"");
        }

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out uint lpNumberOfBytesWritten);

        private const uint MEM_COMMIT = 0x00001000;

        private const uint PAGE_READWRITE = 0x4;
    }
}
