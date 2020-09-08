CREATE PROCEDURE [dbo].[spResponseSummary_GetByResponseSetId]
    @responseSetId INT
AS
begin
      SELECT Id,ResponseSetId,SuccessfullCount,FailedCount,OkCount,ErrorCount,Created
      FROM dbo.ResponseSummary
      WHERE ResponseSetId = @responseSetId
end
