USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAssignCategory]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspAssignCategory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspAssignCategory]
@AccountID int,
@ProjectID int,
@Name varchar(50),
@QuestionnaireID int,
@Category Varchar(MAX),
@Operation char(1)

as

--Insert
IF (@Operation = 'I')

Begin


DELETE FROM [AssignedCategories] WHERE [AccountID] = @AccountID AND  ProjectID =@ProjectID 
					AND QuestionnaireID = @QuestionnaireID
					AND RelationshipName = @Name

		INSERT INTO 
					[AssignedCategories]
				   (
					[AccountID]
					,[ProjectID]
					,[QuestionnaireID]
					,CategoryID
					,[RelationshipName]
				   )
			 
				   (
					
					SELECT   @AccountID
				   ,@ProjectID
				   ,@QuestionnaireID
				   ,items, @Name
				   FROM dbo.[TblUfSplit](@Category,',') WHERE LEN(ltrim(rtrim(items))) > 0
				   )
 



 End
 

--Delete
Else IF (@Operation = 'S')

Begin

		SELECT * FROM [AssignedCategories]
			  WHERE  AccountId=@AccountID AND ProjectID =@ProjectID 
					AND QuestionnaireID = @QuestionnaireID
					 
 


End
GO
