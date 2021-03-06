USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspGetMenuMaster]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspGetMenuMaster]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UspGetMenuMaster]
@ADEVFlag varchar(1)
	
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

	from MenuMaster 
	where visibility=0 and (ADEVFlag=@ADEVFlag or  ADEVFlag is null)
	order by SortOrder

END
GO
