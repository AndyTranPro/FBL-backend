
# FooBooLoo Game API

This is the back-end implementation for the FooBooLoo Game, a customizable FizzBuzz-like online game built using .NET 8 and PostgreSQL. Players can create custom games with unique rules and play against a timer, with the server handling the game logic.

## Features

- Create custom FizzBuzz-like games with user-defined rules.
- Persist game data in a PostgreSQL database.
- Start and manage game sessions.
- Generate unique random numbers and validate player answers.
- End session and receive appropriate feedback according to the score
- Dockerized setup for easy deployment.

## Technology Stack

- **.NET 8**: Back-end API implementation.
- **Entity Framework Core**: ORM for database interactions.
- **PostgreSQL**: Relational database for persisting game data.
- **Docker**: Containerization for deployment.
- **xUnit**: Unit testing framework.
- **Moq**: Mocking library for testing.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [PostgreSQL](https://www.postgresql.org/download/)
- [ReportGenerator](https://github.com/danielpalme/ReportGenerator) (for code coverage report)

### Setting Up the Project

**1. Clone the repository**
```bash
git clone https://github.com/yourusername/FooBooLooGameAPI.git
cd FooBooLooGameAPI
```
**2. Set up the database**

Make sure you have PostgreSQL running. Update the connection string in appsettings.json or provide it via environment variables.

**2. Apply Migrations**
```bash
dotnet ef database update
```

**3. Run the application**
```bash
dotnet run --project FooBooLooGameAPI.csproj
```

**4. Docker Setup**
- Build and run the Docker containers

    ```bash
    docker-compose up --build
    ```
- The API will be available at http://localhost:5000.

### API Endpoints
- Create a new game: **POST** `/api/games`
- Get all games: **GET** `/api/games`
- Start a new session: **POST** `/api/sessions`
- Get the next number in a session: **GET** `/api/sessions/{sessionId}/next-number`
- Submit an answer: **POST** `/api/sessions/{sessionId}/submit-answer`
- End a session: **POST** `/api/sessions/{sessionId}/end`
- Get a session result: **GET** `/api/sessions/{sessionId}/results`

## Testing
The project includes unit tests for the services and repositories.

- Running all the tests

    ```bash
    dotnet test FooBooLooGameAPI.Tests.csproj
    ```
- Running all tests and collecting code coverage

    ```bash
    dotnet test FooBooLooGameAPI.Tests.csproj --collect:"XPlat Code Coverage" --settings .\coverage.runsettings

    ```
- Generating the coverage report

    ```bash
    reportgenerator -reports:TestResults/*/coverage.cobertura.xml -targetdir:coverage-report
    ```
    The coverage report will be generated in the **coverage-report** directory.

## Deployment
The application can be easily deployed using Docker. You can push the Docker images to a container registry and deploy using your preferred orchestration tool (e.g., Kubernetes, Docker Swarm).

## License
This project is licensed under the MIT License.
