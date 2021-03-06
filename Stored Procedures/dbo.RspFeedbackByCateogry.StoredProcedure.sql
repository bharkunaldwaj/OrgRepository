USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspFeedbackByCateogry]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspFeedbackByCateogry]
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
-- [RspFeedbackByCateogry] 38,174,47,'cg'
-- [RspFeedbackByCateogry] 38,174,47,'cf'
CREATE PROCEDURE [dbo].[RspFeedbackByCateogry] 
	@accountid varchar(50),
	@projectid varchar(50),
	@programmeid varchar(50),
	@type varchar(2)
AS
BEGIN
	if(@type = 'cg')
	Begin

		Declare @AccountName varchar(100)
		
		Select @AccountName = OrganisationName from Account where AccountID = @accountid
		
		--Declare @QuestionnaireID INT
		
		--Select top 1  @QuestionnaireID = QuestionnaireID  from AssignQuestionnaire WHERE AccountID = @accountid and ProjecctID = @projectid and ProgrammeID = @programmeid
	
		
		
		select c.CategoryID,c.CategoryName,u.FirstName+' '+u.LastName as ParticipantName, CandidateName,
		pg.ProgrammeName,p.Title,
		RelationShip ,c.Sequence,  REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer, @AccountName as AccountName
		into #temp1 from Account a
		left join Category c on c.AccountID = a.AccountID
		left join Question q on q.CateogryId = c.CategoryId
		left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
		left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
		left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID	and aq.QuestionnaireID = c.QuestionnaireID
		inner join [User] u on u.UserID = aq.targetPersonID
		left join Programme pg on pg.ProgrammeID = aq.ProgrammeID
		left join Project p on p.ProjectID = aq.ProjecctID
		where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' and ad.RelationShip != 'Self'
		and aq.accountID = @accountid and aq.ProgrammeID = @programmeid 		
		and qa.answer !='N/A' and ad.SubmitFlag = 'True'
		
			
		select CategoryID,CategoryName,ParticipantName,ProgrammeName,Title,
		cast(SUBSTRING(cast(sum(Average)/count(Average)  as Varchar(50)),1,3)as decimal(12,1)) as Answer,AccountName
		--cast( sum(Average)/count(Average)as decimal(12,1)) as Answer 
		,Grp
		 from (
		select CategoryID,CategoryName,ParticipantName,'' as CandidateName,ProgrammeName,Title,RelationShip,
		cast(sum(cast(Answer as decimal(12,1))) / count(Answer) as decimal(12,1))  As Average ,'A' as Grp,AccountName
		from
		(
			select * from #temp1
		) as t1
		Group By RelationShip,CategoryID,CategoryName,ParticipantName,CandidateName,ProgrammeName,Title,AccountName	
		) as t2
		Group By CategoryID,CategoryName,ParticipantName,CandidateName,ProgrammeName,Title,Grp,AccountName
		Order By CategoryID
		
	
		drop table #temp1
	end
	--Union
	if(@type = 'cf')
	Begin
		
		select *  into #temp2 from (
		select c.CategoryID,c.CategoryName,'Full Project Group' as ParticipantName,pg.ProgrammeName,p.Title,
		RelationShip ,REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer		
		from dbo.AssignQuestionnaire aq
		inner join AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
		inner join QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
		inner join Question q on q.QuestionID = qa.QuestionID
		inner join Category c on c.CategoryID = q.CateogryID and aq.QuestionnaireID = c.QuestionnaireID
		inner join [User] u on u.UserID = aq.targetPersonID
		inner join Programme pg on pg.ProgrammeID = aq.ProgrammeID
		inner join Project p on p.ProjectID = aq.ProjecctID
		where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and ad.RelationShip != 'Self' and
		aq.accountID = @accountid and aq.ProjecctID = @projectid ) t1
		
		declare @prgname varchar (50)
		select @prgname = ProgrammeName from dbo.Programme where ProgrammeID = @programmeid
		
		select CategoryID,CategoryName,ParticipantName,@prgname as ProgrammeName,Title,
		cast(SUBSTRING(cast(sum(Average)/count(Average)  as Varchar(50)),1,3)as decimal(12,1)) as Answer
		--cast( sum(Average)/count(Average)as decimal(12,1)) as Answer 
		,Grp
		 from (
		select CategoryID,CategoryName,ParticipantName,'' as ProgrammeName,Title,RelationShip,
		cast(sum(cast(Answer as decimal(12,1))) / count(Answer) as decimal(12,1))  As Average ,'B' as Grp
		from
		(
			select * from #temp2
		) as t1
		Group By RelationShip,CategoryID,CategoryName,ParticipantName,ProgrammeName,Title	
		) as t2
		Group By CategoryID,CategoryName,ParticipantName,ProgrammeName,Title,Grp
		Order By CategoryID
		
		drop table #temp2
	end
