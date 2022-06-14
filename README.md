

## **Flipdish Coding Challenge**

> I'm sending some comments / code review. I'm sure there are a more things to be refractor / improved but I think these are a basis to a discuss on next interviews (if you think I should move forward to the next steps, obviously :) )

- Technology  - It would be best to choose `.Net Core` with `Kestrel` to host your application.
  - That way it can run over any OS, Containers, On Premises, Cloud provider agnostic.
  - Using `Azure Functions` it looks a vendor lock-in running server less.
- Transport - Using a `HTTP GET` doesn't look a best fit for this endpoint
  - Some web servers has limitations on `URL` max length.
  - If we really need to have a `HTTP GET` implements it on a distinct endpoint than `HTTP POST`
  - `HTTP POST` sending information from `body` should be a way to go
- Application Layers
  - We could have a better separation of concerns Transport - Business layer. Take for example: [Clean Architecture](https://github.com/ardalis/CleanArchitecture)
  - Our endpoint should just validate request and then send it to be handled by Business Layer
  - Personally, I usually adopt  [MediatR](https://github.com/jbogard/MediatR) to achieve this, taking advantage of  interceptors to implement profiling, logging, validation etc but there another ways to achieve that.
- Exception Handling
  - We should have a centralized point to implement a global exception handling
- Promote use of Dependency Injection to have a application architecture more decouple
- Security 
  - Add Authentication and Authorization 
    - Identity Server
    - RBAC
- Use configuration files to avoid hardcoded configs
  - Example: SettingsService
- Add API documentation (OpenApi spec)
  - [Swagger](https://swagger.io/)
- Observability
  - Add observability patterns such as:
    - Logging (structure)
    - Distributed Tracing
    - Profiling
    - Metrics
  - Create a dashboard to view all this information in a centralized way
- Cover solution with more tests
  - Unit
  - Integration
  - NFT  (load, security)
  - Other
- Always `await` awaitable operations to avoid `fire and forget` operations
  - Always `await` I/O operations.
- Fail fast: Methods must validate all parameters to ensure they are valid 
- There are SOLID Principles violated
  - Single Responsibility Principle
  - Open-Closed Principle
  - Dependency Inversion Principle
  - Interface Segregation Principle (lack of interfaces at all)
- Add resilience 
  - Retry, Timeout or Circuit Breaker are patterns to be used on interact with external systems
    - Example: Email Service
  - Throttling

- WebHooks vs Messaging

  - Why not evaluate Messaging (Even Driven  - Pub Sub Pattern) instead of WebHooks?
  - If our solution and clients are running inside our cluster, react to events instead of request response could lead to a decouple architecture.

  

  

  

  

  

  

  

  

  

  

  

  

  
