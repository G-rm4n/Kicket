# Kicket v2 — Mejoras pedidas por el profesor

Este documento explica qué se cambió en la versión 2, cómo funciona cada parte y qué quedó pendiente.
Todo lo que se describe acá está en el código; cuando algo no se hizo o no se pudo verificar, se dice.

---

## 1. Qué pidió el profesor

1. Paginación en entradas, clubes, usuarios y el resto de los listados.
2. Filtros y búsqueda en esos listados.
3. Más datos del usuario, en especial el **DNI**.
4. Más atributos en las entidades.
5. **Mis Entradas** agrupadas por partido, ordenadas por fecha, con el detalle en un desplegable y **sin mostrar fila ni asiento**.
6. En el escritorio, reemplazar los botones por un **menú desplegable** con **cerrar sesión**.
7. **Reportes exportables** (PDF y Excel) con **gráficos**.
8. Usar **ADO.NET** además de Entity Framework.

---

## 2. Qué se modificó (resumen)

| Pedido | Qué se hizo |
|---|---|
| Paginación | Paginación real en el servidor (`COUNT` + `OFFSET/FETCH`) para clubes, estadios, sectores, eventos, usuarios, compras y Mis Entradas. Paginador reutilizable en Blazor (`Paginador.razor`) y en WinForms (`ControlPaginacion`). |
| Filtros | Filtros resueltos en SQL, combinables, que se conservan al cambiar de página, con botón "Limpiar filtros". Panel reutilizable en Blazor (`PanelFiltros.razor`) y en WinForms (`BarraFiltros`). |
| DNI | Campo real en dominio, base, contratos, registro (web y escritorio), edición de usuarios y búsquedas. Obligatorio en las altas, validado y único. |
| Atributos | Usuario: DNI, teléfono, fecha de nacimiento. Club: ciudad, año de fundación. Estadio: provincia (y capacidad total calculada). Evento: descripción para el comprador. |
| Mis Entradas | Rediseño completo: un acordeón por partido, filtros, 4 órdenes, paginación por partido y QR por entrada, sin fila ni asiento. |
| Escritorio | `MenuStrip` con *Administración ▼* y *Sesión ▼ → Cerrar sesión*. Nueva pantalla de reportes. |
| Reportes | Pantalla en Blazor y en WinForms. 4 gráficos, exportación a PDF (QuestPDF) y a Excel (ClosedXML). |
| ADO.NET | `ReporteVentasRepository` con `SqlConnection`, `SqlCommand`, `SqlDataReader` y parámetros, integrado de punta a punta en los reportes. |
| Seguridad | Se cerraron accesos que antes estaban abiertos (ver sección 12). |
| Base de datos | Actualizador de esquema no destructivo para bases creadas con la versión 1. |
| Pruebas | Nuevo proyecto `Kicket.Tests` (53 tests) y un script de integración contra la API (63 verificaciones). |

---

## 3. Proyectos y archivos tocados

**Kicket.Contracts** (modelos compartidos)
- Nuevos: `Common/PagedRequest.cs`, `Common/PagedResult.cs`, `Usuarios/Dni.cs`, filtros (`ClubFiltro`, `EstadioFiltro`, `SectorFiltro`, `EventoFiltro`, `UsuarioFiltro`, `CompraFiltro`, `MisEntradasFiltro`), `Compras/PartidoConEntradasDto.cs` y `Reportes/ReporteVentasDto.cs`.
- Modificados: DTOs y requests de club, estadio, evento, sector, compra y usuario (campos nuevos).

**Domain**
- `Club`, `Estadio`, `Evento` y `Usuario`: atributos nuevos.

