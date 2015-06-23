USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[check_range_availabilty]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[check_range_availabilty]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[check_range_availabilty](@entered_name 
varchar(200), @chk_count int output)
as
 begin
 select @chk_count=count(*) from Question_Range 
 where Range_Name=@entered_name
 end
GO
