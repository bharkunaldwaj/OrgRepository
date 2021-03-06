USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityGetTeamDetailsByFilter]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityGetTeamDetailsByFilter]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Raj 
-- Create date: 13 Apr 2012
-- Description:	Procedure to Get Team Details Report
-- =============================================
CReate PROCEDURE [dbo].[PersonalityGetTeamDetailsByFilter]
	@AccountID int,
	@QuestionnaireID uniqueidentifier,
	@Name Varchar(Max)=null,
	@Email Varchar(Max)=null,
	@FromDate Datetime2,
	@ToDate Datetime2,
	@Company varchar(max)=null,
	@Department varchar(max)=null
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	

    if (@AccountID <> 0) and (@QuestionnaireID is not null)
    begin
		
		if((YEAR(@FromDate)='0001')or (YEAR(@ToDate)='0001'))
		Begin
		
			select * from vwPersonalityParticiapntDetails 
			where AccountID=@AccountID 
			and QuestionnaireID=@QuestionnaireID
			and (Name like '%'+ @Name+ '%' or @Name is null)
			and (Email like '%'+ @Email+ '%' or @Email is null)
			and (Company like '%'+ @Company+ '%' or @Company is null)
			and (Department like '%'+ @Department+ '%' or @Department is null)
			and (IsFinished=1)
		End
		Else
			Begin
				select * from vwPersonalityParticiapntDetails where AccountID=@AccountID 
				and QuestionnaireID=@QuestionnaireID
				and (IsFinished=1)
				and (Name like '%'+ @Name+ '%' or @Name is null)
				and (Email like '%'+ @Email+ '%' or @Email is null)
				and (Company like '%'+ @Company+ '%' or @Company is null)
				and (Department like '%'+ @Department+ '%' or @Department is null)
				and (CONVERT(datetime,FinishedDate,103) >=  CONVERT(datetime,@FromDate,103) or @FromDate is null)
				and (CONVERT(datetime,FinishedDate,103) <=  CONVERT(datetime,@ToDate,103) or @ToDate is null )
				
			End
	End
ENd

/*
	exec PersonalityGetParticipantDetailsByFilter 48,'F830B9AE-5BA9-4ECD-9A0F-CA53A57F5E06',null,null,'01/01/0001 12:00:00 AM',null,null
	exec PersonalityGetParticipantDetailsByFilter 2,'B819E241-8580-45D0-BFE6-AF1365DF49E3',null,null,'24/03/2012 11:54:38 AM',null,null,'A'
*/
GO
