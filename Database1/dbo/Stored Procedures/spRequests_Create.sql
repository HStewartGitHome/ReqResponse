CREATE PROCEDURE [dbo].[spRequests_Create]
    @inputXml NVARCHAR(255), 
    @method NVARCHAR(50), 
    @value1 NVARCHAR(50), 
    @value2 NVARCHAR(50), 
    @expectedResult INT,
    @expectedValue NVARCHAR(50),
    @Id int OUTPUT
AS
BEGIN
    INSERT INTO Requests (InputXml, Method, Value1, Value2, ExpectedResult, ExpectedValue)
    VALUES (@inputXml, @method, @value1, @value2, @expectedResult, @expectedValue)
    SET @Id = @@IDENTITY
END
