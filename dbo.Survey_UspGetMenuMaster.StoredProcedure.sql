USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspGetMenuMaster]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspGetMenuMaster]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspGetMenuMaster]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspGetMenuMaster]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create PROCEDURE [dbo].[Survey_UspGetMenuMaster]
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    Select MenuID
            , Name
			, Page
			, ParentID
			, Visibility
			, SortOrder
			, ADEVFlag 

	from Survey_MenuMaster 
	where visibility=0
	order by SortOrder

END
' 
END
GO
