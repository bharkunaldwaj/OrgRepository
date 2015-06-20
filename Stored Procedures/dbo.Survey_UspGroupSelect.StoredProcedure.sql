USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspGroupSelect]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspGroupSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspGroupSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspGroupSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create proc [dbo].[Survey_UspGroupSelect]

@AccountID int,
@GroupID int,
@SelectFlag char(1)

as

IF (@AccountID != 2)

Begin

	IF (@SelectFlag = ''I'') -- Id based

	Begin

	SELECT [GroupID]
		  ,[GroupName]
		  ,[Description]
		  ,[WelcomeText]
		  ,[NewsText]
		  ,[IsActive]
		  ,[CreatedDate]
		  ,[ModifiedDate]
	  FROM [Survey_Group]
	WHERE [GroupID] = @GroupID

	End

	ELSE IF (@SelectFlag = ''A'') -- All Records

	Begin

	SELECT [GroupID]
		  ,[GroupName]
		  ,[Description]
		  ,[WelcomeText]
		  ,[NewsText]
		  ,[IsActive]
		  ,[CreatedDate]
		  ,[ModifiedDate]
	FROM [Survey_Group] 
	where GroupID not in (1)
	order by [GroupID] desc

	End

	ELSE IF (@SelectFlag = ''C'') -- Get Record Count

	Begin

	SELECT count(*) FROM [Survey_Group] 

	End

END

ELSE

BEGIN

	IF (@SelectFlag = ''I'') -- Id based

	Begin

	SELECT [GroupID]
		  ,[GroupName]
		  ,[Description]
		  ,[WelcomeText]
		  ,[NewsText]
		  ,[IsActive]
		  ,[CreatedDate]
		  ,[ModifiedDate]
	  FROM [Survey_Group]
	WHERE [GroupID] = @GroupID

	End

	ELSE IF (@SelectFlag = ''A'') -- All Records

	Begin

	SELECT [GroupID]
		  ,[GroupName]
		  ,[Description]
		  ,[WelcomeText]
		  ,[NewsText]
		  ,[IsActive]
		  ,[CreatedDate]
		  ,[ModifiedDate]
	FROM [Survey_Group] 
	where [IsActive]=1
	order by [GroupID] desc

	End

	ELSE IF (@SelectFlag = ''C'') -- Get Record Count

	Begin

	SELECT count(*) FROM [Survey_Group] where [IsActive]=1

	End


END
' 
END
GO