**Data**
- `TPIContext.cs`: configuración de los campos nuevos, índice único filtrado del DNI y datos semilla ampliados.
- Nuevos: `Consultas/Paginacion.cs` (Count + Skip/Take), `Esquema/ActualizadorEsquema.cs` y `Implementaciones/ReporteVentasRepository.cs` (ADO.NET) con su interfaz.
- Repositorios: métodos `ObtenerPaginadoAsync`, `ObtenerMisEntradasAsync`, `ObtenerPorUsuarioAsync`, `ExistsDni` y `ExistsEmailDeOtro`. Se quitaron los `AyncGetPaginated` viejos: nadie los usaba, no devolvían el total y el de usuarios excluía administradores sin avisar.
- `Data.csproj` ahora referencia a `Kicket.Contracts`, para recibir los filtros y devolver `PagedResult`.

**Core**
- Nuevos: `Services/Validaciones.cs` (filtros y datos personales), `Services/ReporteService.cs`, `Reportes/GraficosSvg.cs`, `Reportes/ExportadorPdf.cs` y `Reportes/ExportadorExcel.cs`.
- Servicios: métodos paginados y validaciones de DNI, teléfono, fecha de nacimiento, año de fundación y descripción.
- Paquetes: QuestPDF y ClosedXML.

**WebApi**
- Endpoints `/paginado` en todos los recursos, `GET /compras/mis-entradas` y `/reportes/ventas` (más `/pdf` y `/xlsx`).
- `Seguridad/UsuarioActual.cs` lee el usuario del token. `EndPoints/Mapeos.cs` centraliza la conversión entidad → DTO.
- `ErrorHandler.cs`: un borrado bloqueado por FK ahora responde 409 con un mensaje claro, en lugar de un 500 con el error técnico.
- `Program.cs`: registra los servicios de reportes y ejecuta el actualizador de esquema al arrancar.

**Kicket.ApiClient**
- `ApiClientBase`: `ConQuery` (filtros → query string escapada) y `GetBytesAsync` (descarga de archivos).
- Métodos `GetPaginadoAsync` en todos los clientes, `GetMisEntradasAsync` y el nuevo `ReporteApiClient`.

**Kicket.Blazor**
- Nuevos: `Components/Paginador.razor`, `Components/PanelFiltros.razor`, `Modules/Reportes/pages/Reportes.razor` y `wwwroot/js/descargas.js`.
- Rediseñados: `MisCompras.razor` (Mis Entradas), `ClubCRUD`, `EstadioCrud`, `UsuarioCRUD` (ahora también edita) y `EventosDisponibles`.
- Ajustados: `GenericTable` (estado vacío), `GenericModalForm` (muestra el error de la API dentro del modal), `TicketQrModal` (opción para ocultar la ubicación), `RegisterForm` (DNI, teléfono y fecha de nacimiento), `AdminLayout` (link a Reportes) y `NavBar`.

**Kicket.WinForms**
- Nuevos: `Controles/ControlPaginacion.cs`, `Controles/BarraFiltros.cs`, `Controles/LayoutListado.cs` y `Forms/FormReportes.cs`.
- `FormPrincipal`: menú desplegable en lugar de botones.
- `FormClub`, `FormEstadio`, `FormSectorDetalle`, `FormEvento`, `FormUsuario` y `FormCompras`: filtros, paginador y campos nuevos. `FormRegistro`: DNI.

**Otros**
- `Kicket.Tests/` (nuevo), `README.md`, `docs/sql/kicket_v2_actualizacion.sql` y `docs/pruebas/pruebas_api.py`.

---

## 4. Cómo funciona la paginación

1. El cliente manda `pagina`, `tamanoPagina` (10, 25 o 50; máximo 100), `ordenarPor` y `descendente` en la query string. Ejemplo: `GET /clubes/paginado?pagina=2&tamanoPagina=10&ordenarPor=nombre`.
2. El servicio valida los parámetros. Si alguno es inválido, la API responde **400**.
3. El repositorio arma la consulta como `IQueryable` (todavía sin ejecutar): aplica los filtros, ordena y **siempre agrega el id como segundo criterio**. Sin desempate, dos registros con el mismo nombre podrían aparecer en dos páginas o en ninguna.
4. `Paginacion.PaginarAsync` ejecuta dos consultas SQL:
   - `SELECT COUNT(*)` para obtener el total;
   - `... ORDER BY ... OFFSET (pagina-1)*tamaño ROWS FETCH NEXT tamaño ROWS ONLY` (`Skip` y `Take` de LINQ).
