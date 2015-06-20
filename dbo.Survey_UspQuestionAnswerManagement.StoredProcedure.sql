USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspQuestionAnswerManagement]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspQuestionAnswerManagement]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspQuestionAnswerManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspQuestionAnswerManagement]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create proc [dbo].[Survey_UspQuestionAnswerManagement]

@AssignDetId int
,@QuestionID int
,@Answer varchar(5000)
,@ModifyBy int
,@ModifyDate datetime
,@IsActive int
,@OperationFlag char(1)

as

if (@OperationFlag = ''I'')

Begin

delete from [Survey_QuestionAnswer] where [AssignDetId]= @AssignDetId and [QuestionID]=@QuestionID

INSERT INTO [Survey_QuestionAnswer]
           ([AssignDetId]
           ,[QuestionID]
           ,[Answer]
           ,[ModifyBy]
           ,[ModifyDate]
           ,[IsActive])
     VALUES
           (@AssignDetId
			,@QuestionID
			,@Answer
			,@ModifyBy
			,@ModifyDate
			,@IsActive)

End
' 
END
GO
