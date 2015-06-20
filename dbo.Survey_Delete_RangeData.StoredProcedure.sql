USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_Delete_RangeData]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_Delete_RangeData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_Delete_RangeData]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_Delete_RangeData]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[Survey_Delete_RangeData]
(@Range_Id int)
as
begin

delete from range_data where Range_Id=@Range_Id
delete from question_range where Range_Id=@Range_Id
end
' 
END
GO
