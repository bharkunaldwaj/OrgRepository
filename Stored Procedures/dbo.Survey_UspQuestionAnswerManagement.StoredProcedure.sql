USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspQuestionAnswerManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspQuestionAnswerManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Survey_UspQuestionAnswerManagement]

@AssignDetId int
,@QuestionID int
,@Answer varchar(5000)
,@ModifyBy int
,@ModifyDate datetime
,@IsActive int
,@OperationFlag char(1)

as

if (@OperationFlag = 'I')

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
GO
