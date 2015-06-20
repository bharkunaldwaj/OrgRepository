<Serializable(), CLSCompliant(True)> _
Public Class CAdoData

#Region "Constants - Null-Equivalent Values"
    Public Const NULL_INTEGER As Integer = Integer.MinValue
    Public Const NULL_DOUBLE As Double = Double.NaN
    Public Const NULL_DECIMAL As Decimal = Decimal.MinValue 'Note: Use Decimal.IsNaN instead of equals
    Public Shared NULL_DATE As DateTime = DateTime.MinValue
    Public Shared NULL_GUID As Guid = Guid.Empty
    Public Shared NULL_BOOLEAN As Boolean = False 'One-Way, converts DBNull => bool
#End Region

#Region "Shared - DBNull Substitution (Applied before Insert/Update)"
    Public Shared Function NullVal(ByVal obj As Object) As Object
        If IsNothing(obj) Then Return DBNull.Value 'All reference types, plus 3.5 nullable types

        If TypeOf (obj) Is Guid Then
            If NULL_GUID = CType(obj, Guid) Then Return DBNull.Value Else Return obj
        End If
        If TypeOf (obj) Is Integer Then
            If NULL_INTEGER = CInt(obj) Then Return DBNull.Value Else Return obj
        End If
        If TypeOf (obj) Is Decimal Then
            If NULL_DECIMAL = CDec(obj) Then Return DBNull.Value Else Return obj
        End If
        If TypeOf (obj) Is DateTime Then
            If NULL_DATE = CDate(obj) Then Return DBNull.Value Else Return obj
        End If
        'Special case - IsNan
        If TypeOf (obj) Is Double Then
            If Double.IsNaN(CDbl(obj)) Then Return DBNull.Value Else Return obj
        End If

        Return obj
    End Function
#End Region

