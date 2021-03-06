USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspProjectCopy]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspProjectCopy]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Survey_UspProjectCopy]
	@SupAdmin       int,
	@ProjID		    varchar(5000),
	@AccountID      int,
	@Operation	    char(1)

AS

	--Insert Operation
	set @SupAdmin=2
	
	IF (@Operation = 'C')
	
	BEGIN
	
	INSERT [Survey_Project]  
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
           ,[ModifyBy]
           ,[ModifyDate]
           ,[IsActive]
           ,[Finish_EmailID]
           ,[Finish_EmailID_Chkbox])
	
			( SELECT   [StatusID],[Reference],[Title],[Description],@AccountID,[ManagerID],
			[MaxCandidate],[Logo],[Password],[StartDate],[EndDate],[Reminder1Date],
			[Reminder2Date],[Reminder3Date],[ReportAvaliableFrom] ,[ReportAvaliableTo]
			 ,[EmailTMPLStart],[EmailTMPLReminder1],[EmailTMPLReminder2],[EmailTMPLReminder3],
			 [ModifyBy],getdate(),[IsActive],[Finish_EmailID],[Finish_EmailID_Chkbox]
						FROM dbo.Survey_Project 
			WHERE ProjectID IN (select [value] from fn_csvtotable(@ProjID)) AND Survey_Project.[AccountID] = @SupAdmin )
			
			
			
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
          
						FROM dbo.Survey_Category 
			WHERE CategoryID IN (SELECT DISTINCT dbo.Survey_Question.CateogryID AS CatID
        FROM         dbo.Survey_Project INNER JOIN
       dbo.Survey_Questionnaire ON dbo.Survey_Project.ProjectID = dbo.Survey_Questionnaire.ProjectID INNER JOIN
       dbo.Survey_Question ON dbo.Survey_Questionnaire.QuestionnaireID = dbo.Survey_Question.QuestionnaireID
       WHERE     (dbo.Survey_Project.ProjectID IN (select [value] from fn_csvtotable(@ProjID))) AND [Survey_Category].[AccountID] = @SupAdmin	)) 
			
			
			
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
			   ,[IsActive]
			   
			   )
	
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
						FROM dbo.Survey_Questionnaire
		WHERE ProjectID IN (select [value] from fn_csvtotable(@ProjID)) AND Survey_Questionnaire.[AccountID] = @SupAdmin)
		
		
		
		INSERT [Survey_Question]  
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
           
           ,[ModifyBy]
           ,getdate()
           ,[IsActive]
						FROM dbo.Survey_Question
		WHERE CateogryID IN (SELECT DISTINCT dbo.Survey_Question.CateogryID AS CatID
        FROM         dbo.Survey_Project INNER JOIN
       dbo.Survey_Questionnaire ON dbo.Survey_Project.ProjectID = dbo.Survey_Questionnaire.ProjectID INNER JOIN
       dbo.Survey_Question ON dbo.Survey_Questionnaire.QuestionnaireID = dbo.Survey_Question.QuestionnaireID
       WHERE     (dbo.Survey_Project.ProjectID IN (select [value] from fn_csvtotable(@ProjID))) AND Survey_Question.[AccountID] = @SupAdmin	))
		
		
		
					
	END
	
	SELECT 1
GO
