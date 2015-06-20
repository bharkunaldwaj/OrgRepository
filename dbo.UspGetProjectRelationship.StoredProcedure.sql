USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspGetProjectRelationship]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspGetProjectRelationship]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspGetProjectRelationship]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspGetProjectRelationship]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[UspGetProjectRelationship]
@ProjectId int

as

declare @Relationships varchar(500)

SELECT @Relationships = (Relationship1 + '','' + Relationship2 + '','' + Relationship3 + '','' + Relationship4 + '','' + Relationship5)
FROM         dbo.Project
WHERE     (ProjectID = @ProjectId)

select [value] from dbo.fn_CSVToTable (@Relationships) where value <> '' ''
' 
END
GO
