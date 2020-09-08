CREATE PROCEDURE [dbo].[spResponseSummary_Update]
    @id INT,
    @responseSetId INT,
	@successfullCount NVARCHAR(50),
	@failedCount NVARCHAR(50),
	@okCount NVARCHAR(50),
	@errorCount NVARCHAR(50),
    @created NVARCHAR(50)
AS
BEGIN
    IF EXISTS ( SELECT Id FROM ResponseSummary WHERE Id = @id )
    BEGIN
        UPDATE  ResponseSummary SET ResponseSetId = @responseSetId, SuccessfullCount = @successfullCount,
                 FailedCount = @failedCount, OkCount = @okCount, ErrorCount = @errorCount, Created = @created
        WHERE Id = id AND ResponseSetId = @responseSetId
    END
END
