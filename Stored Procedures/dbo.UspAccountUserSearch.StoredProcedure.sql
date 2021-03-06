USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAccountUserSearch]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspAccountUserSearch]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspAccountUserSearch]

@Condition varchar(5000),
@SelectFlag char(1)

as

IF (@SelectFlag = 'A') -- Id based

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
		WHERE [User].IsActive=1 AND [User].AccountID=@Condition
		order by [User].AccountID Desc

End


ELSE IF (@SelectFlag = 'C') -- Count

Begin

SELECT	 Count(Account.Code)
		FROM   Account INNER JOIN
			 [User] ON  Account.AccountID =  [User].AccountID INNER JOIN
			 [Group] ON  [User].GroupID = [Group].GroupID
		WHERE [User].IsActive=1 AND [User].AccountID=@Condition
		
End
GO
