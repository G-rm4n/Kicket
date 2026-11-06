/*
  Kicket v2 - actualización de esquema (no destructiva, idempotente).

  La API ejecuta exactamente estas sentencias al arrancar (Data/Esquema/ActualizadorEsquema.cs),
  así que normalmente NO hace falta correr este archivo. Sirve para aplicarlo a mano o para revisarlo.

  - Solo AGREGA columnas que aceptan NULL y un índice: no borra ni modifica datos existentes.
  - Se puede ejecutar varias veces: cada sentencia controla si el cambio ya existe.
  - Los usuarios creados antes de la v2 quedan con Dni = NULL (no se inventan DNI).
    El índice único filtrado permite muchos NULL y prohíbe DNI repetidos entre los que sí lo tienen.

  Uso (SQL Server Management Studio o sqlcmd), sobre la base "kicket":
      sqlcmd -S "(localdb)\MSSQLLocalDB" -d kicket -i docs\sql\kicket_v2_actualizacion.sql
*/

IF COL_LENGTH('dbo.Usuarios', 'Dni') IS NULL ALTER TABLE dbo.Usuarios ADD Dni nvarchar(10) NULL;
GO
IF COL_LENGTH('dbo.Usuarios', 'Telefono') IS NULL ALTER TABLE dbo.Usuarios ADD Telefono nvarchar(30) NULL;
GO
IF COL_LENGTH('dbo.Usuarios', 'FechaNacimiento') IS NULL ALTER TABLE dbo.Usuarios ADD FechaNacimiento datetime2 NULL;
GO
IF COL_LENGTH('dbo.Clubs', 'Ciudad') IS NULL ALTER TABLE dbo.Clubs ADD Ciudad nvarchar(100) NULL;
GO
IF COL_LENGTH('dbo.Clubs', 'AnioFundacion') IS NULL ALTER TABLE dbo.Clubs ADD AnioFundacion int NULL;
GO
IF COL_LENGTH('dbo.Estadios', 'Provincia') IS NULL ALTER TABLE dbo.Estadios ADD Provincia nvarchar(100) NULL;
GO
IF COL_LENGTH('dbo.Eventos', 'Descripcion') IS NULL ALTER TABLE dbo.Eventos ADD Descripcion nvarchar(500) NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Usuarios_Dni' AND object_id = OBJECT_ID('dbo.Usuarios'))
    CREATE UNIQUE INDEX IX_Usuarios_Dni ON dbo.Usuarios(Dni) WHERE [Dni] IS NOT NULL;
GO
