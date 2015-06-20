USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Analysis_Header]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Report_Analysis_Header]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Report_Analysis_Header]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Report_Analysis_Header]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[Report_Analysis_Header](@programmeid int)
AS
begin
select Programmename as ''Programme Name''
from
Survey_Analysis_Sheet
where programmeid=@programmeid
end
' 
END
GO
