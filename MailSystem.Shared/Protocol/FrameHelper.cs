using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MailSystem.Shared.Envelopes;
using MailSystem.Shared.Constants;

namespace MailSystem.Shared.Protocol
{
    public static class FrameHelper
    {
        // --- WRITING FRAMES ---

        public static async Task SendEnvelopeAsync(Stream stream, Envelope envelope)
        {
            string json = JsonConvert.SerializeObject(envelope);
            await SendStringAsync(stream, json);
        }

        public static async Task SendResponseAsync(Stream stream, ResponseEnvelope response)
        {
            string json = JsonConvert.SerializeObject(response);
            await SendStringAsync(stream, json);
        }

        private static async Task SendStringAsync(Stream stream, string json)
        {
            byte[] payload = Encoding.UTF8.GetBytes(json);
            byte[] lengthPrefix = new byte[4];

            // 4-byte little-endian length prefix
            BinaryPrimitives.WriteInt32LittleEndian(lengthPrefix, payload.Length);

            await stream.WriteAsync(lengthPrefix, 0, 4);
            await stream.WriteAsync(payload, 0, payload.Length);
            await stream.FlushAsync();
        }

        // --- READING FRAMES ---

        public static async Task<string> ReadFrameAsStringAsync(Stream stream)
        {
            byte[] lengthPrefix = new byte[4];
            int bytesRead = 0;

            // Read the 4-byte length prefix first
            while (bytesRead < 4)
            {
                int read = await stream.ReadAsync(lengthPrefix, bytesRead, 4 - bytesRead);
                if (read == 0) throw new EndOfStreamException("Connection closed by remote host.");
                bytesRead += read;
            }

            int messageLength = BinaryPrimitives.ReadInt32LittleEndian(lengthPrefix);

            // Validate the length
            if (messageLength <= 0 || messageLength > AppConstants.MaxFrameSizeBytes)
            {
                throw new InvalidDataException($"Invalid frame length: {messageLength}");
            }

            // Read the JSON payload
            byte[] payload = new byte[messageLength];
            bytesRead = 0;
            while (bytesRead < messageLength)
            {
                int read = await stream.ReadAsync(payload, bytesRead, messageLength - bytesRead);
                if (read == 0) throw new EndOfStreamException("Connection closed before frame was fully read.");
                bytesRead += read;
            }

            return Encoding.UTF8.GetString(payload);
        }

        public static async Task<Envelope> ReadEnvelopeAsync(Stream stream)
        {
            string json = await ReadFrameAsStringAsync(stream);
            return JsonConvert.DeserializeObject<Envelope>(json) ?? throw new InvalidDataException("Failed to parse Envelope.");
        }

        public static async Task<ResponseEnvelope> ReadResponseAsync(Stream stream)
        {
            string json = await ReadFrameAsStringAsync(stream);
            return JsonConvert.DeserializeObject<ResponseEnvelope>(json) ?? throw new InvalidDataException("Failed to parse ResponseEnvelope.");
        }
    }
}