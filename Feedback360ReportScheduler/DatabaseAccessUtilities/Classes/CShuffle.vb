Imports System.Collections.Generic

Public Class CShuffle : Implements IComparable
    Public Sub New(ByVal obj As Object)
        _object = obj
    End Sub

    Private _object As Object
    Private _number As Integer

    Public ReadOnly Property [Object]() As Object
        Get
            Return _object
        End Get
    End Property
    Public Sub Reset(ByVal r As Random)
        _number = r.Next()
    End Sub


    Public Shared Function Shuffle(ByVal objects As IList) As IList
        Dim cards As New List(Of CShuffle)(objects.Count)
        For Each i As Object In objects
            cards.Add(New CShuffle(i))
        Next

        Return Shuffle(cards)
    End Function
    Public Shared Function Shuffle(ByVal cards As List(Of CShuffle)) As IList
        Dim r As New Random
        For Each i As CShuffle In cards
            i.Reset(r)
        Next
        cards.Sort()

        Dim objects As New List(Of Object)(cards.Count)
        For Each i As CShuffle In cards
            objects.Add(i.Object)
        Next
        Return objects
    End Function

    Public Overridable Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
        Return _number.CompareTo(CType(obj, CShuffle)._number)
    End Function
End Class

