USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspGetProgramme_Analysis3]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspGetProgramme_Analysis3]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_UspGetProgramme_Analysis3]
(
@Programme_ID int
)
as
BEGIN
SELECT Category_Detail,Analysis_Category_Id as CategoryID FROM Survey_AnalysisSheet_Category_Details
where Programme_Id=@Programme_ID and Analysis_Type like 'ANALYSIS- III'
END
GO
