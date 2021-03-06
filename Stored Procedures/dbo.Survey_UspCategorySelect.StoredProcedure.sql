USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspCategorySelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspCategorySelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspCategorySelect]

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
		  ,IntroImgFileName
		  ,IntroPdfFileName
	FROM [Survey_Category]
	WHERE [CategoryID] = @CategoryID AND
			[AccountID] = @AccountID

	End

	ELSE IF (@SelectFlag = 'A') -- All Records

	Begin

	SELECT [CategoryID]
		  ,[Survey_Category].[AccountID]
		  ,[CategoryName]
		  ,[CategoryTitle]
		  ,[Survey_Category].[Description]
		  ,[Sequence]
		  ,[ExcludeFromAnalysis]
		  ,[Survey_Category].[QuestionnaireID]
		  ,[Survey_Questionnaire].[QSTNName]
		  ,[ModifiedBy]
		  ,[ModifiedDate]
		  ,[Survey_Category].[IsActive]
		  ,[Account].[Code]
		  ,[Survey_Category].IntroImgFileName
		  ,[Survey_Category].IntroPdfFileName
	FROM [Survey_Category] INNER JOIN
		  dbo.Account ON [Survey_Category].AccountID = dbo.Account.AccountID
			INNER JOIN [Survey_Questionnaire] on [Survey_Questionnaire].[QuestionnaireID]=[Survey_Category].[QuestionnaireID]
	WHERE [Survey_Category].[AccountID] = @AccountID

	ORDER BY dbo.Account.[Code],Sequence, [Survey_Category].ModifiedDate, CategoryName 

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
		  ,IntroImgFileName
		  ,IntroPdfFileName
	FROM [Survey_Category]
	WHERE [QuestionnaireID] = @CategoryID -- treated as Questionnaire Id
			AND	[Survey_Category].[AccountID] = @AccountID
	End

	ELSE IF (@SelectFlag = 'T')
	BEGIN
	
	select categoryid,categoryname,[CategoryTitle] from Survey_category where categoryid in
	(select distinct cateogryid from Survey_question where questiontypeid=2 and questionnaireid=@CategoryID)

	END

	ELSE IF (@SelectFlag = 'C') -- Get Record Count

	Begin

	SELECT count(*) FROM [Survey_Category] where IsActive=1 AND [AccountID] = @AccountID

	End
	
	
	ELSE IF (@SelectFlag = 'Q') -- Get Record Count

	Begin

	SELECT count(*) FROM  dbo.Survey_AssignQuestionnaire INNER JOIN
                      dbo.Survey_AssignmentDetails ON dbo.Survey_AssignQuestionnaire.AssignmentID = dbo.Survey_AssignmentDetails.AssignmentID INNER JOIN
                      dbo.Survey_Project ON dbo.Survey_AssignQuestionnaire.ProjecctID = dbo.Survey_Project.ProjectID INNER JOIN
                      dbo.Account ON dbo.Survey_AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Survey_Questionnaire ON dbo.Survey_AssignQuestionnaire.QuestionnaireID = dbo.Survey_Questionnaire.QuestionnaireID AND 
                      dbo.Survey_Project.ProjectID = dbo.Survey_Questionnaire.ProjectID INNER JOIN
                      dbo.[User] ON dbo.Survey_AssignQuestionnaire.AccountID = dbo.[User].AccountID where Survey_AssignQuestionnaire.IsActive=1 AND ([User].UserID =  @AccountID) 
                      AND (dbo.Survey_AssignQuestionnaire.ProjecctID = @CategoryID)
	End
	
	
	
	ELSE IF (@SelectFlag = 'P') -- Get Record Count

	Begin

	
	
	SELECT  count(*)   
         FROM         dbo.Survey_AssignQuestionnaireParticipant INNER JOIN
                      dbo.Survey_PaticipantDetails ON dbo.Survey_AssignQuestionnaireParticipant.AssignmentID = dbo.Survey_PaticipantDetails.AssignmentID INNER JOIN
                      dbo.Account ON dbo.Survey_AssignQuestionnaireParticipant.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Survey_Project ON dbo.Survey_AssignQuestionnaireParticipant.ProjecctID = dbo.Survey_Project.ProjectID AND 
                      dbo.Survey_PaticipantDetails.ProjectID = dbo.Survey_Project.ProjectID INNER JOIN
                      dbo.[User] ON dbo.Account.AccountID = dbo.[User].AccountID AND dbo.Survey_Project.ManagerID = dbo.[User].UserID INNER JOIN
                      dbo.Survey_Questionnaire ON dbo.Survey_AssignQuestionnaireParticipant.QuestionnaireID = dbo.Survey_Questionnaire.QuestionnaireID AND 
                      dbo.Survey_Project.ProjectID = dbo.Survey_Questionnaire.ProjectID
          WHERE     (dbo.Survey_AssignQuestionnaireParticipant.AccountID = @AccountID) AND (dbo.Survey_AssignQuestionnaireParticipant.ProjecctID = @CategoryID)
	                 AND Survey_AssignQuestionnaireParticipant.IsActive = 1
	
	
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
		  ,IntroImgFileName
		  ,IntroPdfFileName
	FROM [Survey_Category]
	WHERE [CategoryID] = @CategoryID 

	End

	ELSE IF (@SelectFlag = 'A') -- All Records

	Begin

	SELECT [CategoryID]
		  ,[Survey_Category].[AccountID]
		  ,[CategoryName]
		  ,[CategoryTitle]
		  ,[Survey_Category].[Description]
		  ,[Sequence]
		  ,[ExcludeFromAnalysis]
		  ,[Survey_Category].[QuestionnaireID]
		  ,[Survey_Questionnaire].[QSTNName]
		  ,[ModifiedBy]
		  ,[ModifiedDate]
		  ,[Survey_Category].[IsActive]
		  ,[Account].[Code]
	FROM [Survey_Category] INNER JOIN
		  dbo.Account ON [Survey_Category].AccountID = dbo.Account.AccountID
			INNER JOIN [Survey_Questionnaire] on [Survey_Questionnaire].[QuestionnaireID]=[Survey_Category].[QuestionnaireID]
	Where [Survey_Category].[IsActive] =1
	ORDER BY dbo.Account.[Code],Sequence, [Survey_Category].ModifiedDate, CategoryName 

	END

