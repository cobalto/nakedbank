using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NakedBank.Api.v2;

namespace NakedBank.Api.GrpcServices
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHelloOnlyName(HelloOnlyNameRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = $"Hello {request.Name}",
                Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
            });
        }

        public override Task<HelloReply> SayHelloFullName(HelloFullNameRequest request, ServerCallContext context)
        {
            if (request.Surname is null or "")
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "You forgot your surname, honey?"));
            }

            return Task.FromResult(new HelloReply
            {
                Message = $"Hello {request.Name} {request.Surname}",
                Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
            });
        }
    }
}
