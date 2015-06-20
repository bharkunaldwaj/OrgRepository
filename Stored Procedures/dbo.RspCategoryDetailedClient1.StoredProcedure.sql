USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspCategoryDetailedClient1]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspCategoryDetailedClient1]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RspCategoryDetailedClient1]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspCategoryDetailedClient1]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be used in Main Report to Display the Categories>
-- =============================================
-- [RspCategoryDetailedClient1] 546
CREATE PROCEDURE [dbo].[RspCategoryDetailedClient1] 
	@targetpersonid int    
AS
BEGIN
	-- Procedure will Display First: Range type Question -Category & then Second : Text type question Category.
	-- In Subreport will pass targetpaersonid & CategoryID
	select aq.AccountID,aq.TargetPersonID , c.CategoryID, c.CategoryName, c.Sequence, c.[Description],
	c.QuestionnaireID, q.QuestionTypeID
	from AssignQuestionnaire aq 
	join Category c on c.AccountID =  aq.AccountID
	join Question q ON q.CateogryID = c.CategoryID
	where TargetPersonID = @targetpersonid and c.ExcludeFromAnalysis = 0 
	and c.QuestionnaireID = aq.QuestionnaireID
	Group By aq.AccountID,aq.TargetPersonID , c.CategoryID, c.CategoryName, c.Sequence, c.[Description],
	c.QuestionnaireID, q.QuestionTypeID
	Order by q.QuestionTypeID desc,c.Sequence
END
' 
END
GO
