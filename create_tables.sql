USE JuanManuel_veterinary_system;

-- Eliminar las tablas existentes
DROP TABLE IF EXISTS Atenciones;
DROP TABLE IF EXISTS Mascotas;
DROP TABLE IF EXISTS Veterinarios;
DROP TABLE IF EXISTS Clientes;

-- Crear tabla Clientes
CREATE TABLE Clientes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombres VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20),
    Email VARCHAR(100)
);

-- Crear tabla Veterinarios
CREATE TABLE Veterinarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombres VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    Especialidad VARCHAR(100),
    Telefono VARCHAR(20),
    Email VARCHAR(100)
);

-- Crear tabla Mascotas
CREATE TABLE Mascotas (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Especie VARCHAR(50) NOT NULL,
    Raza VARCHAR(50),
    FechaNacimiento DATE,
    ClienteId INT,
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id) ON DELETE CASCADE
);

-- Crear tabla Atenciones
CREATE TABLE Atenciones (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Fecha DATETIME NOT NULL,
    Diagnostico TEXT,
    MascotaId INT,
    VeterinarioId INT,
    FOREIGN KEY (MascotaId) REFERENCES Mascotas(Id) ON DELETE CASCADE,
    FOREIGN KEY (VeterinarioId) REFERENCES Veterinarios(Id) ON DELETE CASCADE
);

-- Insertar datos como ejemplos
INSERT INTO Clientes (Nombres, Apellidos, Telefono, Email) VALUES
('Juan', 'Pérez', '555-0101', 'juan.perez@email.com'),
('María', 'García', '555-0102', 'maria.garcia@email.com'),
('Carlos', 'López', '555-0103', 'carlos.lopez@email.com');

INSERT INTO Veterinarios (Nombres, Apellidos, Especialidad, Telefono, Email) VALUES
('Dr. Ana', 'Martínez', 'Medicina General', '555-0201', 'ana.martinez@veterinaria.com'),
('Dr. Luis', 'Rodríguez', 'Cirugía', '555-0202', 'luis.rodriguez@veterinaria.com'),
('Dra. Carmen', 'Fernández', 'Dermatología', '555-0203', 'carmen.fernandez@veterinaria.com');

INSERT INTO Mascotas (Nombre, Especie, Raza, FechaNacimiento, ClienteId) VALUES
('Max', 'Perro', 'Labrador', '2020-03-15', 1),
('Luna', 'Gato', 'Persa', '2019-07-22', 1),
('Rex', 'Perro', 'Pastor Alemán', '2021-01-10', 2),
('Mimi', 'Gato', 'Siamés', '2020-11-05', 3);

INSERT INTO Atenciones (Fecha, Diagnostico, MascotaId, VeterinarioId) VALUES
('2024-01-15 10:30:00', 'Vacunación anual y revisión general', 1, 1),
('2024-01-20 14:15:00', 'Control de peso y dieta', 2, 1),
('2024-02-01 09:00:00', 'Cirugía menor - extracción de tumor', 3, 2),
('2024-02-10 16:45:00', 'Tratamiento dermatológico', 4, 3);
