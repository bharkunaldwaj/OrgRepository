USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspGroupSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspGroupSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspGroupSelect]

@AccountID int,
@GroupID int,
@SelectFlag char(1)

as

IF (@AccountID != 2)

Begin

	IF (@SelectFlag = 'I') -- Id based

	Begin

	SELECT [GroupID]
		  ,[GroupName]
		  ,[Description]
		  ,[WelcomeText]
		  ,[NewsText]
		  ,[IsActive]
		  ,[CreatedDate]
		  ,[ModifiedDate]
	  FROM [Group]
	WHERE [GroupID] = @GroupID

	End

	ELSE IF (@SelectFlag = 'A') -- All Records

	Begin

	SELECT [GroupID]
		  ,[GroupName]
		  ,[Description]
		  ,[WelcomeText]
		  ,[NewsText]
		  ,[IsActive]
		  ,[CreatedDate]
		  ,[ModifiedDate]
	FROM [Group] 
	where GroupID not in (1)
	order by [GroupID] desc

	End

	ELSE IF (@SelectFlag = 'C') -- Get Record Count

	Begin

	SELECT count(*) FROM [Group] 

	End

END

ELSE

BEGIN

	IF (@SelectFlag = 'I') -- Id based

	Begin

	SELECT [GroupID]
		  ,[GroupName]
		  ,[Description]
		  ,[WelcomeText]
		  ,[NewsText]
		  ,[IsActive]
		  ,[CreatedDate]
		  ,[ModifiedDate]
	  FROM [Group]
	WHERE [GroupID] = @GroupID

	End

	ELSE IF (@SelectFlag = 'A') -- All Records

	Begin

	SELECT [GroupID]
		  ,[GroupName]
		  ,[Description]
		  ,[WelcomeText]
		  ,[NewsText]
		  ,[IsActive]
		  ,[CreatedDate]
		  ,[ModifiedDate]
	FROM [Group] 
	where [IsActive]=1
	order by [GroupID] desc

	End

	ELSE IF (@SelectFlag = 'C') -- Get Record Count

	Begin

	SELECT count(*) FROM [Group] where [IsActive]=1

	End


END
GO
