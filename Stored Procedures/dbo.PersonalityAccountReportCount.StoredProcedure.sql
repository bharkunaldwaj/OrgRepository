USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityAccountReportCount]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityAccountReportCount]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[PersonalityAccountReportCount] 
@AccountID int, 
@QuestionnaireID uniqueidentifier,
@FromDate varchar(50)=null,
@ToDate varchar(50)=null,
@Company varchar(max)=null
as
Begin

--If @AccountID=0
--Begin
--	select distinct * from [vwPersonalityAccountReportCount]
--		where 1=1
--		and (CONVERT(datetime,FinishedDate ,103) >= CONVERT(datetime,@FromDate,103) or @FromDate is null )
--		and (CONVERT(datetime,FinishedDate ,103) <= CONVERT(datetime,@ToDate,103)or @ToDate is null)
--End  
--Else

--If @QuestionnaireID='00000000-0000-0000-0000-000000000000'
--Begin
--	select distinct * from [vwPersonalityAccountReportCount]
--		where 1=1
--		and AccountID=@AccountID
--		and (CONVERT(datetime,FinishedDate ,103) >= CONVERT(datetime,@FromDate,103) or @FromDate is null )
--		and (CONVERT(datetime,FinishedDate ,103) <= CONVERT(datetime,@ToDate,103)or @ToDate is null)
--END 

--Else

--If @Company is null
--Begin
--	select distinct * from [vwPersonalityAccountReportCount]
--		where 1=1 
--		and QuestionnaireID=@QuestionnaireID-- 'F830B9AE-5BA9-4ECD-9A0F-CA53A57F5E06'
--		and AccountID=@AccountID
--		and (CONVERT(datetime,FinishedDate ,103) >= CONVERT(datetime,@FromDate,103) or @FromDate is null )
--		and (CONVERT(datetime,FinishedDate ,103) <= CONVERT(datetime,@ToDate,103)or @ToDate is null)

--End
--Else
 

	select distinct * from [vwPersonalityAccountReportCountNew]
		where 1=1 
		and (@QuestionnaireID ='00000000-0000-0000-0000-000000000000' OR QuestionnaireID=@QuestionnaireID)-- 'F830B9AE-5BA9-4ECD-9A0F-CA53A57F5E06'
		and (@AccountID = 0 OR AccountID=@AccountID)
		and ( @Company is null OR @Company ='' OR Company=@Company)
		and (CONVERT(date,FinishedDate) >= CONVERT(date,@FromDate) or @FromDate is null )
		and (CONVERT(date,FinishedDate) <= CONVERT(date,@ToDate)or @ToDate is null)
 

END


/*
	Exec PersonalityAccountReportCount 48,'f830b9ae-5ba9-4ecd-9a0f-ca53a57f5e06','15/3/2012','15/3/2012'
	Exec PersonalityAccountReportCount 48,'F830B9AE-5BA9-4ECD-9A0F-CA53A57F5E06',null,null
	Exec PersonalityAccountReportCount 2,'B819E241-8580-45D0-BFE6-AF1365DF49E3',null,null,null
	Exec PersonalityAccountReportCount 2,'94BD7B93-A017-4D5C-9D11-44BB25D56BF3',null,null,'KKK2'
	Exec PersonalityAccountReportCount 2,'00000000-0000-0000-0000-000000000000',null,null,null
 
 */
GO
