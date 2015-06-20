USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspProgrammeSelect]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspProgrammeSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspProgrammeSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspProgrammeSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure [dbo].[UspProgrammeSelect]

@ProgrammeID int,
@AccountID int,
@SelectFlag char(1)

as 

IF (@AccountID != 2)

BEGIN

IF (@SelectFlag = ''I'') -- Id based

Begin

	SELECT [Programme].[ProgrammeID]
		  ,[Programme].[ProgrammeName]
		  ,[Programme].[ProgrammeDescription]
		  ,[Programme].[ClientName]
		  ,[Programme].[Logo] 
		  ,[Programme].[ProjectID]
		  ,[Programme].[AccountID]
		  ,[Programme].[StartDate]
		  ,[Programme].[EndDate]
		  ,[Programme].[Reminder1Date]
		  ,[Programme].[Reminder2Date]
		  ,[Programme].[Reminder3Date]
		  ,[Programme].[ReportAvaliableFrom]
		  ,[Programme].[ReportAvaliableTo]
		  ,[Programme].[PartReminder1Date] 
		  ,[Programme].[PartReminder2Date] 
		  ,[Programme].[ModifyBy]
		  ,[Programme].[ModifyDate]
		  ,[Programme].[Instructions]
		  ,[Programme].[ColleagueNo]
		  ,[Programme].[IsActive]
		  ,[Programme].[ReportLogo]
		
	FROM [Programme]
	WHERE [ProgrammeID] = @ProgrammeID AND
		  [AccountID] = @AccountID

	End

	ELSE IF (@SelectFlag = ''A'') -- All Records

	Begin

	SELECT [ProgrammeID]
		  ,[Programme].[ProgrammeName]
		  ,[Programme].[ProgrammeDescription]
		  ,[Programme].[ClientName]
		  ,[Programme].[Logo] 
		  ,[Programme].[ProjectID]
		  ,[Programme].[AccountID]
		  ,[Programme].[StartDate]
		  ,[Programme].[EndDate]
		  ,[Programme].[Reminder1Date]
		  ,[Programme].[Reminder2Date]
		  ,[Programme].[Reminder3Date]
		  ,[Programme].[ReportAvaliableFrom]
		  ,[Programme].[ReportAvaliableTo]
		  ,[Programme].[PartReminder1Date] 
		  ,[Programme].[PartReminder2Date] 		  
		  ,[Programme].[ModifyBy]
		  ,[Programme].[ModifyDate]
		  ,[Programme].[Instructions]
		  ,[Programme].[ColleagueNo]
		  ,[Programme].[IsActive]
		  ,[Project].Title
		  ,[Account].Code
		  
	FROM [Programme] INNER JOIN
		  dbo.Account ON [Programme].AccountID = dbo.Account.AccountID
			INNER JOIN [Project] on [Project].[ProjectID]=[Programme].[ProjectID]
	WHERE [Programme].[AccountID] = @AccountID

	ORDER BY dbo.Programme.[ProgrammeName]

	END


	ELSE IF (@SelectFlag = ''C'') -- Get Record Count

	Begin

	SELECT count(*) FROM [Programme] where IsActive=1 AND [ProgrammeID] = @AccountID

	End
	



END

Else

	BEGIN

	IF (@SelectFlag = ''I'') -- Id based

	Begin

	SELECT [ProgrammeID]
		  ,[Programme].[ProgrammeName]
		 ,[Programme].[ProgrammeDescription]
		 ,[Programme].[ClientName]
		 ,[Programme].[Logo] 
		  ,[Programme].[ProjectID]
		  ,[Programme].[AccountID]
		  ,[Programme].[StartDate]
		  ,[Programme].[EndDate]
		  ,[Programme].[Reminder1Date]
		  ,[Programme].[Reminder2Date]
		  ,[Programme].[Reminder3Date]
		  ,[Programme].[ReportAvaliableFrom]
		  ,[Programme].[ReportAvaliableTo]
		  ,[Programme].[PartReminder1Date] 
		  ,[Programme].[PartReminder2Date] 		  
		  ,[Programme].[Instructions]
		  ,[Programme].[ColleagueNo]
		  ,[Programme].[ModifyBy]
		  ,[Programme].[ModifyDate]
		  ,[Programme].[IsActive]
		  ,[Programme].[ReportLogo]
		 
	FROM [Programme]
	WHERE [ProgrammeID] = @ProgrammeID

	End

	ELSE IF (@SelectFlag = ''A'') -- All Records

	Begin

	SELECT [ProgrammeID]
		  ,[Programme].[ProgrammeName]
		 ,[Programme].[ProgrammeDescription]
		 ,[Programme].[ClientName]
		 ,[Programme].[Logo] 
		  ,[Programme].[ProjectID]
		  ,[Programme].[AccountID]
		  ,[Programme].[StartDate]
		  ,[Programme].[EndDate]
		  ,[Programme].[Reminder1Date]
		  ,[Programme].[Reminder2Date]
		  ,[Programme].[Reminder3Date]
		  ,[Programme].[ReportAvaliableFrom]
		  ,[Programme].[ReportAvaliableTo]
		  ,[Programme].[PartReminder1Date] 
		  ,[Programme].[PartReminder2Date] 		  
		  ,[Programme].[Instructions]
		  ,[Programme].[ColleagueNo]
		  ,[Programme].[ModifyBy]
		  ,[Programme].[ModifyDate]
		  ,[Programme].[IsActive]
		  ,[Project].Title
		  ,[Account].Code
		  
	FROM [Programme] INNER JOIN
		  dbo.Account ON [Programme].AccountID = dbo.Account.AccountID
			INNER JOIN [Project] on [Project].[ProjectID]=[Programme].[ProjectID]
	Where [Programme].[IsActive] =1
	ORDER BY dbo.Programme.[ProgrammeName]

	END


	

	ELSE IF (@SelectFlag = ''C'') -- Get Record Count

	Begin

	SELECT count(*) FROM [Programme] where IsActive=1

	End


End
' 
END
GO
