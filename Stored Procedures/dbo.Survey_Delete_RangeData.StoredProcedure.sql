USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_Delete_RangeData]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_Delete_RangeData]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_Delete_RangeData]
(@Range_Id int)
as
begin

delete from range_data where Range_Id=@Range_Id
delete from question_range where Range_Id=@Range_Id
end
GO
