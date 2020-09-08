CREATE PROCEDURE [dbo].[spResponseSummary_Create]
    @responseSetId INT,
	@successfullCount NVARCHAR(50),
	@failedCount NVARCHAR(50),
	@okCount NVARCHAR(50),
	@errorCount NVARCHAR(50),
    @created NVARCHAR(50),
    @Id int OUTPUT
AS
BEGIN
    INSERT INTO ResponseSummary (ResponseSetId, SuccessfullCount,FailedCount,OkCount,ErrorCount,Created)
    VALUES (@responseSetId,@successfullCount,@failedCount,@okCount,@errorCount,@created)
	SET @Id = @@IDENTITY
END
