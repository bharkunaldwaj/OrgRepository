USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Usp_get_ParentID]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Usp_get_ParentID]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_get_ParentID]
AS
begin
--select distinct grouprights.menuid, parentid from grouprights inner join menumaster on menumaster.menuid=grouprights.menuid  order by grouprights.menuid
select menuid from menumaster where parentID=0 and ADEVflag is null
end
GO
