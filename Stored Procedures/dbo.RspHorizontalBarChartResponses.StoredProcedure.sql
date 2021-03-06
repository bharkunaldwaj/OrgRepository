USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspHorizontalBarChartResponses]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspHorizontalBarChartResponses]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be Used to Display HorizontalBarChart>
--[RspHorizontalBarChartResponses] 495,'Line Manager,Peer,',1,1,1
-- =============================================
CREATE PROCEDURE [dbo].[RspHorizontalBarChartResponses] 
	@targetpersonid int,
	@grp Varchar(500),
	@fullprjgrpvisibility Varchar(5),
	@selfvisibility Varchar(5)
	,@programmevisibility Varchar(5)
AS 
BEGIN
	select * into #temp1 from dbo.fn_CSVToTable(@grp)
	
	--Here Creating Structure of #tempdetail table and if below condition is true then insertion happend		
	SELECT  @targetpersonid as TargetPersonID,'                                                         ' as Relationship,'              ' AS Expr1
	,'   ' as GRPORD
	into #tempdetail FROM dbo.AssignmentDetails INNER JOIN
	 dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID
	WHERE 1=0
	and dbo.AssignmentDetails.AssignmentID In (select AssignmentID from AssignQuestionnaire
					where ProjecctID IN (select ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid))

	
		--All Below Blocks will be used for Insertion in same #tempdetail table
	IF(@selfvisibility = '1')
	BEGIN
		BEGIN TRAN		--self	
			insert into #tempdetail	
			SELECT dbo.AssignQuestionnaire.TargetPersonID, dbo.AssignmentDetails.RelationShip, COUNT(dbo.AssignmentDetails.AsgnDetailID) AS Expr1
			,'0' as GRPORD
			FROM dbo.AssignmentDetails INNER JOIN
                 dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID
            WHERE RelationShip = 'Self' and dbo.AssignmentDetails.SubmitFlag = 'True'
			GROUP BY dbo.AssignQuestionnaire.TargetPersonID, dbo.AssignmentDetails.RelationShip
			HAVING (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)	
		COMMIT TRAN
	END	


	
	 
	-- Apart from Self : This Block Will Always Run
	
	insert into #tempdetail	
	SELECT dbo.AssignQuestionnaire.TargetPersonID, dbo.AssignmentDetails.RelationShip, COUNT(dbo.AssignmentDetails.AsgnDetailID) AS Expr1,Id as GRPORD
	FROM dbo.AssignmentDetails INNER JOIN
	 dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID
	 inner join #temp1 on [Value] = RelationShip
	 WHERE RelationShip != 'Self' and dbo.AssignmentDetails.SubmitFlag = 'True'
	 GROUP BY dbo.AssignQuestionnaire.TargetPersonID, dbo.AssignmentDetails.RelationShip,Id
	HAVING (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)
	
	
	
	--insert into #tempdetail		
	-- Want AssignmentID by ProgrammeID by TargetPersonID
	IF(@programmevisibility ='1')
	BEGIN
		BEGIN TRAN	-- same Programme : Participant + Candidates will be included here.
		
		declare @count int
		select @count = COUNT(*) 
		from AssignmentDetails
		where AsgnDetailID in 
		(
		select AsgnDetailID from 
		(SELECT     t1.AccountID, t1.QuestionnaireID, t1.TargetPersonID, t1.AsgnDetailID, t1.CandidateName, t1.SubmitFlag, t1.ProjectID, t1.StatusID, t1.Title, 
							  t1.RelationShip, t1.Code, t1.QSTNName, t1.UserID, t1.FullName, t1.ProgrammeID, t1.ProgrammeName, t1.QuestionCount, 
							  COUNT(dbo.QuestionAnswer.QuestionID) AS AnswerCount
		FROM         (SELECT     dbo.AssignQuestionnaire.AccountID, dbo.AssignQuestionnaire.QuestionnaireID, dbo.AssignQuestionnaire.TargetPersonID, 
													  dbo.AssignQuestionnaire.Description, dbo.AssignmentDetails.AsgnDetailID, dbo.AssignmentDetails.CandidateName, 
													  dbo.AssignmentDetails.SubmitFlag, dbo.Project.ProjectID, dbo.Project.StatusID, dbo.Project.Title, dbo.AssignmentDetails.RelationShip, 
													  dbo.Account.Code, dbo.Questionnaire.QSTNName, dbo.[User].UserID, dbo.[User].FirstName + ' ' + dbo.[User].LastName AS FullName, 
													  dbo.Programme.ProgrammeID, dbo.Programme.ProgrammeName, COUNT(dbo.Question.QuestionID) AS QuestionCount
							   FROM          dbo.AssignQuestionnaire INNER JOIN
													  dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
													  dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
													  dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
													  dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
													  dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN
													  dbo.Programme ON dbo.AssignQuestionnaire.ProgrammeID = dbo.Programme.ProgrammeID INNER JOIN
													  dbo.Question ON dbo.Questionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
							   GROUP BY dbo.AssignQuestionnaire.AccountID, dbo.AssignQuestionnaire.QuestionnaireID, dbo.AssignQuestionnaire.TargetPersonID, 
													  dbo.AssignQuestionnaire.Description, dbo.AssignmentDetails.AsgnDetailID, dbo.AssignmentDetails.CandidateName, dbo.Project.ProjectID, 
													  dbo.Project.StatusID, dbo.Project.Title, dbo.AssignmentDetails.RelationShip, dbo.Account.Code, dbo.Questionnaire.QSTNName, 
													  dbo.[User].UserID, dbo.[User].FirstName + ' ' + dbo.[User].LastName, dbo.Programme.ProgrammeID, dbo.Programme.ProgrammeName, 
													  dbo.AssignmentDetails.SubmitFlag
							   HAVING dbo.AssignmentDetails.RelationShip = 'Self' and  dbo.AssignmentDetails.SubmitFlag = 'True' 
							   and Programme.ProgrammeID IN (select Distinct ProgrammeID from AssignQuestionnaire where TargetPersonID = @targetpersonid)
							   --   (dbo.AssignQuestionnaire.AccountID = 34)
							   ) AS t1 INNER JOIN
							  dbo.QuestionAnswer ON t1.AsgnDetailID = dbo.QuestionAnswer.AssignDetId
		GROUP BY t1.AccountID, t1.QuestionnaireID, t1.TargetPersonID, t1.AsgnDetailID, t1.CandidateName, t1.ProjectID, t1.StatusID, t1.Title, t1.RelationShip, t1.Code, 
							  t1.QSTNName, t1.UserID, t1.FullName, t1.ProgrammeID, t1.ProgrammeName, t1.QuestionCount, t1.SubmitFlag                                            
							  ) as t2
		                      
							  where QuestionCount = AnswerCount )
				
			insert into #tempdetail	
			SELECT  @targetpersonid as TargetPersonID,dbo.Programme.ProgrammeName as Relationship,  @count AS Expr1
			,'98' as GRPORD
			FROM dbo.AssignmentDetails 
			INNER JOIN dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID
			INNER JOIN dbo.Programme ON dbo.AssignQuestionnaire.ProgrammeID = dbo.Programme.ProgrammeID
			WHERE RelationShip != 'Self' and dbo.AssignmentDetails.SubmitFlag = 'True' 
			and RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp))
			and dbo.AssignQuestionnaire.ProgrammeID IN (select Distinct ProgrammeID from AssignQuestionnaire where TargetPersonID = @targetpersonid)
			and dbo.AssignmentDetails.AssignmentID In (select AssignmentID from AssignQuestionnaire
					where ProgrammeID IN (select ProgrammeID from AssignQuestionnaire where TargetPersonID = @targetpersonid ))	
			GROUP BY dbo.Programme.ProgrammeName			
		COMMIT TRAN
	END
	
	
	---- Want AssignmentID by ProjectID by TargetPersonID
	IF(@fullprjgrpvisibility ='1')
	BEGIN
		BEGIN TRAN	-- full Project
		
		declare @projcount int
		select @projcount = COUNT(*) 
		from AssignmentDetails
		where AsgnDetailID in 
		(
		select AsgnDetailID from 
		(SELECT     t1.AccountID, t1.QuestionnaireID, t1.TargetPersonID, t1.AsgnDetailID, t1.CandidateName, t1.SubmitFlag, t1.ProjectID, t1.StatusID, t1.Title, 
							  t1.RelationShip, t1.Code, t1.QSTNName, t1.UserID, t1.FullName, t1.ProgrammeID, t1.ProgrammeName, t1.QuestionCount, 
							  COUNT(dbo.QuestionAnswer.QuestionID) AS AnswerCount
		FROM         (SELECT     dbo.AssignQuestionnaire.AccountID, dbo.AssignQuestionnaire.QuestionnaireID, dbo.AssignQuestionnaire.TargetPersonID, 
													  dbo.AssignQuestionnaire.Description, dbo.AssignmentDetails.AsgnDetailID, dbo.AssignmentDetails.CandidateName, 
													  dbo.AssignmentDetails.SubmitFlag, dbo.Project.ProjectID, dbo.Project.StatusID, dbo.Project.Title, dbo.AssignmentDetails.RelationShip, 
													  dbo.Account.Code, dbo.Questionnaire.QSTNName, dbo.[User].UserID, dbo.[User].FirstName + ' ' + dbo.[User].LastName AS FullName, 
													  dbo.Programme.ProgrammeID, dbo.Programme.ProgrammeName, COUNT(dbo.Question.QuestionID) AS QuestionCount
							   FROM          dbo.AssignQuestionnaire INNER JOIN
													  dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
													  dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
													  dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
													  dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
													  dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN
													  dbo.Programme ON dbo.AssignQuestionnaire.ProgrammeID = dbo.Programme.ProgrammeID INNER JOIN
													  dbo.Question ON dbo.Questionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
							   GROUP BY dbo.AssignQuestionnaire.AccountID, dbo.AssignQuestionnaire.QuestionnaireID, dbo.AssignQuestionnaire.TargetPersonID, 
													  dbo.AssignQuestionnaire.Description, dbo.AssignmentDetails.AsgnDetailID, dbo.AssignmentDetails.CandidateName, dbo.Project.ProjectID, 
													  dbo.Project.StatusID, dbo.Project.Title, dbo.AssignmentDetails.RelationShip, dbo.Account.Code, dbo.Questionnaire.QSTNName, 
													  dbo.[User].UserID, dbo.[User].FirstName + ' ' + dbo.[User].LastName, dbo.Programme.ProgrammeID, dbo.Programme.ProgrammeName, 
													  dbo.AssignmentDetails.SubmitFlag
							   HAVING   dbo.AssignmentDetails.SubmitFlag = 'True' and dbo.Project.ProjectID IN (select Distinct ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid)
							   --   (dbo.AssignQuestionnaire.AccountID = 34)
							   ) AS t1 INNER JOIN
							  dbo.QuestionAnswer ON t1.AsgnDetailID = dbo.QuestionAnswer.AssignDetId
		GROUP BY t1.AccountID, t1.QuestionnaireID, t1.TargetPersonID, t1.AsgnDetailID, t1.CandidateName, t1.ProjectID, t1.StatusID, t1.Title, t1.RelationShip, t1.Code, 
							  t1.QSTNName, t1.UserID, t1.FullName, t1.ProgrammeID, t1.ProgrammeName, t1.QuestionCount, t1.SubmitFlag                                            
							  ) as t2
		                      
							  where QuestionCount = AnswerCount )
		
		
		
			insert into #tempdetail
			SELECT Distinct  @targetpersonid as TargetPersonID,'Full Project Group' as Relationship,  @projcount AS Expr1
			,'99' as GRPORD
			FROM dbo.AssignmentDetails INNER JOIN
			dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID
			WHERE RelationShip != 'Self'  and dbo.AssignmentDetails.SubmitFlag = 'True' 
			and RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp))
			and dbo.AssignmentDetails.AssignmentID In (select AssignmentID from AssignQuestionnaire
					where ProjecctID IN (select ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid))
		COMMIT TRAN
	END
	
	
	-- Showing reselt set to Report
	select * from #tempdetail
	
	-- then droping Table
	drop table #tempdetail
	drop table #temp1
 
END
GO
