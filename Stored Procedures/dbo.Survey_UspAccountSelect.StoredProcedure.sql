USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspAccountSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspAccountSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Survey_UspAccountSelect]

@AccountID int,
@SelectFlag char(1)

as

IF (@AccountID != 2)

	BEGIN

	IF (@SelectFlag = 'I') -- Id based

	Begin

	SELECT [AccountID]
		  ,[Code]
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
		  ,[IsActive]
	  FROM [Account]

	WHERE [AccountID] = @AccountID

	End

	ELSE IF (@SelectFlag = 'A') -- All Records

	Begin

	SELECT [AccountID]
		  ,[Code]
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
		  ,[IsActive]
	  FROM [Account]
	WHERE IsActive=1 and [AccountID] = @AccountID order by [AccountID] DESC

	End

	ELSE IF (@SelectFlag = 'F') -- All Records except Super Admin

	Begin

	SELECT [AccountID]
		  ,[Code]
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
		  ,[IsActive]
	  FROM [Account]
	  
	  WHERE   IsActive=1 order by [Code]
	--WHERE [AccountID] not in (2) and  IsActive=1 order by [Code] 

	End

	ELSE IF (@SelectFlag = 'C') -- Get Record Count

	Begin

	SELECT count(*) FROM [Account] where IsActive=1 and [AccountID] = @AccountID

	End

	END

ELSE

	Begin

	IF (@SelectFlag = 'I') -- Id based

	Begin

	SELECT [AccountID]
		  ,[Code]
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
		  ,[IsActive]
	  FROM [Account]

	WHERE [AccountID] = @AccountID

	End

	ELSE IF (@SelectFlag = 'A') -- All Records

	Begin

	SELECT [AccountID]
		  ,[Code]
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
		  ,[IsActive]
	  FROM [Account]
	WHERE IsActive=1 order by [AccountID] DESC

	End

	ELSE IF (@SelectFlag = 'F') -- All Records except Super Admin

	Begin

	SELECT [AccountID]
		  ,[Code]
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
		  ,[IsActive]
	  FROM [Account]
	  
	  WHERE   IsActive=1 order by [Code]
	--WHERE [AccountID] not in (2) and  IsActive=1 order by [Code] 

	End

	ELSE IF (@SelectFlag = 'C') -- Get Record Count

	Begin

	SELECT count(*) FROM [Account] where IsActive=1 

	End

	End
GO
