using ProcessMemoryScanner;
using System;
using System.Diagnostics;
using System.IO;

namespace AmaneroOemToolBypass
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /* 
             * Invalid or not authorized transaction!
             * orig:  74 19 8B 85 78 FF FF FF BA
             * patch: EB 19 8B 85 78 FF FF FF BA
             */

            var targetFile = "ConfigTool.exe";
            var targetBytes = new byte[] { 0x74, 0x19, 0x8B, 0x85, 0x78, 0xFF, 0xFF, 0xFF, 0xBA };

            if (!File.Exists(targetFile))
            {
                Console.WriteLine($"{targetFile} not found!");
                Console.ReadKey();
                Environment.Exit(0);
            }

            var oemtool = Process.Start(targetFile);

            var memory = new MemoryScanner(p => p.Id == oemtool.Id);
            var memoryRegions = memory.FindMemoryRegion(p => p.RegionSize.ToInt64() > 0);

            memory.SuspendProcess();

            var offset = IntPtr.Zero;
            foreach (var region in memoryRegions)
            {
                if (region.RegionSize.ToInt64() > 0x10000000)
                    continue;

                offset = memory.FindByAoB(targetBytes, region);
                if (offset != IntPtr.Zero)
                {
                    memory.WriteMemory(offset, new byte[] { 0xEB });
                    memory.ResumeProcess();
                    Console.WriteLine("Patch done!");
                    break; //only one section need patch
                }
            }

            if (offset == IntPtr.Zero)
            {
                oemtool.Kill();
                Console.WriteLine("AOB scan no result!");
            }

            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
