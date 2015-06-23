USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspProgrammeColleagueNumberSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspProgrammeColleagueNumberSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspProgrammeColleagueNumberSelect]

@ProgrammeID int

as 

SELECT ColleagueNo from Programme 
WHERE ProgrammeID=@ProgrammeID
GO
