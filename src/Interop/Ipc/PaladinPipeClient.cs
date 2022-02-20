using System.IO.Pipes;
using Paladin.Interop.Exceptions;
using ProtoBuf;

namespace Paladin.Interop.Ipc
{
    internal class PaladinPipeClient
    {
        internal static PaladinPipeClient Connect()
        {
            var client = new NamedPipeClientStream(".", "paladin", PipeDirection.InOut, PipeOptions.None);
            client.Connect(2000);
            if (!client.IsConnected)
            {
                client.Close();
                throw new AttachingFailedException($"Could not connect with shellcode pipe.");
            }
            //client.ReadMode = PipeTransmissionMode.Message;

            return new PaladinPipeClient(client);
        }

        private readonly NamedPipeClientStream _client;

        private PaladinPipeClient(NamedPipeClientStream client)
        {
            _client = client;
        }

        internal void Move(IntPtr entityAddr, int xOffset, int yOffset)
        {
            var message = new IpcProtocol.Move
            {
                EntityAddress = (int)entityAddr,
                XOffset = xOffset,
                YOffset = yOffset
            };

            Serializer.Serialize(_client, message);

            _client.Flush();
        }

        internal void Close()
        {
            _client.Close();
        }
    }
}
