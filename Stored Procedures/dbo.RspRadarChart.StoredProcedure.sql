USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspRadarChart]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspRadarChart]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Description:	<This Procedure will be used in Main Report to Display the Categories>
--For the radar graph, as sample attached, we compare the average of the
--scores given for the participant with the average of all scores given for
--this project.
--So for Unilever the sample Al Wilson, Al Wilson is the average of the 10
--people (his Candidates) compared to the Full Project Group, 600 people that
-- =============================================
CREATE PROCEDURE [dbo].[RspRadarChart] 
	@targetpersonid int,      -- *PLease Pass this in like this : aq.TargetPersonID = @targetpersonid *
	@grp Varchar(500)
AS
BEGIN	
	select RelationShip,CategoryName, sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
	cast(sum(cast(Answer as decimal(12,2))) / count(*) as decimal(12,2))  As Average , 'A' as GrpOrder
	from
	(
	select u.FirstName +' '+ u.LastName as RelationShip ,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer	
	from Account a
	left join Category c on c.AccountID = a.AccountID
	left join Question q on q.CateogryId = c.CategoryId
	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
	left join [User] u on u.UserID = aq.TargetPersonID
	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' 
	and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A') as t1
	Group By RelationShip, CategoryName
				
	Union
	-- full Project		
	select t1.RelationShip,t1.CategoryName, sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
	cast(sum(cast(t1.Answer as decimal(12,2))) / count(*) as decimal(12,2))  As Average , 'B' as GrpOrder
	from
	(	
	select 'Full Project Group' as Relationship,c.CategoryName, REPLACE(SUBSTRING ( qa.Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer ,
	count(*) as "No Of Candidate"
	from AssignQuestionnaire aq 
	left join Category c on c.AccountID = aq.AccountID
	left join Question q on q.CateogryId = c.CategoryId
	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join Account a on a.AccountID = aq.AccountID
	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId	
	left join [User] u on u.UserID = aq.TargetPersonID
	where  q.QuestionTypeID = 2 and qa.answer !='N/A' and qa.Answer != ' ' and ad.RelationShip not in (@grp)
	and ad.AssignmentID In (select AssignmentID from AssignQuestionnaire
					where ProjecctID IN (select ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid ))	
	Group By RelationShip,c.CategoryName,answer,q.UpperBound
	) as t1	
	group by t1.RelationShip,t1.CategoryName
	
END

	--select u.FirstName +' '+ u.LastName as RelationShip ,c.CategoryName, sum(cast(Answer as decimal(12,2))) As "Sum" ,
	--count(*) as "No Of Candidate",cast(sum(cast(Answer as decimal(12,2))) / count(*) as decimal(12,2)) As Average,
	--'A' as GrpOrder 
	--from Account a
	--left join Category c on c.AccountID = a.AccountID
	--left join Question q on q.CateogryId = c.CategoryId
	--left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
	--left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	--left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
	--left join [User] u on u.UserID = aq.TargetPersonID
	--where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' 
	--and aq.TargetPersonID = @targetpersonid
	--Group By u.FirstName +' '+ u.LastName,c.CategoryName
				
	--Union
	---- full Project
	--select 'Full Project Group' as Relationship,c.CategoryName, sum(cast(Answer as decimal(12,2))) As "Sum" ,
	--count(*) as "No Of Candidate", cast(sum(cast(Answer as decimal(12,2))) / count(*) as decimal(12,2)) As Average,
	--'B' as GrpOrder 
	--from AssignQuestionnaire aq 
	--left join Category c on c.AccountID = aq.AccountID
	--left join Question q on q.CateogryId = c.CategoryId
	--left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
	--left join Account a on a.AccountID = aq.AccountID
	--left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId	
	--left join [User] u on u.UserID = aq.TargetPersonID
	--where  q.QuestionTypeID = 2 and qa.Answer != ' ' 
	--and ad.AssignmentID In (select AssignmentID from AssignQuestionnaire
	--				where ProjecctID = (select ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid))	
	--Group By c.CategoryName
GO
