USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[subPersonalityGetScoreDetails]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[subPersonalityGetScoreDetails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[subPersonalityGetScoreDetails]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[subPersonalityGetScoreDetails]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure [dbo].[subPersonalityGetScoreDetails]  
	@QuestionnaireID uniqueidentifier
	,@FromDate varchar(50)
	,@ToDate varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
SET NOCOUNT ON;

declare @QuestionnaireName varchar(10)
declare @AccountCode varchar(10)
declare @FromDatedatetime as DateTime
declare @ToDatedatetime as DateTime

set @ToDatedatetime=CONVERT(datetime,@ToDate)
set @FromDatedatetime=CONVERT(datetime,@FromDate)

Select @AccountCode=Code from Account where AccountID=(Select AccountID from PersonalityQuestionnaires where UniqueID=@QuestionnaireID)
Select @QuestionnaireName=Name from PersonalityQuestionnaires where UniqueID=@QuestionnaireID

Select  
	@AccountCode as AccountCode ,
	@QuestionnaireName as Questionnaire,
	CONVERT(Varchar,@FromDatedatetime,107) as FromDate,
	CONVERT(Varchar,@ToDatedatetime,107) as ToDate
	

End

--  exec [subPersonalityGetScoreDetails] ''5c29817f-522b-4d41-b173-7ba98f399e2f'',''2012-03-23 08:48:49.530'',''2012-03-29 08:48:49.530''
' 
END
GO
