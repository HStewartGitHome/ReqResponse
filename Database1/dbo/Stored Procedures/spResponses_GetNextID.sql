CREATE PROCEDURE [dbo].[spResponses_GetNextID]
	@NextID int OUTPUT
AS
BEGIN
	SELECT @NextID = MAX(Id) FROM Responses
END
