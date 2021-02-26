CREATE PROCEDURE [dbo].[spResponses_GetAll]
AS
begin
      SELECT Id,RequestId, ResponseSetId, ActualValue,ActualResult,Success,Created,TimeExecuted,RequestOption
      FROM dbo.Responses
end
