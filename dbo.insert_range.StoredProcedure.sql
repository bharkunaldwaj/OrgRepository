USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[insert_range]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[insert_range]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[insert_range]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[insert_range]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[insert_range](
     @RangeID int ,
     @name varchar(200) ,
           @title varchar(200),
           @r_upto int,
           @rating_text varchar(1000),
           @mode char
           )
as
declare @Question_Range_id int
begin

if(@mode=''I'')
BEGIN
insert into Question_Range(Range_Name,Range_Title,Range_upto)  values(@name,@title,@r_upto)
set @Question_Range_id=scope_IDENTITY() 
exec dbo.insert_rangeData @Question_Range_id ,@rating_Text,'',''
END

if(@mode=''V'')
BEGIN
Update Question_Range set Range_Name=@name,Range_Title=@title where Range_Id=@RangeID
exec dbo.insert_rangeData @RangeID ,@rating_Text,'',''

END



end

' 
END
GO
