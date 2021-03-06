USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[insert_rangeData]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[insert_rangeData]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[insert_rangeData](@Question_Range_id int,@rangeData varchar(1000), @Delimiter char(1))       
   as       
  
    begin       
        declare @idx int       
        declare @slice varchar(8000)       
          
        select @idx = 1       
            if len(@rangeData)<1 or @rangeData is null  return       
            
         delete from Range_Data where Range_Id=@Question_Range_id
         
       while @idx!= 0       
       begin       
           set @idx = charindex(@Delimiter,@rangeData)       
           if @idx!=0       
               set @slice = left(@rangeData,@idx - 1)       
           else       
               set @slice = @rangeData       
             
           if(len(@slice)>=0)  
               insert into Range_Data(Range_Id, Rating_Text) values(@Question_Range_id,REPLACE(@slice,'sorry',''))       
     
           set @rangeData= right(@rangeData,len(@rangeData) - @idx)       
           if len(@rangeData) = 0 break       
       end   
   return       
   end
GO
