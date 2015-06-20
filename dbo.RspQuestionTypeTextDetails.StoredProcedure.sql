USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspQuestionTypeTextDetails]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspQuestionTypeTextDetails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RspQuestionTypeTextDetails]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspQuestionTypeTextDetails]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be used in Sub Report to Display the
-- =============================================
 -- [RspQuestionTypeTextDetails] 761,470,''Line Manager,Peer Group,Customer'',1
CREATE PROCEDURE [dbo].[RspQuestionTypeTextDetails] 
	--@accountid int,
	@questionid int,
	@targetpersonid int,      -- *PLease Pass this in like this : aq.TargetPersonID = @targetpersonid *
	@grp Varchar(500),
	@selfvisibility Varchar(5)
AS
BEGIN

	select * into #temp1 from dbo.fn_CSVToTable(@grp)
	IF(@selfvisibility = ''1'')
		BEGIN
			BEGIN TRAN
				--self
				Select * from(
				select u.FirstName +'' ''+ u.LastName as RelationShip,c.CategoryName, Answer, count(*) as "No Of Candidate",
				ad.CandidateName, q.QuestionID,0 as GrpOrder , 2 as OrderBy
				from Account a
				left join Category c on c.AccountID = a.AccountID
				left join Question q on q.CateogryId = c.CategoryId
				left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
				left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
				left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
				left join [User] u on u.UserID = aq.TargetPersonID
				where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
				and aq.TargetPersonID = @targetpersonid and ad.RelationShip = ''Self'' and Answer != ''N/A''
				and Answer != '' '' and ad.SubmitFlag = ''True''
				Group By u.FirstName +'' ''+ u.LastName ,c.CategoryName, Answer, ad.CandidateName, q.QuestionID

				Union 
				--apart from self
				select ad.RelationShip,c.CategoryName, Answer, count(*) as "No Of Candidate", ad.CandidateName,
				q.QuestionID, #temp1.Id as GrpOrder , 1 as OrderBy
				from Account a
				left join Category c on c.AccountID = a.AccountID
				left join Question q on q.CateogryId = c.CategoryId
				left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
				left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
				left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
				left join [User] u on u.UserID = aq.TargetPersonID
				inner join #temp1 on #temp1.[Value]=ad.RelationShip
				where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
				and aq.TargetPersonID = @targetpersonid and ad.RelationShip != ''Self'' and Answer != ''N/A''
				and Answer != '' '' and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp))
				and ad.SubmitFlag = ''True''
				Group By Id,ad.RelationShip,c.CategoryName, Answer, ad.CandidateName, q.QuestionID
				) as a Order by GrpOrder asc, newid() 
			COMMIT TRAN
		END
	ELSE     
		BEGIN
			BEGIN TRAN	
				--apart from self
				Select * from(
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
				and aq.TargetPersonID = @targetpersonid and ad.RelationShip != ''Self'' and Answer != ''N/A''
				and Answer != '' '' and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp))
				and ad.SubmitFlag = ''True''
				Group By Id,ad.RelationShip,c.CategoryName, Answer, ad.CandidateName, q.QuestionID		
				) as a Order by GrpOrder asc, newid() 
			COMMIT TRAN
		END
END
' 
END
GO
