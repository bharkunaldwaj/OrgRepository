USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[DeleteAllProcedures]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteAllProcedures]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteAllProcedures]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteAllProcedures]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create Procedure [dbo].[DeleteAllProcedures]
As
      declare @procName varchar(500)
      declare cur cursor 
            for select [name] from sys.objects where type = ''p''
      open cur
 

      fetch next from cur into @procName
      while @@fetch_status = 0
      begin
            if @procName <> ''DeleteAllProcedures''
                  exec(''drop procedure '' + @procName)
                  fetch next from cur into @procName
      end
      close cur
      deallocate cur
' 
END
GO
