USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_Edit_Range]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_Edit_Range]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_Edit_Range]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_Edit_Range]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[Survey_Edit_Range](@Range_Id integer)
as
begin
select 
[question_range].Range_ID,
[question_range].Range_Name,
[question_range].Range_Title,
[question_range].Range_upto,
[range_data].Rating_Text

from question_range Inner join 
range_data on question_range.range_id=range_data.range_id
where question_range.Range_Id=@Range_Id
end
' 
END
GO
