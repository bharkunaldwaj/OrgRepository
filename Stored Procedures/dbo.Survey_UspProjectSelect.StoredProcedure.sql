USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspProjectSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspProjectSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspProjectSelect]

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
				   ,Survey_Project.Password
					,[QuestionnaireID]	
				   ,[StartDate]
				   ,[EndDate]
				   ,[Reminder1Date]
				   ,[Reminder2Date]
				   ,[Reminder3Date]
				   
				   ,[EmailTMPLStart]
				   ,[EmailTMPLReminder1]
				   ,[EmailTMPLReminder2]
				   ,[EmailTMPLReminder3]
				   
					,[FaqText]
				   ,[ModifyBy]
				   ,[ModifyDate]
				   ,[IsActive] 
				   ,[Finish_EmailID]  
				   ,[Finish_EmailID_Chkbox]

			   FROM [Survey_Project]
			  WHERE [ProjectID] = @ProjectID
					AND [AccountID] = @AccountID

			 End

		ELSE IF (@SelectFlag = 'A') -- All Records

		Begin

		SELECT      [ProjectID]
				   ,Survey_Project.StatusID
				   ,Survey_Project.AccountID AS AccountID
				   ,[Reference]
				   ,[Title]
				   ,Survey_Project.Description
				   ,[ManagerID]
				   ,[MaxCandidate]
				   ,[Logo]
				   ,Survey_Project.Password
					,[QuestionnaireID]	
				   ,[StartDate]
				   ,[EndDate]
				   ,[Reminder1Date]
				   ,[Reminder2Date]
				   ,[Reminder3Date]
				 
				   ,[EmailTMPLStart]
				   ,[EmailTMPLReminder1]
				   ,[EmailTMPLReminder2]
				   ,[EmailTMPLReminder3]
				   
				   ,[FaqText]
				   ,Survey_Project.ModifyBy
				   ,Survey_Project.ModifyDate
				   ,Survey_Project.IsActive
				   ,Survey_Project.Finish_EmailID
				   ,Survey_Project.Finish_EmailID_Chkbox
				   ,[User].FirstName as firstname
				   ,[User].LastName  as lastname
				   , (firstname + ' ' + lastname) as finalname
				   ,Survey_MSTProjectStatus.Name as ProjectStatus
				   ,[Account].[Code] as Code

			 FROM   [Survey_Project] Inner Join [User] on dbo.Survey_Project.ManagerID = [User].UserID
					Inner Join Survey_MSTProjectStatus on Survey_Project.StatusID = Survey_MSTProjectStatus.PRJStatusID
					INNER JOIN dbo.Account ON Survey_Project.AccountID = dbo.Account.AccountID
			 Where  Survey_Project.[AccountID] = @AccountID
			 
             order by dbo.Account.[Code] ,[ProjectID] desc

		   END
		  
			  
		   
		   ELSE IF (@SelectFlag = 'F') -- All Records

		Begin

		SELECT      [ProjectID]
				   ,Survey_Project.StatusID
				   ,Survey_Project.AccountID AS AccountID
				   ,[Reference]
				   ,[Title]
				   ,Survey_Project.Description
				   ,[ManagerID]
				   ,[MaxCandidate]
				   ,[Logo]
				   ,Survey_Project.Password
					,[QuestionnaireID]	
				   ,[StartDate]
				   ,[EndDate]
				   ,[Reminder1Date]
				   ,[Reminder2Date]
				   ,[Reminder3Date]
				   
				   ,[EmailTMPLStart]
				   ,[EmailTMPLReminder1]
				   ,[EmailTMPLReminder2]
				   ,[EmailTMPLReminder3]
				   
				   ,[FaqText]
				   ,Survey_Project.ModifyBy
				   ,Survey_Project.ModifyDate
				   ,Survey_Project.IsActive
				   ,Survey_Project.Finish_EmailID
				   ,Survey_Project.Finish_EmailID_Chkbox
				   
				   ,[User].FirstName as firstname
				   ,[User].LastName  as lastname
				   , (firstname + ' ' + lastname) as finalname
				   ,Survey_MSTProjectStatus.Name as ProjectStatus

			 FROM   [Survey_Project] Inner Join [User] on dbo.Survey_Project.ManagerID = [User].UserID
					Inner Join Survey_MSTProjectStatus on Survey_Project.StatusID = Survey_MSTProjectStatus.PRJStatusID
		            
					where Survey_Project.[AccountID] = @AccountID
		   END
		   
		   

		ELSE IF (@SelectFlag = 'C') -- Get Record Count

		Begin

		SELECT count(*) FROM [Survey_Project] where IsActive=1 and [AccountID] = @AccountID

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
				   ,Survey_Project.Password
					,[QuestionnaireID]	
				   ,[StartDate]
				   ,[EndDate]
				   ,[Reminder1Date]
				   ,[Reminder2Date]
				   ,[Reminder3Date]
				   
				   ,[EmailTMPLStart]
				   ,[EmailTMPLReminder1]
				   ,[EmailTMPLReminder2]
				   ,[EmailTMPLReminder3]
				  
					,[FaqText]
				   ,[ModifyBy]
				   ,[ModifyDate]
				   ,[IsActive]   
				   
				   ,[Finish_EmailID]  
				   ,[Finish_EmailID_Chkbox]
				   
			   FROM [Survey_Project]
			  WHERE [ProjectID] = @ProjectID

			 End

		ELSE IF (@SelectFlag = 'A') -- All Records

		Begin

		SELECT      [ProjectID]
				   ,Survey_Project.StatusID
				   ,Survey_Project.AccountID AS AccountID
				   ,[Reference]
				   ,[Title]
				   ,Survey_Project.Description
				   ,[ManagerID]
				   ,[MaxCandidate]
				   ,[Logo]
				   ,Survey_Project.Password
					,[QuestionnaireID]	
				   ,[StartDate]
				   ,[EndDate]
				   ,[Reminder1Date]
				   ,[Reminder2Date]
				   ,[Reminder3Date]
				  
				   ,[EmailTMPLStart]
				   ,[EmailTMPLReminder1]
				   ,[EmailTMPLReminder2]
				   ,[EmailTMPLReminder3]
				   
				   ,[FaqText]
				   ,Survey_Project.ModifyBy
				   ,Survey_Project.ModifyDate
				   ,Survey_Project.IsActive
				   
				   ,Survey_Project.Finish_EmailID
				   ,Survey_Project.Finish_EmailID_Chkbox
				   
				   ,[User].FirstName as firstname
				   ,[User].LastName  as lastname
				   , (firstname + ' ' + lastname) as finalname
				   ,Survey_MSTProjectStatus.Name as ProjectStatus
				   ,[Account].[Code] as Code

			 FROM   [Survey_Project] Inner Join [User] on dbo.Survey_Project.ManagerID = [User].UserID
					Inner Join Survey_MSTProjectStatus on Survey_Project.StatusID = Survey_MSTProjectStatus.PRJStatusID
					INNER JOIN dbo.Account ON Survey_Project.AccountID = dbo.Account.AccountID
			Where  Survey_Project.[AccountID] = @AccountID
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
				   ,Survey_Project.Password
					,[QuestionnaireID]	
				   ,[StartDate]
				   ,[EndDate]
				   ,[Reminder1Date]
				   ,[Reminder2Date]
				   ,[Reminder3Date]
				   
				   ,[EmailTMPLStart]
				   ,[EmailTMPLReminder1]
				   ,[EmailTMPLReminder2]
				   ,[EmailTMPLReminder3]
				   
				,[FaqText]
				   ,[ModifyBy]
				   ,[ModifyDate]
				   ,[IsActive]   
				   
				   ,[Finish_EmailID]  
				   ,[Finish_EmailID_Chkbox]
				   
			   FROM [Survey_Project]
			  WHERE [ProjectID] = @ProjectID
					

			 End

		   
		   ELSE IF (@SelectFlag = 'F') -- All Records

		Begin

		SELECT      [ProjectID]
				   ,Survey_Project.StatusID
				   ,Survey_Project.AccountID AS AccountID
				   ,[Reference]
				   ,[Title]
				   ,Survey_Project.Description
				   ,[ManagerID]
				   ,[MaxCandidate]
				   ,[Logo]
				   ,Survey_Project.Password
					,[QuestionnaireID]	
				   ,[StartDate]
				   ,[EndDate]
				   ,[Reminder1Date]
				   ,[Reminder2Date]
				   ,[Reminder3Date]
				   
				   ,[EmailTMPLStart]
				   ,[EmailTMPLReminder1]
				   ,[EmailTMPLReminder2]
				   ,[EmailTMPLReminder3]
				   
					,[FaqText]
				   ,Survey_Project.ModifyBy
				   ,Survey_Project.ModifyDate
				   ,Survey_Project.IsActive
				   
				   ,Survey_Project.Finish_EmailID
				   ,Survey_Project.Finish_EmailID_Chkbox
				   
				   ,[User].FirstName as firstname
				   ,[User].LastName  as lastname
				   , (firstname + ' ' + lastname) as finalname
				   ,Survey_MSTProjectStatus.Name as ProjectStatus

			 FROM   [Survey_Project] Inner Join [User] on dbo.Survey_Project.ManagerID = [User].UserID
					Inner Join Survey_MSTProjectStatus on Survey_Project.StatusID = Survey_MSTProjectStatus.PRJStatusID
		            
					where Survey_Project.AccountID = @AccountID
		   END
		   
		   

		ELSE IF (@SelectFlag = 'C') -- Get Record Count

		Begin

		SELECT count(*) FROM [Survey_Project] where IsActive=1

		End


	END
GO
