CREATE PROCEDURE [dbo].[spRequests_DeleteAll]
AS
begin
	set nocount on;
	delete from dbo.Requests;
end
