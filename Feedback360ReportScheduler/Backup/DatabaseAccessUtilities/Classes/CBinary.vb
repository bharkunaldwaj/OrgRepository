Imports System.Text
Imports System.Text.ASCIIEncoding
Imports System.IO
Imports System.IO.Compression
Imports System.Collections.Generic
Imports System.Security.Cryptography
Imports System.Configuration.ConfigurationManager
Imports System.Runtime.Serialization.Formatters.Binary

Public Class CBinary

#Region "Shared - Serialization"
    'Object to Stream
    Public Shared Function Deserialise(ByVal data As Stream) As Object
        Dim bf As New BinaryFormatter
        Deserialise = bf.Deserialize(data)
        data.Close()
    End Function
    Public Shared Function Serialise(ByVal data As Object) As Stream
        Dim ms As New IO.MemoryStream
        Dim bf As New BinaryFormatter
        bf.Serialize(ms, data)
        Return ms
    End Function

    'Object to Bytes
    Public Shared Function SerialiseToBytes(ByVal data As Object) As Byte()
        Dim ms As New IO.MemoryStream
        Dim bf As New BinaryFormatter
        bf.Serialize(ms, data)
        SerialiseToBytes = ms.ToArray()
        ms.Close()
    End Function
    Public Shared Function DeserialiseFromBytes(ByVal bytes As Byte()) As Object
        Dim ms As New IO.MemoryStream(bytes)
        Dim bf As New BinaryFormatter
        DeserialiseFromBytes = bf.Deserialize(ms)
        ms.Close()
    End Function

    'Object to File
    Public Shared Sub SerialiseToFile(ByVal filePath As String, ByVal data As Object)
        Dim bf As New BinaryFormatter
        Dim fs As IO.FileStream = IO.File.Create(filePath)
        bf.Serialize(fs, data)
        fs.Close()
    End Sub
    Public Shared Function DeserialiseFromFile(ByVal filePath As String) As Object
        Dim bf As New BinaryFormatter
        Dim fs As IO.FileStream = IO.File.OpenRead(filePath)
        DeserialiseFromFile = bf.Deserialize(fs)
        fs.Close()
    End Function

    'Object to zip
    Public Shared Function SerialiseToBytesAndZip(ByVal data As Object) As Byte()
        Return Zip(SerialiseToBytes(data))
    End Function
    Public Shared Function DeserialiseFromBytesAndUnzip(ByVal bytes As Byte()) As Object
        Return DeserialiseFromBytes(Unzip(bytes))
    End Function

    'Object to file
    Public Shared Sub SerialiseToBytesAndZip(ByVal data As Object, ByVal filePath As String)
        IO.File.WriteAllBytes(filePath, SerialiseToBytesAndZip(data))
    End Sub
    Public Shared Function DeserialiseFromBytesAndUnzip(ByVal filePath As String) As Object
        Return DeserialiseFromBytesAndUnzip(IO.File.ReadAllBytes(filePath))
    End Function

    'Object encrypted
    Public Shared Function SerialiseAndEncrypt(ByVal data As Object) As Byte()
        Return SerialiseAndEncrypt(data, EncryptionProvider)
    End Function
    Public Shared Function DeserialiseAndDecrypt(ByVal bytes As Byte()) As Object
        Return DeserialiseAndDecrypt(bytes, EncryptionProvider)
    End Function
    Public Shared Function SerialiseAndEncrypt(ByVal data As Object, ByVal provider As SymmetricAlgorithm) As Byte()
        Return Encrypt(SerialiseToBytes(data), provider)
    End Function
    Public Shared Function DeserialiseAndDecrypt(ByVal bytes As Byte(), ByVal provider As SymmetricAlgorithm) As Object
        Return DeserialiseFromBytes(Decrypt(bytes, provider))
    End Function
#End Region

