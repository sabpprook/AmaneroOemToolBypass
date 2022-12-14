using ProcessMemoryUtilities.Managed;
using ProcessMemoryUtilities.Native;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

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

            var targetExe = "ConfigTool.exe";
            var targetBytes = new byte[] { 0x74, 0x19, 0x8B, 0x85, 0x78, 0xFF, 0xFF, 0xFF, 0xBA };
            var patchBytes = new byte[] { 0xEB };

            if (!File.Exists(targetExe))
            {
                Console.WriteLine($"{targetExe} not found!");
                Console.ReadKey();
                return;
            }

            var oemtool = Process.Start(targetExe);
            var baseAddress = oemtool.MainModule.BaseAddress;
            var matchAddress = IntPtr.Zero;

            var buffer = new byte[2 * 1024 * 1024];

            var handle = NativeWrapper.OpenProcess(ProcessAccessFlags.Read, oemtool.Id);
            NativeWrapper.ReadProcessMemoryArray(handle, baseAddress, buffer);
            NativeWrapper.CloseHandle(handle);

            for (int i = 0; i < buffer.Length - targetBytes.Length; i++)
            {
                var result = buffer.Take(i..(i + targetBytes.Length)).SequenceEqual(targetBytes);
                if (result)
                {
                    matchAddress = baseAddress + i;
                    break;
                }
            }

            if (matchAddress == IntPtr.Zero)
            {
                oemtool.Kill();
                Console.WriteLine("nothing to do...");
                Console.ReadKey();
                return;
            }

            handle = NativeWrapper.OpenProcess(ProcessAccessFlags.ReadWrite, oemtool.Id);
            NativeWrapper.VirtualProtectEx(handle, matchAddress, (IntPtr)patchBytes.Length, MemoryProtectionFlags.ExecuteReadWrite, out var oldProtect);
            NativeWrapper.WriteProcessMemoryArray(handle, matchAddress, patchBytes);
            NativeWrapper.VirtualProtectEx(handle, matchAddress, (IntPtr)patchBytes.Length, oldProtect, out _);
            NativeWrapper.CloseHandle(handle);

            Console.WriteLine("patch done!");
            Console.ReadKey();
        }
    }
}
