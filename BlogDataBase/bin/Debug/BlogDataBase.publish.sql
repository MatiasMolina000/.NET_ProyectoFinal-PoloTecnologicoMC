/*
Script de implementación para BlogDataBase

Una herramienta generó este código.
Los cambios realizados en este archivo podrían generar un comportamiento incorrecto y se perderán si
se vuelve a generar el código.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "BlogDataBase"
:setvar DefaultFilePrefix "BlogDataBase"
:setvar DefaultDataPath "F:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\"
:setvar DefaultLogPath "F:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detectar el modo SQLCMD y deshabilitar la ejecución del script si no se admite el modo SQLCMD.
Para volver a habilitar el script después de habilitar el modo SQLCMD, ejecute lo siguiente:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'El modo SQLCMD debe estar habilitado para ejecutar correctamente este script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL,
                RECOVERY FULL 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET PAGE_VERIFY NONE 
            WITH ROLLBACK IMMEDIATE;
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET TARGET_RECOVERY_TIME = 0 SECONDS 
    WITH ROLLBACK IMMEDIATE;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET QUERY_STORE (QUERY_CAPTURE_MODE = ALL, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 367), MAX_STORAGE_SIZE_MB = 100) 
            WITH ROLLBACK IMMEDIATE;
    END


GO
PRINT N'Iniciando recompilación de la tabla [dbo].[Publicacion]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Publicacion] (
    [id]        INT           IDENTITY (1, 1) NOT NULL,
    [Titulo]    VARCHAR (100) NOT NULL,
    [Subtitulo] VARCHAR (100) NOT NULL,
    [Creador]   VARCHAR (50)  NOT NULL,
    [Cuerpo]    VARCHAR (MAX) NOT NULL,
    [Creacion]  DATE          NOT NULL,
    [Imagen]    VARCHAR (200) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Publicacio1] PRIMARY KEY CLUSTERED ([id] ASC) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Publicacion])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Publicacion] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Publicacion] ([id], [Titulo], [Subtitulo], [Creador], [Cuerpo], [Creacion], [Imagen])
        SELECT   [id],
                 [Titulo],
                 [Subtitulo],
                 [Creador],
                 [Cuerpo],
                 [Creacion],
                 [Imagen]
        FROM     [dbo].[Publicacion]
        ORDER BY [id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Publicacion] OFF;
    END

DROP TABLE [dbo].[Publicacion];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Publicacion]', N'Publicacion';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Publicacio1]', N'PK_Publicacio', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Actualización completada.';


GO
