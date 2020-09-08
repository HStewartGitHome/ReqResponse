CREATE PROCEDURE [dbo].[spResponses_GetAll]
AS
begin
      SELECT Id,RequestId, ResponseSetId, ActualValue,ActualResult,Success,Created
      FROM dbo.Responses
end
