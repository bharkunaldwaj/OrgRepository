USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[uspAssignmentDetailsUpdateCandidateEmail]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAssignmentDetailsUpdateCandidateEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAssignmentDetailsUpdateCandidateEmail]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAssignmentDetailsUpdateCandidateEmail]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[uspAssignmentDetailsUpdateCandidateEmail]

@AsgnDetailID int,
@newEmailValue varchar(100),
@CandidateName varchar(250)=null,
@RelationShip varchar(250)=null

as

update dbo.AssignmentDetails SET CandidateEmail=@newEmailValue ,CandidateName=@CandidateName,RelationShip=@RelationShip
WHERE AsgnDetailID=@AsgnDetailID
' 
END
GO
