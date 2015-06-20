USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Usp_get_ParentID]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usp_get_ParentID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Usp_get_ParentID]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usp_get_ParentID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC [dbo].[Usp_get_ParentID]
AS
begin
--select distinct grouprights.menuid, parentid from grouprights inner join menumaster on menumaster.menuid=grouprights.menuid  order by grouprights.menuid
select menuid from menumaster where parentID=0 and ADEVflag is null
end

' 
END
GO
