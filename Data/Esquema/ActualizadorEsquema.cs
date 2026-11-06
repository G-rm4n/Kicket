using Microsoft.EntityFrameworkCore;

namespace Data.Esquema
{
    /// <summary>
    /// Lleva una base creada con la version 1 (EnsureCreated) al esquema de la version 2 sin borrar nada.
    ///
    /// Por que existe: EnsureCreated solo crea la base si NO existe; si ya existe no le agrega columnas
    /// nuevas. Como el proyecto no usa migraciones, este paso agrega las columnas e indices nuevos con
    /// sentencias idempotentes (IF ... IS NULL): se puede ejecutar todas las veces que se quiera.
    /// Solo agrega columnas que aceptan NULL, por eso los datos existentes quedan intactos.
    /// El mismo script esta en docs/sql/kicket_v2_actualizacion.sql para correrlo a mano.
    /// </summary>
    public static class ActualizadorEsquema
    {
        // Cada sentencia va por separado: SQL Server no deja usar una columna en el mismo lote en que se crea.
        public static readonly string[] Sentencias =
        {
            "IF COL_LENGTH('dbo.Usuarios', 'Dni') IS NULL ALTER TABLE dbo.Usuarios ADD Dni nvarchar(10) NULL;",
            "IF COL_LENGTH('dbo.Usuarios', 'Telefono') IS NULL ALTER TABLE dbo.Usuarios ADD Telefono nvarchar(30) NULL;",
            "IF COL_LENGTH('dbo.Usuarios', 'FechaNacimiento') IS NULL ALTER TABLE dbo.Usuarios ADD FechaNacimiento datetime2 NULL;",
            "IF COL_LENGTH('dbo.Clubs', 'Ciudad') IS NULL ALTER TABLE dbo.Clubs ADD Ciudad nvarchar(100) NULL;",
            "IF COL_LENGTH('dbo.Clubs', 'AnioFundacion') IS NULL ALTER TABLE dbo.Clubs ADD AnioFundacion int NULL;",
            "IF COL_LENGTH('dbo.Estadios', 'Provincia') IS NULL ALTER TABLE dbo.Estadios ADD Provincia nvarchar(100) NULL;",
            "IF COL_LENGTH('dbo.Eventos', 'Descripcion') IS NULL ALTER TABLE dbo.Eventos ADD Descripcion nvarchar(500) NULL;",
            // DNI unico solo entre los usuarios que lo tienen cargado (los viejos quedan en NULL).
            "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Usuarios_Dni' AND object_id = OBJECT_ID('dbo.Usuarios')) " +
                "CREATE UNIQUE INDEX IX_Usuarios_Dni ON dbo.Usuarios(Dni) WHERE [Dni] IS NOT NULL;"
        };

        /// <summary>Ejecuta todas las sentencias contra la base del contexto.</summary>
        public static void Aplicar(TPIContext context)
        {
            foreach (var sentencia in Sentencias)
            {
                context.Database.ExecuteSqlRaw(sentencia);
            }
        }
    }
}
