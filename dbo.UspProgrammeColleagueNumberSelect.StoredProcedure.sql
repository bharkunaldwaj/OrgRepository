USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspProgrammeColleagueNumberSelect]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspProgrammeColleagueNumberSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspProgrammeColleagueNumberSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspProgrammeColleagueNumberSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE procedure [dbo].[UspProgrammeColleagueNumberSelect]

@ProgrammeID int

as 

SELECT ColleagueNo from Programme 
WHERE ProgrammeID=@ProgrammeID 


' 
END
GO
