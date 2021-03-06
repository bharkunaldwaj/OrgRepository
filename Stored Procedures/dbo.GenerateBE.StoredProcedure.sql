USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[GenerateBE]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[GenerateBE]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  Stored Procedure createProperty    Script Date: 1/1/2000 12:08:59 PM ******/
create procedure  [dbo].[GenerateBE](@TableName varchar(200))
as
declare @Str varchar(2000)
declare @tab varchar(20)
declare @Str1 varchar(2000)
--declare @TableName varchar(200)
declare @colname varchar(200)
declare @colDTtype varchar(200)
declare @FrontType varchar(200)
select @TableName = @TableName
declare @enterKey varchar(200)
select @enterKey = CHAR(13) + CHAR(10)
Declare @PropertyName varchar(50)

set @tab=space(4)

declare  c1 cursor for

select syscolumns.name 
,DTtype = 
	CASE xusertype
		WHEN  167 THEN 'VarChar'
		WHEN 108 THEN 'numeric'
		WHEN 104 THEN 'bit'
		WHEN 61 THEN 'DateTime'
		WHEN 60 THEN 'Money'
		WHEN 56 THEN 'int'
		WHEN 175 THEN 'char'
	END
,FrontType = 
	CASE xusertype
		WHEN  167 THEN 'String '
		WHEN  175 THEN 'String '
		WHEN 108 THEN 'int? '
		WHEN 104 THEN 'bool? '
		WHEN 61 THEN 'DateTime? '
		WHEN 60 THEN 'Decimal? '
		WHEN 56 THEN 'int? '
		WHEN 62 THEN 'double? '
	else
		' NotFound '
	END

FROM syscolumns , sysobjects 
WHERE syscolumns.id = sysobjects.id and  sysobjects.name =@TableName

OPEN c1
FETCH NEXT FROM c1 INTO @colname, @colDTtype,@FrontType
set @str1=''
print 'public class ' + @TableName + '_BE {'
WHILE @@FETCH_STATUS = 0
BEGIN
	set @PropertyName = substring(@colname,charindex('_',@colname,1)+1 , len(@colname))
	set @Str = @tab + 'public ' + @FrontType +  @PropertyName +  ' { get; set; }'
	print @Str
	if @FrontType='String'
		set @str1=@str1  +  @tab + @tab + 'this.' + @PropertyName + ' = String.Empty ;' + char(13)
	else
		set @str1=@str1  + @tab + @tab + 'this.' + @PropertyName + ' = null ;' + char(13)

	FETCH NEXT FROM c1 INTO @colname, @colDTtype , @FrontType
END
print char(13) + @tab + 'public ' + @TableName + '_BE() {' + char(13) +  @str1 + @tab + '} ' + char(13) + '}'
CLOSE c1 
DEALLOCATE c1





set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
