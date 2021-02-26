CREATE PROCEDURE [dbo].[spResponseSummary_GetById]
    @id INT
AS
begin
      SELECT Id,ResponseSetId,SuccessfullCount,FailedCount,OkCount,ErrorCount,Created,TimeExecuted,RequestOption
      FROM dbo.ResponseSummary
      WHERE Id = id
end
