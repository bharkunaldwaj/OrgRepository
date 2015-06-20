Imports System.Collections

Public Class CUtilities

#Region "Count Description"
    Public Shared Function FileSize(ByVal size As Integer) As String
        If size > 1000000000 Then Return CDbl(size / 1000000000).ToString("f2") & "Gb"
        If size > 100000000 Then Return CDbl(size / 1000000).ToString("f0") & "Mb"
        If size > 10000000 Then Return CDbl(size / 1000000).ToString("f1") & "Mb"
        If size > 1000000 Then Return CDbl(size / 1000000).ToString("f2") & "Mb"
        If size > 100000 Then Return CDbl(size / 1000).ToString("f0") & "kB"
        If size > 10000 Then Return CDbl(size / 1000).ToString("f1") & "kB"
        If size > 1000 Then Return CInt(size / 1000).ToString("f2") & "kB"
        Return size & "B"
    End Function
    Public Shared Function CountSummary(ByVal count As Integer, ByVal entityName As String) As String
        Return CountSummary(count, entityName, String.Empty)
    End Function
    Public Shared Function CountSummary(ByVal list As IList, ByVal entityName As String) As String
        Return CountSummary(list, entityName, String.Empty)
    End Function
    Public Shared Function CountSummary(ByVal list As IList, ByVal entityName As String, ByVal zeroCase As String) As String
        Return CountSummary(list.Count, entityName, zeroCase)
    End Function
    Public Shared Function CountSummary(ByVal count As Integer, ByVal entityName As String, ByVal zeroCase As String) As String
        Dim plural As String = CStr(IIf(IsNothing(entityName), String.Empty, entityName))
        If Not String.IsNullOrEmpty(plural) AndAlso Not plural.EndsWith("s") Then plural = String.Concat(plural, "s")

        If String.IsNullOrEmpty(zeroCase) Then zeroCase = String.Concat("no ", plural)
        Select Case count
            Case 0, Integer.MinValue : Return zeroCase
            Case 1 : Return String.Concat("1 ", entityName).Trim()
            Case Else : Return String.Concat(count, " ", plural).Trim()
        End Select
    End Function
    Public Shared Function NameAndCount(ByVal name As String, ByVal list As IList) As String
        Return NameAndCount(name, list, String.Empty)
    End Function
    Public Shared Function NameAndCount(ByVal name As String, ByVal count As Integer) As String
        Return NameAndCount(name, count, String.Empty)
    End Function
    Public Shared Function NameAndCount(ByVal name As String, ByVal list As IList, ByVal childEntityName As String) As String
        Return NameAndCount(name, list.Count, childEntityName)
    End Function
    Public Shared Function NameAndCount(ByVal name As String, ByVal count As Integer, ByVal childEntityName As String) As String
        Return NameAndCount(name, count, childEntityName, String.Empty)
    End Function
    Public Shared Function NameAndCount(ByVal name As String, ByVal list As IList, ByVal childEntityName As String, ByVal zeroCase As String) As String
        Return NameAndCount(name, list.Count, childEntityName, zeroCase)
    End Function
    Public Shared Function NameAndCount(ByVal name As String, ByVal count As Integer, ByVal childEntityName As String, ByVal zeroCase As String) As String
        If count = 0 OrElse count = Integer.MinValue Then
            If String.IsNullOrEmpty(zeroCase) Then Return name
            If String.IsNullOrEmpty(childEntityName) Then Return String.Concat(name, " (", zeroCase, ")")
        End If
        Return String.Concat(name, " (", CountSummary(count, childEntityName, zeroCase), ")")
    End Function
#End Region

#Region "Paging"
    Public Shared Function Page(ByVal list As IList, ByVal size As Integer, ByVal index As Integer) As IList
        If 0 = index AndAlso list.Count <= size Then Return New ArrayList(list)

        If index < 0 Then index = 0
        If (index - 1) * size > list.Count Then Return New ArrayList()

        Dim minIndex As Integer = size * index
        Dim maxIndex As Integer = size * (index + 1) - 1
        If maxIndex >= list.Count Then maxIndex = list.Count - 1

        Dim subset As New List(Of Object)(size)
        For i As Integer = minIndex To maxIndex Step 1
            subset.Add(list(i))
        Next
        Return subset
    End Function
#End Region

