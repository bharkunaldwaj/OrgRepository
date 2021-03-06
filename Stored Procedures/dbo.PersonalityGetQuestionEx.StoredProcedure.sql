USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityGetQuestionEx]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityGetQuestionEx]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[dbo].[PersonalityGetQuestionEx]'aac203b8-c41f-4b31-a18e-00e19e29289c',17,GetQuestionByQuestionnaireId
CREATE ProC [dbo].[PersonalityGetQuestionEx] --'93AD3265-E5C0-40C2-9778-675A4173667B';
(
@QuestionnaireId uniqueidentifier = null,
@UserId uniqueidentifier = null,
@QuestionId uniqueidentifier = null,
@ChoiceId nvarchar(200) =null,
@Action varchar(200)=null,
@PageIndex int =null,
@ScoreValue int = null,
@Title varchar(200) = null,
@PageSize int = null,
@SessionData varchar(400) = null,
@FreeTextAnswer nvarchar(1500) = null

)
As 
BEGIN
DECLARE  @upperBand INT,@lowerBand INT 
DECLARE   @StrSQL nvarchar(max)

IF(@Action='GetSessionData')
	 BEGIN
		SELECT UserId,A.AccountID,GroupID,A.Code,SessionData,LoginTime FROM [User] U
		INNER JOIN Account A ON U.AccountID = A.AccountID
		 WHERE SessionData=@SessionData
	 END

IF(@Action='TitleUpdate')
	Begin
		UPDATE PersonalityParticiapntDetails SET Honorific=@Title WHERE UniqueID=@UserId
	End

IF(@Action='GetQuestionByQuestionnaireId')
	BEGIN 
     SET @PageSize=1
			SET @LowerBand =(@PageIndex-1)* @PageSize        
				SET @UpperBand=(@PageIndex*@PageSize)+1;        
		WITH tempPagedProducts AS      
						(
						Select	MainText,Q.Sequence,Q.UniqueID,S.[Type],Q.QuestionType
						,Row_Number() over (ORDER BY Q.Sequence asc  ) AS RowNumber 
						 from PersonalityQuestions Q
						 inner join PersonalityQuestionnaires S on S.UniqueID=Q.QuestionnaireID
						
						where Q.QuestionnaireID=@QuestionnaireId
						)      
				SELECT (Select COUNT(*) FROM [tempPagedProducts]) AS TotalRecords, 
				MainText,
				Sequence,RowNumber,UniqueID,[Type],QuestionType
				  FROM tempPagedProducts 
				WHERE  RowNumber> CAST(@LowerBand AS VARCHAR(100)) AND RowNumber <+CAST(@UpperBand AS VARCHAR(100))
		
	
	
	
	END
IF(@Action='GetChoicesByQuestionId')
	BEGIN 
      
			select UniqueID,[Text],Sequence,
			isnull((select ScoreValue from PersonalityQuestionsAnswers
			where QuestionnaireID=@QuestionnaireId and QuestionID=PersonalityQuestionChoices.QuestionID and QuestionChoiceID=PersonalityQuestionChoices.UniqueID and 
			ParticiapntDetailsID=@UserId),'')[score],
			
			(select TOP 1 FreeTextAnswer from PersonalityQuestionsAnswers
			where QuestionnaireID=@QuestionnaireId and QuestionID=PersonalityQuestionChoices.QuestionID and 
			ParticiapntDetailsID=@UserId)[FreeTextAnswer]
			
			from PersonalityQuestionChoices where QuestionID=@QuestionId
			order by Sequence asc
	
	END	

IF(@Action='GetStartingEndingText')
	BEGIN 
      
      select Epilogue,Prologue from PersonalityQuestionnaires where UniqueID=@QuestionnaireId
     
	
	END	 
IF(@Action='GetQuestionpercentage')
	BEGIN 
      
      declare @totalQuestion int,@totalAns int
      
      set @totalQuestion=(select COUNT(*) from PersonalityQuestions where QuestionnaireID=@QuestionnaireId)
      set @totalAns=(select COUNT(*) from PersonalityQuestions
                       where UniqueID in(select distinct QuestionID from PersonalityQuestionsAnswers  
                       where QuestionnaireID=@QuestionnaireId and ParticiapntDetailsID=@UserId))
      
      select cast(ISNULL(@totalAns,0)*100/ISNULL(@totalQuestion,1) as Int)	     
      	
	END		

IF(@Action='AddQuestionAnswers')
	BEGIN 
      
      IF(@ChoiceId !='00000000-0000-0000-0000-000000000000')
      BEGIN
      
		  if not exists(select 1 from PersonalityQuestionsAnswers where QuestionnaireID=@QuestionnaireId and QuestionID=@QuestionId
		  and QuestionChoiceID=@ChoiceId and ParticiapntDetailsID=@UserId)
		  BEGIN
		  INSERT INTO [PersonalityQuestionsAnswers]
			   ([UniqueID]
			   ,[CreatedDate]
			   ,[CreatedBy]
			   ,[UpdatedDate]
			   ,[UpdatedBy]
			   ,[ParticiapntDetailsID]
			   ,[QuestionnaireID]
			   ,[QuestionID]
			   ,[QuestionChoiceID]
			   ,[ScoreValue]
			   ,[FreeTextAnswer])
		 VALUES
			   (newid(),GETDATE(),@UserId,GETDATE(),@UserId,@UserId,@QuestionnaireId,@QuestionId,@ChoiceId,@ScoreValue,@FreeTextAnswer)
		  END
		  ELSE
		  BEGIN
	       
	        
				Update [PersonalityQuestionsAnswers]
				set ScoreValue=@ScoreValue 
				where QuestionnaireID=@QuestionnaireId and QuestionID=@QuestionId
				and QuestionChoiceID=@ChoiceId and ParticiapntDetailsID=@UserId		
	      
		  END
	  END
	  ELSE
	  BEGIN
		
		if not exists(select 1 from PersonalityQuestionsAnswers where QuestionnaireID=@QuestionnaireId and QuestionID=@QuestionId
		  and ParticiapntDetailsID=@UserId)
		  BEGIN
		  INSERT INTO [PersonalityQuestionsAnswers]
			   ([UniqueID]
			   ,[CreatedDate]
			   ,[CreatedBy]
			   ,[UpdatedDate]
			   ,[UpdatedBy]
			   ,[ParticiapntDetailsID]
			   ,[QuestionnaireID]
			   ,[QuestionID]
			   ,[QuestionChoiceID]
			   ,[ScoreValue]
			   ,[FreeTextAnswer])
		 VALUES
			   (newid(),GETDATE(),@UserId,GETDATE(),@UserId,@UserId,@QuestionnaireId,@QuestionId,@ChoiceId,@ScoreValue,@FreeTextAnswer)
		  END
		  ELSE
		  BEGIN	       
	        
				Update [PersonalityQuestionsAnswers]
				set ScoreValue=@ScoreValue ,
				[FreeTextAnswer]=@FreeTextAnswer,
				[QuestionChoiceID]=@ChoiceId
				where QuestionnaireID=@QuestionnaireId and QuestionID=@QuestionId
				and ParticiapntDetailsID=@UserId			      
		  END
		
	  END
		
	END	
	
IF(@Action='UpdateFinishedDate')
      BEGIN
       
			Update PersonalityParticiapntDetails
			set FinishedDate=GETDATE(),IsFinished=1
			where UniqueID=@UserId
      
	END	
	
END
GO