#Region "Zip/Unzip"
    Public Shared Function Zip(ByVal input As Byte()) As Byte()
        Dim memStr As New MemoryStream
        Dim zipper As New GZipStream(memStr, CompressionMode.Compress)
        Dim bw As New BinaryWriter(zipper)
        bw.Write(input)
        bw.Close()
        Return memStr.ToArray()
    End Function
    Public Shared Function Unzip(ByVal input As Byte()) As Byte()
        Dim memStr As New MemoryStream(input)
        Dim unzipper As New GZipStream(memStr, CompressionMode.Decompress)
        Dim br As New BinaryReader(unzipper)

        Dim output As New List(Of Byte)(4096)
        Dim buffer As Byte() = Nothing
        While True
            buffer = br.ReadBytes(4096)
            If buffer.Length = 0 Then Exit While
            output.AddRange(buffer)
        End While
        br.Close()
        Return output.ToArray()
    End Function
#End Region

#Region "Bytes/String"
    Public Shared Function BytesToStringVb(ByVal binary As Byte()) As String
        Dim c As Char(), i As Integer, bound As Integer = UBound(binary)
        ReDim c(bound)
        For i = 0 To bound
            c(i) = Chr(binary(i))
        Next
        Return New String(c)
    End Function
    Public Shared Function BytesToString(ByVal binary As Byte()) As String
        Return ASCII.GetString(binary)
    End Function
    Public Shared Function StringToBytes(ByVal s As String) As Byte()
        Return ASCII.GetBytes(s)
    End Function
#End Region

#Region "Bytes/Hex"
    Public Shared Function BytesToHex(ByVal data As Byte()) As String
        Return BitConverter.ToString(data).Replace("-", "")
    End Function
    Public Shared Function HexToBytes(ByVal data As String) As Byte()
        Dim binaryLength As Integer = CInt(data.Length / 2)
        Dim result As New List(Of Byte)(binaryLength)
        For i As Integer = 0 To binaryLength - 1
            Try
                result.Add(Byte.Parse(data.Substring(i * 2, 2), Globalization.NumberStyles.HexNumber))
            Catch ex As Exception
                result.Add(Byte.Parse(data.Substring(i * 2, 1), Globalization.NumberStyles.HexNumber))
            End Try
        Next
        Return result.ToArray()
    End Function
#End Region

#Region "Bytes/Stream"
    Public Shared Function BytesToStream(ByVal b As Byte()) As IO.Stream
        Return New IO.MemoryStream(b)
    End Function

    Const BUFFER_SIZE As Integer = 4096
    Public Shared Function StreamToBytes(ByVal s As IO.Stream) As Byte()
        If s.CanSeek Then
            With New IO.BinaryReader(s)
                StreamToBytes = .ReadBytes(CInt(s.Length))
                .Close()
                Exit Function
            End With
        End If

        Dim output As New List(Of Byte)(BUFFER_SIZE)
        Dim buffer As Byte() = Nothing
        Dim br As New BinaryReader(s)
        While True
            buffer = br.ReadBytes(BUFFER_SIZE)
            If buffer.Length = 0 Then Exit While
            output.AddRange(buffer)
        End While
        br.Close()
        Return output.ToArray()
    End Function

    Public Shared Sub StreamToFile(ByVal s As Stream, ByVal filePath As String)
        Dim buffer(BUFFER_SIZE) As Byte
        Dim read As Integer
        With New IO.FileStream(filePath, FileMode.Create)
            While True
                read = s.Read(buffer, 0, buffer.Length)
                If read > 0 Then .Write(buffer, 0, read)
                If read < BUFFER_SIZE Then
                    .Close()
                    Exit Sub
                End If
            End While
        End With
    End Sub
#End Region

