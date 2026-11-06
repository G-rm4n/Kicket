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
| `Data` | Acceso a datos: `TPIContext` y repositorios (EF Core), reportes (ADO.NET) y actualización de esquema |
| `Core` | Lógica de negocio: servicios, JWT, hash de contraseñas, Mercado Pago, reportes (PDF y Excel) |
| `Kicket.Contracts` | DTOs y requests compartidos entre la API y los clientes |
| `WebApi` | API REST (minimal APIs) con Swagger |
| `Kicket.ApiClient` | Cliente HTTP de la API, usado por WinForms y Blazor |
| `Kicket.WinForms` | Cliente de escritorio |
| `Kicket.Blazor` | Cliente web |
| `Kicket.Tests` | Pruebas automatizadas (xUnit) |

Los cambios de la versión 2 (paginación, filtros, DNI, Mis Entradas, menú de escritorio, reportes y ADO.NET) están explicados en [`MEJORAS_KICKET_V2.md`](MEJORAS_KICKET_V2.md).

## Base de datos

La API usa SQL Server y **crea sola la base `kicket`** (con tablas y datos de prueba) la primera vez que se ejecuta. No hace falta crear nada a mano.

Por defecto se conecta a **LocalDB** (`(localdb)\MSSQLLocalDB`), que se instala junto con Visual Studio. Si se prefiere usar **SQL Server Express**, hay que cambiar la cadena de conexión en `WebApi/appsettings.json`, clave `ConnectionStrings:Local`:

```
Server=.\SQLEXPRESS;Database=kicket;Trusted_Connection=True;TrustServerCertificate=True;
```

Para cualquier otra instancia de SQL Server, reemplazar el valor de `Server=` por el nombre de esa instancia.

**Bases creadas con la versión anterior:** no hace falta borrarlas. Al arrancar, la API agrega sola las columnas nuevas de la versión 2 (DNI, teléfono, ciudad del club, etc.) sin tocar los datos existentes (`Data/Esquema/ActualizadorEsquema.cs`). El mismo script está en [`docs/sql/kicket_v2_actualizacion.sql`](docs/sql/kicket_v2_actualizacion.sql) por si se prefiere correrlo a mano. Los datos de prueba nuevos solo aparecen en bases creadas desde cero.

## EF Core y ADO.NET

El proyecto usa las dos tecnologías de acceso a datos, cada una donde conviene:

| | EF Core | ADO.NET |
|---|---|---|
| **Dónde** | `TPIContext` y los repositorios de `Data/Implementaciones` (ABM, compras, listados paginados, Mis Entradas) | `Data/Implementaciones/ReporteVentasRepository.cs` (reportes de ventas) |
| **Para qué** | Crear, leer, modificar y borrar entidades con sus relaciones, validaciones de FK y transacciones | Consultas de solo lectura con agregaciones (`GROUP BY`, `SUM`, `COUNT DISTINCT`, CTE) |
| **Ventajas** | Menos código, consultas LINQ tipadas, el esquema y los datos semilla salen del modelo | SQL explícito y controlado, se lee fila por fila con `SqlDataReader` sin cargar entidades, varios resultados en un solo viaje (`NextResultAsync`) |
| **Limitaciones** | Las consultas agregadas complejas son más difíciles de expresar y de controlar | Hay que escribir el SQL y mapear cada columna a mano; un cambio de columna no lo detecta el compilador |

El flujo de los reportes es **Pantalla de reportes → `Kicket.ApiClient` → WebApi (`/reportes/ventas`) → `Core/ReporteService` → `ReporteVentasRepository` (ADO.NET) → SQL Server**. Las dos tecnologías usan **la misma cadena de conexión**: el repositorio ADO.NET la toma del propio `TPIContext`. Todos los valores del usuario viajan como `SqlParameter` (nunca se concatenan al SQL).

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

- Clubes: Newell's Old Boys, Rosario Central, Boca Juniors y River Plate
- Estadios: Coloso del Parque (Platea Este, Popular Sur) y Gigante de Arroyito (Platea Norte, Popular Norte)
- Eventos: Clásico Rosarino y Central vs Boca (próximos), Newell's vs River (ya jugado) y Central vs River (cancelado)
- 7 compras aprobadas con 16 entradas, para que los reportes y "Mis Entradas" tengan datos
- Usuarios (todos con contraseña `Has`):

| Email | Rol | DNI |
|---|---|---|
| `EmailRaro@gmail.com` | Admin | 30111222 |
| `lucia.fernandez@mail.com` | Cliente | 38456789 |
| `martin.gomez@mail.com` | Cliente | 40123987 |

Los usuarios que se registran desde la app quedan con el rol `Cliente`. El registro exige DNI (7 u 8 dígitos, con o sin puntos) y no permite repetirlo.

## Seguridad y roles

- Login con JWT (vence a las 2 horas). Las contraseñas se guardan con hash BCrypt.
- Roles: `Admin`, `Usuario`, `Cliente`.
- Crear, modificar y borrar clubes, estadios, eventos, sectores y usuarios requiere rol `Admin`.
- Compras, pagos y entradas requieren estar logueado. Cada usuario solo ve sus propias compras y entradas: la API toma el usuario del token, no del id que manda el navegador.
- Listar todas las compras, anular compras, consultar entradas sueltas, ver usuarios (con su DNI) y los reportes requieren rol `Admin`.
- La consulta de clubes, estadios, eventos y sectores es pública.

## Pagos con Mercado Pago

Por defecto el token de `WebApi/appsettings.json` es un placeholder, así que la API usa un **simulador local** de pagos: genera un pago ficticio (`SIM-...`) y emite la compra igual que con un pago real. Para usar el sandbox de Mercado Pago, reemplazar `MercadoPago:AccessToken` por un access token de prueba (empieza con `TEST-`).

El simulador acepta como válido cualquier pago cuyo id empiece con `SIM-`, `TEST-` o `MP-`: está pensado solo para desarrollo y no para producción.

Cada entrada comprada tiene su código QR.

## Pruebas

```
dotnet test Kicket.Tests
```

Prueban las reglas del DNI, la validación de filtros, la paginación y el agrupamiento de "Mis Entradas" con los datos semilla, la exportación a PDF y Excel, y que los formularios de escritorio se armen sin superponer controles.

## Problemas frecuentes

- **No puedo iniciar sesión con usuarios creados antes del hash de contraseñas, o falla al crear usuarios.** La base se crea una sola vez (`EnsureCreated`) y no se actualiza sola. Hay que detener la API, borrar la base y volver a levantar la API:

  ```sql
  DROP DATABASE kicket;
  ```

  También se puede borrar desde Visual Studio (*Ver > Explorador de objetos de SQL Server*, clic derecho sobre la base `kicket` > *Eliminar*, tildando "Cerrar conexiones existentes") o desde SQL Server Management Studio.

- **La API no conecta con la base.** Revisar que la instancia de SQL Server configurada en `WebApi/appsettings.json` exista y esté disponible en la máquina (LocalDB por defecto, o la que se haya indicado). Si aparece un error de conexión al iniciar, la API sigue corriendo pero la base no se crea, y el error queda en su consola.
