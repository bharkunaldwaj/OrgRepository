USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Custom_Pagination]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Custom_Pagination]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Custom_Pagination]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Custom_Pagination]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Custom_Pagination]
(
	@startRowIndex int,
	@maximumRows int, 
	@totalRows int OUTPUT
)
AS
BEGIN
	DECLARE @First_ID int, @startRow int

	SET @startRowIndex =  (@startRowIndex - 1)  * @maximumRows

	IF @startRowIndex = 0 
		SET @startRowIndex = 1

	SET ROWCOUNT @startRowIndex
	SELECT @First_ID = UserID FROM Users ORDER BY UserID

	PRINT @First_ID
	SET ROWCOUNT @maximumRows

	SELECT UserID, UserName FROM Users WHERE 
	UserID >= @First_ID 
	ORDER BY UserID
	 
	SET ROWCOUNT 0

	-- GEt the total rows 

	SELECT @totalRows = COUNT(UserID) FROM Users
END
' 
END
GO
