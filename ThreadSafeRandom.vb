Public Class ThreadSafeRandom
    Private Shared _global As New Random()
    <ThreadStatic> Private Shared _local As Random

    Public Function NewNext(ByVal min As Integer, max As Integer) As Integer
        Dim inst As Random = _local
        If inst Is Nothing Then
            Dim seed As Integer
            SyncLock _global
                seed = _global.Next()
            End SyncLock
            inst = New Random(seed)
            _local = inst
        End If
        Return inst.Next(min, max)
    End Function

    Public Function NewNext(max As Integer) As Integer
        Dim inst As Random = _local
        If inst Is Nothing Then
            Dim seed As Integer
            SyncLock _global
                seed = _global.Next()
            End SyncLock
            inst = New Random(seed)
            _local = inst
        End If
        Return inst.Next(max)
    End Function

    Public Function NextDouble() As Double
        Dim inst As Random = _local
        If inst Is Nothing Then
            Dim seed As Integer
            SyncLock _global
                seed = _global.Next()
            End SyncLock
            inst = New Random(seed)
            _local = inst
        End If
        Return inst.NextDouble()
    End Function

End Class
