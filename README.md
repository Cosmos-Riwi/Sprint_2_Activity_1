# San Miguel Veterinary System

Veterinary management system developed in C# with Entity Framework Core and MySQL, allowing management of clients, pets, veterinarians, medical appointments, and medical history.

## Features

- Complete client management (CRUD)
- Complete pet management (CRUD)
- Complete medical history management (CRUD)
- Complete veterinarian management (CRUD)
- Complete medical appointment management (CRUD)
- Advanced queries with LINQ
- Intuitive console interface in Spanish
- MySQL database with Entity Framework Core
- OOP concepts application (Inheritance, Polymorphism, Encapsulation, Abstraction, Method Overloading)
- Entity relationships (1:1, 1:N, N:N)

## System Requirements

- .NET 8.0 SDK
- MySQL 8.0 or higher
- Visual Studio 2022 or VS Code (recommended)

## Installation

### 1. Clone the Repository
```bash
git clone <repository-url>
cd Sprint_2_Activity_1
```

### 2. Install MySQL

#### On Ubuntu/Debian:
```bash
sudo apt update
sudo apt install mysql-server
```

#### On Windows:
Download and install MySQL from: https://dev.mysql.com/downloads/mysql/

### 3. Configure the Database

Create the database using MySQL Workbench or command line:
```sql
CREATE DATABASE veterinaria_san_miguel_jeronimo;
```

### 4. Restore Dependencies
```bash
dotnet restore
```

### 5. Database Connection

The system is configured to connect to MySQL with the following credentials:
- Host: 168.119.183.3
- Port: 3307
- Database: veterinaria_san_miguel_jeronimo
- User: root
- Password: g0tIFJEQsKHm5$34Pxu1

### 6. Run the Application
```bash
dotnet run
```

## System Usage

### Main Menu
When you run the application, you'll see the following menu:

```
MENU PRINCIPAL
--------------

1. Gestionar Clientes
2. Gestionar Mascotas
3. Gestionar Historial Médico
4. Gestionar Veterinarios
5. Gestionar Citas Médicas
6. Consultas Avanzadas
7. Salir
```

### Features by Module

#### 1. Client Management
- Register new client
- List all clients
- Edit client information
- Delete client

#### 2. Pet Management
- Register new pet
- List all pets
- Edit pet information
- Delete pet

#### 3. Medical History Management
- Create medical history for pet
- View pet medical history
- Update medical history
- Track allergies and chronic conditions
- Record vaccinations

#### 4. Veterinarian Management
- Register new veterinarian
- List all veterinarians
- Edit veterinarian information
- Delete veterinarian

#### 5. Medical Appointment Management
- Register new medical appointment
- List all appointments
- Edit appointment information
- Delete appointment

#### 6. Advanced Queries
- Query pets of a specific client
- Find veterinarian with most appointments
- Identify most attended species
- Find client with most pets
- Complex LINQ queries

## Use Case Diagram

The following diagram shows the functional requirements of the system from the user's perspective:

![Use Case Diagram](Diagramas/DiagramaCasosDeUso.drawio.png)

This diagram illustrates all the use cases available in the system, including client management, pet management, veterinarian management, appointment management, and advanced queries. It shows the relationships between different use cases and how they interact with each other.

## System Architecture

### Folder Structure
```
Sprint_2_Activity_1/
├── Models/                 # Domain entities
│   ├── Client.cs
│   ├── Pet.cs
│   ├── MedicalHistory.cs
│   ├── Veterinarian.cs
│   └── Appointment.cs
├── Data/                   # Database context
│   └── VeterinaryDbContext.cs
├── Services/               # Business logic
│   ├── BaseService.cs
│   ├── ClientService.cs
│   ├── PetService.cs
│   ├── MedicalHistoryService.cs
│   ├── VeterinarianService.cs
│   ├── AppointmentService.cs
│   └── QueryService.cs
├── Diagramas/              # UML diagrams
│   ├── DiagramaCasosDeUso.drawio.png
│   ├── DiagramaClasesUML.drawio.png
│   └── DiagramaEntidadRelacion.drawio.png
├── Program.cs              # Entry point
├── Sprint_2_Activity_1.csproj  # Project file
├── JUSTIFICACION_POO.md    # OOP justification
└── README.md              # This file
```

### OOP Application

#### 1. Encapsulation
- Private attributes with public properties
- Controlled access methods
- Data protection and validation

#### 2. Inheritance
- `BaseService` as parent class
- Specific services inherit common functionalities
- Code reusability and consistency

#### 3. Polymorphism
- `ToString()` method overridden in each entity
- Virtual methods in base classes
- Different behaviors for same interface

#### 4. Abstraction
- Abstract base service class
- Hidden implementation complexity
- Simple interfaces for complex operations

#### 5. Method Overloading
- Multiple constructors in each class
- Different ways to create objects
- Flexible API design

## Class Diagram

The following UML class diagram shows the structure and relationships of the core components:

![Class Diagram](Diagramas/DiagramaClasesUML.drawio.png)

This diagram illustrates the domain models, their properties, methods, and the relationships between them. It shows the architectural separation into data and service layers, including the inheritance hierarchy from BaseService to specific service classes.

## Database

### Main Tables
- `Clients`: Pet owners information
- `Pets`: Animal information
- `MedicalHistories`: Pet medical records
- `Veterinarians`: Medical staff information
- `Appointments`: Medical consultation records

### Entity Relationships
- Client (1) → Pet (N) - One client can have many pets
- Pet (1) → MedicalHistory (1) - One pet has one medical history
- Pet (N) → Veterinarian (N) - Many pets can be treated by many veterinarians
- Pet (1) → Appointment (N) - One pet can have many appointments
- Veterinarian (1) → Appointment (N) - One veterinarian can perform many appointments

## Entity-Relationship Diagram

The following ER diagram shows the database schema design:

![Entity-Relationship Diagram](Diagramas/DiagramaEntidadRelacion.drawio.png)

This diagram illustrates the database structure with all entities, their attributes, data types, and relationships. It shows the primary keys, foreign keys, and the junction table for the many-to-many relationship between pets and veterinarians.

## Technologies Used

- **C# 8.0**: Programming language
- **Entity Framework Core 8.0**: ORM for data access
- **MySQL**: Relational database
- **Pomelo.EntityFrameworkCore.MySql**: MySQL provider for EF Core
- **LINQ**: Language Integrated Query for data operations

## Development Notes

### Data Validation
- Required fields validation
- Date format validation (dd/MM/yyyy)
- Decimal numbers for costs
- Referential integrity
- Email format validation

### Error Handling
- Try-catch blocks for database operations
- Descriptive error messages in Spanish
- User input validation
- Graceful handling of console input redirection

### User Interface
- Intuitive menu navigation
- Clear error and success messages
- User-friendly data entry prompts

## Project Structure

The project follows a clean architecture pattern with:
- **Models**: Domain entities with proper relationships
- **Data**: Database context and configuration
- **Services**: Business logic and CRUD operations
- **Program**: Application entry point and menu management

## OOP Concepts Implementation

This project demonstrates the application of Object-Oriented Programming principles:
- **Encapsulation**: Data protection and controlled access
- **Inheritance**: Code reusability through BaseService
- **Polimorphism**: Different behaviors for same interface
- **Abstraction**: Hidden complexity, simple interfaces
- **Method Overloading**: Multiple ways to create objects

## Database Configuration

The system connects to a MySQL database with the following configuration:
- Server: 168.119.183.3
- Port: 3307
- Database: veterinaria_san_miguel_jeronimo
- User: root
- Password: g0tIFJEQsKHm5$34Pxu1