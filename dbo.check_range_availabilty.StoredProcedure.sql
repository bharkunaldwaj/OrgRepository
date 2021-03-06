USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[check_range_availabilty]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[check_range_availabilty]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[check_range_availabilty]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[check_range_availabilty]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[check_range_availabilty](@entered_name 
varchar(200), @chk_count int output)
as
 begin
 select @chk_count=count(*) from Question_Range 
 where Range_Name=@entered_name
 end
' 
END
GO
