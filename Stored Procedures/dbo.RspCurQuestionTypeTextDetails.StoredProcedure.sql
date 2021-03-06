USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspCurQuestionTypeTextDetails]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspCurQuestionTypeTextDetails]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be used in Sub Report to Display the
-- =============================================
 -- [RspCurQuestionTypeTextDetails] 761,470,'Line Manager,Peer Group,Customer',1
Create PROCEDURE [dbo].[RspCurQuestionTypeTextDetails] 
	--@accountid int,
	@questionid int,
	@targetpersonid int,      -- *PLease Pass this in like this : aq.TargetPersonID = @targetpersonid *
	@grp Varchar(500),
	@selfvisibility Varchar(5)
AS
BEGIN

	select * into #temp1 from dbo.fn_CSVToTable(@grp)
	IF(@selfvisibility = '1')
		BEGIN
			BEGIN TRAN
				--self
				select u.FirstName +' '+ u.LastName as RelationShip,c.CategoryName, Answer, count(*) as "No Of Candidate",
				ad.CandidateName, q.QuestionID,'0' as GrpOrder 
				from Account a
				left join Category c on c.AccountID = a.AccountID
				left join Question q on q.CateogryId = c.CategoryId
				left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
				left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
				left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
				left join [User] u on u.UserID = aq.TargetPersonID
				where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
				and aq.TargetPersonID = @targetpersonid and ad.RelationShip = 'Self' and Answer != 'N/A'
				and Answer != ' ' and ad.SubmitFlag = 'True'
				Group By u.FirstName +' '+ u.LastName ,c.CategoryName, Answer, ad.CandidateName, q.QuestionID

				Union 
				--apart from self
				select ad.RelationShip,c.CategoryName, Answer, count(*) as "No Of Candidate", ad.CandidateName,
				q.QuestionID, #temp1.Id as GrpOrder 
				from Account a
				left join Category c on c.AccountID = a.AccountID
				left join Question q on q.CateogryId = c.CategoryId
				left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
				left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
				left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
				left join [User] u on u.UserID = aq.TargetPersonID
				inner join #temp1 on #temp1.[Value]=ad.RelationShip
				where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
				and aq.TargetPersonID = @targetpersonid and ad.RelationShip != 'Self' and Answer != 'N/A'
				and Answer != ' ' and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp))
				and ad.SubmitFlag = 'True'
				Group By Id,ad.RelationShip,c.CategoryName, Answer, ad.CandidateName, q.QuestionID
				
			COMMIT TRAN
		END
	ELSE     
		BEGIN
			BEGIN TRAN	
				--apart from self
				select ad.RelationShip,c.CategoryName, Answer, count(*) as "No Of Candidate", ad.CandidateName,
				q.QuestionID, #temp1.Id as GrpOrder 
				from Account a
				left join Category c on c.AccountID = a.AccountID
				left join Question q on q.CateogryId = c.CategoryId
				left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
				left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
				left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
				left join [User] u on u.UserID = aq.TargetPersonID
				inner join #temp1 on #temp1.[Value]=ad.RelationShip
				where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
				and aq.TargetPersonID = @targetpersonid and ad.RelationShip != 'Self' and Answer != 'N/A'
				and Answer != ' ' and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp))
				and ad.SubmitFlag = 'True'
				Group By Id,ad.RelationShip,c.CategoryName, Answer, ad.CandidateName, q.QuestionID		
			COMMIT TRAN
		END
END
GO
