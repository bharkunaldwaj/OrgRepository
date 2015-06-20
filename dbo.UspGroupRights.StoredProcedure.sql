USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspGroupRights]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspGroupRights]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspGroupRights]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspGroupRights]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create PROCEDURE [dbo].[UspGroupRights]
(
	@intID as int,
	@chvMenuID as varchar(500),
	@chvAccessRights as varchar(1000)
)
AS

BEGIN TRAN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(EXISTS(SELECT * FROM GroupRights WHERE GROUPID = @intID))
	BEGIN
		DELETE FROM GroupRights WHERE GROUPID = @intID
	END

    INSERT INTO GroupRights (GROUPID,MENUID,ACCESSRIGHTS)
	SELECT @intID AS GROUPID, A.[VALUE] AS MENUID, Replace(REPLACE(B.[VALUE], ''-'' , ''''),''@'','','') AS ACCESSRIGHTS
	FROM FN_CSVTOTABLE(@chvMenuID) A 
	INNER JOIN FN_CSVTOTABLE(@chvAccessRights) B ON A.ID = B.ID
	
	-- To insert menu rights which are not appear in tree view menu	
	INSERT INTO GroupRights (GROUPID,MENUID,ACCESSRIGHTS)
	SELECT @intID AS GROUPID, MenuID,''A,E,D,V'' FROM MenuMaster WHERE VISIBILITY=1 AND ISACTIVE=1

IF( @@ERROR <> 0)
Begin
	--Print ''Rollbacking''
	print @@ERROR
	ROLLBACK TRAN
End
ELSE
	COMMIT TRAN
' 
END
GO
