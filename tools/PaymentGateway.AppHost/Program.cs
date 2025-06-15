var builder = DistributedApplication.CreateBuilder(args);

var bankSimulator = builder.AddContainer("bank-simulator", "bbyars/mountebank", "2.8.1")
    .WithHttpEndpoint(port: 2525, targetPort: 2525, name: "admin")
    .WithHttpEndpoint(port: 8080, targetPort: 8080, name: "bank")
    .WithBindMount("../../imposters", "/imposters")
    .WithArgs("--configfile", "/imposters/bank_simulator.ejs", "--allowInjection");

var api = builder
    .AddProject<Projects.PaymentGateway_Api>("api")
    .WithReference(bankSimulator.GetEndpoint("bank"))
    .WaitFor(bankSimulator);

builder.Build().Run();