#Region "Encryption (Symetric)"
    'Simple Overloads (uses the default triple des provider, based on std config settings)
    Public Shared Function Encrypt(ByVal data As String) As Byte()
        Return Encrypt(data, EncryptionProvider)
    End Function
    Public Shared Function DecryptAsStr(ByVal data As Byte()) As String
        If IsNothing(data) Then Return String.Empty
        Return DecryptAsStr(data, EncryptionProvider)
    End Function
    Public Shared Function Encrypt(ByVal data As Byte()) As Byte()
        If IsNothing(data) Then Return Nothing
        Return Encrypt(data, EncryptionProvider)
    End Function
    Public Shared Function Decrypt(ByVal data As Byte()) As Byte()
        If IsNothing(data) Then Return Nothing
        Return Decrypt(data, EncryptionProvider)
    End Function
    Public Shared Function Encrypt(ByVal data As Stream) As Stream
        Return Encrypt(data, EncryptionProvider)
    End Function
    Public Shared Function Decrypt(ByVal data As Stream) As Stream
        Return Decrypt(data, EncryptionProvider)
    End Function

    'String Interface
    Public Shared Function Encrypt(ByVal data As String, ByVal provider As SymmetricAlgorithm) As Byte()
        Return Encrypt(ASCII.GetBytes(data), provider)
    End Function
    Public Shared Function DecryptAsStr(ByVal data As Byte(), ByVal provider As SymmetricAlgorithm) As String
        Return ASCII.GetString(Decrypt(data, provider))
    End Function

    'Byte Interface
    Public Shared Function Encrypt(ByVal data As Byte(), ByVal provider As SymmetricAlgorithm) As Byte()
        Return StreamToBytes(Encrypt(BytesToStream(data), provider))
    End Function
    Public Shared Function Decrypt(ByVal data As Byte(), ByVal provider As SymmetricAlgorithm) As Byte()
        Return StreamToBytes(Decrypt(BytesToStream(data), provider))
    End Function

    'Stream Interface
    Public Shared Function Encrypt(ByVal data As Stream, ByVal provider As SymmetricAlgorithm) As Stream
        Return New CryptoStream(data, provider.CreateEncryptor(), CryptoStreamMode.Read)
    End Function
    Public Shared Function Decrypt(ByVal data As Stream, ByVal provider As SymmetricAlgorithm) As Stream
        Return New CryptoStream(data, provider.CreateDecryptor(), CryptoStreamMode.Read)
    End Function

    'Default Encryption Provider (TripleDes)
    Private Shared _provider As TripleDESCryptoServiceProvider
    Private Shared ReadOnly Property EncryptionProvider() As TripleDESCryptoServiceProvider
        Get
            If IsNothing(_provider) Then
                _provider = New TripleDESCryptoServiceProvider()
                With _provider
                    .IV = HexToBytes(CConfigBase.TripleDesIV)
                    .Key = HexToBytes(CConfigBase.TripleDesKey)
                End With
            End If
            Return _provider
        End Get
    End Property
#End Region

#Region "Encryption (Rijndal)"
    'Default Key (based on CConfigBase.FastEncryptionKey)
    Private Shared _rijndael As RijndaelManaged = Nothing
    Public Shared ReadOnly Property Rijndael() As SymmetricAlgorithm
        Get
            If IsNothing(_rijndael) Then
                Dim rij As New RijndaelManaged()
                Dim key As Byte() = LongerKey(FastEncryptionKey, rij.LegalKeySizes(0))
                rij.Key = key
                rij.IV = LongerKey(FastEncryptionKey, CInt(rij.BlockSize / 8), CInt(rij.BlockSize / 8))
                _rijndael = rij
            End If
            Return _rijndael
        End Get
    End Property

    'Rijndael - Encrypt
    Public Shared Function EncryptRijndael(ByVal data As String) As Byte()
        Return Encrypt(data, Rijndael)
    End Function
    Public Shared Function EncryptRijndael(ByVal data As Byte()) As Byte()
        Return Encrypt(data, Rijndael)
    End Function
    Public Shared Function EncryptRijndaelToBase64(ByVal data As String) As String
        Return ToBase64(Encrypt(data, Rijndael))
    End Function
    Public Shared Function EncryptRijndaelToBase64(ByVal data As Byte()) As String
        Return ToBase64(Encrypt(data, Rijndael))
    End Function

    'Rijndael - Decrypt
    Public Shared Function DecryptRijndael(ByVal data As Byte()) As Byte()
        Return Decrypt(data, Rijndael)
    End Function
    Public Shared Function DecryptRijndaelAsStr(ByVal data As Byte()) As String
        Return DecryptAsStr(data, Rijndael)
    End Function
    Public Shared Function DecryptRijndael(ByVal base64 As String) As Byte()
        Return Decrypt(FromBase64(base64), Rijndael)
    End Function
    Public Shared Function DecryptRijndaelAsStr(ByVal base64 As String) As String
        Return DecryptAsStr(FromBase64(base64), Rijndael)
    End Function

    'Rijndael - Control key Length (derived from a password)
    Public Shared Function LongerKey(ByVal key As String, ByVal minMax As KeySizes) As Byte()
        Return LongerKey(StringToBytes(key), minMax)
    End Function
    Public Shared Function LongerKey(ByVal key As Byte(), ByVal minMax As KeySizes) As Byte()
        Return LongerKey(key, CInt(minMax.MinSize / 8), CInt(minMax.MaxSize / 8))
    End Function
    Public Shared Function LongerKey(ByVal key As Byte(), ByVal min As Integer, ByVal max As Integer) As Byte()
        If key.Length = 0 Then key = New Byte() {123}
        If key.Length >= min AndAlso key.Length <= max Then Return key
        Dim longer As New List(Of Byte)(max)
        For i As Integer = 0 To max - 1
            longer.Add(key(i Mod key.Length))
        Next
        Return longer.ToArray()
    End Function

    Public Shared Function FromBase64(ByVal base64 As String) As Byte()
        Return System.Convert.FromBase64String(base64)
    End Function
    Public Shared Function ToBase64(ByVal binary As Byte()) As String
        Return System.Convert.ToBase64String(binary)
    End Function