ELSE IF (@SelectFlag = 'Q') -- Get Record Count

	Begin

	SELECT count(*) FROM  dbo.Survey_AssignQuestionnaire INNER JOIN
                      dbo.Survey_AssignmentDetails ON dbo.Survey_AssignQuestionnaire.AssignmentID = dbo.Survey_AssignmentDetails.AssignmentID INNER JOIN
                      dbo.Survey_Project ON dbo.Survey_AssignQuestionnaire.ProjecctID = dbo.Survey_Project.ProjectID INNER JOIN
                      dbo.Account ON dbo.Survey_AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Survey_Questionnaire ON dbo.Survey_AssignQuestionnaire.QuestionnaireID = dbo.Survey_Questionnaire.QuestionnaireID AND 
                      dbo.Survey_Project.ProjectID = dbo.Survey_Questionnaire.ProjectID INNER JOIN
                      dbo.[User] ON dbo.Survey_AssignQuestionnaire.AccountID = dbo.[User].AccountID
                      where Survey_AssignQuestionnaire.IsActive=1 AND ([User].UserID =  @AccountID) 
                      AND (dbo.Survey_AssignQuestionnaire.ProjecctID = @CategoryID)

	End
	
	
	ELSE IF (@SelectFlag = 'P') -- Get Record Count

	Begin

	
	
	SELECT  count(*)   
         FROM         dbo.Survey_AssignQuestionnaireParticipant INNER JOIN
                      dbo.Survey_PaticipantDetails ON dbo.Survey_AssignQuestionnaireParticipant.AssignmentID = dbo.Survey_PaticipantDetails.AssignmentID INNER JOIN
                      dbo.Account ON dbo.Survey_AssignQuestionnaireParticipant.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Survey_Project ON dbo.Survey_AssignQuestionnaireParticipant.ProjecctID = dbo.Survey_Project.ProjectID AND 
                      dbo.Survey_PaticipantDetails.ProjectID = dbo.Survey_Project.ProjectID INNER JOIN
                      dbo.[User] ON dbo.Account.AccountID = dbo.[User].AccountID AND dbo.Survey_Project.ManagerID = dbo.[User].UserID INNER JOIN
                      dbo.Survey_Questionnaire ON dbo.Survey_AssignQuestionnaireParticipant.QuestionnaireID = dbo.Survey_Questionnaire.QuestionnaireID AND 
                      dbo.Survey_Project.ProjectID = dbo.Survey_Questionnaire.ProjectID
          WHERE     
	                  Survey_AssignQuestionnaireParticipant.IsActive = 1
	
	
	End
	
	
	
	ELSE IF (@SelectFlag = 'T')
	BEGIN
	
	select categoryid,categoryname,[CategoryTitle] from Survey_category where categoryid in
	(select distinct cateogryid from Survey_question where questiontypeid=2 and questionnaireid=@CategoryID)

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
	FROM [Survey_Category]
	WHERE [QuestionnaireID] = @CategoryID -- treated as Questionnaire Id

	End




	ELSE IF (@SelectFlag = 'C') -- Get Record Count

	Begin

	SELECT count(*) FROM [Survey_Category] where IsActive=1

	End


End
GO