5. Si la página pedida ya no existe (por ejemplo, el filtro dejó menos resultados), se devuelve la **última página válida**.
6. La respuesta es un `PagedResult<T>`: `items`, `pagina`, `tamanoPagina`, `totalRegistros`, `totalPaginas`, `tieneAnterior` y `tieneSiguiente`.

Los endpoints viejos (`GET /clubes`, `GET /eventos`, etc.) **se mantienen** porque los usan los combos de los formularios.

## 5. Cómo funcionan los filtros

- Cada listado tiene su clase de filtro (`ClubFiltro`, `EventoFiltro`, …) que hereda de `PagedRequest`. Los filtros se combinan con *Y lógico*.
- Se traducen a SQL **parametrizado** (`Contains` → `LIKE @p`). El texto del usuario nunca se concatena.
- El orden solo acepta valores de una **lista blanca** (por ejemplo `nombre`, `fecha`, `id`). Cualquier otro valor da 400.
- Los rangos de fechas toman días completos ("hasta el 31/12" incluye todo ese día) y se valida que "desde" no sea posterior a "hasta".
- Al buscar o limpiar filtros se vuelve a la página 1. Al cambiar de página, los filtros se mantienen.

| Listado | Filtros |
|---|---|
| Clubes | texto (nombre, sigla, descripción), ciudad |
| Estadios | texto (nombre, dirección), ciudad, provincia |
| Sectores | estadio, nombre, tipo (Platea/Popular), capacidad mínima |
| Eventos | texto (evento, clubes, estadio), club (local o visitante), club local, club visitante, estadio, fecha desde/hasta, estado (próximos/pasados/cancelados) |
| Usuarios (Admin) | texto (nombre, apellido, email, DNI), DNI, rol |
| Compras (Admin) | texto (email, apellido, id de pago), evento, fecha de compra desde/hasta, fecha del partido desde/hasta, estado del pago, id de compra, usuario |
| Mis Entradas | texto (partido, clubes, estadio), fecha del partido desde/hasta, estado (próximos/finalizados/cancelados) |

El **tipo de sector** no es una columna: se deduce del nombre con la misma regla que ya usaba `CompraService` (si contiene "Platea" es numerado). Así no hay un dato duplicado que pueda contradecir al nombre.

## 6. Cómo se ordenan las entradas (Mis Entradas)

| Opción | Orden |
|---|---|
| **Partido más próximo** (por defecto) | Primero los partidos que todavía no se jugaron, del más cercano al más lejano; después los ya jugados, del más reciente al más viejo. |
| Partido más lejano | Por fecha del partido, descendente. |
| Compra más reciente | Por la fecha de la **compra más reciente del grupo**, descendente. |
| Compra más antigua | Por la fecha de la compra más reciente del grupo, ascendente. |

Siempre se desempata por el id del evento. Se eligió la **compra más reciente** del grupo (como pidió la consigna) porque representa la última actividad del usuario en ese partido.

## 7. Cómo se agrupan las compras por partido

En `CompraRepository.ObtenerMisEntradasAsync`:

1. **Agrupar en la base:** se unen `Entradas` y `Compras` del usuario y se hace `GROUP BY EventoId`, contando las entradas y tomando la compra más reciente. Cada partido queda en **una sola fila**, aunque venga de varias compras.
2. **Filtrar, ordenar y paginar los grupos:** se unen con `Eventos`, `Clubs` y `Estadios` para filtrar y mostrar nombres, y se pagina sobre los **partidos**.
3. **Traer el detalle** de las entradas, pero solo de los partidos de esa página.