#End Region

#Region "Encryption (Fast)"
    'Zip combos (used for winforms/webservices comms)
    Public Shared Function Pack(ByVal obj As Object) As Byte()
        Return Pack(obj, CConfigBase.WebServicePassword)
    End Function
    Public Shared Function Unpack(ByVal data As Byte()) As Object
        Return Unpack(data, CConfigBase.WebServicePassword)
    End Function
    Public Shared Function Pack(ByVal obj As Object, ByVal password As String) As Byte()
        If IsNothing(obj) Then Return Nothing
        Dim data As Byte() = CBinary.SerialiseToBytesAndZip(obj)
        If Len(password) > 0 Then data = CBinary.EncryptFast(data, password)
        Return data
    End Function
    Public Shared Function Unpack(ByVal data As Byte(), ByVal password As String) As Object
        If IsNothing(data) Then Return Nothing
        If Len(password) > 0 Then data = CBinary.EncryptFast(data, password)
        Try
            Return CBinary.DeserialiseFromBytesAndUnzip(data)
        Catch ex As Exception
            Throw New Exception("Failed to unpack data - check password", ex)
        End Try
    End Function


    'Default Key
    Private Const ENCRYPTED_MARKER As String = "0x"
    Private Shared _encryptionKey As Byte() = Nothing
    Private Shared ReadOnly Property FastEncryptionKey() As Byte()
        Get
            If IsNothing(_encryptionKey) Then
                Dim s As String = CConfigBase.FastEncryptionKey
                If Len(s) > 0 Then
                    If IsHexOnly(s) Then
                        _encryptionKey = HexToBytes(s)
                    Else
                        _encryptionKey = ASCII.GetBytes(s)
                    End If
                Else
                    _encryptionKey = New Byte() {234, 26, 58, 19, 200, 206, 94, 201, 238, 15, 1, 117}
                End If
            End If
            Return _encryptionKey
        End Get
    End Property
    Private Shared Function IsHexOnly(ByVal s As String) As Boolean
        If IsNothing(s) Then Return True
        Dim hex As New List(Of Char)(CStr("0123456789abcdef").ToCharArray())
        For Each i As Char In s.ToLower().ToCharArray()
            If Not hex.Contains(i) Then Return False
        Next
        Return True
    End Function

    'Binary Versions (has no '0x' marker to distinguish encrypted vs decrypted, and decrypt function is also encrypt function)
    Public Shared Function EncryptFast(ByVal b As Byte(), ByVal key As String) As Byte()
        Return EncryptFast(b, StringToBytes(key))
    End Function
    Public Shared Function EncryptFast(ByVal b As Byte(), ByVal key As Byte()) As Byte()
        If key.Length > 0 Then
            For i As Integer = 0 To b.Length - 1
                b(i) = b(i) Xor key(i Mod key.Length)
            Next
        End If
        Return b
    End Function


    'String versions (encrypts string to a hex string with a marker)
    Public Shared Function EncryptFast(ByVal s As String) As String
        If IsEncrypted(s) Then Return s

        Dim chars As Char() = s.ToCharArray
        Dim encrypted As New StringBuilder(ENCRYPTED_MARKER)
        Dim i As Integer
        For i = 0 To chars.Length - 1
            encrypted.Append(Mush(i, chars))
        Next
        Return encrypted.ToString
    End Function
    Public Shared Function DecryptFast(ByVal s As String) As String
        If Not IsEncrypted(s) Then Return s

        Dim size As Integer = CInt(s.Length / 2)
        Dim al As New List(Of Integer)(size)
        For i As Integer = 1 To size - 1
            al.Add(Hex2Int(s.Substring(i * 2, 2)))
        Next

        Dim hex As Integer() = al.ToArray()
        Dim decrypted As New StringBuilder
        For i As Integer = 0 To hex.Length - 1
            decrypted.Append(Mush(i, hex))
        Next
        Return decrypted.ToString
    End Function
    Public Shared Function IsEncrypted(ByVal s As String) As Boolean
        If Len(s) > 1 Then Return s.Substring(0, 2) = ENCRYPTED_MARKER
        Return False
    End Function


    'String version overloads (external key)
    Public Shared Function EncryptFast(ByVal s As String, ByVal key As Byte()) As String
        SyncLock (FastEncryptionKey)
            _encryptionKey = key
            EncryptFast = EncryptFast(s)
            _encryptionKey = Nothing
        End SyncLock
    End Function
    Public Shared Function DecryptFast(ByVal s As String, ByVal key As Byte()) As String
        _encryptionKey = key
        DecryptFast = DecryptFast(s)
        _encryptionKey = Nothing
    End Function


    'Utilities
    Private Shared Function KeyByteAt(ByVal i As Integer) As Byte
        Return FastEncryptionKey(i Mod FastEncryptionKey.Length)
    End Function
    Private Shared Function Mush(ByVal i As Integer, ByVal input As Byte) As Byte
        Return input Xor KeyByteAt(i)
    End Function
    Private Shared Function Mush(ByVal i As Integer, ByVal input As Char()) As String
        Return Int2Hex(CByte(AscW(input(i))) Xor KeyByteAt(i))
    End Function
    Private Shared Function Mush(ByVal i As Integer, ByVal input As Integer()) As String
        Return ChrW(input(i) Xor KeyByteAt(i))
    End Function
    Private Shared Function Int2Hex(ByVal i As Integer) As String
        Int2Hex = Hex(i)
        If Len(Int2Hex) = 1 Then Return "0" & Int2Hex
    End Function
    Private Shared Function Hex2Int(ByVal s As String) As Integer
        If Len(s) = 2 Then Return 16 * Hex2Int(s.Chars(0)) + Hex2Int(s.Chars(1))
        Return 0
    End Function
    Private Shared Function Hex2Int(ByVal c As Char) As Integer
        c = Char.ToLower(c)
        If "a" = c Then Return 10
        If "b" = c Then Return 11
        If "c" = c Then Return 12
        If "d" = c Then Return 13
        If "e" = c Then Return 14
        If "f" = c Then Return 15
        Return Integer.Parse(c)
    End Function
#End Region

#Region "Hashing"
    Public Shared Function Sha1(ByVal plainText As String) As String
        Return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(plainText, "SHA1")
    End Function
    Public Shared Function MD5(ByVal plainText As String) As String
        Return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(plainText, "MD5")

        'Dim encoder As New UTF8Encoding()
        'Dim plainBinary As Byte() = encoder.GetBytes(plainText)

        'Dim md5Hasher As New MD5CryptoServiceProvider()
        'Dim md5Binary As Byte() = md5Hasher.ComputeHash(plainBinary)

        'Return encoder.GetString(md5Binary)
    End Function
#End Region

End Class