USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspDeleteReport]    Script Date: 07/16/2015 11:08:17 ******/
DROP PROCEDURE [dbo].[UspDeleteReport]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UspDeleteReport]
@AccountID int,
@ProgramID int 
As

Delete from ReportManagement 
where ProgramID = @ProgramID and AccountID = @AccountID