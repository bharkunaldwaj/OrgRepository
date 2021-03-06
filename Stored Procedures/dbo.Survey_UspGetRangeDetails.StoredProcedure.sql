USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspGetRangeDetails]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspGetRangeDetails]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspGetRangeDetails]

@RangeName varChar(200)

as

Begin

select 
[dbo].[Range_Data].[Range_Id],
[dbo].[Range_Data].[Rating_Text],
[dbo].[Question_Range].[Range_Name],
[dbo].[Question_Range].[Range_upto],
[dbo].[Question_Range].[Range_title]

From [dbo].[Question_Range] INNER JOIN 
[dbo].[Range_Data] on [dbo].[Range_Data].Range_Id=[dbo].[Question_Range].Range_Id
where 
[dbo].[Question_Range].Range_Name=@RangeName

End
GO
