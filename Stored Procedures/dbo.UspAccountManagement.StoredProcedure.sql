USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAccountManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspAccountManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspAccountManagement]

@AccountID int
,@Code char(5)
,@LoginID varchar(50)
,@Password varchar(50)
,@OrganisationName varchar(100)
,@AccountTypeID int
,@Description varchar(1000)
,@EmailID varchar(100)
,@Website varchar(50)
,@StatusID int
,@CompanyLogo varchar(100)
,@CopyRightLine varchar(300)
,@HeaderBGColor varchar(50)
,@MenuBGColor varchar(50)
,@ModifyBy int
,@ModifyDate datetime
,@IsActive int
,@Operation char(1)

AS

-- Insert 

IF (@Operation = 'I')

BEGIN

INSERT INTO [Account]
           ([Code]
           ,[LoginID]
           ,[Password]
           ,[OrganisationName]
           ,[AccountTypeID]
           ,[Description]
           ,[EmailID]
           ,[Website]
           ,[StatusID]
           ,[CompanyLogo]
			,[CopyRightLine]
           ,[HeaderBGColor]
           ,[MenuBGColor]
           ,[ModifyBy]
           ,[ModifyDate]
           ,[IsActive])
     VALUES
           (@Code
			,@LoginID
			,@Password
			,@OrganisationName
			,@AccountTypeID 
			,@Description 
			,@EmailID 
			,@Website
			,@StatusID 
			,@CompanyLogo 
			,@CopyRightLine
			,@HeaderBGColor
			,@MenuBGColor
			,@ModifyBy
			,@ModifyDate
			,@IsActive)

End

-- Update

ELSE IF (@Operation = 'U')

BEGIN

UPDATE .[Account]

   SET [Code] = @Code
      ,[LoginID] = @LoginID
      ,[Password] = @Password
      ,[OrganisationName] = @OrganisationName
      ,[AccountTypeID] = @AccountTypeID
      ,[Description] = @Description
      ,[EmailID] = @EmailID
      ,[Website] = @Website
      ,[StatusID] = @StatusID
      ,[CompanyLogo] = @CompanyLogo
		,[CopyRightLine]=@CopyRightLine
      ,[HeaderBGColor] = @HeaderBGColor
      ,[MenuBGColor] = @MenuBGColor
      ,[ModifyBy] = @ModifyBy
      ,[ModifyDate] = @ModifyDate
      ,[IsActive] = @IsActive

 WHERE AccountID = @AccountID

END

-- Delete

ELSE IF (@Operation = 'D')

BEGIN

DELETE FROM [Account] WHERE AccountID = @AccountID

END
GO
