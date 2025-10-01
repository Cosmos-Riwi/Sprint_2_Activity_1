# Sistema Veterinaria San Miguel

## ¿Qué es?
Sistema de consola en C# para gestionar una veterinaria. Permite registrar clientes, sus mascotas, veterinarios y las consultas médicas realizadas. Todo se guarda en una base de datos MySQL.

## ¿Cómo funciona?

### Menú Principal
Al ejecutar el programa aparece un menú con estas opciones:

```
==== Veterinaria San Miguel ====
1. Gestión de Clientes
2. Gestión de Mascotas  
3. Gestión de Veterinarios
4. Gestión de Atenciones Médicas
5. Historial Médico por Mascota
6. Consultas Avanzadas
0. Salir
```

### Funcionalidades Principales

#### 1. **Gestión de Clientes**
- **Registrar**: Agregar nuevos clientes con nombre, apellido, teléfono y email
- **Listar**: Ver todos los clientes registrados
- **Editar**: Modificar datos de clientes existentes
- **Eliminar**: Borrar clientes del sistema

**Ejemplo de uso:**
```
-- Clientes --
1. Registrar
2. Listar
3. Editar
4. Eliminar
0. Volver
```

#### 2. **Gestión de Mascotas**
- **Registrar**: Agregar mascotas asignándolas a un cliente
- **Listar**: Ver todas las mascotas con su dueño
- **Editar**: Cambiar datos de mascotas
- **Eliminar**: Quitar mascotas del sistema

**Ejemplo de registro:**
```
Nombre de la mascota: Max
Especie: 1 (Perro)
Raza: Labrador
Fecha de nacimiento: 2020-03-15
ID del cliente dueño: 1
```

#### 3. **Gestión de Veterinarios**
- **Registrar**: Agregar veterinarios con especialidad
- **Listar**: Ver todos los veterinarios
- **Editar**: Modificar datos de veterinarios
- **Eliminar**: Quitar veterinarios del sistema

#### 4. **Atenciones Médicas**
- **Registrar**: Crear consultas médicas con diagnóstico
- **Listar**: Ver todas las atenciones realizadas
- **Editar**: Modificar consultas existentes
- **Eliminar**: Borrar atenciones del sistema

**Ejemplo de atención:**
```
Fecha: 2023-10-26 10:00:00
Diagnóstico: Vacunación anual
Mascota: Max (ID: 1)
Veterinario: Dr. Ana Martínez (ID: 1)
```

#### 5. **Historial Médico**
Muestra todas las consultas de una mascota específica:
```
--- Historial Médico de Max ---
Dueño: Juan Pérez

Atenciones médicas:
Fecha: 2023-10-26
Veterinario: Dr. Ana Martínez
Diagnóstico: Vacunación anual
```

#### 6. **Consultas Avanzadas**
Reportes estadísticos del sistema:
- Mascotas de un cliente específico
- Veterinario con más atenciones
- Especie más atendida en la clínica
- Cliente con más mascotas

## Instalación y Configuración

### 1. Requisitos
- .NET 8.0
- MySQL Server
- Visual Studio Code o Visual Studio

### 2. Instalación
```bash
# Restaurar paquetes
dotnet restore

# Compilar proyecto
dotnet build

# Ejecutar programa
dotnet run
```

### 3. Base de Datos
Ejecutar el archivo `create_tables.sql` en MySQL para crear las tablas.

## Credenciales de Conexión
- **Servidor**: 168.119.183.3
- **Base de datos**: JuanManuel_veterinary_system
- **Usuario**: root
- **Contraseña**: g0tIFJEQsKHm5$34Pxu1
- **Puerto**: 3307

## Justificación de cómo se aplicó POO

### 1. **Encapsulación**
Cada entidad está en su propia clase con propiedades controladas:

```csharp
public class Cliente
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Nombres { get; set; } = string.Empty;
    public string? Email { get; set; }
}
```
- **Validaciones**: Uso de `[Required]` y `[MaxLength]` para validar datos
- **Acceso controlado**: Propiedades públicas para lectura/escritura segura

### 2. **Herencia**
- **Entity Framework**: Todas las entidades usan el sistema de EF Core
- **Interfaces**: Uso de `ICollection<T>` para las relaciones
- **Atributos**: Herencia de `System.ComponentModel.DataAnnotations`

### 3. **Polimorfismo**
- **Sobrecarga de métodos**: Diferentes formas de validar y consultar datos
- **Enums**: Uso de `Especie` para diferentes tipos de mascotas:

```csharp
public enum Especie
{
    Perro = 1, Gato = 2, Ave = 3, Roedor = 4, Reptil = 5, Otro = 99
}
```

### 4. **Relaciones entre Objetos**
Implementación de relaciones uno-a-muchos:

```csharp
// Un cliente puede tener muchas mascotas
public class Cliente
{
    public ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
}

// Una mascota puede tener muchas atenciones
public class Mascota
{
    public ICollection<Atencion> Atenciones { get; set; } = new List<Atencion>();
}
```

### 5. **Sobrecarga de Métodos**
- **Múltiples formas de validar**: Diferentes métodos para validar datos
- **Consultas variadas**: Diferentes formas de buscar información
- **Presentación flexible**: Múltiples formatos de mostrar datos

### 6. **Principios SOLID**

#### Single Responsibility (SRP)
- `Cliente` → Solo maneja datos de clientes
- `Mascota` → Solo maneja datos de mascotas  
- `Veterinario` → Solo maneja datos de veterinarios
- `Atencion` → Solo maneja datos de consultas

#### Open/Closed (OCP)
- Las clases están abiertas para extensión (nuevas propiedades)
- Cerradas para modificación (estructura base estable)

### 7. **Manejo de Excepciones**
```csharp
try
{
    Db.SaveChanges();
    Console.WriteLine("Operación exitosa.");
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}
```

### 8. **LINQ para Consultas Avanzadas**
Uso de programación orientada a objetos en consultas:

```csharp
// Encontrar veterinario con más atenciones
var veterinarioMasAtenciones = Db.Atenciones
    .GroupBy(a => a.Veterinario)
    .Select(g => new { Veterinario = g.Key, Count = g.Count() })
    .OrderByDescending(x => x.Count)
    .FirstOrDefault();
```

## **Estructura del Proyecto**
```
Veterinaria/
├── Data/
│   └── VeterinariaContext.cs 
├── Models/                        # Entidades del sistema
│   ├── Cliente.cs
│   ├── Mascota.cs
│   ├── Veterinario.cs
│   └── Atencion.cs
├── Program.cs                     # Lógica principal
├── create_tables.sql              # Script de base de datos
└── README.md                      # Este archivo
```

## **Ejemplos de Uso**

### *Registrar un Cliente*
```
Nombres: Juan
Apellidos: Pérez
Email: juan@email.com
Teléfono: 555-0101
```

### *Registrar una Mascota*
```
Nombre: Max
Especie: 1 (Perro)
Raza: Labrador
Fecha nacimiento: 2020-03-15
Cliente dueño: 1
```

### *Crear una Atención Médica*
```
Fecha: 2023-10-26 10:00:00
Diagnóstico: Vacunación anual
Mascota: Max (ID: 1)
Veterinario: Dr. Ana (ID: 1)
```
