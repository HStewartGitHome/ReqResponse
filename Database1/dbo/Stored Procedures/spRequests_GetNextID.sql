CREATE PROCEDURE [dbo].[spRequests_GetNextID]
	@NextID int OUTPUT
AS
BEGIN
	SELECT @NextID = MAX(Id) FROM Requests
END
