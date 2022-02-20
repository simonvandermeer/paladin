using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Paladin.Interop.Exceptions;
using Paladin.Interop.Extensions;
using Paladin.Interop.Ipc;
using Paladin.Interop.Models.Generated;

namespace Paladin.Interop
{
    public class GameProxy
    {
        private const string ProcessName = "NecroDancer";
        private const string ShellcodeDllPath = @"C:/shared/Default/projects/paladin/src/Shellcode/Release/PaladinShellcode.dll";
        private const string CryptOfTheNecroDancerPath = @"C:\Program Files (x86)\Steam\steamapps\common\Crypt of the NecroDancer\NecroDancer.exe";

        public static GameProxy Attach()
        {
            // Quick check if the shellcode is available.
            if (!File.Exists(ShellcodeDllPath))
            {
                throw new AttachingFailedException($"Shellcode dll not found at '{ShellcodeDllPath}'.");
            }

            // Find the process.
            var processes = Process.GetProcesses();

            var process = processes.FirstOrDefault(process =>
                process.ProcessName == ProcessName);

            if (process == null)
            {
                // Let's start it up.
                var startInfo = new ProcessStartInfo
                {
                    FileName = CryptOfTheNecroDancerPath,
                    WorkingDirectory = Directory.GetParent(CryptOfTheNecroDancerPath).FullName
                };

                process = Process.Start(startInfo);
            }

            // Get a process handle.
            var processHandle = Imports.OpenProcess(Imports.PROCESS_CREATE_THREAD
                | Imports.PROCESS_QUERY_INFORMATION
                | Imports.PROCESS_VM_OPERATION
                | Imports.PROCESS_VM_WRITE
                | Imports.PROCESS_VM_READ,
                false, process.Id);

            // Inject shellcode if we haven't yet.
            if (!process.Modules.Any(module => module.ModuleName == Path.GetFileName(ShellcodeDllPath)))
            {
                // Inject shellcode.
                // Find the address of LoadLibraryA in the target process.
                var loadLibraryAddress = Imports.GetProcAddress(Imports.GetModuleHandle("kernel32.dll"), "LoadLibraryA");

                // Allocate enough memory within the target process to store the path to the dll and get a pointer to it.
                var dllPathMemoryLength = (uint)((ShellcodeDllPath.Length + 1) * Marshal.SizeOf(typeof(char)));
                var allocMemAddress = Imports.VirtualAllocEx(processHandle, IntPtr.Zero,
                    dllPathMemoryLength,
                    Imports.MEM_COMMIT | Imports.MEM_RESERVE,
                    Imports.PAGE_READWRITE);

                // Write the path to previously allocated memory.
                Imports.WriteProcessMemory(processHandle, allocMemAddress, Encoding.Default.GetBytes(ShellcodeDllPath), dllPathMemoryLength, out _);

                // Create a thread that will call LoadLibraryA with allocMemAddress as argument.
                Imports.CreateRemoteThread(processHandle, IntPtr.Zero, 0, loadLibraryAddress, allocMemAddress, 0, IntPtr.Zero);
            }

            // Connect to the named pipe server that is hosted by the shellcode.
            var pipeClient = PaladinPipeClient.Connect();

            return new GameProxy(process, processHandle, pipeClient);
        }

        private Process _process;
        private IntPtr _processHandle;
        private IntPtr _baseAddress;
        private PaladinPipeClient _pipeClient;

        private GameProxy(Process process, IntPtr processHandle, PaladinPipeClient pipeClient)
        {
            _process = process;
            _processHandle = processHandle;
            _baseAddress = process.MainModule.BaseAddress;
            _pipeClient = pipeClient;
        }

        public bool HasProcessExited => _process.HasExited;

        internal string GetObjectType(IntPtr objectAddr)
        {
            // RTTI pointer can be found just behind the VTable in this game.
            var vTableAddr = Memory.ReadPointer(_processHandle, objectAddr);
            var rttiAddr = Memory.ReadPointer(_processHandle, vTableAddr - 0x4);

            // TypeInfo pointer can be found at 0xC.
            var typeInfoAddr = Memory.ReadPointer(_processHandle, rttiAddr + 0xC);

            // Type name begins at 0xC and terminates with '@@'.
            var typeNameAddr = typeInfoAddr + 0xC;

            // Reading 64 characters should be more than enough...
            var typeNameBytes = Memory.Read(_processHandle, typeNameAddr, 64, out _);
            var typeName = Encoding.UTF8.GetString(typeNameBytes);

            return typeName.Substring(0, typeName.IndexOf("@@"));
        }

