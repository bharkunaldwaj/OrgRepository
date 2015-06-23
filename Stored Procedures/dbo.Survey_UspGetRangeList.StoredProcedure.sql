USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspGetRangeList]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspGetRangeList]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspGetRangeList]

as

Begin

select 
[dbo].[Question_Range].[Range_Id],
[dbo].[Question_Range].[Range_Name],
[dbo].[Question_Range].[Range_upto],
[dbo].[Question_Range].[Range_title]

From [dbo].[Question_Range] 

End
GO
