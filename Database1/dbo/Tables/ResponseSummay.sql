CREATE TABLE [dbo].[ResponseSummary]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ResponseSetId] INT NULL,
    [SuccessfullCount] NVARCHAR(50) NULL, 
    [FailedCount] NVARCHAR(50) NULL, 
    [OkCount] NVARCHAR(50) NULL, 
    [ErrorCount] NVARCHAR(50) NULL,  
    [Created] NVARCHAR(50) NULL
)
