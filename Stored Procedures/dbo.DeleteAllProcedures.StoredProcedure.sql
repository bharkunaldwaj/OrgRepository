USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[DeleteAllProcedures]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[DeleteAllProcedures]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create Procedure [dbo].[DeleteAllProcedures]
As
      declare @procName varchar(500)
      declare cur cursor 
            for select [name] from sys.objects where type = 'p'
      open cur
 

      fetch next from cur into @procName
      while @@fetch_status = 0
      begin
            if @procName <> 'DeleteAllProcedures'
                  exec('drop procedure ' + @procName)
                  fetch next from cur into @procName
      end
      close cur
      deallocate cur
GO
