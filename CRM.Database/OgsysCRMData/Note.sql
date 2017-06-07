CREATE TABLE [dbo].[Note]
(
	[Id]			INT NOT NULL PRIMARY KEY,
	[Body]			NVARCHAR (500) NULL,
	[CreatedDate]	DATETIME NOT NULL,	
	[UserId]		INT NOT NULL,
	[CustomerId]	INT NOT NULL,
	
	CONSTRAINT [FK_dbo.Note_dbo.Customer_Id] FOREIGN KEY ([CustomerId])
		REFERENCES [dbo].[Customer] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_dbo.Note_dbo.User_Id] FOREIGN KEY ([UserId])
		REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE	
)
