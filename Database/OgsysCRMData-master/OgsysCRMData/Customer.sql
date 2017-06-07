CREATE TABLE [dbo].[Customer] (
    [Id]          INT           NOT NULL,
    [FirstName]   NVARCHAR (50) NULL,
    [LastName]    NVARCHAR (50) NULL,
    [CompanyName] NVARCHAR (50) NULL,
    [Email]       NVARCHAR (50) NULL,
    [Phone]       NVARCHAR (15) NULL,
	[AddressId]	  INT			NOT NULL,

    PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_dbo.Customer_dbo.Address_Id] FOREIGN KEY ([AddressId]) 
        REFERENCES [dbo].[Address] ([Id])
);

