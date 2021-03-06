USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_GetCategory]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_GetCategory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_GetCategory]
@AccountID int,
@PogramID int,
@Analysis varchar(50),
@SelectFlag char(1)
as

	IF (@SelectFlag = 'C')
		begin
			select * from survey_category where AccountID=@AccountID
		end
	ELSE IF (@SelectFlag = 'A')
		begin
			select * from Survey_AnalysisSheet_Category_Details
		where programme_Id=@PogramID and Category_Name=@Analysis
		end
GO
