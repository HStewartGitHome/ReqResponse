CREATE PROCEDURE [dbo].[spResponses_DeleteAll]
AS
begin
	set nocount on;
	delete from dbo.Responses;
end
