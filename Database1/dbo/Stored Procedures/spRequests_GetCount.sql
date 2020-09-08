CREATE PROCEDURE [dbo].[spRequests_GetCount]
	@Count int OUTPUT
AS
BEGIN
	SELECT Id FROM Requests
	SELECT @Count = @@ROWCOUNT
END
