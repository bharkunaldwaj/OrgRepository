USE [Feedback360_Dev2]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_CSVToTable]    Script Date: 06/23/2015 10:42:52 ******/
DROP FUNCTION [dbo].[fn_CSVToTable]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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
                                ISNULL(NULLIF(CHARINDEX(',', @StringInput) - 1, -1),
                                LEN(@StringInput)))
        SET @StringInput = SUBSTRING(@StringInput,
                                     ISNULL(NULLIF(CHARINDEX(',', @StringInput), 0),
                                     LEN(@StringInput)) + 1, LEN(@StringInput))

        INSERT INTO @OutputTable ( [Value] )
        VALUES ( @String )
    END
    
    RETURN
END
GO
