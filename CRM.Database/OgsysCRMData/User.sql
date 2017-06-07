CREATE TABLE [dbo].[User]
(
	[Id]			 INT NOT NULL PRIMARY KEY,
    [FirstName]      NVARCHAR (50) NULL,
    [LastName]       NVARCHAR (50) NULL,
    [Email]			 NVARCHAR (50) NULL,
    [Password]		 NVARCHAR (100) NULL,
	[Salt]			 NVARCHAR (100) NULL
)