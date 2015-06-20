USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspGetRangeList]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspGetRangeList]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspGetRangeList]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspGetRangeList]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure [dbo].[Survey_UspGetRangeList]

as

Begin

select 
[dbo].[Question_Range].[Range_Id],
[dbo].[Question_Range].[Range_Name],
[dbo].[Question_Range].[Range_upto],
[dbo].[Question_Range].[Range_title]

From [dbo].[Question_Range] 

End
' 
END
GO
