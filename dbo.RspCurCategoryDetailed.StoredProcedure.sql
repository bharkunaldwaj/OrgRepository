USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspCurCategoryDetailed]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspCurCategoryDetailed]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RspCurCategoryDetailed]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspCurCategoryDetailed]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be used in Main Report to Display the Categories>
-- =============================================
-- [RspCurCategoryDetailed] 546
Create PROCEDURE [dbo].[RspCurCategoryDetailed] 
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
' 
END
GO
