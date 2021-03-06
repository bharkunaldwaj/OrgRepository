USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspCategoryDetailed]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspCategoryDetailed]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be used in Main Report to Display the Categories>
-- =============================================
-- [RspCategoryDetailed] 546
CREATE PROCEDURE [dbo].[RspCategoryDetailed] 
	@targetpersonid int    
AS
BEGIN
	select aq.AccountID,aq.TargetPersonID , c.CategoryID, c.CategoryName, c.Sequence, c.Description,
	c.QuestionnaireID
	from AssignQuestionnaire aq 
	join Category c on c.AccountID =  aq.AccountID
	where TargetPersonID = @targetpersonid and c.ExcludeFromAnalysis = 0 
	and c.QuestionnaireID = aq.QuestionnaireID
	Group By aq.AccountID,aq.TargetPersonID , c.CategoryID, c.CategoryName, c.Sequence, c.Description,
	c.QuestionnaireID
	Order by c.Sequence
END
GO
