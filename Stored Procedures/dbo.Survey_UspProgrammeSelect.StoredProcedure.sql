USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspProgrammeSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspProgrammeSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspProgrammeSelect]

@ProgrammeID int,
@AccountID int,
@SelectFlag char(1)

as 

IF (@AccountID != 2)

BEGIN

IF (@SelectFlag = 'I') -- Id based

Begin

	SELECT [Survey_Analysis_Sheet].[ProgrammeID]
		  ,[Survey_Analysis_Sheet].[ProgrammeName]
		  ,[Survey_Analysis_Sheet].[ProgrammeDescription]
		  ,[Survey_Analysis_Sheet].[ClientName]
		  ,[Survey_Analysis_Sheet].[Logo] 
		  ,[Survey_Analysis_Sheet].[ProjectID]
		  ,[Survey_Analysis_Sheet].CompanyID
		  ,[Survey_Analysis_Sheet].[AccountID]
		  ,[Survey_Analysis_Sheet].[StartDate]
		  ,[Survey_Analysis_Sheet].[EndDate]
		  ,[Survey_Analysis_Sheet].[Reminder1Date]
		  ,[Survey_Analysis_Sheet].[Reminder2Date]
		  ,[Survey_Analysis_Sheet].[Reminder3Date]
		  
		 	  
		  
		  ,[Survey_Analysis_Sheet].[ModifyBy]
		  ,[Survey_Analysis_Sheet].[ModifyDate]
		  ,[Survey_Analysis_Sheet].[IsActive]
		  
		   ,[Survey_Analysis_Sheet].[Analysis_I_Name]
		  ,[Survey_Analysis_Sheet].[Analysis_I_Category]
		  ,[Survey_Analysis_Sheet].[Analysis_II_Name]
		  ,[Survey_Analysis_Sheet].[Analysis_II_Category]
		  ,[Survey_Analysis_Sheet].[Analysis_III_Name]
		  ,[Survey_Analysis_Sheet].[Analysis_III_Category]	
		
	FROM [Survey_Analysis_Sheet]
	WHERE [ProgrammeID] = @ProgrammeID AND
		  [AccountID] = @AccountID

	End

	ELSE IF (@SelectFlag = 'A') -- All Records

	Begin

	SELECT [ProgrammeID]
		  ,[Survey_Analysis_Sheet].[ProgrammeName]
		  ,[Survey_Analysis_Sheet].[ProgrammeDescription]
		  ,[Survey_Analysis_Sheet].[ClientName]
		  ,[Survey_Analysis_Sheet].[Logo] 
		  ,[Survey_Analysis_Sheet].[ProjectID]
		  ,[Survey_Analysis_Sheet].CompanyID
		  ,[Survey_Analysis_Sheet].[AccountID]
		  ,[Survey_Analysis_Sheet].[StartDate]
		  ,[Survey_Analysis_Sheet].[EndDate]
		  ,[Survey_Analysis_Sheet].[Reminder1Date]
		  ,[Survey_Analysis_Sheet].[Reminder2Date]
		  ,[Survey_Analysis_Sheet].[Reminder3Date]
		  
		 
		  		  
		  ,[Survey_Analysis_Sheet].[ModifyBy]
		  ,[Survey_Analysis_Sheet].[ModifyDate]
		  ,[Survey_Analysis_Sheet].[IsActive]
		  
		  
		  ,[Survey_Analysis_Sheet].[Analysis_I_Name]
		  ,[Survey_Analysis_Sheet].[Analysis_I_Category]
		  ,[Survey_Analysis_Sheet].[Analysis_II_Name]
		  ,[Survey_Analysis_Sheet].[Analysis_II_Category]
		  ,[Survey_Analysis_Sheet].[Analysis_III_Name]
		  ,[Survey_Analysis_Sheet].[Analysis_III_Category]	
		  
		  
		  ,[Survey_Project].Title
		  ,[Account].Code
		  
	FROM [Survey_Analysis_Sheet] INNER JOIN
		  dbo.Account ON [Survey_Analysis_Sheet].AccountID = dbo.Account.AccountID
			INNER JOIN [Survey_Project] on [Survey_Project].[ProjectID]=[Survey_Analysis_Sheet].[ProjectID]
	WHERE [Survey_Analysis_Sheet].[AccountID] = @AccountID

	ORDER BY dbo.Survey_Analysis_Sheet.[ProgrammeName]

	END


	ELSE IF (@SelectFlag = 'C') -- Get Record Count

	Begin

	SELECT count(*) FROM [Survey_Analysis_Sheet] where IsActive=1 AND [ProgrammeID] = @AccountID

	End
	



