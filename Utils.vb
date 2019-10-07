Imports Keras
Imports Numpy
Module Utils

#Region "File Loading/Saving Functions"

    Public Sub fastWriteBinaryData(ByVal filename As String, ByRef data() As Single)
        Dim bb(data.Length * 4 - 1) As Byte
        Buffer.BlockCopy(data, 0, bb, 0, data.Length * 4)
        System.IO.File.WriteAllBytes(filename, bb)
        bb = Nothing
    End Sub

    Public Sub fastReadBinaryData(ByVal filename As String, ByRef data() As Single, ByVal size As Integer)
        If data Is Nothing Then data = New Single(size - 1) {} Else ReDim data(size - 1)
        Dim bb() As Byte = System.IO.File.ReadAllBytes(filename)
        Buffer.BlockCopy(bb, 0, data, 0, size * 4)
        bb = Nothing
    End Sub

    Public Sub fastWriteBinaryData(ByVal filename As String, ByRef data()() As Single)
        Dim x As Long = data.GetUpperBound(0) + 1
        Dim y As Long = data(0).Length

        If x * y * 4 < 600000000 Then
            Dim bb(x * y * 4 - 1) As Byte
            For i As Long = 0 To x - 1
                Buffer.BlockCopy(data(i), 0, bb, i * y * 4, y * 4)
            Next
            System.IO.File.WriteAllBytes(filename, bb)
        Else
            Dim f As New System.IO.FileStream(filename, System.IO.FileMode.Create)
            Dim bw As New System.IO.BinaryWriter(f)
            Dim bbs(y * 4 - 1) As Byte
            For i As Long = 0 To x - 1
                Dim td() As Single = data(i).Clone()
                Buffer.BlockCopy(td, 0, bbs, 0, y * 4)
                bw.Write(bbs, 0, y * 4)
            Next
            bw.Close()
            f.Close()
        End If
    End Sub

    Public Sub fastReadBinaryData(ByVal filename As String, ByRef data()() As Single, ByVal x As Long, ByVal y As Long)

        ReDim data(x - 1)(y - 1)

        If x * y * 4 < 600000000 Then
            Dim bb() As Byte = System.IO.File.ReadAllBytes(filename)
            For i As Long = 0 To x - 1
                Dim ta(y - 1) As Single
                Buffer.BlockCopy(bb, i * y * 4, ta, 0, y * 4)
                data(i) = ta
            Next
        Else
            Dim f As New System.IO.FileStream(filename, System.IO.FileMode.Open)
            Dim br As New System.IO.BinaryReader(f)
            Dim bb() As Byte
            For i As Long = 0 To x - 1
                bb = br.ReadBytes(y * 4)
                Dim ta(y - 1) As Single
                Buffer.BlockCopy(bb, 0, ta, 0, y * 4)
                data(i) = ta.Clone()
            Next
            br.Close()
            f.Close()
        End If

    End Sub

    Public Sub loadBinaryData(ByVal filename As String, ByRef data As Array)
        'Try
        Dim binfmt As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim s As System.IO.Stream = System.IO.File.Open(filename, IO.FileMode.Open)
        data = binfmt.Deserialize(s)
        s.Close()
        s.Dispose()
        binfmt = Nothing
        s = Nothing
        'Catch ex As Exception
        '    nn = Nothing
        'End Try
    End Sub

    Public Sub loadBinaryData(ByVal filename As String, ByRef data As List(Of (Double(), Double())))
        'Try
        Dim binfmt As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim s As System.IO.Stream = System.IO.File.Open(filename, IO.FileMode.Open)
        data = binfmt.Deserialize(s)
        s.Close()
        s.Dispose()
        binfmt = Nothing
        s = Nothing
        'Catch ex As Exception
        '    nn = Nothing
        'End Try
    End Sub


    Public Sub loadBinaryData(ByVal filename As String, ByRef data As List(Of Double()))
        'Try
        Dim binfmt As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim s As System.IO.Stream = System.IO.File.Open(filename, IO.FileMode.Open)
        data = binfmt.Deserialize(s)
        s.Close()
        s.Dispose()
        binfmt = Nothing
        s = Nothing
        'Catch ex As Exception
        '    nn = Nothing
        'End Try
    End Sub

    Public Sub saveBinaryData(ByVal filename As String, ByRef data As Object)
        Dim binfmt As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim s As System.IO.Stream = System.IO.File.Open(filename, IO.FileMode.Create)
        binfmt.Serialize(s, data)
        s.Close()
        s.Dispose()
        binfmt = Nothing
        s = Nothing
    End Sub

    Public Function readFromMemoryMap(ByVal map_name As String, ByVal index As Long, ByVal y As Long, Optional ByVal get_or_kill As Boolean = True) As Single()
        Try
            Dim fr As System.IO.MemoryMappedFiles.MemoryMappedFile = System.IO.MemoryMappedFiles.MemoryMappedFile.OpenExisting(map_name)
            Dim s(y - 1) As Single
            Using r As System.IO.MemoryMappedFiles.MemoryMappedViewAccessor =
            fr.CreateViewAccessor(y * 4 * index, y * 4, System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Read)
                r.ReadArray(Of Single)(0, s, 0, y)
                r.Dispose() ' New as of 9/13/19 (may cause or fix problems)
            End Using
            fr.Dispose() ' New as of 9/13/19 (may cause or fix problems)
            Return s
        Catch ex As Exception
            ' The memory map probably disappeared.  Why(?)
            If get_or_kill Then
                Dim p As Process = Process.GetCurrentProcess()
                p.CloseMainWindow()
                p.Kill()
            End If
        End Try
        Return Nothing
    End Function

