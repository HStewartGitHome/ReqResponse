CREATE PROCEDURE [dbo].[spResponses_Create]
    @requestId INT,
    @responseSetId INT,
    @actualValue NVARCHAR(50),
    @actualResult INT, 
    @success BIT, 
    @created NVARCHAR(50),
    @timeExecuted INT,
    @requestOption INT,
    @Id int OUTPUT
AS
BEGIN
    INSERT INTO Responses (RequestId, ResponseSetId, ActualValue,ActualResult,Success,Created,TimeExecuted,RequestOption)
    VALUES (@requestId, @responseSetId,@actualValue,@actualResult,@success,@created,@timeExecuted,@requestOption)
    SET @Id = @@IDENTITY
END
