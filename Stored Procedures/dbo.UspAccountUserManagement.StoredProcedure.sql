USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAccountUserManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspAccountUserManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspAccountUserManagement]

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

IF (@Operation = 'I')

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

ELSE IF (@Operation = 'U')

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

ELSE IF (@Operation = 'D')

BEGIN

DELETE FROM [User]
 WHERE [UserID]=@UserID

END
GO
