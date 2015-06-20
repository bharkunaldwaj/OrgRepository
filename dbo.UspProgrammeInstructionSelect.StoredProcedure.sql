USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspProgrammeInstructionSelect]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspProgrammeInstructionSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspProgrammeInstructionSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspProgrammeInstructionSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE procedure [dbo].[UspProgrammeInstructionSelect]

@ProgrammeID int

as 

SELECT Instructions from Programme 
WHERE ProgrammeID=@ProgrammeID 


' 
END
GO