#End Region

    Public Sub shuffleArray(Of T)(ByRef items As T(), Optional ByRef r As ThreadSafeRandom = Nothing)
        If r Is Nothing Then r = New ThreadSafeRandom()
        Dim temp As T
        Dim j As Long
        For i As Long = items.Count - 1 To 0 Step -1
            j = r.NewNext(i + 1)
            temp = items(i)
            items(i) = items(j)
            items(j) = temp
        Next
    End Sub

    Public Sub shuffleList(ByRef items As List(Of (Double(), Double())), Optional ByRef r As ThreadSafeRandom = Nothing)
        If r Is Nothing Then r = New ThreadSafeRandom()
        Dim temp As (Double(), Double())
        Dim j As Long
        For i As Long = items.Count - 1 To 0 Step -1
            j = r.NewNext(i + 1)
            temp = items(i)
            items(i) = items(j)
            items(j) = temp
        Next
    End Sub

    ' Used when playing with Keras(.NET)/Tensorflow.  
    Public Function loadOrBuildModel(ByVal fn As String, ByVal model_num As Integer, ByVal load_or_nothing As Boolean) As Keras.Models.Sequential
        Const dropout As Double = 0.5

        Dim nm As New Keras.Models.Sequential

        Dim r As ThreadSafeRandom = GameTreeNode.rand

        Select Case model_num
            Case 1
                ' =================== Model #1 =================== 
                nm.Add(New Keras.Layers.Dense(512, activation:="tanh"))
                nm.Add(New Keras.Layers.Dense(2048, activation:="sigmoid"))
                nm.Add(New Keras.Layers.Dropout(0.15))
                nm.Add(New Keras.Layers.Dense(2048, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(2048, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(2048, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout)) ' New
                nm.Add(New Keras.Layers.Dense(1, activation:="softmax"))
                nm.Compile(optimizer:="nadam", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                ' ================================================
                Exit Select
            Case 2
                ' =================== Model #2 =================== 
                ' cepheus_model(0.0706320257756384, 0.825256400335874).h5; training size: 250000; test_size: 0.2
                ' cepheus_model(0.0797464520889433, 0.831480466567256).h5; training size: 250000; test_size: 0.2
                ' cepheus_model(0.0696515568326796, 0.795219622805916).h5; training size: 500000; test_size: 0.5
                ' First two layers act as a "conversion" for the negative inputs,
                ' as relu output drops off at < 0.
                nm.Add(New Keras.Layers.Dense(512, activation:="tanh"))
                nm.Add(New Keras.Layers.Dense(512, activation:="sigmoid"))
                nm.Add(New Keras.Layers.Dropout(0.15))
                nm.Add(New Keras.Layers.Dense(512, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(512, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(512, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(512, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(512, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(512, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(512, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(512, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(1, activation:="softmax"))
                If r.NextDouble() < 0.5 Then
                    nm.Compile(optimizer:="nadam", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                Else
                    nm.Compile(optimizer:="adadelta", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                End If
                Exit Select
            Case 3
                ' =================== Model #3 =================== 
                ' First two layers act as a "conversion" for the negative inputs,
                ' as relu output drops off at < 0.
                nm.Add(New Keras.Layers.Dense(512, activation:="tanh"))
                nm.Add(New Keras.Layers.Dense(512, activation:="sigmoid"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(256, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(256, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(256, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(128, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(128, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(128, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(64, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(64, activation:="sigmoid"))
                nm.Add(New Keras.Layers.Dense(1, activation:="tanh"))
                nm.Compile(optimizer:="nadam", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                'nm.Compile(optimizer:="adadelta", loss:="mse", metrics:=New String() {"mse", "accuracy"})

                'If r.NextDouble() < 0.5 Then
                '    m.Compile(optimizer:="nadam", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                'Else
                '    m.Compile(optimizer:="adadelta", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                'End If
                Exit Select
            Case 4
                ' =================== Model #4 =================== 
                ' First two layers act as a "conversion" for the negative inputs,
                ' as relu output drops off at < 0.
                nm.Add(New Keras.Layers.Dense(512, activation:="tanh"))
                nm.Add(New Keras.Layers.Dense(512, activation:="sigmoid"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(256, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(256, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(256, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(128, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(128, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(128, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(64, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(64, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(1, activation:="relu"))
                nm.Compile(optimizer:="nadam", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                'nm.Compile(optimizer:="adadelta", loss:="mse", metrics:=New String() {"mse", "accuracy"})

                'If r.NextDouble() < 0.5 Then
                '    m.Compile(optimizer:="nadam", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                'Else
                '    m.Compile(optimizer:="adadelta", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                'End If
                Exit Select
            Case 5
                ' =================== Model #5 =================== 
                ' First two layers act as a "conversion" for the negative inputs,
                ' as relu output drops off at < 0.
                nm.Add(New Keras.Layers.Dense(512, activation:="tanh"))
                nm.Add(New Keras.Layers.Dense(512, activation:="sigmoid"))
                nm.Add(New Keras.Layers.Dense(256, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(256, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(256, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(128, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(128, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(128, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(64, activation:="linear"))
                nm.Add(New Keras.Layers.Dense(1, activation:="linear"))
                'nm.Compile(optimizer:="nadam", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                nm.Compile(optimizer:="adadelta", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                'nm.Compile(optimizer:="adagrad", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                Exit Select
            Case 6
                ' =================== Model #6 =================== 
                nm.Add(New Keras.Layers.Dense(512))
                nm.Add(New Keras.Layers.PReLU())
                nm.Add(New Keras.Layers.Dense(512))
                nm.Add(New Keras.Layers.PReLU())
                nm.Add(New Keras.Layers.Dense(256, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(256, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(256, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(128, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(128, activation:="relu"))
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(128, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(64, activation:="relu"))
                nm.Add(New Keras.Layers.Dense(1, activation:="relu"))
                'nm.Compile(optimizer:="nadam", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                nm.Compile(optimizer:="adadelta", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                'nm.Compile(optimizer:="adagrad", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                Exit Select
            Case 7
                ' =================== Model #7 =================== 
                nm.Add(New Keras.Layers.Dense(512))
                nm.Add(New Keras.Layers.PReLU())
                nm.Add(New Keras.Layers.Dropout(0.1))
                nm.Add(New Keras.Layers.Dense(512))
                nm.Add(New Keras.Layers.PReLU())
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(256))
                nm.Add(New Keras.Layers.PReLU())
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(256))
                nm.Add(New Keras.Layers.PReLU())
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(256))
                nm.Add(New Keras.Layers.PReLU())
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(128))
                nm.Add(New Keras.Layers.PReLU())
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(128))
                nm.Add(New Keras.Layers.PReLU())
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(128))
                nm.Add(New Keras.Layers.PReLU())
                nm.Add(New Keras.Layers.Dropout(dropout))
                nm.Add(New Keras.Layers.Dense(64))
                nm.Add(New Keras.Layers.PReLU())
                nm.Add(New Keras.Layers.Dense(1))
                nm.Add(New Keras.Layers.PReLU())
                nm.Compile(optimizer:="nadam", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                'nm.Compile(optimizer:="adadelta", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                'nm.Compile(optimizer:="adagrad", loss:="mse", metrics:=New String() {"mse", "accuracy"})
                Exit Select
        End Select

        ' Needed to instantiate the model so weights can be loaded.
        Dim temp_ins(0, 321 + 103 + 43 + 3) As Double, temp_outs(0, 0) As Double
        nm.TrainOnBatch(np.array(temp_ins), np.array(temp_outs))

        If fn <> "" AndAlso System.IO.File.Exists(fn) Then
            Console.WriteLine("Loading weights: " & fn)
            Try
                nm.LoadWeight(fn)
            Catch ex As Exception
                Console.WriteLine("Couldn't load weights: " & ex.Message)
                If load_or_nothing Then Return Nothing
            End Try
            Dim o As NDarray = nm.Predict(temp_ins)
            Console.WriteLine(CDbl(o(0, 0)))
        End If

        Return nm

    End Function

    Public Function getHighestWeightsFile(ByVal search_params As String, Optional ByVal num As Integer = 1) As String()
        Dim f() As String = System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory, search_params & "*.bin")

        If f.Length = 0 Then Return New String() {""}

        Dim highest(f.Length - 1) As Integer
        For i As Integer = 0 To f.Length - 1
            highest(i) = CInt(f(i).Substring(f(i).LastIndexOf("\") + 1).Replace(search_params, "").Replace(".bin", ""))
        Next

        Array.Sort(highest, f)

        Dim highs(Math.Min(Math.Max(num, 1), f.Length) - 1) As String

        For i As Integer = 0 To highs.Length - 1
            highs(i) = f(f.Length - 1 - i)
        Next

        Return highs
    End Function

    Public Function totalFileSizes(ByVal search_pattern As String) As Long
        Dim f() As String = System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory, search_pattern)
        Dim fs As Long = 0
        If f.Count > 0 Then
            For q As Integer = 0 To f.Length - 1 : fs += My.Computer.FileSystem.GetFileInfo(f(q)).Length : Next
        End If
        Return fs
    End Function

    Public Function totalFileSizes(ByVal f() As String) As Long
        Dim fs As Long = 0
        If f.Count > 0 Then
            For q As Integer = 0 To f.Length - 1
                If System.IO.File.Exists(f(q)) Then fs += My.Computer.FileSystem.GetFileInfo(f(q)).Length
            Next
        End If
        Return fs
    End Function

End Module
