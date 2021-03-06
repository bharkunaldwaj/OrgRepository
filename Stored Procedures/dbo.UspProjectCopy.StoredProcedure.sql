USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspProjectCopy]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspProjectCopy]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UspProjectCopy]
	@SupAdmin       int,
	@ProjID		    varchar(5000),
	@AccountID      int,
	@Operation	    char(1)

AS

	--Insert Operation
	set @SupAdmin=2
	
	IF (@Operation = 'C')
	
	BEGIN
	
	INSERT [Project]  
           ([StatusID]
           ,[Reference]
           ,[Title]
           ,[Description]
		   ,[AccountID]
           ,[ManagerID]
           ,[MaxCandidate]
           ,[Logo]
           ,[Password]
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
           ,[EmailTMPPartReminder1] 
           ,[EmailTMPPartReminder2] 
           ,[EmailTMPManager]
           ,[ModifyBy]
           ,[ModifyDate]
           ,[IsActive])
	
			( SELECT   [StatusID],[Reference],[Title],[Description],@AccountID,[ManagerID],[MaxCandidate],[Logo],[Password],[StartDate],[EndDate],[Reminder1Date],[Reminder2Date],[Reminder3Date],[ReportAvaliableFrom] ,[ReportAvaliableTo] ,[EmailTMPLStart],[EmailTMPLReminder1],[EmailTMPLReminder2],[EmailTMPLReminder3],[EmailTMPLReportAvalibale],[EmailTMPPartReminder1],[EmailTMPPartReminder2],[EmailTMPManager],[ModifyBy],getdate(),[IsActive]
						FROM dbo.Project 
			WHERE ProjectID IN (select [value] from fn_csvtotable(@ProjID)) AND Project.[AccountID] = @SupAdmin )
			
			
			
			INSERT [Category]  
           ([CategoryName]
           ,[AccountID]
           ,[Description]
           ,[Sequence]
           ,[ExcludeFromAnalysis]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[IsActive])
	
			( SELECT   [CategoryName],@AccountID,[Description]
           ,[Sequence]
           ,[ExcludeFromAnalysis]
           ,[ModifiedBy]
           ,getdate()
           ,[IsActive]
						FROM dbo.Category 
			WHERE CategoryID IN (SELECT DISTINCT dbo.Question.CateogryID AS CatID
        FROM         dbo.Project INNER JOIN
       dbo.Questionnaire ON dbo.Project.ProjectID = dbo.Questionnaire.ProjectID INNER JOIN
       dbo.Question ON dbo.Questionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
       WHERE     (dbo.Project.ProjectID IN (select [value] from fn_csvtotable(@ProjID))) AND [Category].[AccountID] = @SupAdmin	)) 
			
			
			
			INSERT [Questionnaire]  
           (    [AccountID]
			   ,[QSTNType]
			   ,[QSTNCode]
			   ,[QSTNName]
			   ,[QSTNDescription]
			   ,[ProjectID]
			   ,[ManagerID]			   
			   ,[QSTNPrologue]
			   ,[QSTNEpilogue]
			   ,[ModifyBy]			   
			   ,[ModifyDate]
			   ,[IsActive])
	
			( SELECT   @AccountID
			   ,[QSTNType]
			   ,[QSTNCode]
			   ,[QSTNName]
			   ,[QSTNDescription]
			   ,[ProjectID]
			   ,[ManagerID]			   
			   ,[QSTNPrologue]
			   ,[QSTNEpilogue]
			   ,[ModifyBy]			   
			   ,getdate()
			   ,[IsActive]
						FROM dbo.Questionnaire
		WHERE ProjectID IN (select [value] from fn_csvtotable(@ProjID)) AND Questionnaire.[AccountID] = @SupAdmin)
		
		
		
		INSERT [Question]  
           (   [AccountID]
           ,[CompanyID]
           ,[QuestionnaireID]
           ,[QuestionTypeID]
           ,[CateogryID]
           ,[Sequence]
           ,[Validation]
           ,[ValidationText]
           ,[Title]
           ,[Description]
           ,[Hint]
           ,[Token]
           ,[TokenText]
           ,[LengthMIN]
           ,[LengthMAX]
           ,[Multiline]
           ,[LowerLabel]
           ,[UpperLabel]
           ,[LowerBound]
           ,[UpperBound]
           ,[Increment]
           ,[Reverse]
           ,[ModifyBy]
           ,[ModifyDate]
           ,[IsActive])
	
			( SELECT   @AccountID
			   ,[CompanyID]
           ,[QuestionnaireID]
           ,[QuestionTypeID]
           ,[CateogryID]
           ,[Sequence]
           ,[Validation]
           ,[ValidationText]
           ,[Title]
           ,[Description]
           ,[Hint]
           ,[Token]
           ,[TokenText]
           ,[LengthMIN]
           ,[LengthMAX]
           ,[Multiline]
           ,[LowerLabel]
           ,[UpperLabel]
           ,[LowerBound]
           ,[UpperBound]
           ,[Increment]
           ,[Reverse]
           ,[ModifyBy]
           ,getdate()
           ,[IsActive]
						FROM dbo.Question
		WHERE CateogryID IN (SELECT DISTINCT dbo.Question.CateogryID AS CatID
        FROM         dbo.Project INNER JOIN
       dbo.Questionnaire ON dbo.Project.ProjectID = dbo.Questionnaire.ProjectID INNER JOIN
       dbo.Question ON dbo.Questionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
       WHERE     (dbo.Project.ProjectID IN (select [value] from fn_csvtotable(@ProjID))) AND Question.[AccountID] = @SupAdmin	))
		
		
		
					
	END
	
	SELECT 1
GO
