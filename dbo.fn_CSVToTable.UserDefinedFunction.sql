USE [Feedback360_Dev2]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_CSVToTable]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_CSVToTable]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_CSVToTable]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_CSVToTable]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		Rajesh Kumar
-- Create date: 26Aug2010
-- Description:	This function is used to take string with commas and ouput as a table
-- Modification History:
-- =============================================
create FUNCTION [dbo].[fn_CSVToTable] ( @StringInput nVARCHAR(MAX) )
RETURNS @OutputTable TABLE ([Id] int IDENTITY(1,1), [Value] nVARCHAR(MAX) )
AS
BEGIN

    DECLARE @String    nVARCHAR(max)

    WHILE LEN(@StringInput) > 0
    BEGIN
        SET @String      = LEFT(@StringInput, 
                                ISNULL(NULLIF(CHARINDEX('','', @StringInput) - 1, -1),
                                LEN(@StringInput)))
        SET @StringInput = SUBSTRING(@StringInput,
                                     ISNULL(NULLIF(CHARINDEX('','', @StringInput), 0),
                                     LEN(@StringInput)) + 1, LEN(@StringInput))

        INSERT INTO @OutputTable ( [Value] )
        VALUES ( @String )
    END
    
    RETURN
END
' 
END
GO
