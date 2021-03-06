USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspProjectSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspProjectSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspProjectSelect]

@ProjectID int,
@AccountID int,
@SelectFlag char(1)

as

IF (@AccountID != 2)

	BEGIN

		IF (@SelectFlag = 'I') -- Id based

		Begin

		SELECT      [ProjectID]
				   ,[StatusID]
				   ,[AccountID]
				   ,[Reference]
				   ,[Title]
				   ,[Description]
				   ,[ManagerID]
				   ,[MaxCandidate]
				   ,[Logo]
				   ,Project.Password
					,[QuestionnaireID]	
				   ,[StartDate]
				   ,[EndDate]
				   ,[Reminder1Date]
				   ,[Reminder2Date]
				   ,[Reminder3Date]
				   ,[ReportAvaliableFrom]
				   ,[ReportAvaliableTo]
				   ,[EmailTMPLStart]
				   ,[EmailTMPLReminder1]
				   ,[EmailTMPLReminder2]
				   ,[EmailTMPLReminder3]
				   ,[EmailTMPLReportAvalibale]
					,[EmailTMPLParticipant]
					,[EmailTMPPartReminder1] 
					,[EmailTMPPartReminder2] 
					,[EmailTMPManager]
					,[EmailTMPSelfReminder]
					,[Relationship1]
					,[Relationship2]
					,[Relationship3]
					,[Relationship4]
					,[Relationship5]
					,[FaqText]
				   ,[ModifyBy]
				   ,[ModifyDate]
				   ,[IsActive]   

			   FROM [Project]
			  WHERE [ProjectID] = @ProjectID
					AND [AccountID] = @AccountID

			 End

		ELSE IF (@SelectFlag = 'A') -- All Records

		Begin

		SELECT      [ProjectID]
				   ,Project.StatusID
				   ,Project.AccountID AS AccountID
				   ,[Reference]
				   ,[Title]
				   ,Project.Description
				   ,[ManagerID]
				   ,[MaxCandidate]
				   ,[Logo]
				   ,Project.Password
					,[QuestionnaireID]	
				   ,[StartDate]
				   ,[EndDate]
				   ,[Reminder1Date]
				   ,[Reminder2Date]
				   ,[Reminder3Date]
				   ,[ReportAvaliableFrom]
				   ,[ReportAvaliableTo]
				   ,[EmailTMPLStart]
				   ,[EmailTMPLReminder1]
				   ,[EmailTMPLReminder2]
				   ,[EmailTMPLReminder3]
				   ,[EmailTMPLReportAvalibale]
					,[EmailTMPLParticipant]
					,[EmailTMPPartReminder1] 
					,[EmailTMPPartReminder2] 	
					,[EmailTMPManager]	
					,[EmailTMPSelfReminder]			
					,[Relationship1]
				   ,[Relationship2]
				   ,[Relationship3]
				   ,[Relationship4]
				   ,[Relationship5]
					,[FaqText]
				   ,Project.ModifyBy
				   ,Project.ModifyDate
				   ,Project.IsActive
				   ,[User].FirstName as firstname
				   ,[User].LastName  as lastname
				   , (firstname + ' ' + lastname) as finalname
				   ,MSTProjectStatus.Name as ProjectStatus
				   ,[Account].[Code] as Code

			 FROM   [Project] Inner Join [User] on dbo.Project.ManagerID = [User].UserID
					Inner Join MSTProjectStatus on Project.StatusID = MSTProjectStatus.PRJStatusID
					INNER JOIN dbo.Account ON Project.AccountID = dbo.Account.AccountID
			 Where  Project.[AccountID] = @AccountID
			 
             order by dbo.Account.[Code] ,[ProjectID] desc

		   END
		  
			  
		   
		   ELSE IF (@SelectFlag = 'F') -- All Records

		Begin

		SELECT      [ProjectID]
				   ,Project.StatusID
				   ,Project.AccountID AS AccountID
				   ,[Reference]
				   ,[Title]
				   ,Project.Description
				   ,[ManagerID]
				   ,[MaxCandidate]
				   ,[Logo]
				   ,Project.Password
					,[QuestionnaireID]	
				   ,[StartDate]
				   ,[EndDate]
				   ,[Reminder1Date]
				   ,[Reminder2Date]
				   ,[Reminder3Date]
				   ,[ReportAvaliableFrom]
				   ,[ReportAvaliableTo]
				   ,[EmailTMPLStart]
				   ,[EmailTMPLReminder1]
				   ,[EmailTMPLReminder2]
				   ,[EmailTMPLReminder3]
				   ,[EmailTMPLReportAvalibale]
					,[EmailTMPLParticipant]
					,[EmailTMPPartReminder1] 
					,[EmailTMPPartReminder2] 	
					,[EmailTMPManager]		
					,[EmailTMPSelfReminder]		
					,[Relationship1]
					,[Relationship2]
					,[Relationship3]
					,[Relationship4]
					,[Relationship5]
				   ,[FaqText]
				   ,Project.ModifyBy
				   ,Project.ModifyDate
				   ,Project.IsActive
				   ,[User].FirstName as firstname
				   ,[User].LastName  as lastname
				   , (firstname + ' ' + lastname) as finalname
				   ,MSTProjectStatus.Name as ProjectStatus

			 FROM   [Project] Inner Join [User] on dbo.Project.ManagerID = [User].UserID
					Inner Join MSTProjectStatus on Project.StatusID = MSTProjectStatus.PRJStatusID
		            
					where Project.[AccountID] = @AccountID
		   END
		   
		   

		ELSE IF (@SelectFlag = 'C') -- Get Record Count

		Begin

		SELECT count(*) FROM [Project] where IsActive=1 and [AccountID] = @AccountID

		End



	END

