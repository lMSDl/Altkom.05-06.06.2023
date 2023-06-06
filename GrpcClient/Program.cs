using Grpc.Core;
using Grpc.Net.Client;
using GrpcService.Protos.Users;
using System.Security.Cryptography.X509Certificates;

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



using (var grpcChannel = GrpcChannel.ForAddress("https://localhost:7280/"))
{
    var client = new GrpcService.Services.GrpcStream.GrpcStreamClient(grpcChannel);

    var stream = client.FromServer(new GrpcService.Services.Request() { Text = "Hello!" });

    var tokenSource = new CancellationTokenSource();
    var counter = 0;

    try
    {
        while (await stream.ResponseStream.MoveNext(tokenSource.Token))
        {
            Console.WriteLine($"{counter} {stream.ResponseStream.Current.Text}");
            if (++counter == 1000)
            {
                tokenSource.Cancel();
                //break;
            }
        }
    }
    catch (RpcException e)
    {
    }

    var toServer = client.ToServer();

    foreach (var item in "ala ma kota".Split(" "))
    {
        await toServer.RequestStream.WriteAsync(new GrpcService.Services.Request() { Text = item});
    }
    await toServer.RequestStream.CompleteAsync();
    Console.WriteLine((await toServer.ResponseAsync).Text);




    var fromToServer = client.FromAndToServer();
    _ = Task.Run(async () =>
    {
        for (int i = 0; i < int.MaxValue; i++)
        {
            await fromToServer.RequestStream.WriteAsync(new GrpcService.Services.Request() { Text = i.ToString() });
        }

        await fromToServer.RequestStream.CompleteAsync();
    });


    _ = Task.Run(async () =>
    {
        while (await fromToServer.ResponseStream.MoveNext(CancellationToken.None))
        {
            Console.Write(fromToServer.ResponseStream.Current.Text);
        }
    });

    Console.ReadLine();
}