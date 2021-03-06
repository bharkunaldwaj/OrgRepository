USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspGetProjectRelationship]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspGetProjectRelationship]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspGetProjectRelationship]
@ProjectId int

as

declare @Relationships varchar(500)

SELECT @Relationships = (Relationship1 + ',' + Relationship2 + ',' + Relationship3 + ',' + Relationship4 + ',' + Relationship5)
FROM         dbo.Project
WHERE     (ProjectID = @ProjectId)

select [value] from dbo.fn_CSVToTable (@Relationships) where value <> ' '
GO
