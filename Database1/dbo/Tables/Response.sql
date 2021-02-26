CREATE TABLE [dbo].[Responses]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ResponseSetId] INT NULL,
    [RequestId] INT NULL,
    [ActualValue] NVARCHAR(50) NULL,
    [ActualResult] INT NULL, 
    [Success] BIT NULL, 
    [Created] NVARCHAR(50) NULL, 
    [TimeExecuted] INT NULL, 
    [RequestOption] INT NULL
)
