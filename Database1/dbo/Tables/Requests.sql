CREATE TABLE [dbo].[Requests]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [InputXml] NVARCHAR(255) NULL, 
    [Method] NVARCHAR(50) NULL, 
    [Value1] NVARCHAR(50) NULL, 
    [Value2] NVARCHAR(50) NULL, 
    [ExpectedResult] INT NULL,
    [ExpectedValue] NVARCHAR(50) NULL,
)
