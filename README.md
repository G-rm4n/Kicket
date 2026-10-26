# Kicket

Sistema de venta de entradas para espectáculos deportivos. Una API REST concentra la lógica de negocio y dos clientes la consumen: una aplicación de escritorio (Windows Forms) y una web (Blazor).

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server LocalDB**, que viene incluido con Visual Studio. La API está configurada para usar `(localdb)\MSSQLLocalDB` (ver la nota sobre la base de datos más abajo).
- Windows para la app de escritorio (`net8.0-windows`). La API y la web Blazor corren en cualquier sistema.

## Estructura

| Proyecto | Qué es |
|---|---|
| `Domain` | Entidades (Club, Estadio, Evento, Sector, Compra, Entrada, Usuario) |
| `Data` | Acceso a datos: `TPIContext` (EF Core) y repositorios |
| `Core` | Lógica de negocio: servicios, JWT, hash de contraseñas, Mercado Pago |
| `Kicket.Contracts` | DTOs y requests compartidos entre la API y los clientes |
| `WebApi` | API REST (minimal APIs) con Swagger |
| `Kicket.ApiClient` | Cliente HTTP de la API, usado por WinForms y Blazor |
| `Kicket.WinForms` | Cliente de escritorio |
| `Kicket.Blazor` | Cliente web |

## Base de datos

La API usa SQL Server y **crea sola la base `kicket`** (con tablas y datos de prueba) la primera vez que se ejecuta. No hace falta crear nada a mano.

Por defecto se conecta a **LocalDB** (`(localdb)\MSSQLLocalDB`), que se instala junto con Visual Studio. Si se prefiere usar **SQL Server Express**, hay que cambiar la cadena de conexión en `WebApi/appsettings.json`, clave `ConnectionStrings:Local`:

```
Server=.\SQLEXPRESS;Database=kicket;Trusted_Connection=True;TrustServerCertificate=True;
```

Para cualquier otra instancia de SQL Server, reemplazar el valor de `Server=` por el nombre de esa instancia.

## Cómo ejecutarlo

1. **Levantar la API primero.** Los dos clientes la esperan en `http://localhost:5268/`.

   ```
   dotnet run --project WebApi
   ```

   Al iniciar crea la base `kicket` si no existe y la carga con datos de prueba. Swagger queda en `http://localhost:5268/swagger`.

2. **Levantar un cliente** (en otra terminal):

   ```
   dotnet run --project Kicket.Blazor     # web, http://localhost:5139
   dotnet run --project Kicket.WinForms   # escritorio
   ```

En Visual Studio también se puede abrir `Kicket.sln` y usar el perfil de inicio múltiple **Interfaz+servicios**.

Si la API corre en otro puerto, hay que cambiar `BaseUrl` en `Kicket.WinForms/Program.cs` y en `Kicket.Blazor/Program.cs`.

## Datos de prueba

Se cargan solos al crear la base:

- Clubes: Newell's Old Boys, Rosario Central
- Estadio: Coloso del Parque (Rosario), con los sectores Platea Este y Popular Sur
- Evento: Clásico Rosarino (15/12/2027)
- Usuario administrador: `EmailRaro@gmail.com` / contraseña `Has`

Los usuarios que se registran desde la app quedan con el rol `Cliente`.

## Seguridad y roles

- Login con JWT (vence a las 2 horas). Las contraseñas se guardan con hash BCrypt.
- Roles: `Admin`, `Usuario`, `Cliente`.
- Crear, modificar y borrar clubes, estadios, eventos, sectores y usuarios requiere rol `Admin`.
- Compras, pagos y entradas requieren estar logueado.
- La consulta de clubes, estadios, eventos y sectores es pública.

## Pagos con Mercado Pago

Por defecto el token de `WebApi/appsettings.json` es un placeholder, así que la API usa un **simulador local** de pagos: genera un pago ficticio (`SIM-...`) y emite la compra igual que con un pago real. Para usar el sandbox de Mercado Pago, reemplazar `MercadoPago:AccessToken` por un access token de prueba (empieza con `TEST-`).

El simulador acepta como válido cualquier pago cuyo id empiece con `SIM-`, `TEST-` o `MP-`: está pensado solo para desarrollo y no para producción.

Cada entrada comprada tiene su código QR.

## Problemas frecuentes

- **No puedo iniciar sesión con usuarios creados antes del hash de contraseñas, o falla al crear usuarios.** La base se crea una sola vez (`EnsureCreated`) y no se actualiza sola. Borrarla y volver a levantar la API: El comando se puede ejecutar desde Visual Studio (*Ver > Explorador de objetos de SQL Server*, clic derecho sobre la base `kicket` > *Eliminar*, tildando "Cerrar conexiones existentes") o desde SQL Server Management Studio. Hay que detener la API antes.

- **La API no conecta con la base.** Revisar que la instancia de SQL Server configurada en `WebApi/appsettings.json` exista y esté disponible en la máquina (LocalDB por defecto, o la que se haya indicado). Si aparece un error de conexión al iniciar, la API sigue corriendo pero la base no se crea, y el error queda en su consola.

- **La API no conecta con la base.** Revisar que SQL Server Express esté corriendo y que la cadena de conexión de `WebApi/appsettings.json` coincida con el nombre de tu instancia.
