CREATE PROCEDURE [dbo].[spResponseSummay_DeleteAll]
AS
begin
	set nocount on;
	delete from dbo.ResponseSummary;
end
