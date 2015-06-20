USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspGetProgramme_Analysis1]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspGetProgramme_Analysis1]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspGetProgramme_Analysis1]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspGetProgramme_Analysis1]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[Survey_UspGetProgramme_Analysis1]
(
@Programme_ID int
)
as
BEGIN
SELECT Category_Detail,Analysis_Category_Id as CategoryID FROM Survey_AnalysisSheet_Category_Details
where Programme_Id=@Programme_ID and Analysis_Type like ''ANALYSIS- I''
END
' 
END
GO
