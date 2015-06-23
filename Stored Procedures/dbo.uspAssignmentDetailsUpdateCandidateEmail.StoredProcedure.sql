USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[uspAssignmentDetailsUpdateCandidateEmail]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[uspAssignmentDetailsUpdateCandidateEmail]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[uspAssignmentDetailsUpdateCandidateEmail]

@AsgnDetailID int,
@newEmailValue varchar(100),
@CandidateName varchar(250)=null,
@RelationShip varchar(250)=null

as

update dbo.AssignmentDetails SET CandidateEmail=@newEmailValue ,CandidateName=@CandidateName,RelationShip=@RelationShip
WHERE AsgnDetailID=@AsgnDetailID
GO
