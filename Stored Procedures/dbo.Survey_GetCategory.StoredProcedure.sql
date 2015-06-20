USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_GetCategory]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_GetCategory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_GetCategory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_GetCategory]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[Survey_GetCategory]
@AccountID int,
@PogramID int,
@Analysis varchar(50),
@SelectFlag char(1)
as

	IF (@SelectFlag = ''C'')
		begin
			select * from survey_category where AccountID=@AccountID
		end
	ELSE IF (@SelectFlag = ''A'')
		begin
			select * from Survey_AnalysisSheet_Category_Details
		where programme_Id=@PogramID and Category_Name=@Analysis
		end
' 
END
GO
