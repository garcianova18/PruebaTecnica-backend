# PruebaTecnica Backend

API REST desarrollada en **.NET 9** siguiendo los principios de **Clean Architecture** y **CQRS** (con MediatR). El proyecto expone endpoints de autenticación con JWT, un CRUD de productos protegido por roles, y un endpoint de clima que consume APIs externas (Open-Meteo).

## Tabla de contenidos

- [Arquitectura](#arquitectura)
- [Tecnologías y paquetes](#tecnologías-y-paquetes)
- [Requisitos previos](#requisitos-previos)
- [Configuración](#configuración)
- [Ejecución del proyecto](#ejecución-del-proyecto)
- [Usuario semilla (seed)](#usuario-semilla-seed)
- [Endpoints disponibles](#endpoints-disponibles)
- [Formato de respuesta](#formato-de-respuesta)
- [Manejo de errores](#manejo-de-errores)

## Arquitectura

El proyecto está organizado en 4 capas siguiendo **Clean Architecture**:

```

PruebaTecnica.Api│ Controllers, Middleware, Program.cs

PruebaTecnica.Application  │ CQRS (MediatR), DTOs, Validadores, Contratos

PruebaTecnica.Infrastructure │ EF Core, Repositorios, JWT, Clientes externos

PruebaTecnica.Domain │ Entidades del dominio

```

- **Domain**: entidades base del negocio (`Product`, `User`, `Role`), sin dependencias externas.
- **Application**: casos de uso implementados con el patrón **CQRS** (Commands/Queries + Handlers) usando **MediatR**, validaciones con **FluentValidation**, mapeos con **AutoMapper** y contratos (interfaces) de repositorios y servicios.
- **Infrastructure**: implementación de persistencia con **Entity Framework Core** (SQL Server), repositorios, servicio de hashing de contraseñas (**BCrypt**), generación de JWT y clientes HTTP hacia APIs externas de clima/geocodificación.
- **Api**: capa de presentación con los controllers, middleware de manejo global de excepciones, configuración de Swagger, CORS y autenticación/autorización.

## Tecnologías y paquetes

| Tecnología | Uso |
|---|---|
| .NET 9 | Framework principal |
| ASP.NET Core Web API | Exposición de endpoints REST |
| Entity Framework Core 8 (SQL Server) | Acceso a datos / ORM |
| MediatR | Implementación del patrón CQRS |
| FluentValidation | Validación de comandos/DTOs |
| AutoMapper | Mapeo entre entidades y DTOs |
| JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer) | Autenticación basada en tokens |
| BCrypt.Net-Next | Hashing de contraseñas |
| Swashbuckle (Swagger) | Documentación interactiva de la API |
| Open-Meteo API | Consulta de coordenadas (geocoding) y clima |

## Requisitos previos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server o [SQL Server LocalDB](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb) (usado por defecto en `appsettings.json`)
- Un IDE compatible (Visual Studio 2022+, JetBrains Rider o VS Code)

## Configuración

La configuración principal se encuentra en `src/PruebaTecnica.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PruebaTecnicaDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "JwtSettings": {
    "Secret": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "PruebaTecnica.Api",
    "Audience": "PruebaTecnica.Client",
    "ExpiryMinutes": 60
  },
  "WeatherApiSettings": {
    "BaseAddress": "https://api.open-meteo.com/v1/"
  },
  "GeocodingApiSettings": {
    "BaseAddress": "https://geocoding-api.open-meteo.com/v1/"
  }
}
```

> ⚠️ **Importante**: antes de desplegar a un ambiente real, reemplaza `JwtSettings:Secret` por un valor seguro y gestiona la cadena de conexión mediante variables de entorno o `dotnet user-secrets`, en lugar de dejarla en el archivo versionado.

Para desarrollo local puedes usar `dotnet user-secrets` (el proyecto Api ya tiene configurado un `UserSecretsId`):

```bash
cd src/PruebaTecnica.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "tu-cadena-de-conexion"
dotnet user-secrets set "JwtSettings:Secret" "tu-secreto-seguro"
```

## Ejecución del proyecto

1. Clonar el repositorio:

   ```bash
   git clone https://github.com/garcianova18/PruebaTecnica-backend.git
   cd PruebaTecnica-backend
   ```

2. Restaurar dependencias:

   ```bash
   dotnet restore
   ```

3. Ejecutar la API (las migraciones de base de datos y el seed de datos se aplican automáticamente al iniciar la aplicación):

   ```bash
   dotnet run --project src/PruebaTecnica.Api
   ```

4. Abrir Swagger para explorar y probar los endpoints (disponible en ambiente de desarrollo):

   ```
   https://localhost:{puerto}/swagger
   ```

   El puerto se define en `src/PruebaTecnica.Api/Properties/launchSettings.json`.

## Usuario semilla (seed)

Al iniciar la aplicación, `SeedDataService` crea automáticamente:

- Un rol `admin`.
- Un usuario administrador con las siguientes credenciales por defecto:

| Campo | Valor |
|---|---|
| Usuario | `admin` |
| Contraseña | `admin` |
| Email | `admin@pruebatecnica.com` |

> Se recomienda cambiar esta contraseña en cualquier ambiente distinto al local.

## Endpoints disponibles

### Autenticación (`/api/auth`)

| Método | Ruta | Descripción | Autenticación |
|---|---|---|---|
| POST | `/api/auth/register` | Registra un nuevo usuario | No |
| POST | `/api/auth/login` | Inicia sesión y devuelve un token JWT | No |
| POST | `/api/auth/refresh-token` | Renueva el token de acceso a partir de un refresh token | No |

### Productos (`/api/Product`)

| Método | Ruta | Descripción | Autenticación |
|---|---|---|---|
| GET | `/api/Product` | Lista productos paginados (`PageIndex`, `PageSize`, `SearchTerm`) | No |
| GET | `/api/Product/{id}` | Obtiene un producto por id | No |
| POST | `/api/Product` | Crea un producto | Sí — rol `admin` |
| PUT | `/api/Product/{id}` | Actualiza un producto | Sí — rol `admin` |
| DELETE | `/api/Product/{id}` | Elimina un producto | Sí — rol `admin` |

### Clima (`/api/weather`)

| Método | Ruta | Descripción | Autenticación |
|---|---|---|---|
| GET | `/api/weather/{city}` | Devuelve el clima actual de una ciudad (usa Open-Meteo para geocodificar y consultar el clima) | No |

### Autenticación con JWT

Para acceder a los endpoints protegidos, envía el token obtenido en `login` o `register` en el header:

```
Authorization: Bearer {token}
```

## Formato de respuesta

Todas las respuestas de la API siguen un formato estándar (`ApiResponse<T>`):

```json
{
  "isSuccess": true,
  "data": { },
  "statusCode": 200
}
```

En caso de error:

```json
{
  "isSuccess": false,
  "error": "Mensaje descriptivo del error",
  "statusCode": 400
}
```

## Manejo de errores

El proyecto cuenta con un middleware global (`GlobalExceptionHandler`) que captura las excepciones de la aplicación (`BadRequestException`, `NotFoundException`, `UnauthorizedException`, `ValidationException`, entre otras) y las traduce automáticamente al formato `ApiResponse<T>` con el código de estado HTTP correspondiente.
