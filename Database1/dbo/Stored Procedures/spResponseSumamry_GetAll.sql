CREATE PROCEDURE [dbo].[spResponseSummary_GetAll]	
AS
begin
      SELECT Id,ResponseSetId,SuccessfullCount,FailedCount,OkCount,ErrorCount,Created
      FROM dbo.ResponseSummary
end
