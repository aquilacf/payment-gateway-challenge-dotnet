This is a demo API built in C# ASP.NET with the structure and features I normally use. The goal is to provide default
observability, resilience and high maintainability.

Requirements to run the project:

- Docker
- .NET SDK 9

```shell
cd tools/PaymentGateway.AppHost
dotnet run
```

Copy the URL with the access token to see the Aspire dashboard.

How to test (requires aspire for DistributedApplicationTestingBuilder):

```shell
dotnet test
```
⚠️ Make sure you have trusted dotnet certificates with: dotnet dev-certs https --trust

As an alternative to Aspire, use docker compose:

```shell
docker compose up
```

Visit http://localhost:18888/login?t=test123 to see the dashboard with traces, metrics and logs. The API will be on http://localhost:5000

# Requirements

- Code must compile [x]
- Your code is covered by automated tests. It is your choice which type of tests and the number of them you want to
  implement [x]
- The code to be simple and maintainable. We do not want to encourage over-engineering [x]
- Your API design and architecture should be focused on meeting the functional requirements outlined above [x]

Business logic requirements:

- A merchant should be able to process a payment through the payment gateway and receive one of the following types of
  response:
    - Authorized - the payment was authorized by the call to the acquiring bank [x]
    - Declined - the payment was declined by the call to the acquiring bank [x]
    - Rejected - No payment could be created as invalid information was supplied to the payment gateway and therefore it
      has rejected the request without calling the acquiring bank [x]
- A merchant should be to be able to retrieve the details of a previously made payment. [x]

⚠️ The requirements imply that even rejected payments should be retrieved later one, so bad requests should be stored?

Endpoint requirements:

- POST /payments: Processes a new payment
- GET /payments/{id}: Retrieves the details of a previously made payment

# Assumptions

Some assumptions were made as simplification for the challenge:

- In-memory storage: In-memory repository to store the payment data. In production, a persistent database would be used
- Bank simulator: Bank simulator to process the payments. In production, an actual acquiring bank would be used
- No authentication/authorization: No implemented authentication or authorization mechanisms. In production, it could
  use oAuth, JWT, API Keys

# Structure

Clean architecture with vertical slicing endpoints.

- src:
    - PaymentGateway.Api: The presentation layer, exposes the API
    - PaymentGateway.Application: The application layer, contains business logic
    - PaymentGateway.Domain: The domain layer, contains entity and interfaces
    - PaymentGateway.Infrastructure: The infrastructure layer, implementation of the interfaces / bridge with extenal
      service
- tests:
    - PaymentGateway.Infrastructure.UnitTests
    - PaymentGateway.IntegrationTests
- tools:
    - PaymentGateway.AppHost: Aspire application host, good to orchestrate multiple services locally
    - PaymentGateway.ServiceDefaults: Good defaults for any service

# Features

I try to use Microsoft packages as much as possible; I don't dislike public packages but, to me Microsoft packages tell
the general vision of their plans for C# and .NET.

- Aspire: good to orchestrate multiple services locally
- OpenTelemetry: Joins traces, logs and metric in a single SDK, new standard
- OpenAPI: Sitemap for APIs
- Resiliency (Polly): Retry mechanism
- Service Discovery: Consequence of Aspire, service discovery helps pointing request to the service
- CQRS: separation of concerns and also makes decouples the presentation layer from infrastructure layer
- System Data Annotations: built-in validation in dotnet

For testing:

- xUnit
- FluentAssertions

### Decisions
IValidableObject vs Data Annotation attributes: I have chosen IValidableObject due to ease of implementation, but I personally prefer using DataAnnotations and create custom attributes like DateInFutureAttribute.
IApplicationHandler<,>: I made this class to simulate a Mediator, it goes well with CQRS

### Extra

These are features I also use in my APIs:

- Wiremock (Aspire)
- Structured logging: Make logs indexable and better searchable
- Event sourcing: Safe and auditable records
- FeatureManager: live A/B testing
- Kiota: Sometimes vendors don't have SDKs or don't update them frequently enough; Kiota solves that
- Result class: exceptions are more expensive than return Result.Fail

### Room for improvement

- Idempotency
- Mediator
- Terraform: infrastructure as code
- GitHub Actions: CI
- GitHub Deployments: CD
