USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Analysis_Header]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_Analysis_Header]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Report_Analysis_Header](@programmeid int)
AS
begin
select Programmename as 'Programme Name'
from
Survey_Analysis_Sheet
where programmeid=@programmeid
end
GO
