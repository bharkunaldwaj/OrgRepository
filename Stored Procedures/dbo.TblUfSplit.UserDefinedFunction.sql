USE [Feedback360_Dev2]
GO
/****** Object:  UserDefinedFunction [dbo].[TblUfSplit]    Script Date: 06/23/2015 10:42:52 ******/
DROP FUNCTION [dbo].[TblUfSplit]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[TblUfSplit] (@String nvarchar(MAX), @Delimiter char(1))
Returns @Results Table (Items nvarchar(MAX))
As
Begin
Declare @Index int
Declare @Slice nvarchar(4000)

Select @Index = 1
If @String Is NULL Return

While @Index != 0
Begin
Select @Index = CharIndex(@Delimiter, @String)
If @Index <> 0

Select @Slice = left(@String, @Index - 1)

else

Select @Slice = @String
Insert into @Results(Items) Values (@Slice)
Select @String = right(@String, Len(@String) - @Index)

If Len(@String) = 0 break

End
Return
End
GO
