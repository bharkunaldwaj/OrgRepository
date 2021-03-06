USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_GetCategory_or_analysis]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_GetCategory_or_analysis]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_GetCategory_or_analysis]
@AccountID int,
@ProgramID int,
@Analysis varchar(50)=null,
@SelectFlag char(1)
as

	IF (@SelectFlag = 'C')
		begin
			select * from survey_category where AccountID=@AccountID
		end
	ELSE IF (@SelectFlag = 'A')
		begin
			select * from Survey_AnalysisSheet_Category_Details
		where programme_Id=@ProgramID and (@Analysis is null or LTrim(Rtrim(@Analysis))='' or Category_Name=@Analysis)
		end
GO
