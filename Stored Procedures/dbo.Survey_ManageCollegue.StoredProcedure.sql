USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_ManageCollegue]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_ManageCollegue]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[Survey_ManageCollegue]


@AssignmentID int,
@SelectFlag char(1)

as
if (@SelectFlag = 'D')
BEGIN
	DELETE			
	FROM        dbo.Survey_AssignmentDetails
	WHERE		AssignmentID = @AssignmentID 

END
GO