Se pagina **antes** de traer el detalle y **sobre los grupos**. Si se paginaran las compras y después se agrupara, un partido podría quedar partido entre dos páginas o aparecer repetido. Ejemplo con los datos semilla: Lucía tiene 3 compras en 2 partidos, y la pantalla muestra 2 tarjetas. Las 2 compras de "Central vs Boca" quedan dentro de la misma tarjeta, con 4 entradas y $63.000.

El total pagado se suma **por compra** (no por entrada), para no contar el mismo monto varias veces.

## 8. Por qué fila y asiento siguen existiendo aunque no se vean en Mis Entradas

El pedido fue de **presentación**: el usuario no tiene que ver fila ni asiento en esa pantalla. Pero el dato sigue siendo necesario:

- `CompraService` lo usa para no vender dos veces el mismo asiento en Platea;
- el administrador lo ve en *Compras y entradas* (escritorio), para control.

Por eso **no se borró la columna ni se cambió la asignación de asientos**. Para que no se muestre, la API **directamente no lo manda** en este listado (`MiEntradaDto` no tiene `FilaAsiento`), y el modal del QR se abre con `MostrarUbicacion = false`. Hay un test que lo verifica (`MiEntradaDto_NoTieneFilaNiAsiento`). Otro verifica que el dato sigue en la base (`FilaYAsiento_SeConservanEnLaBase`).

## 9. Cómo se implementó el menú de escritorio

- `FormPrincipal` tiene un `MenuStrip` con dos menús:
  - **Administración ▼**: Clubes, Estadios y sectores, Eventos / Partidos, Usuarios, Compras y entradas, Reportes de ventas (atajos Ctrl+1 a Ctrl+6).
  - **Sesión ▼**: el usuario logueado y **Cerrar sesión** (Ctrl+Q).
- Las pantallas se abren con un único método `Abrir<TFormulario>()`, como diálogos modales: hay una sola ventana a la vez y se libera al cerrarse.
- **Cerrar sesión** borra token y usuario (`ISesionUsuario.Cerrar()`). Desde ese momento las requests salen sin credenciales y la API las rechaza. Después cierra el menú con `DialogResult.Retry`, y `FormLogin` se vuelve a mostrar (sin abrir otra instancia).
- Antes de abrir una pantalla y al volver de ella se controla que la sesión siga vigente. Si el token venció (o la API respondió 401 y `AuthTokenHandler` cerró la sesión), se avisa y se vuelve al login.

## 10. Biblioteca para PDF

**QuestPDF 2026.9.1**, con licencia *Community*: gratuita para uso académico y para organizaciones con ingresos menores a USD 1 millón al año. Se configura en `ExportadorPdf` (`QuestPDF.Settings.License = LicenseType.Community`).

El PDF incluye título, fecha de generación, filtros aplicados, 6 indicadores, el criterio de qué cuenta como venta, los 4 gráficos (vectoriales), 4 tablas con fila de totales y "Página X de Y".

## 11. Biblioteca para Excel

**ClosedXML 0.105.1**, licencia MIT.

El `.xlsx` tiene una hoja *Resumen* (filtros e indicadores) y una hoja por tabla: *Por partido*, *Por club*, *Por sector* y *Por día*. Las fechas y los montos son valores reales (no texto), con formato de fecha y de moneda. Cada hoja tiene autofiltro, encabezado congelado y una fila de **totales con fórmula `SUM`**, que se mantiene correcta si el usuario filtra.

**Limitación:** ClosedXML no puede crear gráficos de Excel, así que el `.xlsx` no los trae. Los gráficos están en la web y en el PDF.

## 12. Cómo se generan los gráficos

`Core/Reportes/GraficosSvg.cs` dibuja los gráficos **en el servidor como SVG**, a partir de los datos reales del reporte:

