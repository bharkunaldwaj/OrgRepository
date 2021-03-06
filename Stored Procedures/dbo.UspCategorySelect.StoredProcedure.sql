USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspCategorySelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspCategorySelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspCategorySelect]

@AccountID int,
@CategoryID int,
@SelectFlag char(1)

as

IF (@AccountID != 2)

BEGIN

IF (@SelectFlag = 'I') -- Id based

Begin

	SELECT [CategoryID]
		  ,[AccountID]
		  ,[CategoryName]
		  ,[CategoryTitle]
		  ,[Description]
		  ,[Sequence]
		  ,[ExcludeFromAnalysis]
		  ,[QuestionnaireID]
		  ,[ModifiedBy]
		  ,[ModifiedDate]
		  ,[IsActive]
		  ,[ReportCategoryDescription]  
          ,[QuestionnaireCategoryDescription] 
	FROM [Category]
	WHERE [CategoryID] = @CategoryID AND
			[AccountID] = @AccountID

	End

	ELSE IF (@SelectFlag = 'A') -- All Records

	Begin

	SELECT [CategoryID]
		  ,[Category].[AccountID]
		  ,[CategoryName]
		  ,[CategoryTitle]
		  ,[Category].[Description]
		  ,[Sequence]
		  ,[ExcludeFromAnalysis]
		  ,[Category].[QuestionnaireID]
		  ,[Questionnaire].[QSTNName]
		  ,[ModifiedBy]
		  ,[ModifiedDate]
		  ,[Category].[IsActive]
		  ,[Account].[Code]
	FROM [Category] INNER JOIN
		  dbo.Account ON [Category].AccountID = dbo.Account.AccountID
			INNER JOIN [Questionnaire] on [Questionnaire].[QuestionnaireID]=[Category].[QuestionnaireID]
	WHERE [Category].[AccountID] = @AccountID

	ORDER BY dbo.Account.[Code],Sequence, [Category].ModifiedDate, CategoryName 

	END


	ELSE IF (@SelectFlag = 'S') -- Id based

	Begin

	SELECT [CategoryID]
		  ,[AccountID]
		  ,[CategoryName]
		  ,[CategoryTitle]
		  ,[Description]
		  ,[Sequence]
		  ,[ExcludeFromAnalysis]
		  ,[QuestionnaireID]
		  ,[ModifiedBy]
		  ,[ModifiedDate]
		  ,[IsActive]
	FROM [Category]
	WHERE [QuestionnaireID] = @CategoryID -- treated as Questionnaire Id
			AND	[Category].[AccountID] = @AccountID
	End

	ELSE IF (@SelectFlag = 'T')
	BEGIN
	
	select categoryid,categoryname,[CategoryTitle] from category where categoryid in
	(select distinct cateogryid from question where questiontypeid=2 and questionnaireid=@CategoryID)

	END

	ELSE IF (@SelectFlag = 'C') -- Get Record Count

	Begin

	SELECT count(*) FROM [Category] where IsActive=1 AND [AccountID] = @AccountID

	End
	
	
	ELSE IF (@SelectFlag = 'Q') -- Get Record Count

	Begin

	SELECT count(*) FROM  dbo.AssignQuestionnaire INNER JOIN
                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
                      dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID AND 
                      dbo.Project.ProjectID = dbo.Questionnaire.ProjectID INNER JOIN
                      dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID where AssignQuestionnaire.IsActive=1 AND ([User].UserID =  @AccountID) 
                      AND (dbo.AssignQuestionnaire.ProjecctID = @CategoryID)
	End
	
	
	
	ELSE IF (@SelectFlag = 'P') -- Get Record Count

	Begin

	
	
	SELECT  count(*)   
         FROM         dbo.AssignQuestionnaireParticipant INNER JOIN
                      dbo.PaticipantDetails ON dbo.AssignQuestionnaireParticipant.AssignmentID = dbo.PaticipantDetails.AssignmentID INNER JOIN
                      dbo.Account ON dbo.AssignQuestionnaireParticipant.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaireParticipant.ProjecctID = dbo.Project.ProjectID AND 
                      dbo.PaticipantDetails.ProjectID = dbo.Project.ProjectID INNER JOIN
                      dbo.[User] ON dbo.Account.AccountID = dbo.[User].AccountID AND dbo.Project.ManagerID = dbo.[User].UserID INNER JOIN
                      dbo.Questionnaire ON dbo.AssignQuestionnaireParticipant.QuestionnaireID = dbo.Questionnaire.QuestionnaireID AND 
                      dbo.Project.ProjectID = dbo.Questionnaire.ProjectID
          WHERE     (dbo.AssignQuestionnaireParticipant.AccountID = @AccountID) AND (dbo.AssignQuestionnaireParticipant.ProjecctID = @CategoryID)
	                 AND AssignQuestionnaireParticipant.IsActive = 1
	
	
	End



