USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_Get_no_of_Analysis]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_Get_no_of_Analysis]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_Get_no_of_Analysis](@Programme_ID int)
as
 begin
 select Analysis_I_Category,
 Analysis_II_Category,
 Analysis_III_Category
 FROM Survey_Analysis_Sheet
 WHERE ProgrammeID=@Programme_ID
 end
GO
