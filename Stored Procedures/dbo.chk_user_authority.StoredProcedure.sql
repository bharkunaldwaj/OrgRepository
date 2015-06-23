USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[chk_user_authority]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[chk_user_authority]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[chk_user_authority](@grpID int ,@mnID int)
as
begin
select count(AccessRights) from GroupRights where (GroupID=@grpID and MenuID=@mnID and 
AccessRights='A,E,D,V') or (AccessRights='A' and GroupID=@grpID and MenuID=@mnID)
end
GO
