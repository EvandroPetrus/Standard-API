# .NET 8 API Standard Structure and Guidelines

This repository provides a structured .NET 8 API utilizing **Docker**, **Domain-Driven Design (DDD)** and **RabbitMQ**, whilst following SOLID and CCode fundamentals. It also uses the repository pattern, serving as a foundational template for building scalable, maintainable, and modular systems.

## Table of Contents

- [Technologies Used](#technologies-used)
- [Architecture Overview](#architecture-overview)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Running the Project](#running-the-project)
- [Project Structure](#project-structure)
- [Domain-Driven Design (DDD)](#domain-driven-design-ddd)
- [Message Queue Integration (RabbitMQ)](#message-queue-integration-rabbitmq)
- [Docker Integration](#docker-integration)
- [Contributing](#contributing)
- [License](#license)

## Technologies Used

- **.NET 8**
- **Docker**
- **RabbitMQ**
- **Domain-Driven Design (DDD)**
- **Entity Framework Core**
- **SQL Server** (or any other RDBMS of choice)
- **Redis** (optional, for caching)
- **ELK stack / Elastic Stack** (ElasticSearch, Logstash and Kibana, for logging and better debugging)

## Architecture Overview

This API follows the principles of **Domain-Driven Design (DDD)**, splitting the project into distinct layers:
- **Domain**: Contains core business logic and domain models.
- **Application**: Manages use cases and application workflows.
- **Infrastructure**: Handles communication with external systems (e.g., database, RabbitMQ, third-party services).
- **API**: Exposes RESTful endpoints for client interaction.

RabbitMQ is integrated to facilitate **Pub/Sub messaging** and **event-driven architecture**, supporting distributed and decoupled systems.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [RabbitMQ](https://www.rabbitmq.com/download.html) (Docker container recommended)
- SQL Server (or preferred database)
- [MongoDB](https://www.mongodb.com/try/download/community) (or an Atlas cluster)
  
### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/EvandroPetrus/Standard-API.git
   cd Standard-API
   ```

2. Build the Docker containers:
   ```bash
   docker-compose build
   ```

3. Start the services:
   ```bash
   docker-compose up
   ```

### Running the Project
`Database will be auto-generated.`
1. Start the API:
   ```bash
   dotnet run
   ```

2. The API will be running at `http://localhost:5000`.

## Project Structure

```bash
src/
│
├── Api/
│   ├── Controllers/
│   ├── Exceptions/
│   ├── Extensions/
│   └── Middlewares/
│
├── Application/
│   ├── Interfaces/
│   └── Services/
│
├── Domain/
│   ├── Entities/
│   ├── ValueObjects/
│   ├── DomainEvents/
│   ├── DTOs/
│   │   ├── Request/
│   │   └── Response/
│   ├── Interfaces/
│   │   ├── Repositories/
│   │   └── Services/
│   └── Validator/
│
├── Infrastructure/
│   ├── AutoMapper/
│   ├── Repositories/
│   ├── Messaging/    # RabbitMQ integrations
│   ├── Persistence/  # Database configurations
│   │   ├── SQL/
│   │   │   ├── Configurations/
│   │   │   └── Migrations/
│   │   └── NoSQL/
│   │       └── Persistences/
│
├── Service/
│   └── Services/
│
└── Docker/
    ├── docker-compose.yml
    └── Dockerfile

```

## Domain-Driven Design (DDD)

The application is built following **Domain-Driven Design** principles:

- **Entities**: Represent the core business objects with identities.
- **Value Objects**: Immutable objects that are defined by their properties.
- **Aggregates**: Cluster of domain objects treated as a single unit.
- **Repositories**: Handle data persistence for aggregates.
- **Services**: Perform business operations that don’t naturally fit within aggregates.

## Message Queue Integration (RabbitMQ)

RabbitMQ is utilized for **event-driven communication** between microservices, handling asynchronous messaging. The system implements:

- **Producers**: Responsible for publishing domain events to queues.
- **Consumers**: Services that subscribe and listen to the events from RabbitMQ.

### RabbitMQ Docker Setup

The `docker-compose.yml` file includes a RabbitMQ service. To start RabbitMQ along with the API, simply run:

```bash
docker-compose up
```

You can access the RabbitMQ management UI at `http://localhost:15672` (default login: guest/guest).

## Docker Integration

Docker is used to simplify the development and deployment processes. The `docker-compose.yml` file orchestrates the API, RabbitMQ, and database containers.

### Build and Run with Docker

1. Build Docker images:
   ```bash
   docker-compose build
   ```

2. Run the containers:
   ```bash
   docker-compose up
   ```

## Contributing

Contributions are welcome! Please submit a pull request or open an issue to discuss your ideas.

## License

This project is licensed under the GPL 3.0 License.
