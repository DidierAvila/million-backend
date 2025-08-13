# Million Real Estate Platform - Backend

## Descripción del proyecto

Million es una plataforma moderna de gestión inmobiliaria que permite administrar propiedades, propietarios, imágenes y trazabilidad de operaciones inmobiliarias. El backend está diseñado siguiendo principios de arquitectura limpia y buenas prácticas de desarrollo para garantizar escalabilidad, mantenibilidad y rendimiento.

## Tecnologías utilizadas

- **Lenguaje**: C# 12
- **Framework**: .NET 9
- **Base de datos**: MongoDB (NoSQL)
- **Autenticación**: JWT (JSON Web Tokens)
- **Almacenamiento de archivos**: AWS S3
- **Documentación API**: Swagger
- **Pruebas**: xUnit, Moq, FluentAssertions

## Arquitectura

El proyecto sigue una **arquitectura limpia** (Clean Architecture) con separación clara de responsabilidades, organizada en las siguientes capas:

### Estructura de capas

1. **Million.Domain**
   - Entidades de dominio (Owner, Property, PropertyImage, PropertyTrace, etc.)
   - Interfaces de repositorios
   - DTOs (Data Transfer Objects)
   - Validadores
   - Clases Value Object

2. **Million.Application**
   - Lógica de negocio
   - Implementación de casos de uso
   - Interfaces de servicios
   - Implementación de servicios
   - Mapeos con AutoMapper
   - Patrón CQRS (Command Query Responsibility Segregation)

3. **Million.Infrastructure**
   - Implementación de repositorios
   - Contextos de base de datos MongoDB
   - Configuraciones
   - Servicios de infraestructura
   - Migraciones de MongoDB

4. **Million.API**
   - Controladores API REST
   - Configuración de middleware
   - Configuración de servicios
   - Gestión de autenticación y autorización
   - Documentación Swagger

### Diagrama de capas

```
┌─────────────────┐         ┌─────────────────┐
│    Million.API  │ ──────► │Million.Application│
└────────┬────────┘         └────────┬─────────┘
         │                           │
         │                           │
         │                           │
         │                  ┌────────▼─────────┐
         └─────────────────►│  Million.Domain  │
                            └──────────────────┘
                                     ▲
                                     │
                            ┌────────┴─────────┐
                            │Million.Infrastructure│
                            └──────────────────┘
                                     ▲
                                     │
                            ┌────────┴─────────┐
                            │Million.Application│
                            └──────────────────┘
```

Este diagrama muestra la dependencia de Million.Application hacia Million.Infrastructure.

## Patrones de diseño implementados

### 1. Repository Pattern

Implementado a través de interfaces como `IOwnerRepository`, `IPropertyRepository`, etc., y sus respectivas implementaciones en la capa de infraestructura. Este patrón proporciona una abstracción de la capa de persistencia y facilita la sustitución de tecnologías de base de datos.

```csharp
public interface IOwnerRepository
{
    Task<Owner> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<IEnumerable<Owner>> GetAllAsync(CancellationToken cancellationToken);
    Task<Owner> AddAsync(Owner owner, CancellationToken cancellationToken);
    // Otros métodos...
}
```

### 2. CQRS (Command Query Responsibility Segregation)

Separación de operaciones de lectura (queries) y escritura (commands) para optimizar el rendimiento y la escalabilidad:

- **Commands**: Manejan operaciones que modifican el estado (crear, actualizar, eliminar).
- **Queries**: Manejan operaciones de solo lectura.

```csharp
// Command Handler
public class OwnerCommandHandler : IOwnerCommandHandler
{
    public async Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createDto, CancellationToken cancellationToken)
    {
        // Implementación...
    }
}

// Query Handler
public class OwnerQueryHandler : IOwnerQueryHandler
{
    public async Task<OwnerDto> GetOwnerByIdAsync(string id, CancellationToken cancellationToken)
    {
        // Implementación...
    }
}
```

### 3. Facade Pattern

Implementado para proporcionar una interfaz simplificada a los controladores:

```csharp
public class OwnerFacade
{
    private readonly IOwnerCommandHandler _commandHandler;
    private readonly IOwnerQueryHandler _queryHandler;
    
    // Constructor y métodos...
}
```

### 4. Dependency Injection

Utilización extensiva de inyección de dependencias para desacoplar componentes y facilitar pruebas:

```csharp
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IOwnerCommandHandler, OwnerCommandHandler>();
builder.Services.AddScoped<IOwnerQueryHandler, OwnerQueryHandler>();
```

### 5. Specification Pattern

Utilizado para encapsular reglas de negocio y consultas:

```csharp
public Expression<Func<Owner, bool>> NameContains(string name)
{
    return owner => owner.Name.ToLower().Contains(name.ToLower());
}
```

### 6. Unit of Work

Gestión consistente de transacciones y operaciones de base de datos.

## Principios SOLID aplicados

### 1. Single Responsibility Principle (SRP)

Cada clase tiene una única responsabilidad claramente definida:
- Repositorios: Acceso a datos
- Command Handlers: Procesamiento de comandos
- Query Handlers: Procesamiento de consultas
- Controladores: Gestión de endpoints HTTP

### 2. Open/Closed Principle (OCP)

El diseño está abierto a extensión pero cerrado a modificación. Por ejemplo, agregar nuevas funcionalidades para propiedades no requiere modificar el código existente para propietarios.

### 3. Liskov Substitution Principle (LSP)

Las implementaciones concretas pueden sustituir a sus interfaces base sin alterar el comportamiento. Por ejemplo, todas las implementaciones de repositorios siguen el mismo contrato definido por sus interfaces.

### 4. Interface Segregation Principle (ISP)

