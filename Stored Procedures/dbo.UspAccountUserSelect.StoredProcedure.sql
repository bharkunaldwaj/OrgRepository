USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAccountUserSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspAccountUserSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspAccountUserSelect]

@AccountUserID int,
@AccountID int,
@SelectFlag char(1)

as

IF (@AccountID != 2)

	BEGIN

		IF (@SelectFlag = 'I') -- Id based

		Begin

		SELECT [UserID]
			  ,[User].[LoginID]
			  ,[User].[Password]
			  ,[GroupID]
			  ,[Account].Code
			  ,[User].[AccountID]
			  ,[User].[StatusID]
			  ,[Salutation]
			  ,[FirstName]
			  ,[LastName]
			  ,[User].[EmailID]
			  ,[Notification]
			  ,[User].[ModifyBy]
			  ,[User].[ModifyDate]
			  ,[User].[IsActive]
		  FROM dbo.[User] INNER JOIN
               dbo.Account ON dbo.[User].AccountID = dbo.Account.AccountID
		WHERE [UserID]=@AccountUserID

		END

		ELSE IF (@SelectFlag = 'A') -- All Records

		Begin

		SELECT	 Account.Code, 
				 [Group].GroupName, 
				 [User].UserID, 
				 [User].LoginID, 
				 [User].Password, 
				 [User].GroupID, 
				 [User].AccountID, 
				 [User].StatusID, 
				 [User].Salutation, 
				 [User].FirstName + ' ' + [User].LastName as UserName, 
				 [User].EmailID, 
				 [User].[Notification],
				 [User].ModifyBy, 
				 [User].ModifyDate, 
				 [User].IsActive
		FROM   Account INNER JOIN
			 [User] ON  Account.AccountID =  [User].AccountID INNER JOIN
			 [Group] ON  [User].GroupID = [Group].GroupID
		WHERE [User].IsActive=1 AND [User].AccountID=@AccountID
		order by [User].UserID Desc

		END

		ELSE IF (@SelectFlag = 'C') -- Get Record Count

		Begin

		SELECT count(*) FROM [User] where IsActive=1 AND AccountID=@AccountID

		End

		ELSE IF (@SelectFlag = 'P') -- All Records

		Begin

		SELECT	 Account.Code, 
				 [Group].GroupName, 
				 [User].UserID, 
				 [Account].Code,
				 [User].LoginID, 
				 [User].Password, 
				 [User].GroupID, 
				 [User].AccountID, 
				 [User].StatusID, 
				 [User].Salutation, 
				 [User].FirstName + ' ' + [User].LastName as UserName, 
				 [User].EmailID, 
				 [User].[Notification],
				 [User].ModifyBy, 
				 [User].ModifyDate, 
				 [User].IsActive
		FROM   Account INNER JOIN
			 [User] ON  Account.AccountID =  [User].AccountID INNER JOIN
			 [Group] ON  [User].GroupID = [Group].GroupID
		WHERE [User].IsActive=1 AND [User].AccountID=@AccountID and [User].GroupID=35
		order by [User].UserID Desc

		END

	END

ELSE

	BEGIN

		IF (@SelectFlag = 'I') -- Id based

		Begin

		SELECT [UserID]
			  ,[User].[LoginID]
			  ,[User].[Password]
			  ,[GroupID]
			  ,[Account].Code
			  ,[User].[AccountID]
			  ,[User].[StatusID]
			  ,[Salutation]
			  ,[FirstName]
			  ,[LastName]
			  ,[User].[EmailID]
			  ,[Notification]
			  ,[User].[ModifyBy]
			  ,[User].[ModifyDate]
			  ,[User].[IsActive]
		  FROM dbo.[User] INNER JOIN
               dbo.Account ON dbo.[User].AccountID = dbo.Account.AccountID
		WHERE [UserID]=@AccountUserID

		END

		ELSE IF (@SelectFlag = 'A') -- All Records

		Begin

		SELECT	 Account.Code, 
				 [Group].GroupName, 
				 [User].UserID, 
				 [User].LoginID, 
				 [User].Password, 
				 [User].GroupID, 
				 [User].AccountID, 
				 [User].StatusID, 
				 [User].Salutation, 
				 [User].FirstName + ' ' + [User].LastName as UserName, 
				 [User].EmailID, 
				 [User].[Notification],
				 [User].ModifyBy, 
				 [User].ModifyDate, 
				 [User].IsActive
		FROM   Account INNER JOIN
			 [User] ON  Account.AccountID =  [User].AccountID INNER JOIN
			 [Group] ON  [User].GroupID = [Group].GroupID
		WHERE [User].IsActive=1 
		order by [User].UserID Desc

		END

		ELSE IF (@SelectFlag = 'C') -- Get Record Count

		Begin

		SELECT count(*) FROM [User] where IsActive=1 

		End

		ELSE IF (@SelectFlag = 'P') -- All Records

		Begin

		SELECT	 Account.Code, 
				 [Group].GroupName, 
				 [User].UserID, 
				 [User].LoginID, 
				 [Account].Code,
				 [User].Password, 
				 [User].GroupID, 
				 [User].AccountID, 
				 [User].StatusID, 
				 [User].Salutation, 
				 [User].FirstName + ' ' + [User].LastName as UserName, 
				 [User].EmailID, 
				 [User].[Notification],
				 [User].ModifyBy, 
				 [User].ModifyDate, 
				 [User].IsActive
		FROM   Account INNER JOIN
			 [User] ON  Account.AccountID =  [User].AccountID INNER JOIN
			 [Group] ON  [User].GroupID = [Group].GroupID
		WHERE [User].IsActive=1 AND [User].AccountID=@AccountID and [User].GroupID=35
		order by [User].UserID Desc

		END
	END
GO
