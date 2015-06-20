USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAssignCategory]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspAssignCategory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspAssignCategory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspAssignCategory]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

 

CREATE procedure [dbo].[UspAssignCategory]
@AccountID int,
@ProjectID int,
@Name varchar(50),
@QuestionnaireID int,
@Category Varchar(MAX),
@Operation char(1)

as

--Insert
IF (@Operation = ''I'')

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
				   FROM dbo.[TblUfSplit](@Category,'','') WHERE LEN(ltrim(rtrim(items))) > 0
				   )
 



 End
 

--Delete
Else IF (@Operation = ''S'')

Begin

		SELECT * FROM [AssignedCategories]
			  WHERE  AccountId=@AccountID AND ProjectID =@ProjectID 
					AND QuestionnaireID = @QuestionnaireID
					 
 


End

' 
END
GO
