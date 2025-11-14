using Grpc.Net.Client;
using GrpcAPI.Protos;

namespace GrpcAPI
{
    public class CalendarMainGrpcHandler
    {
        private readonly CalendarProtoService.CalendarProtoServiceClient _client;

        public CalendarMainGrpcHandler(GrpcChannel channel)
        {
            _client = new CalendarProtoService.CalendarProtoServiceClient(channel);
        }
        
        /// <summary>
        /// Sends a request to the gRPC server specifying the handler and action
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The server response</returns>
        public async Task<ResponseProto> SendRequestAsync(RequestProto request)
        {
            try
            {
                ResponseProto  response= await _client.SendRequestAsync(request);
                return  response;
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"RPC failed: {ex.Status}");
                throw;
            }
        }
    }
}