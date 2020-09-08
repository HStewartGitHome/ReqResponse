CREATE PROCEDURE [dbo].[spRequests_GetAll]	
AS
begin
      SELECT Id,InputXml, Method, Value1, Value2, ExpectedResult, ExpectedValue
      FROM dbo.Requests
end
