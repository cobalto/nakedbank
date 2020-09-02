# Nakedbank 🍑
> A simple full stack banking application.

### Stack Composition
+ .Net Core 3.1 [link](https://dotnet.microsoft.com/)
+ Entity Framework Core [link](https://docs.microsoft.com/pt-br/ef/core/)
+ Blazor WebAssembly [link](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
+ Swagger [link](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio)
+ MySql [link](https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework-core.html)
+ Docker [link](https://docs.microsoft.com/en-us/dotnet/architecture/containerized-lifecycle/design-develop-containerized-apps/visual-studio-tools-for-docker)

### Third Party Libraries
+ AutoMapper [link](https://automapper.org/)
+ Coravel [link](https://docs.coravel.net/)
+ XUnix [link](https://xunit.net/)

> SPA Blazor app based on Jason Watmore's github examples [[github]](https://github.com/cornflourblue/blazor-webassembly-jwt-authentication-example) [[doc]](https://jasonwatmore.com/post/2020/08/13/blazor-webassembly-jwt-authentication-example-tutorial#app-route-view-cs)

### Project Composition
Project | Description
--- | ---
NakedBank.Application | Domain Service
NakedBank.Domain | Domain Models & Value Objects
NakedBank.Infrastructure | Data Layer, DbContext and Repositories
NakedBank.Shared | Common DTOs
NakedBank.WebApi | API Project
NakedBank.Front | Blazor SPA
NakedBank.Application.Tests | Test Solution
NakedBank.Domain.Tests | Test Solution

## Tests
![nakedbank_tests](https://user-images.githubusercontent.com/1196314/92039307-3f62ef80-ed4b-11ea-8acf-e1060d01fcc4.PNG)

### First time setup
With Docker installed, just open the solution on VS and run it on the default "Docker Compose" profile, the back-end should run without problems.

#### The project consists of three containers:
+ Dotnet Core image built with the project
+ MySql Official image
+ Adminer image for database management

![docker_naked](https://user-images.githubusercontent.com/1196314/91937111-a76ff200-ecc7-11ea-984c-62a756d74d73.PNG)

### Running the front-end:
According with the address set for the back-end, the Blazor front end will require to have it's appsettings.json changed accordly.
Find the file at ~\NakedBank.Front\wwwroot\appsettings.json and update it

### What's missing
+ Auth Token Recycling
+ "Transfer" Operations
+ User Profile Page
+ Parametrization of some "magic strings"
+ Better use of the Domain models, too much control on the services logic
+ Better control of some visual components when not logged in (Blazor Webassembly right now stil has some problems updating components when values change)
+ Charts (most of them are paid, Chart.js has a port to Blazor but only documentation for the Server-Side version)
+ More tests

## Open API
![open_api_nakedbank](https://user-images.githubusercontent.com/1196314/91937110-a76ff200-ecc7-11ea-9365-98d8f6ab2ba9.PNG)

## Front-End
![nakedbank_fron1](https://user-images.githubusercontent.com/1196314/91937098-a212a780-ecc7-11ea-8f8c-3e5c7dd5c9be.PNG)
![nakedbank_fron2](https://user-images.githubusercontent.com/1196314/91937103-a3dc6b00-ecc7-11ea-8f70-fd313e50d710.PNG)
![nakedbank_fron3](https://user-images.githubusercontent.com/1196314/91937105-a5a62e80-ecc7-11ea-9c3a-d6f5a4574b3a.PNG)
![nakedbank_fron4](https://user-images.githubusercontent.com/1196314/91937106-a63ec500-ecc7-11ea-9330-b92af01fb762.PNG)
![nakedbank_fron5](https://user-images.githubusercontent.com/1196314/91937108-a63ec500-ecc7-11ea-8145-e7d063701675.PNG)
![nakedbank_fron6](https://user-images.githubusercontent.com/1196314/91937109-a6d75b80-ecc7-11ea-9344-90ea971a3824.PNG)

> DISCLAIMER:  
> YES. The name is a [silly joke](https://blog.nubank.com.br/por-que-nubank-chama-nubank/) with a purple brazilian bank