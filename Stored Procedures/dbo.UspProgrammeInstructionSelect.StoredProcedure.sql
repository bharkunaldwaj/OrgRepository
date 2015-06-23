USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspProgrammeInstructionSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspProgrammeInstructionSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspProgrammeInstructionSelect]

@ProgrammeID int

as 

SELECT Instructions from Programme 
WHERE ProgrammeID=@ProgrammeID
GO