Las interfaces están bien definidas y específicas para cada necesidad:
- `IOwnerCommandHandler` para operaciones de escritura
- `IOwnerQueryHandler` para operaciones de lectura

### 5. Dependency Inversion Principle (DIP)

El código depende de abstracciones, no de implementaciones concretas:
- Los controladores dependen de interfaces de fachada
- Los handlers dependen de interfaces de repositorio
- Los repositorios dependen de interfaces de contexto de base de datos

## Funcionalidades clave

### Autenticación y Autorización
- Sistema basado en JWT (JSON Web Tokens)
- Roles: Admin, User
- Endpoints protegidos con atributos `[Authorize]`

### Gestión de Imágenes
- Almacenamiento en AWS S3
- Manejo eficiente de subida, actualización y eliminación de imágenes
- Validación de tipos y tamaños de archivos

### Migraciones de MongoDB
- Sistema personalizado para manejo de versiones de esquemas
- Soporte para migraciones hacia arriba y hacia abajo
- Registro de historial de migraciones

#### Funcionamiento del sistema de migraciones

Million implementa un sistema de migraciones personalizado para MongoDB que permite evolucionar el esquema de la base de datos de manera controlada y versionada, similar a los sistemas de migración en bases de datos relacionales.

**Componentes principales:**

1. **Interfaz IMigration**: Define el contrato que deben implementar todas las migraciones:
   - `Version`: Número entero único que identifica la versión de la migración
   - `Name`: Nombre descriptivo de la migración
   - `Description`: Descripción detallada de los cambios
   - `UpAsync()`: Método para aplicar la migración
   - `DownAsync()`: Método para revertir la migración

2. **Clase Migration**: Entidad que registra las migraciones aplicadas en MongoDB:
   - Almacena información sobre cada migración (versión, nombre, descripción)
   - Registra cuándo se aplicó y si fue exitosa

3. **MigrationService**: Servicio central que gestiona la ejecución de migraciones:
   - Detecta migraciones pendientes comparando con el registro histórico
   - Ejecuta migraciones en orden ascendente de versión
   - Permite revertir migraciones hasta una versión específica
   - Registra cada operación en la colección `migrations`

**Flujo de trabajo:**

1. **Crear una nueva migración**:
   - Implementar la interfaz `IMigration` con una nueva clase
   - Definir un número de versión único y secuencial
   - Implementar la lógica para aplicar (`UpAsync`) y revertir (`DownAsync`) cambios

2. **Registrar la migración**:
   - Añadir la nueva migración en `MigrationExtensions.cs` usando `services.AddScoped<IMigration, NuevaMigration>()`

3. **Ejecutar migraciones**:
   - Las migraciones se pueden ejecutar mediante el controlador `MigrationsController`
   - Endpoint `POST /api/Migrations/up` para aplicar migraciones pendientes
   - Endpoint `POST /api/Migrations/down/{version}` para revertir hasta una versión específica

**Ejemplo de migración:**

```csharp
public class UpdateOwnerPhotosToStrings : IMigration
{
    // Propiedades de la migración
    public int Version => 1;
    public string Name => "Update_Owner_Photos_To_Strings";
    public string Description => "Convierte el campo photo de byte[] a string para almacenar URLs de S3";
    
    // Métodos para aplicar y revertir
    public async Task<bool> UpAsync(CancellationToken cancellationToken = default)
    {
        // Lógica para actualizar de byte[] a string
    }
    
    public async Task<bool> DownAsync(CancellationToken cancellationToken = default)
    {
        // Lógica para revertir de string a byte[]
    }
}
```

Esta implementación permite mantener la base de datos MongoDB sincronizada con los cambios en el modelo de datos, proporcionando un historial claro de modificaciones y la capacidad de revertir cambios si es necesario.

## Configuración y despliegue

### Requisitos
- .NET 9 SDK
- MongoDB (local o en la nube)
- Cuenta de AWS para almacenamiento S3 (opcional, solo para imágenes)

### Configuración

1. **Base de datos**

   En `appsettings.json` configurar la conexión a MongoDB:
   ```json
   "MongoDbSettings": {
     "ConnectionString": "mongodb://localhost:27017",
     "DatabaseName": "million"
   }
   ```

2. **AWS S3 (opcional)**

   En `appsettings.json` configurar credenciales de AWS:
   ```json
   "AWS": {
     "Region": "us-east-1",
     "AccessKey": "YOUR_ACCESS_KEY",
     "SecretKey": "YOUR_SECRET_KEY",
     "S3": {
       "BucketName": "million-property-images"
     }
   }
   ```

3. **JWT**

   Configurar la clave de JWT:
   ```json
   "JwtSettings": {
     "key": "YOUR_SECRET_KEY",
     "Issuer": "million-api",
     "Audience": "million-clients"
   }
   ```

### Ejecución

```bash
# Restaurar dependencias
dotnet restore

# Ejecutar migraciones (mediante API)
# POST /api/Migrations/up

# Iniciar la aplicación
dotnet run --project Million.API
```

## Pruebas

El proyecto incluye pruebas unitarias exhaustivas utilizando xUnit, Moq y FluentAssertions:

```bash
# Ejecutar todas las pruebas
dotnet test

# Ejecutar pruebas específicas
dotnet test --filter "Category=Integration"
```

## Contribución

1. Hacer un fork del repositorio
2. Crear una rama para tu funcionalidad (`git checkout -b feature/amazing-feature`)
3. Hacer commit de tus cambios (`git commit -m 'Add some amazing feature'`)
4. Hacer push a la rama (`git push origin feature/amazing-feature`)
5. Abrir un Pull Request

## Autor

- **Didier Avila** - *Desarrollador Principal* - [DidierAvila](https://github.com/DidierAvila)

## Licencia

Este proyecto está licenciado bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.