- barras horizontales: entradas vendidas por partido y recaudación por club;
- línea: evolución de la recaudación por día;
- dona: distribución de entradas por sector.

El **mismo SVG** se muestra en la web (Blazor lo inserta en la página) y se dibuja dentro del PDF (QuestPDF lo renderiza como vector). Así el PDF no depende de capturas de pantalla y los gráficos de los dos lados son idénticos. Los textos se escapan (no se puede inyectar HTML) y los números usan punto decimal sin importar la configuración regional.

## 13. Qué parte usa ADO.NET

`Data/Implementaciones/ReporteVentasRepository.cs` (reportes de ventas):

- `SqlConnection`, `SqlCommand`, `SqlDataReader`, todo asíncrono, con `CancellationToken` y `await using` para liberar recursos;
- **parámetros** `@estado`, `@desde`, `@hasta` y `@eventoId` (los nulos viajan como `DBNull`);
- una CTE base (`CompraVenta`) que agrupa primero **por compra** y evita sumar el monto una vez por cada entrada;
- `ObtenerResumenAsync` ejecuta 3 consultas en un solo comando y las lee con `NextResultAsync`;
- usa **la misma cadena de conexión que EF Core**, porque la toma de `TPIContext.Database.GetConnectionString()`.

Recorrido completo: **Pantalla de reportes → `ReporteApiClient` → `GET /reportes/ventas` → `ReporteService` → `ReporteVentasRepository` (ADO.NET) → SQL Server**. También lo usa *Compras y entradas* del escritorio para mostrar los totales del filtro.

**Qué cuenta como venta:** solo las compras con `EstadoPago = 'Aprobado'`. La recaudación sale del `MontoTotal` de cada compra. En el reporte **por club**, cada venta se atribuye al **club local**: si se sumara a los dos clubes, el total por club sería el doble del real. Hay pruebas que verifican que el total por partido, por club, por sector y por día coinciden con el total general.

## 14. Qué parte usa EF Core

Todo lo demás: ABM de todas las entidades, compras (con la asignación de asientos y el control de stock), pagos, autenticación, listados paginados y Mis Entradas. La creación del esquema (`EnsureCreated`) y los datos semilla también son de EF Core.

**Por qué se conservaron los dos:** EF Core es más productivo para el ABM y las relaciones. ADO.NET da control total del SQL en consultas agregadas de solo lectura, que es justo el caso de los reportes. El detalle de ventajas y limitaciones está en el README.

## 15. Endpoints y DTOs nuevos

| Endpoint | Acceso | Devuelve |
|---|---|---|
| `GET /clubes/paginado` | público | `PagedResult<ClubDto>` |
| `GET /estadios/paginado` | público | `PagedResult<EstadioDto>` (con capacidad total) |
| `GET /sectores/paginado` | público | `PagedResult<SectorDto>` (con estadio y tipo) |
| `GET /eventos/paginado` | público | `PagedResult<EventoDto>` (con nombres de clubes y estadio) |
| `GET /usuarios/paginado` | Admin | `PagedResult<UsuarioDto>` |
| `GET /compras/paginado` | Admin | `PagedResult<CompraDto>` (con usuario, evento y sector) |
| `GET /compras/mis-entradas` | logueado (el usuario sale del token) | `PagedResult<PartidoConEntradasDto>` |
| `GET /reportes/ventas` | Admin | `ReporteVentasDto` |
| `GET /reportes/ventas/pdf` | Admin | archivo PDF |
| `GET /reportes/ventas/xlsx` | Admin | archivo Excel |

DTOs nuevos: `PagedRequest`, `PagedResult<T>`, los 7 filtros, `PartidoConEntradasDto`, `MiEntradaDto`, `ReporteFiltro`, `ReporteVentasDto`, `ResumenVentasDto`, `VentaEventoDto`, `VentaClubDto`, `VentaPeriodoDto`, `VentaSectorDto` y `GraficoDto`.

