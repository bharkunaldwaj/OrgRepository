Imports System.IO

<Serializable(), CLSCompliant(True)> _
Public Class CFileNameAndContent
    Public Name As String
    Public Content As Byte()


    Public Sub SaveToFolder(ByVal folderPath As String)
        If Not IO.Directory.Exists(folderPath) Then IO.Directory.CreateDirectory(folderPath)
        Dim filePath As String = folderPath & Name
        Dim bw As New IO.BinaryWriter(IO.File.OpenWrite(filePath))
        bw.Write(Content)
        bw.Close()
    End Sub


    Public Shared Function UniqueName(ByVal name As String) As String
        Dim extension As String = Path.GetExtension(name)
        Dim baseName As String = name.Substring(0, name.LastIndexOf(extension))

        If 0 = baseName.Length Then Return "_" & name 'trivial case

        Dim len As Integer = baseName.Length
        Dim suffix As String = "(" & 1 & ")" & extension
        If ")" <> baseName.Substring(len - 1, 1) Then Return baseName & suffix

        Dim startAt As Integer = baseName.LastIndexOf("(")
        If -1 = startAt Then Return baseName + suffix

        Dim number As String = baseName.Substring(startAt + 1, len - startAt - 2)
        Try
            Dim nextNumber As Integer = Integer.Parse(number) + 1
            Return baseName.Substring(0, startAt) + "(" + nextNumber.ToString() + ")" + extension
        Catch
            Return baseName + suffix
        End Try
    End Function

End Class
