USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspGroupRights]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspGroupRights]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Survey_UspGroupRights]
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

	IF(EXISTS(SELECT * FROM Survey_GroupRights WHERE GROUPID = @intID))
	BEGIN
		DELETE FROM Survey_GroupRights WHERE GROUPID = @intID
	END

    INSERT INTO Survey_GroupRights (GROUPID,MENUID,ACCESSRIGHTS)
	SELECT @intID AS GROUPID, A.[VALUE] AS MENUID, Replace(REPLACE(B.[VALUE], '-' , ''),'@',',') AS ACCESSRIGHTS
	FROM FN_CSVTOTABLE(@chvMenuID) A 
	INNER JOIN FN_CSVTOTABLE(@chvAccessRights) B ON A.ID = B.ID
	
	-- To insert menu rights which are not appear in tree view menu	
	INSERT INTO Survey_GroupRights (GROUPID,MENUID,ACCESSRIGHTS)
	SELECT @intID AS GROUPID, MenuID,'A,E,D,V' FROM Survey_MenuMaster WHERE VISIBILITY=1 AND ISACTIVE=1

IF( @@ERROR <> 0)
Begin
	--Print 'Rollbacking'
	print @@ERROR
	ROLLBACK TRAN
End
ELSE
	COMMIT TRAN
GO
