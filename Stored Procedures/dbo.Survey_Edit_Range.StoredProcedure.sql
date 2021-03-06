USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_Edit_Range]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_Edit_Range]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_Edit_Range](@Range_Id integer)
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
GO
