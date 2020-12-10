# Checkout Payment Gateway
## Introduction
This project consists of two APIs that allow a merchant to submit and retrieve payments. Both APIs have a simple Swagger documentation that gets generated on runtime.

The project is completely implemented using .NET 3.1 using ASP.NET for the API layer and Entity Framework Core for the persistence layer. The different parts were designed using DDD principles for better scalability and extendability.
### Bank API
This API returns has one operation and returns a very simple response. The API can be configured to have a programmed delay in milliseconds and to fail with specific cards. The API performs model validation as well.
### Payment Gateway
The payment gateway was modelled as a single bounded context with a Domain, Application, Infrastructure and API layer.
#### Domain
The domain contains the DDD framework I build for this project (`Entity`, `ValueObject`, `AggregateRoot`...) and the domain-specific functionality for the model.

The model contains a single aggregate called `Payment`, and that stores the payment-related information. The entity does not contain any methods as the requirements only consist of creation and retrieval operations.
This project also contains a repository for the `Payment` aggregate and a few commands and queries.
#### Infrastructure
The infrastructure layer contains the `IPaymentRepository` implementation and the data persistence mechanism. Entity Framework Core is used to store the `Payment` aggregate. This layer also includes the configuration for the `Payment` aggregate in the Database as writing the configuration on the `Payment` itself would be a bad separation of concerns.
#### Application
This layer contains the application logic and separates the domain model from the API.
As the requirement document said, the business is experiencing exponential growth, and for that reason, I implemented a Command & Query dispatching mechanism to be able to extend the functionality very easily and promote decoupling.

The communication with the model is done through command and query handlers. These handlers can be decorated with `DecoratorAttribute`s to add functionality to the handlers. These decorators are automatically configured by using the appropriate attributes and calling the `IServiceCollection.AddHandlers()` method in the `Setup` class. The command decorators are executed before the command handler, and the query decorators can be executed before and after the query handler.

An example of this is the `Encrypt` decorator on the `CreatePayment` command to encrypt the card number before storing it and using `Decrypt` and `Mask` in the queries to decrypt the card number and mask it after the retrieval. The `MessageDispatcher` class takes care of dispatching both the commands and the queries.

This layer also contains the two implementations of `IBankingService` (one mocking the bank and another calling the API), `IEncryptionService` and `ICreatePaymentService`. This last service was created to encapsulate the communication with the bank and the dispatching of the command in one place.
#### API
The API layers just takes care of model validation and command and query dispatching.
## Deliverables
### Process a payment
This operation follows the following process:
 - Model validation in the `PaymentsController`
 - `PaymentsController` calls the `ICreatePaymentService`
 - The `ICreatePaymentService` submits a payment request to the bank and dispatches a `CreatePayment` command to the model.
 - The `EncryptCreatePayment` decorator gets executed and encrypts the card number in the command. Then the `IPaymentRepository` stores the aggregate in the database.
If the bank returns an error a `BankingException` is thrown.
The `CreatePayment` command gets dispatched even if the payment is not successful.
### Retrieve a payment
This operation follows the following process:
 - `PaymentsController` dispatches a `GetPaymentByBankingPaymentId` query.
 - After the handler retrieves the aggregate from the `IPaymentRepository` the decorators decrypt and mask the card number.
 - The result goes back to the controller and gets returned.
### Retrieve various payments
This operation wasn't mentioned in the requirements document but I thought it could be a nice feature.

The operation follows a very similar process to the one above, the only difference being handling multiple aggregates instead of one.
## Final details
The card number is encrypted and decrypted using RSA Asymmetric encryption. The RSA keys are in the `appsettings.Development.json` file although details like these should never make it the source control. I left them there as I thought the could add some clarity while reading.

The database connection string is not in the source control as I thought it didn't add any value.

Logging is implemented through the solution although is not configured. If I had time to configure logging I would just configure Serilog to use the `ILogger<>` abstraction and stream the logs to a database or a text file.

Authentication is not implemented because of time constraints. The original idea was to have an Identity Provider in the solution and have a JWT sent from the client to authorise the requests.

All the layers except the API have been unit tested to an extent. I didn't implement more unit tests because of time constraints.

The command/query messaging mechanism might be a bit overkill for a project this size, but I think it demonstrates the approach I would have to decouple a real system in a real project.

Originally I intended to have a `PaymentId ValueObject` as, under my understanding, that value would be in other bounded contexts. I stopped using this approach because Entity Framework Core would not accept a non-CLR type as a Primary Key and with automatic value generation. You can find the original implementation checking old versions of the project on git or checking [this experiment branch](https://github.com/odeibreton/CheckoutPaymentGateway/tree/experiment/ids-as-value-objects).