**Cambios de seguridad en endpoints existentes:**

| Endpoint | Antes | Ahora |
|---|---|---|
| `GET /compras` | cualquier logueado veía las compras de todos | solo Admin |
| `GET /compras/usuario/{id}` y `GET /compras/{id}` | cualquier logueado | el dueño o un Admin (403 si no) |
| `DELETE /compras/{id}` | cualquier logueado podía anular cualquier compra | solo Admin |
| `GET /entradas/...` | cualquier logueado (exponía códigos QR ajenos) | solo Admin |
| `POST /compras`, `/pagos/crear-preferencia` y `/pagos/confirmar` | usaban el `UsuarioId` del cuerpo | un usuario común siempre compra a su nombre (id del token) |

## 16. Cambios en la base de datos

| Tabla | Columna nueva | Tipo |
|---|---|---|
| Usuarios | `Dni` | `nvarchar(10) NULL`, con índice único filtrado `IX_Usuarios_Dni` (`WHERE Dni IS NOT NULL`) |
| Usuarios | `Telefono` | `nvarchar(30) NULL` |
| Usuarios | `FechaNacimiento` | `datetime2 NULL` |
| Clubs | `Ciudad` | `nvarchar(100) NULL` |
| Clubs | `AnioFundacion` | `int NULL` |
| Estadios | `Provincia` | `nvarchar(100) NULL` |
| Eventos | `Descripcion` | `nvarchar(500) NULL` |

**Estrategia:** el proyecto usa `EnsureCreated`, que **no** agrega columnas a una base que ya existe. Por eso, al arrancar, la API ejecuta `ActualizadorEsquema`: sentencias `IF COL_LENGTH(...) IS NULL ALTER TABLE ... ADD ...` que solo agregan lo que falta. Se pueden ejecutar todas las veces que haga falta, no borran nada y no modifican datos. El mismo script está en `docs/sql/kicket_v2_actualizacion.sql`.

**DNI de usuarios viejos:** quedan en `NULL`. No se inventan DNI. El índice filtrado permite muchos `NULL` y prohíbe repetidos entre los que sí lo tienen. Al editar un usuario viejo, el DNI es opcional: si se deja vacío, se conserva el que tenía. En las altas nuevas es obligatorio.

**Regla del DNI:** 7 u 8 dígitos, con o sin puntos de miles. Se guarda normalizado como texto (solo dígitos) para no perder ceros a la izquierda.

**Datos semilla:** se ampliaron (4 clubes, 2 estadios, 4 sectores, 4 eventos, 3 usuarios, 7 compras, 16 entradas), pero solo aparecen en **bases nuevas**. En una base existente no se insertan ni se reemplazan datos.

## 17. Pruebas ejecutadas

| Prueba | Resultado |
|---|---|
| `dotnet restore` + `dotnet build Kicket.sln` | Compilación correcta: **0 errores, 0 advertencias** |
| `dotnet test Kicket.Tests` | **53/53 correctas** |
| `docs/pruebas/pruebas_api.py` contra la API sobre una base nueva | **63/63 verificaciones correctas** |
| Actualización de una base v1 real | Se levantó la API v1 en una base de prueba, se creó un usuario sin DNI y una compra, y después se levantó la API v2 sobre esa base. Se agregaron las columnas y el índice, se conservaron usuarios, compras y entradas, el usuario viejo pudo loguearse, ver sus entradas y editarse sin DNI. Una segunda corrida no dio errores (idempotente). |
| Arranque de la web Blazor | Las rutas `/`, `/eventos`, `/mis-entradas`, `/admin/reportes` y el JS de descarga responden 200 |
| PDF y Excel | Se generaron y se revisaron: el PDF tiene 3 páginas con gráficos; el Excel tiene 5 hojas con fechas y montos tipados, autofiltro y totales con `SUM` |

