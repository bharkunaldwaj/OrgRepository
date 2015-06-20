USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspAccountUserManagement]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspAccountUserManagement]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspAccountUserManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspAccountUserManagement]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create proc [dbo].[Survey_UspAccountUserManagement]

@UserID int
,@LoginID varchar(50)
,@Password varchar(50)
,@GroupID int
,@AccountID int
,@StatusID int
,@Salutation varchar(5)
,@FirstName varchar(50)
,@LastName varchar(50)
,@EmailID varchar(100)
,@Notification bit
,@ModifyBy int
,@ModifyDate datetime
,@IsActive int
,@Operation char(1)

as

-- Insert

IF (@Operation = ''I'')

BEGIN

INSERT INTO [User]
           ([LoginID]
           ,[Password]
           ,[GroupID]
           ,[AccountID]
           ,[StatusID]
           ,[Salutation]
           ,[FirstName]
           ,[LastName]
           ,[EmailID]
           ,[Notification]
           ,[ModifyBy]
           ,[ModifyDate]
           ,[IsActive])
     VALUES
           (@LoginID 
			,@Password 
			,@GroupID 
			,@AccountID 
			,@StatusID 
			,@Salutation 
			,@FirstName 
			,@LastName 
			,@EmailID 
			,@Notification 
			,@ModifyBy 
			,@ModifyDate 
			,@IsActive)

END

-- Update

ELSE IF (@Operation = ''U'')

BEGIN

UPDATE [User]
   SET [LoginID] = @LoginID
      ,[Password] = @Password
      ,[GroupID] = @GroupID
      ,[AccountID] = @AccountID
      ,[StatusID] = @StatusID
      ,[Salutation] = @Salutation
      ,[FirstName] = @FirstName
      ,[LastName] = @LastName
      ,[EmailID] = @EmailID
      ,[Notification] = @Notification
      ,[ModifyBy] = @ModifyBy
      ,[ModifyDate] = @ModifyDate
      ,[IsActive] = @IsActive
 WHERE [UserID]=@UserID

END

-- Delete

ELSE IF (@Operation = ''D'')

BEGIN

DELETE FROM [User]
 WHERE [UserID]=@UserID

END
' 
END
GO
