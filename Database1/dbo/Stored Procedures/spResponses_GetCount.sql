CREATE PROCEDURE [dbo].[spResponses_GetCount]
	@Count int OUTPUT
AS
BEGIN
	SELECT Id FROM Responses
	SELECT @Count = @@ROWCOUNT
END
