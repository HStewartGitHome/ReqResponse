CREATE PROCEDURE [dbo].[spResponses_Create]
    @requestId INT,
    @responseSetId INT,
    @actualValue NVARCHAR(50),
    @actualResult INT, 
    @success BIT, 
    @created NVARCHAR(50),
    @Id int OUTPUT
AS
BEGIN
    INSERT INTO Responses (RequestId, ResponseSetId, ActualValue,ActualResult,Success,Created)
    VALUES (@requestId, @responseSetId,@actualValue,@actualResult,@success,@created)
    SET @Id = @@IDENTITY
END
