CREATE TABLE [dbo].[Address]
(
	[Id]			 INT NOT NULL PRIMARY KEY,
	[Address]		 NVARCHAR (50) NULL,
	[City]			 NVARCHAR (50) NULL,
	[State]			 NVARCHAR (20) NUll,
	[Country]		 NVARCHAR (50) NULL,
	[Zipcode]		 NVARCHAR (20) NULL
)
