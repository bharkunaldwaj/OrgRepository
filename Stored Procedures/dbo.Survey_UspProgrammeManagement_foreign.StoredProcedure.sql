USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspProgrammeManagement_foreign]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspProgrammeManagement_foreign]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_UspProgrammeManagement_foreign](@prog_id int,@Data1 varchar(max)
,@Data2 varchar(max),@Data3 varchar(max), @Delimiter char(1))       
   as    
        declare @idx1 int       
        declare @idx2 int       
        declare @idx3 int       
        
        declare @slice1 varchar(max)       
        declare @slice2 varchar(max)       
        declare @slice3 varchar(max)       
    
   begin
		/************************************************************/
		DELETE from [Survey_AnalysisSheet_Category_Details] where [Programme_Id]=@prog_id       
   		/************************************************************/
   		select @idx1 = 1       
            if len(@Data1)<1 or @Data1 is null  return       
            
            select @idx2 = 1       
            if len(@Data2)<1 or @Data2 is null  return       
            
            select @idx3 = 1       
            if len(@Data3)<1 or @Data3 is null  return       
         
       while @idx1!= 0 or @idx2!= 0 or @idx3!= 0      
       begin       
           set @idx1 = charindex(@Delimiter,@Data1)       
           if @idx1!=0       
               set @slice1 = left(@Data1,@idx1 - 1)       
           else       
               set @slice1 = @Data1       
               
               
               set @idx2 = charindex(@Delimiter,@Data2)       
           if @idx2!=0       
               set @slice2 = left(@Data2,@idx2 - 1)       
           else       
               set @slice2 = @Data2       
               
               
               
               set @idx3 = charindex(@Delimiter,@Data3)       
           if @idx3!=0       
               set @slice3 = left(@Data3,@idx3 - 1)       
           else       
               set @slice3 = @Data3       
               
   
           if(len(@slice1)>0)
				insert into Survey_AnalysisSheet_Category_Details(Category_Detail ,Programme_Id, Analysis_Type,Category_Name) values(@slice1,@prog_id,@slice2,@slice3)       
     
           set @Data1= right(@Data1,len(@Data1) - @idx1)       
           if len(@Data1) = 0 break       
           
                      set @Data2= right(@Data2,len(@Data2) - @idx2)       
           if len(@Data2) = 0 break       


           set @Data3= right(@Data3,len(@Data3) - @idx3)       
           if len(@Data3) = 0 break       

       end   
       
   return       
   end
GO
