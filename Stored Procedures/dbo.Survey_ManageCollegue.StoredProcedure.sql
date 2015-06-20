USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_ManageCollegue]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_ManageCollegue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_ManageCollegue]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_ManageCollegue]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'Create procedure [dbo].[Survey_ManageCollegue]


@AssignmentID int,
@SelectFlag char(1)

as
if (@SelectFlag = ''D'')
BEGIN
	DELETE			
	FROM        dbo.Survey_AssignmentDetails
	WHERE		AssignmentID = @AssignmentID 

END
' 
END
GO