ELSE

	BEGIN

		IF (@SelectFlag = 'I') -- Id based

		Begin

		SELECT      [ProjectID]
				   ,[StatusID]
				   ,[AccountID]
				   ,[Reference]
				   ,[Title]
				   ,[Description]
				   ,[ManagerID]
				   ,[MaxCandidate]
				   ,[Logo]
				   ,Project.Password
					,[QuestionnaireID]	
				   ,[StartDate]
				   ,[EndDate]
				   ,[Reminder1Date]
				   ,[Reminder2Date]
				   ,[Reminder3Date]
				   ,[ReportAvaliableFrom]
				   ,[ReportAvaliableTo]
				   ,[EmailTMPLStart]
				   ,[EmailTMPLReminder1]
				   ,[EmailTMPLReminder2]
				   ,[EmailTMPLReminder3]
				   ,[EmailTMPLReportAvalibale]
					,[EmailTMPLParticipant]
					,[EmailTMPPartReminder1] 
					,[EmailTMPPartReminder2] 	
					,[EmailTMPManager]	
					,[EmailTMPSelfReminder]			
					,[Relationship1]
					,[Relationship2]
					,[Relationship3]
					,[Relationship4]
					,[Relationship5]
					,[FaqText]
				   ,[ModifyBy]
				   ,[ModifyDate]
				   ,[IsActive]   

			   FROM [Project]
			  WHERE [ProjectID] = @ProjectID

			 End

		ELSE IF (@SelectFlag = 'A') -- All Records

		Begin

		SELECT      [ProjectID]
				   ,Project.StatusID
				   ,Project.AccountID AS AccountID
				   ,[Reference]
				   ,[Title]
				   ,Project.Description
				   ,[ManagerID]
				   ,[MaxCandidate]
				   ,[Logo]
				   ,Project.Password
					,[QuestionnaireID]	
				   ,[StartDate]
				   ,[EndDate]
				   ,[Reminder1Date]
				   ,[Reminder2Date]
				   ,[Reminder3Date]
				   ,[ReportAvaliableFrom]
				   ,[ReportAvaliableTo]
				   ,[EmailTMPLStart]
				   ,[EmailTMPLReminder1]
				   ,[EmailTMPLReminder2]
				   ,[EmailTMPLReminder3]
				   ,[EmailTMPLReportAvalibale]
					,[EmailTMPLParticipant]
					,[EmailTMPPartReminder1] 
					,[EmailTMPPartReminder2] 
					,[EmailTMPManager]	
					,[EmailTMPSelfReminder]				
					,[Relationship1]
				   ,[Relationship2]
				   ,[Relationship3]
				   ,[Relationship4]
				   ,[Relationship5]
					,[FaqText]
				   ,Project.ModifyBy
				   ,Project.ModifyDate
				   ,Project.IsActive
				   ,[User].FirstName as firstname
				   ,[User].LastName  as lastname
				   , (firstname + ' ' + lastname) as finalname
				   ,MSTProjectStatus.Name as ProjectStatus
				   ,[Account].[Code] as Code

			 FROM   [Project] Inner Join [User] on dbo.Project.ManagerID = [User].UserID
					Inner Join MSTProjectStatus on Project.StatusID = MSTProjectStatus.PRJStatusID
					INNER JOIN dbo.Account ON Project.AccountID = dbo.Account.AccountID
			Where  Project.[AccountID] = @AccountID
					order by dbo.Account.[Code] ,[ProjectID] desc
		   END
		  
			  IF (@SelectFlag = 'P') -- Id based

		Begin

		SELECT      [ProjectID]
				   ,[StatusID]
				   ,[AccountID]
				   ,[Reference]
				   ,[Title]
				   ,[Description]
				   ,[ManagerID]
				   ,[MaxCandidate]
				   ,[Logo]
				   ,Project.Password
					,[QuestionnaireID]	
				   ,[StartDate]
				   ,[EndDate]
				   ,[Reminder1Date]
				   ,[Reminder2Date]
				   ,[Reminder3Date]
				   ,[ReportAvaliableFrom]
				   ,[ReportAvaliableTo]
				   ,[EmailTMPLStart]
				   ,[EmailTMPLReminder1]
				   ,[EmailTMPLReminder2]
				   ,[EmailTMPLReminder3]
				   ,[EmailTMPLReportAvalibale]
					,[EmailTMPLParticipant]
					,[EmailTMPPartReminder1] 
					,[EmailTMPPartReminder2] 	
					,[EmailTMPManager]	
					,[EmailTMPSelfReminder]			
					,[Relationship1]
					,[Relationship2]
					,[Relationship3]
					,[Relationship4]
					,[Relationship5]
				,[FaqText]
				   ,[ModifyBy]
				   ,[ModifyDate]
				   ,[IsActive]   

			   FROM [Project]
			  WHERE [ProjectID] = @ProjectID
					

			 End

		   
		   ELSE IF (@SelectFlag = 'F') -- All Records

		Begin

		SELECT      [ProjectID]
				   ,Project.StatusID
				   ,Project.AccountID AS AccountID
				   ,[Reference]
				   ,[Title]
				   ,Project.Description
				   ,[ManagerID]
				   ,[MaxCandidate]
				   ,[Logo]
				   ,Project.Password
					,[QuestionnaireID]	
				   ,[StartDate]
				   ,[EndDate]
				   ,[Reminder1Date]
				   ,[Reminder2Date]
				   ,[Reminder3Date]
				   ,[ReportAvaliableFrom]
				   ,[ReportAvaliableTo]
				   ,[EmailTMPLStart]
				   ,[EmailTMPLReminder1]
				   ,[EmailTMPLReminder2]
				   ,[EmailTMPLReminder3]
				   ,[EmailTMPLReportAvalibale]
					,[EmailTMPLParticipant]
					,[EmailTMPPartReminder1] 
					,[EmailTMPPartReminder2] 	
					,[EmailTMPManager]
					,[EmailTMPSelfReminder]					
					,[Relationship1]
					,[Relationship2]
					,[Relationship3]
					,[Relationship4]
					,[Relationship5]
					,[FaqText]
				   ,Project.ModifyBy
				   ,Project.ModifyDate
				   ,Project.IsActive
				   ,[User].FirstName as firstname
				   ,[User].LastName  as lastname
				   , (firstname + ' ' + lastname) as finalname
				   ,MSTProjectStatus.Name as ProjectStatus

			 FROM   [Project] Inner Join [User] on dbo.Project.ManagerID = [User].UserID
					Inner Join MSTProjectStatus on Project.StatusID = MSTProjectStatus.PRJStatusID
		            
					where Project.AccountID = @AccountID
		   END
		   
		   

		ELSE IF (@SelectFlag = 'C') -- Get Record Count

		Begin

		SELECT count(*) FROM [Project] where IsActive=1

		End


	END
GO