#Region "Truncation"
    Public Shared Function Truncate(ByVal original As String) As String
        Return Truncate(original, 30)
    End Function
    Public Shared Function Truncate(ByVal original As String, ByVal maxLength As Integer) As String
        If String.IsNullOrEmpty(original) Then Return String.Empty
        If maxLength < 1 Then Return original
        If maxLength < 3 Then maxLength = 3
        If original.Length <= maxLength Then Return original
        Return original.Substring(0, maxLength - 3) & "..."
    End Function
#End Region

#Region "Saving Files - UniqueName"
    'Modifies fileName slightly until its unique
    Public Shared Function UniqueFileName(ByVal folderPath As String, ByVal fileName As String) As String
        If String.IsNullOrEmpty(fileName) Then Return String.Empty
        fileName = IO.Path.GetFileName(fileName)

        While IO.File.Exists(String.Concat(folderPath, "\", fileName))
            fileName = UniqueNameGuess(fileName)
        End While
        Return fileName
    End Function
    'Suggests a friendly fileName that might be unique
    Private Shared Function UniqueNameGuess(ByVal fileName As String) As String
        Dim extension As String = IO.Path.GetExtension(fileName)
        Dim baseName As String = fileName.Substring(0, fileName.LastIndexOf(extension))

        If baseName.Length = 0 Then Return "_" & baseName

        Dim len As Integer = baseName.Length
        Dim suffix As String = String.Concat("(", 1, ")", extension)
        If ")" <> baseName.Substring(len - 1, 1) Then Return String.Concat(baseName, suffix)

        Dim startAt As Integer = baseName.LastIndexOf("(")
        If -1 = startAt Then Return baseName + suffix

        Dim number As String = baseName.Substring(startAt + 1, len - startAt - 2)
        Try
            Dim nextNumber As Integer = Integer.Parse(number) + 1
            Return String.Concat(baseName.Substring(0, startAt), "(", nextNumber, ")", extension)
        Catch
            Return String.Concat(baseName, suffix)
        End Try
    End Function
#End Region

#Region "Date Formats"
    Public Shared Function LongDateTime(ByVal d As DateTime) As String
        Return LongDateTime(d, "ddd d", " MMM yyyy h:mm:ss tt")
    End Function
    Public Shared Function LongDate(ByVal d As DateTime) As String
        Return LongDate(d, "ddd d", " MMM yyyy")
    End Function
    Public Shared Function LongTime(ByVal d As DateTime) As String
        Return d.ToString("h:mm:ss tt")
    End Function
    Public Shared Function LongDateTime(ByVal d As DateTime, ByVal beforeFormat As String, ByVal afterFormat As String) As String
        If DateTime.MinValue = d Then Return String.Empty
        Return String.Concat(d.ToString(beforeFormat), NumberSuffix(d.Day), d.ToString(afterFormat))
    End Function
    Public Shared Function LongDate(ByVal d As DateTime, ByVal beforeFormat As String, ByVal afterFormat As String) As String
        If DateTime.MinValue = d Then Return String.Empty
        Return String.Concat(d.ToString(beforeFormat), NumberSuffix(d.Day), d.ToString(afterFormat))
    End Function
    Public Shared Function NumberSuffix(ByVal i As Integer) As String
        Select Case i
            Case 1 : Return "st"
            Case 2 : Return "nd"
            Case 3 : Return "rd"
            Case 4 : Return "th"
            Case Else : Return String.Empty
        End Select
    End Function

    Private Const DAYS_IN_YEAR As Integer = 365
    Private Const MONTHS_IN_YEAR As Integer = 12
    Private Const DAYS_IN_MONTH As Double = CDbl(DAYS_IN_YEAR) / CDbl(MONTHS_IN_YEAR)
    Public Shared Function Timespan(ByVal t As TimeSpan) As String
        Dim sb As New System.Text.StringBuilder
        Dim days As Integer = CInt(Math.Floor(t.TotalDays))
        Dim years As Integer = CInt(Math.Floor(days / DAYS_IN_YEAR))
        If years > 0 Then
            sb.Append(CountSummary(years, "year"))
            days -= 365 * years
        End If
        Dim months As Integer = CInt(Math.Floor(days / DAYS_IN_MONTH))
        If months > 0 Then
            If sb.Length > 0 Then sb.Append(" ")
            sb.Append(CountSummary(months, "month"))
            days -= CInt(Math.Floor(DAYS_IN_MONTH * months))
        End If
        If t.TotalDays > DAYS_IN_YEAR Then Return sb.ToString 'Years, months
        If days > 0 Then sb.Append(" ").Append(CountSummary(days, " day"))
        If months > 0 Then Return sb.ToString 'Months, days
        If t.Hours > 0 Then sb.Append(" ").Append(CountSummary(t.Hours, " hr"))
        If t.TotalDays > 1 Then Return sb.ToString 'Days, hours
        If t.Minutes > 0 Then sb.Append(" ").Append(CountSummary(t.Minutes, " min"))
        If t.TotalHours > 1 Then Return sb.ToString 'Hours, Mins
        If t.Seconds > 0 Then sb.Append(" ").Append(CountSummary(t.Seconds, " sec"))
        If t.TotalMinutes > 1 Then Return sb.ToString 'Mins, Secs
        If t.Milliseconds > 0 Then sb.Append(" ").Append(t.Milliseconds).Append(" msec")
        Return sb.ToString 'Secs, Ms
    End Function
#End Region

#Region "Split"
    Public Shared Function SplitOn(ByVal s As String, ByVal lookFor As String) As List(Of String)
        Return SplitOn(s, lookFor, False)
    End Function
    Public Shared Function SplitOn(ByVal s As String, ByVal lookFor As String, ByVal caseSensitive As Boolean) As List(Of String)
        Dim list As New List(Of String)()

        'Trivial checks
        If String.IsNullOrEmpty(lookFor) Then
            list.Add(s)
            Return list
        End If

        Return SplitOn(s, lookFor, caseSensitive, list)
    End Function
    Private Shared Function SplitOn(ByVal s As String, ByVal lookFor As String, ByVal caseSensitive As Boolean, ByVal list As List(Of String)) As List(Of String)
        'Trivial checks
        If String.IsNullOrEmpty(s) Then Return list

        'Split once and recurse
        Dim index As Integer = s.IndexOf(lookFor)
        If index = -1 And Not caseSensitive Then index = s.ToLower.IndexOf(lookFor.ToLower)
        If index = -1 Then
            list.Add(s)
            Return list
        End If

        'First bit - add to list
        list.Add(s.Substring(0, index))

        'Last bit - recurse
        Return SplitOn(s.Substring(index + lookFor.Length), lookFor, caseSensitive, list)
    End Function
    Public Shared Function ReplaceAll(ByVal s As String, ByVal lookFor As String, ByVal replaceWith As String) As String
        'eg. "blah---blah" => "blah-blah" can be achieved with ReplaceAll(s, "--", "-")
        If String.IsNullOrEmpty(s) Then Return String.Empty
        While s.Contains(lookFor)
            s = s.Replace(lookFor, replaceWith)
        End While
        Return s
    End Function
#End Region

#Region "Comma-Separated"
    Private Const DELIMITER As String = ","

    'Integers
    Public Shared Function StringToListInt(ByVal s As String) As List(Of Integer)
        Return StringToListInt(s, DELIMITER)
    End Function
    Public Shared Function StringToListInt(ByVal s As String, ByVal delim As String) As List(Of Integer)
        If String.IsNullOrEmpty(s) Then Return New List(Of Integer)(0)
        Dim ss As String() = s.Split(delim.ToCharArray())
        Dim list As New List(Of Integer)(ss.Length)
        Dim int As Integer
        For Each i As String In ss
            If Integer.TryParse(i, int) Then list.Add(int)
        Next
        Return list
    End Function

    'General
    Public Shared Function ListToString(ByVal list As IList) As String
        Return ListToString(list, DELIMITER)
    End Function
    Public Shared Function ListToString(ByVal list As IList, ByVal delimiter As String) As String
        Dim sb As New System.Text.StringBuilder
        For Each i As Object In list
            If sb.Length > 0 Then sb.Append(delimiter)
            sb.Append(i)
        Next
        Return sb.ToString
    End Function

    'Html
    Public Shared Function ListToHtml(ByVal list As IList) As String
        Return ListToHtml(list, "<br/>")
    End Function
    Public Shared Function ListToHtml(ByVal list As IList, ByVal delimiter As String) As String
        Dim sb As New System.Text.StringBuilder
        For Each i As Object In list
            If sb.Length > 0 Then sb.Append(delimiter)
            sb.Append(System.Web.HttpUtility.HtmlEncode(i.ToString()))
        Next
        Return sb.ToString
    End Function
#End Region

End Class