Qué cubren los **tests unitarios**: regla del DNI; validación de filtros (página, tamaño, orden fuera de la lista blanca, rangos invertidos, estados inválidos); paginación sin repetir ni saltear registros; página fuera de rango; filtros combinados; agrupamiento, orden, filtros y paginación por partido de Mis Entradas; que Mis Entradas solo traiga entradas del usuario; que fila y asiento sigan en la base; registro con DNI válido, inválido, faltante y repetido; edición de usuarios viejos sin DNI; gráficos SVG válidos y escapados; PDF y Excel con datos tipados y totales; menú desplegable con cerrar sesión; y que los formularios de escritorio tengan filtros y paginador sin superponerse.

Qué cubren las **pruebas de integración**, contra la API real y SQL Server:
- paginación, filtros, orden e inyección SQL en la búsqueda;
- todos los permisos (401/403);
- compra a nombre del token;
- Mis Entradas: grupos, totales, orden, filtros, paginación y ausencia de fila/asiento;
- DNI: alta, duplicado, inválido, faltante, normalizado y edición;
- reportes: totales coherentes entre tablas, filtros, PDF y Excel.

Las pruebas usaron bases propias (`kicket_v1_upgrade_test`, `kicket_v2_test`). **La base `kicket` del proyecto no se tocó.**

## 18. Limitaciones y pendientes

- **Las pantallas no se probaron con mouse y teclado.** Blazor compila y arranca, y todo el backend que usa está probado de punta a punta, pero la interacción (clics, desplegables, descargas) no se verificó desde un navegador en este entorno. Lo mismo con WinForms: los formularios se construyen y su distribución está cubierta por tests, pero no se navegaron. **Hay que recorrerlas antes de la defensa** (guía en el resumen de entrega).
- **Excel sin gráficos**, por la limitación de ClosedXML (sección 11).
- En **Blazor** no hay pantallas de administración de eventos, sectores ni compras; nunca las hubo y se administran desde el escritorio. No se crearon pantallas nuevas fuera de Reportes.
- **No hay un listado general de entradas**: las entradas se paginan dentro de las compras (admin) y de Mis Entradas (usuario).
- **"Confirmar compra directa"** (botón de prueba de la web) sigue existiendo, como antes. Crea compras "Aprobado" sin pasar por el pago, y esas compras cuentan en los reportes. Ahora quedan siempre a nombre del usuario logueado.
- La validación de pagos de Mercado Pago sigue siendo permisiva (pensada para el sandbox), igual que en la versión 1.
- El **estado de una entrada** (usada/no usada) no existe en el modelo y no se inventó. El estado del partido se deduce de la fecha y de `EstaCancelado`.
- El menú lateral del panel admin de Blazor tiene ancho fijo: en pantallas muy chicas conviene usar el sitio en horizontal.

---

## PREGUNTAS QUE PUEDE HACER EL PROFESOR

**¿Por qué implementaron paginación en el servidor?**
Porque así viajan por la red y se cargan en memoria solo los registros de la página (10, 25 o 50), sin importar cuántos haya en la tabla. Si se paginara en Blazor, primero habría que traer la tabla entera.

**¿Por qué no filtraron todo directamente en Blazor?**
Por la misma razón: filtrar en el cliente obliga a descargar todo. Además, el total de registros y la cantidad de páginas tienen que salir del resultado ya filtrado, y eso solo lo sabe la base. Y como el filtro está en la API, lo reutilizan igual la web y el escritorio.

**¿Cómo funciona Skip y Take?**
`Skip((pagina-1)*tamaño)` saltea las páginas anteriores y `Take(tamaño)` trae las de la página actual. EF Core los traduce a `OFFSET ... ROWS FETCH NEXT ... ROWS ONLY`. Necesitan un `ORDER BY` estable; por eso siempre se agrega el id como desempate. Aparte se hace un `COUNT(*)` con los mismos filtros para el total.

