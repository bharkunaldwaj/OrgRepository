USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspParticiantUserManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspParticiantUserManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Survey_UspParticiantUserManagement]

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

AS

-- Insert 

IF (@Operation = 'I')

BEGIN

declare @MaxID varchar(10)

SELECT @MaxID = ISNULL(MAX([UserID]) + 1,1)  FROM [User] 

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
           (@LoginID + @MaxID
			,@Password + @MaxID
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
			
SELECT ISNULL(MAX([UserID]),1)
  FROM [User]

End

-- Update

ELSE IF (@Operation = 'U')

BEGIN

UPDATE .[User]

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

 WHERE UserID = @UserID

END

-- Delete

ELSE IF (@Operation = 'D')

BEGIN

DELETE FROM [User] WHERE UserID = @UserID

END
GO
