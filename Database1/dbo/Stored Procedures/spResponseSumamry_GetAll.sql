CREATE PROCEDURE [dbo].[spResponseSummary_GetAll]	
AS
begin
      SELECT Id,ResponseSetId,SuccessfullCount,FailedCount,OkCount,ErrorCount,Created,TimeExecuted,RequestOption
      FROM dbo.ResponseSummary
end
