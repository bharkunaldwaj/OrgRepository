USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspPreviousScore]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspPreviousScore]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspPreviousScore]
@CompanyID	int=null,
@ProjectID	int=null,
@Title varchar(50)=null,
@AccountID int=null,
@ProgrammeID int=null,
@Score1Title varchar(50)=null,
@Score2Title varchar(50)=null,
@TeamName varchar(50)=null,
@PreviousScoreID int=null,
@ModifyBy int=null,
@ModifyDate datetime=null,
@IsActive int=null,
@Operation varchar(15)=null,
@AnalysisType varchar(20)=null,

@CategoryID int=null,
@QuestionID int=null,
@Score1 decimal(18,2)=null, 
@Score2 decimal(18,2)=null

as


IF (@Operation = 'GETQUEST')
Begin

select 
       sqn.QuestionID,
       sqn.Description,
       '' PreviousScoreID,
       '' AnalysisType,
       '' Score1,
       '' Score2
         
from Survey_Question sqn
inner join Survey_Questionnaire sqnr
on sqn.QuestionnaireID=sqnr.QuestionnaireID
inner join Survey_Project sp
on sqnr.QuestionnaireID=sp.QuestionnaireID
--for future purpose
--left join Survey_PrvScore_QstDetails spqd 
--on sqn.QuestionID=spqd.QuestionID

where sp.ProjectID=@ProjectID and QuestionTypeID=2
--and (spqd.AnalysisType is null or spqd.AnalysisType=@AnalysisType)
--and (spqd.PreviousScoreID is null or spqd.PreviousScoreID=@PreviousScoreID)

End

IF (@Operation = 'ADDPRVSCR')
Begin

declare @previuosId int=0
Select @previuosId=PreviousScoreID from Survey_PreviousScores 
						where AccountID=@AccountID and ProjectID=@ProjectID 
						 and ProgrammeID=@ProgrammeID and CompanyID=@CompanyID
						 --and TeamName=@TeamName and Score1Title=@Score1Title
						 --and Score2Title=@Score2Title
						 --and CompanyID=@CompanyID)
delete from Survey_PrvScore_QstDetails where PreviousScoreID IN 
(Select PreviousScoreID from Survey_PreviousScores where  AccountID=@AccountID and ProjectID=@ProjectID  and ProgrammeID=@ProgrammeID and CompanyID=@CompanyID)
						 
delete from Survey_PreviousScores where  AccountID=@AccountID and ProjectID=@ProjectID 
						 and ProgrammeID=@ProgrammeID and CompanyID=@CompanyID


insert Survey_PreviousScores(AccountID,CompanyID,ProgrammeID,ProjectID,TeamName,Score1Title,Score2Title)
values(@AccountID,@CompanyID,@ProgrammeID,@ProjectID,@TeamName,@Score1Title,@Score2Title)

;select SCOPE_IDENTITY()

End

IF (@Operation = 'ADDPRVSCRDET')
Begin

insert Survey_PrvScore_QstDetails(CategoryID,PreviousScoreID,QuestionID,AnalysisType,Score1,Score2)
values(@CategoryID,@PreviousScoreID,@QuestionID,@AnalysisType,@Score1,@Score2)

;select SCOPE_IDENTITY();

End

IF (@Operation = 'GETOLDPREV')
Begin

select * 
from Survey_PrvScore_QstDetails spqd
inner join Survey_PreviousScores sps
on spqd.PreviousScoreID=sps.PreviousScoreID
where AccountID=@AccountID and ProjectID=@ProjectID
and CompanyID=@CompanyID and ProgrammeID=@ProgrammeID
and CategoryID=@CategoryID 
--and Ltrim(Rtrim(TeamName))=@TeamName and Ltrim(Rtrim(Score1Title))=@Score1Title and Ltrim(Rtrim(Score2Title))=@Score2Title

End


IF (@Operation = 'GETPREVSCORE')
Begin

select * FROM
Survey_PreviousScores sps

where AccountID=@AccountID and ProjectID=@ProjectID
and CompanyID=@CompanyID and ProgrammeID=@ProgrammeID

--and Ltrim(Rtrim(TeamName))=@TeamName and Ltrim(Rtrim(Score1Title))=@Score1Title and Ltrim(Rtrim(Score2Title))=@Score2Title

End
GO
