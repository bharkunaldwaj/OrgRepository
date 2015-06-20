USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspCategoryDetailedClient2]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspCategoryDetailedClient2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RspCategoryDetailedClient2]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspCategoryDetailedClient2]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be used in Main Report to Display the Categories>
-- =============================================
-- [RspCategoryDetailedClient2] 725
CREATE PROCEDURE [dbo].[RspCategoryDetailedClient2] 
	@targetpersonid int    
AS
BEGIN
	-- This Query Will Return only Those category-name which have Text-Type Questions
	select aq.AccountID,aq.TargetPersonID , c.CategoryID, c.CategoryName, c.Sequence, c.Description,
	c.QuestionnaireID
	from AssignQuestionnaire aq 
	left join Category c on c.AccountID =  aq.AccountID
	left join Question q on q.CateogryID = c.CategoryID
	where TargetPersonID = @targetpersonid and c.ExcludeFromAnalysis = 0 
	and c.QuestionnaireID = aq.QuestionnaireID and QuestionTypeID = 1
	Group By aq.AccountID,aq.TargetPersonID , c.CategoryID, c.CategoryName, c.Sequence, c.Description,
	c.QuestionnaireID
	Order by c.Sequence
END
' 
END
GO
