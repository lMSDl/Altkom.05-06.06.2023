using Grpc.Net.Client;
using GrpcService.Protos.Users;

using (var grpcChannel = GrpcChannel.ForAddress("https://localhost:7280/"))
{

    var client = new GrpcService.Greeter.GreeterClient(grpcChannel);

    var reply = client.SayHello(new GrpcService.HelloRequest { Name = "Paul" });

    Console.WriteLine(reply.Message);

}


using (var grpcChannel = GrpcChannel.ForAddress("https://localhost:7280/"))
{
    var client = new GrpcService.Protos.Users.GrpcUsers.GrpcUsersClient(grpcChannel);

    var user = new User() { Password = "password", Username = "username" };

    user = client.Create(user);
    var users = client.Read(new None());

    client.Update(user);
}