#Region "Shared - Get Data From DataRow"
    'Column names, with auto defaults for NULL
    Public Shared Function GetStr(ByVal dr As DataRow, ByVal columnName As String) As String
        Return GetStr(dr, columnName, Nothing)
    End Function
    Public Shared Function GetBool(ByVal dr As DataRow, ByVal columnName As String) As Boolean
        Return GetBool(dr, columnName, NULL_BOOLEAN)
    End Function
    Public Shared Function GetDec(ByVal dr As DataRow, ByVal columnName As String) As Decimal
        Return GetDec(dr, columnName, NULL_DECIMAL)
    End Function
    Public Shared Function GetInt(ByVal dr As DataRow, ByVal columnName As String) As Integer
        Return GetInt(dr, columnName, NULL_INTEGER)
    End Function
    Public Shared Function GetDbl(ByVal dr As DataRow, ByVal columnName As String) As Double
        Return GetDbl(dr, columnName, NULL_DOUBLE)
    End Function
    Public Shared Function GetDate(ByVal dr As DataRow, ByVal columnName As String) As DateTime
        Return GetDate(dr, columnName, NULL_DATE)
    End Function
    Public Shared Function GetByte(ByVal dr As DataRow, ByVal columnName As String) As Byte
        Return GetByte(dr, columnName, Nothing)
    End Function
    Public Shared Function GetBytes(ByVal dr As DataRow, ByVal columnName As String) As Byte()
        Return GetBytes(dr, columnName, Nothing)
    End Function
    Public Shared Function GetGuid(ByVal dr As DataRow, ByVal columnName As String) As Guid
        Return GetGuid(dr, columnName, NULL_GUID)
    End Function

    'Column numbers, with auto defaults for NULL
    Public Shared Function GetStr(ByVal dr As DataRow, ByVal columnIndex As Integer) As String
        Return GetStr(dr, columnIndex, Nothing)
    End Function
    Public Shared Function GetBool(ByVal dr As DataRow, ByVal columnIndex As Integer) As Boolean
        Return GetBool(dr, columnIndex, NULL_BOOLEAN)
    End Function
    Public Shared Function GetDec(ByVal dr As DataRow, ByVal columnIndex As Integer) As Decimal
        Return GetDec(dr, columnIndex, NULL_DECIMAL)
    End Function
    Public Shared Function GetInt(ByVal dr As DataRow, ByVal columnIndex As Integer) As Integer
        Return GetInt(dr, columnIndex, NULL_INTEGER)
    End Function
    Public Shared Function GetDbl(ByVal dr As DataRow, ByVal columnIndex As Integer) As Double
        Return GetDbl(dr, columnIndex, NULL_DOUBLE)
    End Function
    Public Shared Function GetDate(ByVal dr As DataRow, ByVal columnIndex As Integer) As DateTime
        Return GetDate(dr, columnIndex, NULL_DATE)
    End Function
    Public Shared Function GetByte(ByVal dr As DataRow, ByVal columnIndex As Integer) As Byte
        Return GetByte(dr, columnIndex, Nothing)
    End Function
    Public Shared Function GetBytes(ByVal dr As DataRow, ByVal columnIndex As Integer) As Byte()
        Return GetBytes(dr, columnIndex, Nothing)
    End Function
    Public Shared Function GetGuid(ByVal dr As DataRow, ByVal columnIndex As Integer) As Guid
        Return GetGuid(dr, columnIndex, NULL_GUID)
    End Function

    'Column names, with custom defaults for NULL
    Public Shared Function GetStr(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As String) As String
        Return CStr(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetBool(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Boolean) As Boolean
        Return CBool(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetDec(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Decimal) As Decimal
        Return CDec(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetInt(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Integer) As Integer
        Return CInt(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetDbl(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Double) As Double
        Return CDbl(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetDate(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Date) As Date
        Return CDate(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetByte(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Byte()) As Byte
        Return CType(GetValue(dr, columnName, nullValue), Byte)
    End Function
    Public Shared Function GetBytes(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Byte()) As Byte()
        Return CType(GetValue(dr, columnName, nullValue), Byte())
    End Function
    Public Shared Function GetGuid(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Guid) As Guid
        Return CType(GetValue(dr, columnName, nullValue), Guid)
    End Function

    'Column numbers, with custom defaults for NULL
    Public Shared Function GetStr(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As String) As String
        Return CStr(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetBool(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Boolean) As Boolean
        Return CBool(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetDec(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Decimal) As Decimal
        Return CDec(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetInt(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Integer) As Integer
        Return CInt(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetDbl(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Double) As Double
        Return CDbl(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetDate(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Date) As Date
        Return CDate(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetByte(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Byte()) As Byte
        Return CType(GetValue(dr, columnIndex, nullValue), Byte)
    End Function
    Public Shared Function GetBytes(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Byte()) As Byte()
        Return CType(GetValue(dr, columnIndex, nullValue), Byte())
    End Function
    Public Shared Function GetGuid(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Guid) As Guid
        Return CType(GetValue(dr, columnIndex, nullValue), Guid)
    End Function
#End Region

#Region "Shared - Get Data From DataReader"
    'Column names, with auto defaults for NULL
    Public Shared Function GetStr(ByVal dr As IDataReader, ByVal columnName As String) As String
        Return GetStr(dr, columnName, Nothing)
    End Function
    Public Shared Function GetBool(ByVal dr As IDataReader, ByVal columnName As String) As Boolean
        Return GetBool(dr, columnName, NULL_BOOLEAN)
    End Function
    Public Shared Function GetDec(ByVal dr As IDataReader, ByVal columnName As String) As Decimal
        Return GetDec(dr, columnName, NULL_DECIMAL)
    End Function
    Public Shared Function GetInt(ByVal dr As IDataReader, ByVal columnName As String) As Integer
        Return GetInt(dr, columnName, NULL_INTEGER)
    End Function
    Public Shared Function GetLong(ByVal dr As IDataReader, ByVal columnName As String) As Long
        Return Getlong(dr, columnName, NULL_INTEGER)
    End Function
    Public Shared Function GetDbl(ByVal dr As IDataReader, ByVal columnName As String) As Double
        Return GetDbl(dr, columnName, NULL_DOUBLE)
    End Function
    Public Shared Function GetDate(ByVal dr As IDataReader, ByVal columnName As String) As DateTime
        Return GetDate(dr, columnName, NULL_DATE)
    End Function
    Public Shared Function GetByte(ByVal dr As IDataReader, ByVal columnName As String) As Byte
        Return GetByte(dr, columnName, Nothing)
    End Function
    Public Shared Function GetBytes(ByVal dr As IDataReader, ByVal columnName As String) As Byte()
        Return GetBytes(dr, columnName, Nothing)
    End Function
    Public Shared Function GetGuid(ByVal dr As IDataReader, ByVal columnName As String) As Guid
        Return GetGuid(dr, columnName, NULL_GUID)
    End Function

    'Column names, with custom defaults for NULL
    Public Shared Function GetStr(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As String) As String
        Return CStr(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetBool(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Boolean) As Boolean
        Return CBool(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetDec(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Decimal) As Decimal
        Return CDec(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetInt(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Integer) As Integer
        Return CInt(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetLong(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Integer) As Long
        Return CLng(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetDbl(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Double) As Double
        Return CDbl(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetDate(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Date) As Date
        If dr.GetType.ToString = "MySql.Data.MySqlClient.MySqlDataReader" Then
            Return CMySqlClient.GetDate(dr(GetOrdinal(dr, columnName)), nullValue)
        End If
        Return CDate(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetByte(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Byte()) As Byte
        Return CType(GetValue(dr, columnName, nullValue), Byte)
    End Function
    Public Shared Function GetBytes(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Byte()) As Byte()
        Return CType(GetValue(dr, columnName, nullValue), Byte())
    End Function
    Public Shared Function GetGuid(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Guid) As Guid
        Return CType(GetValue(dr, columnName, nullValue), Guid)
    End Function


    'Column indexes, with auto defaults for NULL
    Public Shared Function GetStr(ByVal dr As IDataReader, ByVal columnIndex As Integer) As String
        Return GetStr(dr, columnIndex, Nothing)
    End Function
    Public Shared Function GetBool(ByVal dr As IDataReader, ByVal columnIndex As Integer) As Boolean
        Return GetBool(dr, columnIndex, NULL_BOOLEAN)
    End Function
    Public Shared Function GetDec(ByVal dr As IDataReader, ByVal columnIndex As Integer) As Decimal
        Return GetDec(dr, columnIndex, NULL_DECIMAL)
    End Function
    Public Shared Function GetInt(ByVal dr As IDataReader, ByVal columnIndex As Integer) As Integer
        Return GetInt(dr, columnIndex, NULL_INTEGER)
    End Function
    Public Shared Function GetDbl(ByVal dr As IDataReader, ByVal columnIndex As Integer) As Double
        Return GetDbl(dr, columnIndex, NULL_DOUBLE)
    End Function
    Public Shared Function GetDate(ByVal dr As IDataReader, ByVal columnIndex As Integer) As DateTime
        Return GetDate(dr, columnIndex, NULL_DATE)
    End Function
    Public Shared Function GetByte(ByVal dr As IDataReader, ByVal columnIndex As Integer) As Byte
        Return GetByte(dr, columnIndex, Nothing)
    End Function
    Public Shared Function GetBytes(ByVal dr As IDataReader, ByVal columnIndex As Integer) As Byte()
        Return GetBytes(dr, columnIndex, Nothing)
    End Function
    Public Shared Function GetGuid(ByVal dr As IDataReader, ByVal columnIndex As Integer) As Guid
        Return GetGuid(dr, columnIndex, NULL_GUID)
    End Function


    'Column indexes, with custom defaults for NULL
    Public Shared Function GetStr(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As String) As String
        Return CStr(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetBool(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Boolean) As Boolean
        Return CBool(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetDec(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Decimal) As Decimal
        Return CDec(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetInt(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Integer) As Integer
        Return CInt(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetDbl(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Double) As Double
        Return CDbl(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetDate(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Date) As Date
        If dr.GetType.ToString = "MySql.Data.MySqlClient.MySqlDataReader" Then
            Return CMySqlClient.GetDate(dr(columnIndex), nullValue)
        End If
        Return CDate(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetByte(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Byte()) As Byte
        Return CType(GetValue(dr, columnIndex, nullValue), Byte)
    End Function
    Public Shared Function GetBytes(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Byte()) As Byte()
        Return CType(GetValue(dr, columnIndex, nullValue), Byte())
    End Function
    Public Shared Function GetGuid(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Guid) As Guid
        Return CType(GetValue(dr, columnIndex, nullValue), Guid)
    End Function
#End Region

#Region "Shared - Nullable Types (Not normally used due to small performance hit)"
    'DataReader
    Public Shared Function GetIntNullable(ByVal dr As IDataReader, ByVal columnName As String) As Integer?
        Return CType(GetValue(dr, columnName, Nothing), Integer?)
    End Function
    Public Shared Function GetDecNullable(ByVal dr As IDataReader, ByVal columnName As String) As Decimal?
        Return CType(GetValue(dr, columnName, Nothing), Decimal?)
    End Function
    Public Shared Function GetDateNullable(ByVal dr As IDataReader, ByVal columnName As String) As DateTime?
        Return CType(GetValue(dr, columnName, Nothing), DateTime?)
    End Function
    Public Shared Function GetDblNullable(ByVal dr As IDataReader, ByVal columnName As String) As Double?
        Return CType(GetValue(dr, columnName, Nothing), Double?)
    End Function
    Public Shared Function GetGuidNullable(ByVal dr As IDataReader, ByVal columnName As String) As Guid?
        Return CType(GetValue(dr, columnName, Nothing), Guid?)
    End Function
    Public Shared Function GetBoolNullable(ByVal dr As IDataReader, ByVal columnName As String) As Boolean?
        Return CType(GetValue(dr, columnName, Nothing), Boolean?)
    End Function

    'DataSet
    Public Shared Function GetIntNullable(ByVal dr As DataRow, ByVal columnName As String) As Integer?
        Return CType(GetValue(dr, columnName, Nothing), Integer?)
    End Function
    Public Shared Function GetDecNullable(ByVal dr As DataRow, ByVal columnName As String) As Decimal?
        Return CType(GetValue(dr, columnName, Nothing), Decimal?)
    End Function
    Public Shared Function GetDateNullable(ByVal dr As DataRow, ByVal columnName As String) As DateTime?
        Return CType(GetValue(dr, columnName, Nothing), DateTime?)
    End Function
    Public Shared Function GetDblNullable(ByVal dr As DataRow, ByVal columnName As String) As Double?
        Return CType(GetValue(dr, columnName, Nothing), Double?)
    End Function
    Public Shared Function GetGuidNullable(ByVal dr As DataRow, ByVal columnName As String) As Guid?
        Return CType(GetValue(dr, columnName, Nothing), Guid?)
    End Function
    Public Shared Function GetBoolNullable(ByVal dr As DataRow, ByVal columnName As String) As Boolean?
        Return CType(GetValue(dr, columnName, Nothing), Boolean?)
    End Function
#End Region

#Region "Shared (Private) - Get Data From DataReader/DataRow"
    Public Shared Function GetValue(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Object) As Object
        Dim index As Integer = dr.Table.Columns.IndexOf(columnName)
        If index = -1 Then Return nullValue
        Return GetValue(dr, index, nullValue)
    End Function
    Public Shared Function GetValue(ByVal dr As DataRow, ByVal columnNumber As Integer, ByVal nullValue As Object) As Object
        GetValue = dr(columnNumber)
        If TypeOf (GetValue) Is System.DBNull Then Return nullValue
        If IsNothing(GetValue) Then Return nullValue
    End Function
    Public Shared Function GetValue(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Object) As Object
        Dim i As Integer = GetOrdinal(dr, columnName)

        If dr.IsDBNull(i) Then Return nullValue

        Try
            Return dr.Item(i)
        Catch ex As System.Exception
            Throw New Exception("Invalid column name/number: " & columnName & " (" & ex.Message & ")")
        End Try
    End Function
    Public Shared Function GetValue(ByVal dr As IDataReader, ByVal columnNumber As Integer, ByVal nullValue As Object) As Object
        If dr.IsDBNull(columnNumber) Then Return nullValue
        Return dr.Item(columnNumber)
    End Function
    Public Shared Function GetOrdinal(ByVal dr As IDataReader, ByVal columnName As String) As Integer
        Try
            Return dr.GetOrdinal(columnName)
        Catch ex As IndexOutOfRangeException
            'Might be a column number
            Dim i As Integer
            If Integer.TryParse(columnName, i) Then Return i

            'Or a CSV file with whitespace around the column name
            For i = 0 To dr.FieldCount - 1
                If dr.GetName(i).Trim.ToLower = columnName.Trim.ToLower Then Return i
            Next

            Throw New Exception("Invalid columnName '" & columnName & "'.")
        End Try
    End Function
#End Region

End Class