**¿Por qué agruparon las entradas por partido?**
Porque al usuario le importa a qué partido va, no cuántas operaciones de pago hizo. Si compró en dos momentos para el mismo partido, ve un solo partido con todas sus entradas.

**¿Cómo mantienen las compras históricas?**
No se borran ni cambian de estado cuando el partido pasa. El estado "Finalizado" se calcula comparando la fecha con la actual, así que los partidos jugados siguen apareciendo en Mis Entradas (filtro "Finalizados").

**¿Cómo ordenan las entradas?**
Por defecto, primero los partidos próximos (el más cercano arriba) y después los jugados (el más reciente arriba). También se puede ordenar por fecha del partido o por la compra más reciente del grupo. Siempre se desempata por el id del evento.

**¿Cómo resuelven varios tickets de un mismo evento?**
Con `GROUP BY EventoId` en la base, paginando sobre esos grupos y recién después trayendo el detalle de las entradas de los partidos de la página. Así ningún partido aparece duplicado ni partido entre páginas.

**¿Por qué ocultaron fila y asiento?**
Lo pidió el profesor para esa pantalla. Es solo presentación: el dato sigue en la base porque la asignación de asientos lo necesita para no vender dos veces el mismo lugar, y el administrador lo ve en el escritorio. La API ni siquiera lo manda en ese listado.

**¿Cómo generan el reporte PDF?**
La API arma el reporte (datos con ADO.NET y gráficos SVG) y QuestPDF lo convierte en un documento con indicadores, gráficos, tablas, totales y número de página. La web y el escritorio lo descargan de `/reportes/ventas/pdf`.

**¿Cómo generan el Excel?**
Con ClosedXML, a partir del mismo reporte: una hoja de resumen y una por tabla, con datos tipados, formato de moneda y fecha, autofiltro y totales con fórmula `SUM`.

**¿De dónde obtienen los datos de los gráficos?**
De las mismas consultas ADO.NET del reporte, sobre las compras con pago aprobado. El servidor los convierte en SVG y ese mismo SVG se muestra en la web y va al PDF.

**¿Para qué utilizaron ADO.NET?**
Para los reportes: son consultas de solo lectura con agregaciones (CTE, `GROUP BY`, `SUM`, `COUNT DISTINCT`) que conviene escribir y controlar en SQL. Se leen con `SqlDataReader` sin crear entidades.

**¿Por qué conservaron EF Core?**
Porque para el ABM, las relaciones, las transacciones de compra y la creación del esquema es más simple y seguro. Cada tecnología se usa donde aporta más.

**¿Cómo evitan SQL Injection?**
En EF Core, LINQ genera SQL parametrizado. En ADO.NET, todos los valores van como `SqlParameter`. El orden dinámico usa una lista blanca: el nombre de una columna nunca sale del texto del usuario. Hay una prueba que manda `'; DROP TABLE Clubs;--` en la búsqueda y la API responde normalmente, con 0 resultados.

**¿Cómo protegen los reportes administrativos?**
Con la política `"Solo admin"` en la API (`RequireAuthorization`), no solo ocultando el botón. Un cliente recibe 403 y un anónimo 401; está verificado en las pruebas de integración.

**¿Cómo evolucionó la base de datos?**
Sin migraciones (el proyecto usa `EnsureCreated`), con un actualizador que solo agrega columnas `NULL` y un índice, de forma idempotente. Las bases de la versión 1 se actualizan solas al arrancar la API, sin perder datos.

**¿Qué mejoras aplicarían a futuro?**
Pasar a migraciones de EF Core. Agregar un estado a las entradas (para el control de acceso con el QR). Validar los pagos contra Mercado Pago de forma estricta. Sumar gráficos nativos al Excel con otra biblioteca. Crear en Blazor las pantallas de administración de eventos, sectores y compras. Agregar pruebas automáticas de interfaz.
