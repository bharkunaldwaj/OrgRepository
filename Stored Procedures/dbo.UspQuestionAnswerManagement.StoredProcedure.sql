USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspQuestionAnswerManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspQuestionAnswerManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspQuestionAnswerManagement]

@AssignDetId int
,@QuestionID int
,@Answer nvarchar(MAX)
,@ModifyBy int
,@ModifyDate datetime
,@IsActive int
,@OperationFlag char(1)

as

if (@OperationFlag = 'I')

Begin

delete from [QuestionAnswer] where [AssignDetId]= @AssignDetId and [QuestionID]=@QuestionID
--declare @sql varchar(max)
--INSERT INTO [QuestionAnswer]
--           ([AssignDetId]
--           ,[QuestionID]
--           ,[Answer]
--           ,[ModifyBy]
--           ,[ModifyDate]
--           ,[IsActive])
--     VALUES
--           (@AssignDetId
--			,@QuestionID
--			,@Answer
--			,@ModifyBy
--			,@ModifyDate
--			,@IsActive)
set @Answer = N'' + @Answer +''
INSERT INTO [QuestionAnswer]
           ([AssignDetId]
           ,[QuestionID]
           ,[Answer]
           ,[ModifyBy]
           ,[ModifyDate]
           ,[IsActive])
     VALUES (@AssignDetId
			,@QuestionID
			,CONVERT(nvarchar(MAX),@Answer)
			,@ModifyBy
			,getdate()
			,@IsActive)
End
GO
