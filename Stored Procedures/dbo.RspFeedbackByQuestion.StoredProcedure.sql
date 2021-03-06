USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspFeedbackByQuestion]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspFeedbackByQuestion]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RspFeedbackByQuestion] 
	@accountid varchar(50),
	@projectid varchar(50),
	@programmeid varchar(50),
	@type varchar(2)
AS
BEGIN
	if(@type = 'qg')
	Begin
		select q.QuestionID,q.Title as "QuestionTitle", q.Description,u.FirstName+' '+u.LastName as ParticipantName,pg.ProgrammeName,p.Title,
		cast(sum(cast(REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),'&','') as decimal(12,1))) / count(*) as decimal(12,1)) as Answer,'A' as Grp
		from dbo.AssignQuestionnaire aq
		inner join AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
		inner join QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
		inner join Question q on q.QuestionID = qa.QuestionID
		inner join Category c on c.CategoryID = q.CateogryID
		inner join [User] u on u.UserID = aq.targetPersonID
		inner join Programme pg on pg.ProgrammeID = aq.ProgrammeID
		inner join Project p on p.ProjectID = aq.ProjecctID
		where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and ad.RelationShip != 'Self' and
		aq.accountID = @accountid and aq.ProgrammeID = @programmeid
		Group By q.QuestionID,q.Title,q.Description ,u.FirstName+' '+u.LastName,pg.ProgrammeName,p.Title
	end
	--Union
	if(@type = 'qf')
	Begin
		select QuestionID,QuestionTitle, Description,'Full Project Group' as ParticipantName,ProgrammeName,Title,
		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1)) as Answer,
		'B' as Grp
		From
		(select q.QuestionID,q.Title as "QuestionTitle", q.Description,'Full Project Group' as ParticipantName,pg.ProgrammeName,p.Title,
		REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer		
		from dbo.AssignQuestionnaire aq
		inner join AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
		inner join QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
		inner join Question q on q.QuestionID = qa.QuestionID
		inner join Category c on c.CategoryID = q.CateogryID
		inner join [User] u on u.UserID = aq.targetPersonID
		inner join Programme pg on pg.ProgrammeID = aq.ProgrammeID
		inner join Project p on p.ProjectID = aq.ProjecctID
		where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and ad.RelationShip != 'Self' and
		aq.accountID = @accountid and aq.ProjecctID = @projectid ) as t1
		Group By QuestionID,QuestionTitle,Description,ParticipantName,ProgrammeName,Title
	end
END

	--if(@type = 'qg')
	--Begin
	--	--select q.Description,ad.CandidateName,
	--	--cast(sum(cast(REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as decimal(12,1))) / count(*) as decimal(12,1)) as Answer
	--	--from dbo.AssignQuestionnaire aq
	--	--inner join AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
	--	--inner join QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
	--	--inner join Question q on q.QuestionID = qa.QuestionID
	--	--inner join Category c on c.CategoryID = q.CateogryID
	--	--where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and
	--	--aq.accountID = @accountid and aq.ProgrammeID = @programmeid
	--	--Group By q.Description ,ad.CandidateName
	--end
	----Union
	--if(@type = 'qf')
	--Begin
	--	--select q.Description,'Full Project Group' as CandidateName,
	--	--cast(sum(cast(REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as decimal(12,1))) / count(*) as decimal(12,1)) as Answer
	--	--from dbo.AssignQuestionnaire aq
	--	--inner join AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
	--	--inner join QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
	--	--inner join Question q on q.QuestionID = qa.QuestionID
	--	--inner join Category c on c.CategoryID = q.CateogryID
	--	--where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and
	--	--aq.accountID = @accountid and aq.ProjecctID = @projectid
	--	--Group By q.Description ,CandidateName
	--end
GO
