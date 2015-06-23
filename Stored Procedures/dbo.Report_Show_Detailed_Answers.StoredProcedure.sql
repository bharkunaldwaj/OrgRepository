USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Show_Detailed_Answers]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_Show_Detailed_Answers]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Report_Show_Detailed_Answers](@accountid int,
	@projectid int)
	--Report_Show_Detailed_Answers 106,282
	as
	Declare @AnaI int,@AnaII int,@AnaIII int,@PA int,@FPG int
	BEGIN
	select @AnaI=AnalysisI ,@AnaII=AnalysisII ,@AnaIII=AnalysisIII ,@PA=Programme_Average ,@FPG=FullProjectGrp  from Survey_ProjectReportSetting where AccountID=@accountid and ProjectID=@projectid
	if(@AnaI=0 and @AnaII=0 and @AnaIII=0)
	Begin
	Select 0 as 'Show'
	end
	else
	begin
	Select 1 as 'Show'
	end
	END
GO
