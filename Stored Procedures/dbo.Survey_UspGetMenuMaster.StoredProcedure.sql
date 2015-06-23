USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspGetMenuMaster]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspGetMenuMaster]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Survey_UspGetMenuMaster]
	
	
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
GO
