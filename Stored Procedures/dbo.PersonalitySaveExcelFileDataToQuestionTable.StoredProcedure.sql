USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalitySaveExcelFileDataToQuestionTable]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalitySaveExcelFileDataToQuestionTable]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

CREATE PROCEDURE [dbo].[PersonalitySaveExcelFileDataToQuestionTable] 
	@AccountCode NVarChar (50),
    @QuestionnairesCode NVarChar (20),
    @MainText nvarchar (2500),
    @Sequence NVarChar (5),
	@RedText NVarChar (2500),
    @RedSequence NVarChar (5),
    @YellowText nvarchar (2500),
    @YellowSequence NVarChar (5),
    @GreenText NVarChar (2500),
    @GreenSequence NVarChar (5),
    @BlueText NVarChar (2500),
    @BlueSequence NVarChar (5)

AS

BEGIN--4

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @AccountID as int
	Declare @QuestionnairesID as uniqueidentifier
	Declare @QuestionID as uniqueidentifier
	
	SELECT @AccountID = AccountID FROM Account  where Code = @AccountCode
	SELECT @QuestionnairesID = UniqueID from dbo.PersonalityQuestionnaires where Code = @QuestionnairesCode
	
	set @QuestionID =NEWID()
	IF (Len(@MainText)>0 and Len(@Sequence)>0 and 
		Len(@RedText)>0 and Len(@RedSequence)>0 and
		Len(@YellowText)>0 and Len(@YellowSequence)>0 and 
		Len(@GreenText)>0 and Len(@GreenSequence)>0 and 
		Len(@BlueText)>0 and Len(@BlueSequence)>0 )
	Begin--3
		IF  (@AccountID is not null)
	BEGIN--2
		 If (@QuestionnairesID is not null) and  (@AccountID is not null)
		 
		 Begin--1
		   -- Insert statements here
		   
		   --Insert into Question Table
			INSERT into  PersonalityQuestions(UniqueID,CreatedDate,CreatedBy,AccountID,QuestionnaireID,Sequence,MainText)
			values (@QuestionID,GETDATE(), newid(),@AccountID,@QuestionnairesID,Convert(int,coalesce(@Sequence,0)),@MainText)
	
			--Insert into QusetionChoices Tabel on the basis of Check Question ID
			INSERT INTO PersonalityQuestionChoices
		   ([UniqueID],[CreatedDate],[CreatedBy],[QuestionID],[Text],[ColorCode],[Sequence])
			VALUES(NEWID(),GETDATE(),NEWID(),@QuestionID,@RedText,'Red',Convert(int,coalesce(@RedSequence,0)))
			
			INSERT INTO PersonalityQuestionChoices
		   ([UniqueID],[CreatedDate],[CreatedBy],[QuestionID],[Text],[ColorCode],[Sequence])
			VALUES(NEWID(),GETDATE(),NEWID(),@QuestionID,@YellowText,'Yellow',Convert(int,coalesce(@YellowSequence,0)))
			
			INSERT INTO PersonalityQuestionChoices
		   ([UniqueID],[CreatedDate],[CreatedBy],[QuestionID],[Text],[ColorCode],[Sequence])
			VALUES(NEWID(),GETDATE(),NEWID(),@QuestionID,@GreenText,'Green',Convert(int,coalesce(@GreenSequence,0)))
			
			INSERT INTO PersonalityQuestionChoices
		   ([UniqueID],[CreatedDate],[CreatedBy],[QuestionID],[Text],[ColorCode],[Sequence])
			VALUES(NEWID(),GETDATE(),NEWID(),@QuestionID,@BlueText,'Blue',Convert(int,coalesce(@BlueSequence,0)))

		 End--1
	END--2
	END--3
END--4
GO