END

Else

	BEGIN

	IF (@SelectFlag = 'I') -- Id based

	Begin

	SELECT [ProgrammeID]
		  ,[Survey_Analysis_Sheet].[ProgrammeName]
		 ,[Survey_Analysis_Sheet].[ProgrammeDescription]
		 ,[Survey_Analysis_Sheet].[ClientName]
		 ,[Survey_Analysis_Sheet].[Logo] 
		  ,[Survey_Analysis_Sheet].[ProjectID]
		  ,[Survey_Analysis_Sheet].CompanyID
		  ,[Survey_Analysis_Sheet].[AccountID]
		  ,[Survey_Analysis_Sheet].[StartDate]
		  ,[Survey_Analysis_Sheet].[EndDate]
		  ,[Survey_Analysis_Sheet].[Reminder1Date]
		  ,[Survey_Analysis_Sheet].[Reminder2Date]
		  ,[Survey_Analysis_Sheet].[Reminder3Date]
		 
		 
		   
		  ,[Survey_Analysis_Sheet].[ModifyBy]
		  ,[Survey_Analysis_Sheet].[ModifyDate]
		  ,[Survey_Analysis_Sheet].[IsActive]
		  
		  
		  
		   ,[Survey_Analysis_Sheet].[Analysis_I_Name]
		  ,[Survey_Analysis_Sheet].[Analysis_I_Category]
		  ,[Survey_Analysis_Sheet].[Analysis_II_Name]
		  ,[Survey_Analysis_Sheet].[Analysis_II_Category]
		  ,[Survey_Analysis_Sheet].[Analysis_III_Name]
		  ,[Survey_Analysis_Sheet].[Analysis_III_Category]	
		  
		 
	FROM [Survey_Analysis_Sheet]
	WHERE [ProgrammeID] = @ProgrammeID

	End

	ELSE IF (@SelectFlag = 'A') -- All Records

	Begin

	SELECT [ProgrammeID]
		  ,[Survey_Analysis_Sheet].[ProgrammeName]
		 ,[Survey_Analysis_Sheet].[ProgrammeDescription]
		 ,[Survey_Analysis_Sheet].[ClientName]
		 ,[Survey_Analysis_Sheet].[Logo] 
		  ,[Survey_Analysis_Sheet].[ProjectID]
		  ,[Survey_Analysis_Sheet].CompanyID
		  ,[Survey_Analysis_Sheet].[AccountID]
		  ,[Survey_Analysis_Sheet].[StartDate]
		  ,[Survey_Analysis_Sheet].[EndDate]
		  ,[Survey_Analysis_Sheet].[Reminder1Date]
		  ,[Survey_Analysis_Sheet].[Reminder2Date]
		  ,[Survey_Analysis_Sheet].[Reminder3Date]
		  
		   
		  
		  ,[Survey_Analysis_Sheet].[ModifyBy]
		  ,[Survey_Analysis_Sheet].[ModifyDate]
		  ,[Survey_Analysis_Sheet].[IsActive]
		  
		  
		  ,[Survey_Analysis_Sheet].[Analysis_I_Name]
		  ,[Survey_Analysis_Sheet].[Analysis_I_Category]
		  ,[Survey_Analysis_Sheet].[Analysis_II_Name]
		  ,[Survey_Analysis_Sheet].[Analysis_II_Category]
		  ,[Survey_Analysis_Sheet].[Analysis_III_Name]
		  ,[Survey_Analysis_Sheet].[Analysis_III_Category]	
		  
		  
		  ,[Survey_Project].Title
		  ,[Account].Code
		  
		  
	FROM [Survey_Analysis_Sheet] INNER JOIN
		  dbo.Account ON [Survey_Analysis_Sheet].AccountID = dbo.Account.AccountID
			INNER JOIN [Survey_Project] on [Survey_Project].[ProjectID]=[Survey_Analysis_Sheet].[ProjectID]
	Where [Survey_Analysis_Sheet].[IsActive] =1
	ORDER BY dbo.Survey_Analysis_Sheet.[ProgrammeName]

	END


	

	ELSE IF (@SelectFlag = 'C') -- Get Record Count

	Begin

	SELECT count(*) FROM [Survey_Analysis_Sheet] where IsActive=1

	End


End
GO
