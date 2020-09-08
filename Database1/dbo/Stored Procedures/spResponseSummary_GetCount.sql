CREATE PROCEDURE [dbo].[spResponseSummary_GetCount]
	@Count int OUTPUT
AS
BEGIN
	SELECT Id FROM ResponseSummary
	SELECT @Count = @@ROWCOUNT
END