END

Else

	BEGIN

	IF (@SelectFlag = 'I') -- Id based

	Begin

	SELECT [CategoryID]
		  ,[AccountID]
		  ,[CategoryName]
		  ,[CategoryTitle]
		  ,[Description]
		  ,[Sequence]
		  ,[ExcludeFromAnalysis]
		  ,[QuestionnaireID]
		  ,[ModifiedBy]
		  ,[ModifiedDate]
		  ,[IsActive]
		  ,[ReportCategoryDescription]  
          ,[QuestionnaireCategoryDescription] 
	FROM [Category]
	WHERE [CategoryID] = @CategoryID 

	End

	ELSE IF (@SelectFlag = 'A') -- All Records

	Begin

	SELECT [CategoryID]
		  ,[Category].[AccountID]
		  ,[CategoryName]
		  ,[CategoryTitle]
		  ,[Category].[Description]
		  ,[Sequence]
		  ,[ExcludeFromAnalysis]
		  ,[Category].[QuestionnaireID]
		  ,[Questionnaire].[QSTNName]
		  ,[ModifiedBy]
		  ,[ModifiedDate]
		  ,[Category].[IsActive]
		  ,[Account].[Code]
	FROM [Category] INNER JOIN
		  dbo.Account ON [Category].AccountID = dbo.Account.AccountID
			INNER JOIN [Questionnaire] on [Questionnaire].[QuestionnaireID]=[Category].[QuestionnaireID]
	Where [Category].[IsActive] =1
	ORDER BY dbo.Account.[Code],Sequence, [Category].ModifiedDate, CategoryName 

	END

ELSE IF (@SelectFlag = 'Q') -- Get Record Count

	Begin

	SELECT count(*) FROM  dbo.AssignQuestionnaire INNER JOIN
                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
                      dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID AND 
                      dbo.Project.ProjectID = dbo.Questionnaire.ProjectID INNER JOIN
                      dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID
                      where AssignQuestionnaire.IsActive=1 AND ([User].UserID =  @AccountID) 
                      AND (dbo.AssignQuestionnaire.ProjecctID = @CategoryID)

	End
	
	
	ELSE IF (@SelectFlag = 'P') -- Get Record Count

	Begin

	
	
	SELECT  count(*)   
         FROM         dbo.AssignQuestionnaireParticipant INNER JOIN
                      dbo.PaticipantDetails ON dbo.AssignQuestionnaireParticipant.AssignmentID = dbo.PaticipantDetails.AssignmentID INNER JOIN
                      dbo.Account ON dbo.AssignQuestionnaireParticipant.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaireParticipant.ProjecctID = dbo.Project.ProjectID AND 
                      dbo.PaticipantDetails.ProjectID = dbo.Project.ProjectID INNER JOIN
                      dbo.[User] ON dbo.Account.AccountID = dbo.[User].AccountID AND dbo.Project.ManagerID = dbo.[User].UserID INNER JOIN
                      dbo.Questionnaire ON dbo.AssignQuestionnaireParticipant.QuestionnaireID = dbo.Questionnaire.QuestionnaireID AND 
                      dbo.Project.ProjectID = dbo.Questionnaire.ProjectID
          WHERE     
	                  AssignQuestionnaireParticipant.IsActive = 1
	
	
	End
	
	
	
	ELSE IF (@SelectFlag = 'T')
	BEGIN
	
	select categoryid,categoryname,[CategoryTitle] from category where categoryid in
	(select distinct cateogryid from question where questiontypeid=2 and questionnaireid=@CategoryID)

	END
	
	
	ELSE IF (@SelectFlag = 'S') -- Id based

	Begin

	SELECT [CategoryID]
		  ,[AccountID]
		  ,[CategoryName]
		  ,[CategoryTitle]
		  ,[Description]
		  ,[Sequence]
		  ,[ExcludeFromAnalysis]
		  ,[QuestionnaireID]
		  ,[ModifiedBy]
		  ,[ModifiedDate]
		  ,[IsActive]
	FROM [Category]
	WHERE [QuestionnaireID] = @CategoryID -- treated as Questionnaire Id

	End




	ELSE IF (@SelectFlag = 'C') -- Get Record Count

	Begin

	SELECT count(*) FROM [Category] where IsActive=1

	End


End
GO