END




--
--BEGIN
--	if(@type = 'cg')
--	Begin
	
--		select *  into #temp1 from (
--	select c.CategoryID,c.CategoryName,u.FirstName+' '+u.LastName as ParticipantName,pg.ProgrammeName,p.Title,
--	SUBSTRING ( Answer ,0 , 3 ) as Answer,
--	'A' as Grp
--	from dbo.AssignQuestionnaire aq
--	inner join AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
--	inner join QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
--	inner join Question q on q.QuestionID = qa.QuestionID
--	inner join Category c on c.CategoryID = q.CateogryID
--	inner join [User] u on u.UserID = aq.targetPersonID
--	inner join Programme pg on pg.ProgrammeID = aq.ProgrammeID
--	inner join Project p on p.ProjectID = aq.ProjecctID
--	where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and ad.RelationShip != 'Self' and
--	aq.accountID = @accountid and aq.ProgrammeID = @programmeid
--	Group By c.CategoryID,c.CategoryName	,u.FirstName+' '+u.LastName,pg.ProgrammeName,p.Title,answer	
--		) t1
		
		
	
--		select CategoryID,CategoryName,ParticipantName,ProgrammeName,Title,
--		 cast(sum(cast(REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as decimal(12,1))) / count(*) as decimal(12,1))  as Answer,
--		 'A' as Grp from #temp1 
--		group by CategoryID,CategoryName,ParticipantName,ProgrammeName,Title,Answer
	
--		drop table #temp1
--	end
--	--Union
--	if(@type = 'cf')
--	Begin
		
--		select *  into #temp2 from (
--		select c.CategoryID,c.CategoryName,'Full Project Group' as ParticipantName,pg.ProgrammeName,p.Title,
--		REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer		
--		from dbo.AssignQuestionnaire aq
--		inner join AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
--		inner join QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
--		inner join Question q on q.QuestionID = qa.QuestionID
--		inner join Category c on c.CategoryID = q.CateogryID
--		inner join [User] u on u.UserID = aq.targetPersonID
--		inner join Programme pg on pg.ProgrammeID = aq.ProgrammeID
--		inner join Project p on p.ProjectID = aq.ProjecctID
--		where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and ad.RelationShip != 'Self' and
--		aq.accountID = @accountid and aq.ProjecctID = @projectid ) t1

--		select CategoryID,CategoryName,'Full Project Group' as ParticipantName,ProgrammeName,Title,
--		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1)) as Answer,
--		'B' as Grp
--		From #temp2
--		Group By CategoryID,CategoryName,ParticipantName,ProgrammeName,Title	
		
--		drop table #temp2
--	end
--END
--

--
--BEGIN
--	if(@type = 'cg')
--	Begin
--		select c.CategoryID,c.CategoryName,u.FirstName+' '+u.LastName as ParticipantName,pg.ProgrammeName,p.Title,
--		cast(sum(cast(REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as decimal(12,1))) / count(*) as decimal(12,1)) as Answer,
--		'A' as Grp
--		from dbo.AssignQuestionnaire aq
--		inner join AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
--		inner join QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
--		inner join Question q on q.QuestionID = qa.QuestionID
--		inner join Category c on c.CategoryID = q.CateogryID
--		inner join [User] u on u.UserID = aq.targetPersonID
--		inner join Programme pg on pg.ProgrammeID = aq.ProgrammeID
--		inner join Project p on p.ProjectID = aq.ProjecctID
--		where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and ad.RelationShip != 'Self' and
--		aq.accountID = @accountid and aq.ProgrammeID = @programmeid
--		Group By c.CategoryID,c.CategoryName	,u.FirstName+' '+u.LastName,pg.ProgrammeName,p.Title	
		
	
--	end
--	--Union
--	if(@type = 'cf')
--	Begin
--		select CategoryID,CategoryName,'Full Project Group' as ParticipantName,ProgrammeName,Title,
--		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1)) as Answer,
--		'B' as Grp
--		From
--		(select c.CategoryID,c.CategoryName,'Full Project Group' as ParticipantName,pg.ProgrammeName,p.Title,
--		REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer		
--		from dbo.AssignQuestionnaire aq
--		inner join AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
--		inner join QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
--		inner join Question q on q.QuestionID = qa.QuestionID
--		inner join Category c on c.CategoryID = q.CateogryID
--		inner join [User] u on u.UserID = aq.targetPersonID
--		inner join Programme pg on pg.ProgrammeID = aq.ProgrammeID
--		inner join Project p on p.ProjectID = aq.ProjecctID
--		where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and ad.RelationShip != 'Self' and
--		aq.accountID = @accountid and aq.ProjecctID = @projectid ) as t1
--		Group By CategoryID,CategoryName,ParticipantName,ProgrammeName,Title	
--	end
--END
--
GO
