USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[chk_user_authority]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[chk_user_authority]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[chk_user_authority]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[chk_user_authority]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE proc [dbo].[chk_user_authority](@grpID int ,@mnID int)
as
begin
select count(AccessRights) from GroupRights where (GroupID=@grpID and MenuID=@mnID and 
AccessRights=''A,E,D,V'') or (AccessRights=''A'' and GroupID=@grpID and MenuID=@mnID)
end

' 
END
GO
