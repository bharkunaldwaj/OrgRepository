USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_Get_no_of_Analysis]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_Get_no_of_Analysis]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_Get_no_of_Analysis]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_Get_no_of_Analysis]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[Survey_Get_no_of_Analysis](@Programme_ID int)
as
 begin
 select Analysis_I_Category,
 Analysis_II_Category,
 Analysis_III_Category
 FROM Survey_Analysis_Sheet
 WHERE ProgrammeID=@Programme_ID
 end
' 
END
GO
