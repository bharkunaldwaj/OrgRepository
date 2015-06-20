USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityGetUserByFilter]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityGetUserByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PersonalityGetUserByFilter]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityGetUserByFilter]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		Raj 
-- Create date: 10 Jan 2012
-- Description:	Procedure to Get Users
-- =============================================
CREATE PROCEDURE [dbo].[PersonalityGetUserByFilter]
	@AccountID int,
	@FirstName Varchar(Max),
	@LastName Varchar(Max),
	@LoginID Varchar(Max)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Select statements for procedure here
    if (@AccountID is not null)
    begin
		
		if (Len(RTRIM(LTRIM(@FirstName)))=0)
		begin
			set @FirstName=null
		end
		if (Len(RTRIM(LTRIM(@LastName)))=0)
		begin
			set @LastName=null
		end
		if (Len(RTRIM(LTRIM(@LoginID)))=0)
		begin
			set @LoginID=null
		end
	 	    
		SELECT * FROM [User]U
		
		where AccountID=@AccountID
		  AND (@FirstName is null or UPPER(FirstName)like UPPER(@FirstName))
		  AND (@LastName is null or UPPER(LastName)like UPPER(@LastName))
		  AND (@LoginID is null or UPPER(LoginID)like UPPER(@LoginID))
	end
END
' 
END
GO