        public void Close()
        {
            _pipeClient.Close();
            _pipeClient = null;
            _baseAddress = IntPtr.Zero;
            _processHandle = IntPtr.Zero;
        }

        public Player GetPlayer()
        {
            EnsureProcessConnection();

            var playerAddr = Memory.ReadPointer(_processHandle, _baseAddress + 0x3BF584);
            var player = Memory.Read<Player>(_processHandle, playerAddr, out _);

            return player;
        }

        public IEnumerable<(Enemy Enemy, string EnemyType)> GetEnemies()
        {
            EnsureProcessConnection();

            var listAddr = Memory.ReadPointer(_processHandle, _baseAddress + 0x003BF768);
            var headNodeAddr = Memory.ReadPointer(_processHandle, listAddr + 0x10);

            var headNode = Memory.Read<EnemyHeadNode>(_processHandle, headNodeAddr, out _);

            for (var nodePtr = headNode._next; nodePtr != headNodeAddr;)
            {
                var node = Memory.Read<EnemyNode>(_processHandle, nodePtr, out _);
                nodePtr = node._next;

                var entity = Memory.Read<Enemy>(_processHandle, node._entity, out _);
                var entityType = GetObjectType(node._entity);

                yield return (entity, entityType);
            }
        }

        public IEnumerable<(Trap Trap, string TrapType)> GetTraps()
        {
            EnsureProcessConnection();

            var listAddr = Memory.ReadPointer(_processHandle, _baseAddress + 0x003BF704);
            var headNodeAddr = Memory.ReadPointer(_processHandle, listAddr + 0x10);

            var headNode = Memory.Read<TrapHeadNode>(_processHandle, headNodeAddr, out _);

            for (var nodePtr = headNode._next; nodePtr != headNodeAddr;)
            {
                var node = Memory.Read<TrapNode>(_processHandle, nodePtr, out _);
                nodePtr = node._next;

                var entity = Memory.Read<Trap>(_processHandle, node._entity, out _);
                var entityType = GetObjectType(node._entity);

                yield return (entity, entityType);
            }
        }

        public IEnumerable<Tile> GetTiles()
        {
            EnsureProcessConnection();

            var listAddr = Memory.ReadPointer(_processHandle, _baseAddress + 0x003BF96C);
            var headNodeAddr = Memory.ReadPointer(_processHandle, listAddr + 0x10);

            var headNode = Memory.Read<TileNode>(_processHandle, headNodeAddr, out _);

            for (var nodePtr = headNode._next; nodePtr != headNodeAddr;)
            {
                var node = Memory.Read<TileNode>(_processHandle, nodePtr, out _);
                nodePtr = node._next;

                var entityType = GetObjectType(node._entity);

                if (entityType == "c_Tile")
                {
                    yield return Memory.Read<Tile>(_processHandle, node._entity, out _);
                }
            }
        }

        private void EnsureProcessConnection()
        {
            if (HasProcessExited)
            {
                throw new CotndExitedException();
            }
        }

        public Sprite GetTileSprite(Tile tile)
        {
            EnsureProcessConnection();

            return Memory.Read<Sprite>(_processHandle, tile._sprite, out _);
        }

        public string GetSpriteImagePath(Sprite sprite)
        {
            EnsureProcessConnection();

            var text = Memory.Read<Text>(_processHandle, sprite._imagePath, out _);
            return TextToString(text, sprite._imagePath);
        }

        public string TextToString(Text text, IntPtr textAddr)
        {
            EnsureProcessConnection();

            var startAddr = textAddr + 0x8;

            var bytes = Memory.Read(_processHandle, startAddr, text._length * 2, out _); // * 2 for Unicode.

            return Encoding.Unicode.GetString(bytes);
        }

        public void Move(int xOffset, int yOffset)
        {
            EnsureProcessConnection();

            var playerAddr = Memory.ReadPointer(_processHandle, _baseAddress + 0x3BF584);
            _pipeClient.Move(playerAddr, xOffset, yOffset);
        }
    }
}
