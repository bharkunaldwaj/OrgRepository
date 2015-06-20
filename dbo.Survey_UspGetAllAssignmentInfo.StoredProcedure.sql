USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspGetAllAssignmentInfo]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspGetAllAssignmentInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspGetAllAssignmentInfo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspGetAllAssignmentInfo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[Survey_UspGetAllAssignmentInfo] --2338

@CandidateId int

as

SELECT DISTINCT    sad.CandidateName, sad.CandidateEmail, sq.QSTNCode, sq.QSTNName, 
                      sq.QSTNDescription, dbo.Survey_Project.Title, dbo.Survey_Project.Description, dbo.Survey_Project.Logo, dbo.Account.Code, dbo.Account.AccountID,SC.Title  as OrganisationName, 
                      dbo.Account.CompanyLogo, dbo.Account.HeaderBGColor, dbo.Account.MenuBGColor, dbo.Account.CopyRightLine,saq.ProgrammeID,SC.EmailFinishEmailTemplate,SC.CompanyID,sp.ProgrammeName 
FROM				dbo.Account INNER JOIN
                     dbo.Survey_Project  ON dbo.Survey_Project.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Survey_Company SC ON SC.ProjectID = dbo.Survey_Project.ProjectID  INNER JOIN
                     Survey_Analysis_Sheet sp on sp.CompanyID = SC.CompanyId   inner join
                      dbo.Survey_Questionnaire SQ on  sq.AccountID=dbo.Account.AccountID INNER JOIN 
                      dbo.Survey_AssignQuestionnaire saq ON sq.QuestionnaireID = saq.QuestionnaireID and sp.ProgrammeID = saq.ProgrammeID
                      INNER JOIN Survey_AssignmentDetails sad ON sad.AssignmentID = saq.AssignmentID
WHERE    
 (sad.AsgnDetailID = @CandidateId)



Select * from Survey_AssignmentDetails
' 
END
GO
