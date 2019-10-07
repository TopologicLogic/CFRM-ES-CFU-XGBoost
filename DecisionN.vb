
Class DecisionN : Inherits GameTreeNode

    Public tround As Integer = 0
    Private player As Integer
    Private pot As Single

    Public m_children() As GameTreeNode
    Public m_children_values() As Double ' As percentage of stack (in BB)
    Public m_node_string As String = ""

    Private Shared prefterm() As String = {"cc", "rc", "crc", "crrc", "rrc", "crrrc", "rrrc", "crrrrc", "rrrrc"}
    Private Shared posfterm() As String = {"cc", "crc", "rc", "crrc", "rrc", "crrrc", "rrrc", "crrrrc", "rrrrc"}

    Private Shared posf() As String = {"", "c", "cr", "r", "crr", "rr", "crrr", "rrr", "crrrr", "rrrr"}
    Private Shared posf_dl() As Boolean = {False, True, False, True, True, False, False, True, True, False}
    Private Shared posf_br() As Integer = {0, 0, 1, 1, 2, 2, 3, 3, 4, 4}

    Private bet_histories(8 * 3 + 9) As Double ' size: 34

#Region "Instantiation Functions"

    Public Sub New(ByVal player As Integer, ByVal dround As Integer, ByVal bb As Double, ByVal stacks() As Single,
                   ByVal in_pot() As Single, ByVal hh As String, ByVal num_bets As Integer)
        Me.player = player
        Me.tround = dround
        Me.m_node_string = hh
        Me._is_decision = True
        Me.pot = in_pot(0) + in_pot(1)

        'Console.WriteLine(hh & vbTab &
        '                  in_pot(player) & vbTab & in_pot(player Xor 1) & " | " &
        '                  stacks(player) & vbTab & stacks(player Xor 1))

        If in_pot(player) + in_pot(player Xor 1) > 3000 Then _
            MsgBox("Pot problem: " & in_pot(player) & ", " & in_pot(player Xor 1))

        Dim hs() As String = hh.Split("_")

        For i As Integer = 0 To bet_histories.Length - 1
            bet_histories(i) = -1
        Next


        For j As Integer = 0 To hs.Length - 1
            Dim bi As Integer = -1

            If j = 0 And tround = 0 Then
                bi = Array.IndexOf(posf, hs(j))
            ElseIf j = 0 Then
                bi = Array.IndexOf(prefterm, hs(j))
            ElseIf j = hs.Length - 1 Then
                bi = Array.IndexOf(posf, hs(j))
            Else
                bi = Array.IndexOf(posfterm, hs(j))
            End If

            If bi = -1 Then
                MsgBox(hh)
                Throw New Exception("Invalid history: " & hh)
            End If

            bet_histories(j * 8 + bi) = 1
        Next

        If num_bets <= 0 Then
            ReDim m_children(2)
            ReDim m_children_values(0)
            createSubtrees(player, dround, bb, stacks, in_pot, hh, num_bets, False) ' HUL
        Else
            ReDim m_children(2 + num_bets) ' Fold, Call, num_bets, All-in
            ReDim m_children_values(num_bets)
            createSubtreesNL(player, dround, bb, stacks, in_pot, hh, num_bets, False) ' HUNL
        End If


    End Sub

    Public Sub createSubtrees(ByVal player As Integer, ByVal dround As Integer, ByVal bb As Double, ByVal stacks() As Single,
                              ByVal in_pot() As Single, ByVal hh As String, ByVal num_bets As Integer, Optional ByVal verbose As Boolean = False)

        Dim sb As Double = bb / 2

        Dim hs() As String = hh.Split("_")

        If verbose Then Console.WriteLine(hh)

        If hs(dround) = "" Then

            If dround = 0 Then
                ' Start of a new game
                m_children(0) = New Fold(player, -in_pot(player), IIf(verbose, hh & "f", ""))
                ' Call from the SB
                m_children(1) = New DecisionN(player Xor 1, dround, bb, New Single() {stacks(0) - sb, stacks(1)},
                                             New Single() {in_pot(0) + sb, in_pot(1)}, hh & "c", num_bets)
            Else
                ' Can't fold, only check or raise
                m_children(0) = New Null()
                ' Check
                m_children(1) = New DecisionN(player Xor 1, dround, bb, stacks, in_pot, hh & "c", num_bets)
            End If

            ' Or raise...

        ElseIf hs(dround) = "c" Then
            ' Can't fold
            m_children(0) = New Null()

            If dround = 3 Then
                ' Check, showdown
                m_children(1) = New Showdown(in_pot(player), IIf(verbose, hh & "c", ""))
            Else
                ' Check, next round
                m_children(1) = New DecisionN(1, dround + 1, bb, stacks, in_pot, hh & "c_", num_bets)
            End If

            ' Or raise...
        Else

            ' Op raised.

            m_children(0) = New Fold(player, -in_pot(player), IIf(verbose, hh & "f", ""))

            If dround = 3 Then
                ' Call and go to showdown 
                m_children(1) = New Showdown(in_pot(player Xor 1), IIf(verbose, hh & "c", ""))
            Else
                ' Call and go to the next round
                Dim ts() As Single = stacks.Clone(), tp() As Single = in_pot.Clone()
                ts(player) = ts(player Xor 1)
                tp(player) = tp(player Xor 1)
                m_children(1) = New DecisionN(1, dround + 1, bb, ts, tp, hh & "c_", num_bets)
            End If

            ' Or, re-raise... 
        End If

        If hs(dround) = "rrrr" Or hs(dround) = "crrrr" Then

            ' End of round, can't raise anymore
            For i As Integer = 2 To m_children.Length - 1
                m_children(i) = New Null()
            Next

        Else ' Raise/Re-Raise

            Dim v As Double = in_pot(player Xor 1) - in_pot(player) ' Amount to see
            If dround = 0 Then v += sb Else v += bb
            Dim ts() As Single = stacks.Clone(), tp() As Single = in_pot.Clone()
            ts(player) -= v
            tp(player) += v
            m_children(2) = New DecisionN(player Xor 1, dround, bb, ts, tp, hh & "r", num_bets)
            m_children_values(0) = v / stacks(player)

            'GameTreeNode.raise_vals.Add(m_node_string, m_children_values(0))

        End If


    End Sub

    Public Sub createSubtreesNL(ByVal player As Integer, ByVal dround As Integer, ByVal bb As Double, ByVal stacks() As Single,
                                ByVal in_pot() As Single, ByVal hh As String, ByVal num_bets As Integer, ByVal verbose As Boolean)

        Dim sb As Double = bb / 2

        Dim hs() As String = hh.Split("_")

        If verbose AndAlso rand.NextDouble() < 0.001 Then Console.WriteLine(hh)

        If stacks(player Xor 1) <= 0 Then

            ' Op is all-in, can't bet anymore
            m_children(0) = New Fold(player, -in_pot(player))

            ' Call and go to showdown
            m_children(1) = New Showdown(in_pot(player Xor 1))

            For i As Integer = 2 To m_children.Length - 1
                m_children(i) = New Null()
            Next

        Else

            If hs(dround) = "" Then

                If dround = 0 Then
                    ' Start of a new game
                    m_children(0) = New Fold(player, -in_pot(player))
                    ' Call from the SB
                    m_children(1) = New DecisionN(player Xor 1, dround, bb, New Single() {stacks(0) - sb, stacks(1)},
                                             New Single() {in_pot(0) + sb, in_pot(1)}, hh & "c", num_bets)
                Else
                    ' Can't fold, only check or raise
                    m_children(0) = New Null()
                    ' Check
                    m_children(1) = New DecisionN(player Xor 1, dround, bb, stacks, in_pot, hh & "c", num_bets)
                End If

                ' Or raise...

            ElseIf hs(dround) = "c" Then
                ' Can't fold
                m_children(0) = New Null()

                If dround = 3 Then
                    ' Check, showdown
                    m_children(1) = New Showdown(in_pot(player))
                Else
                    ' Check, next round
                    m_children(1) = New DecisionN(1, dround + 1, bb, stacks, in_pot, hh & "c_", num_bets)
                End If

                ' Or raise...
            Else

                ' Op raised.

                m_children(0) = New Fold(player, -in_pot(player))

                If dround = 3 Then
                    ' Call and go to showdown 
                    m_children(1) = New Showdown(in_pot(player Xor 1))
                Else
                    ' Call and go to the next round
                    Dim ts() As Single = stacks.Clone(), tp() As Single = in_pot.Clone()
                    ts(player) = ts(player Xor 1)
                    tp(player) = tp(player Xor 1)
                    m_children(1) = New DecisionN(1, dround + 1, bb, ts, tp, hh & "c_", num_bets)
                End If

                ' Or, re-raise... 
            End If

            If hs(dround) = "rrrr" Or hs(dround) = "crrrr" Then

                ' End of round, can't raise anymore
                For i As Integer = 2 To m_children.Length - 1
                    m_children(i) = New Null()
                Next

                For i As Integer = 0 To m_children_values.Length - 1
                    m_children_values(i) = Double.NaN
                Next

            Else ' Raise/Re-Raise

                Dim diff As Double = in_pot(player Xor 1) - in_pot(player) ' Amount to see

                If stacks(player) - (diff + sb) <= 0 Then

                    ' Can only go all in
                    For i As Integer = 0 To num_bets - 1
                        m_children_values(i) = Double.NaN
                        m_children(2 + i) = New Null()
                    Next

                Else

                    'Dim tries As Integer = 0
                    'Dim j As Integer = 0
                    'While j < num_bets And tries < 1000
                    '    Dim v As Double = (diff + sb) + Math.Floor((stacks(player) - (diff + sb)) * rand.NextDouble())
                    '    If v < stacks(player) AndAlso (Not m_children_values.Contains(v)) Then
                    '        Dim ts() As Single = stacks.Clone(), tp() As Single = in_pot.Clone()
                    '        ts(player) -= v
                    '        tp(player) += v
                    '        m_children(2 + j) = New DecisionN(player Xor 1, dround, bb, ts, tp,
                    '                                 hh & "r", num_bets)
                    '        m_children_values(j) = v
                    '        j += 1
                    '    End If
                    '    tries += 1
                    'End While

                    '' Couldn't find an appropriate bet size (too narrow a range)
                    'If tries >= 1000 Then
                    '    For i As Integer = j To num_bets - 1
                    '        m_children_values(j) = Double.NaN
                    '        m_children(2 + i) = New Null()
                    '    Next
                    'End If

                    Dim po As New ParallelOptions
                    po.MaxDegreeOfParallelism = System.Environment.ProcessorCount

                    Parallel.For(0, num_bets, po, Sub(j, state)
                                                      Dim tries As Integer = 0
                                                      Dim r As Double
                                                      While tries < 100
                                                          r = rand.NextDouble()
                                                          Dim v As Double = (diff + sb) + Math.Floor((stacks(player) - (diff + sb)) * r)
                                                          If v < stacks(player) AndAlso (Not m_children_values.Contains(v)) Then
                                                              Dim ts() As Single = stacks.Clone(), tp() As Single = in_pot.Clone()
                                                              ts(player) -= v
                                                              tp(player) += v
                                                              m_children(2 + j) = New DecisionN(player Xor 1, dround, bb, ts, tp,
                                                                                  hh & "r", num_bets)
                                                              m_children_values(j) = v
                                                              Exit While
                                                          End If
                                                          tries += 1
                                                      End While
                                                      ' Couldn't find an appropriate bet size (probably too narrow a range)
                                                      If tries >= 100 Then
                                                          'Console.WriteLine("Couldn't find a bet size for: " & hh & vbTab & stacks(player) & vbTab &
                                                          '                  (diff + sb) + Math.Floor((stacks(player) - (diff + sb))) & vbTab & r)
                                                          m_children_values(j) = Double.NaN
                                                          m_children(2 + j) = New Null()
                                                      End If
                                                  End Sub)


                End If

                ' All in
                Dim tsa() As Single = stacks.Clone(), tpa() As Single = in_pot.Clone()
                tsa(player) = 0
                tpa(player) += stacks(player)
                m_children(m_children.Length - 1) = New DecisionN(player Xor 1, dround, bb, tsa, tpa,
                                                                  hh & "r", num_bets)

            End If

        End If

    End Sub

#End Region

#Region "Training Functions"

    Public Overrides Function TrainProbing(ByVal trainplayer As Integer, ByRef ti As train_info, ByVal ooq As Double, ByVal op As Double, ByVal probe As Boolean) As Double

        Dim nnd(m_children.Length - 1) As nn_data

        SetInputs(player, ti, nnd)

        Dim rs(m_children.Length - 1) As Double

        If (ti.m IsNot Nothing) Then
            rs = GetStrategyRaw(ti.m, nnd)
        ElseIf (ti.mxgb IsNot Nothing) Then
            rs = GetStrategyRaw(ti.mxgb, nnd)
        End If

        Dim rsn() As Double = Normalize(rs) ' if sum(rs) = 0, rsn(i) = 1/possible_actions 

        If probe Then

            Dim r As Double = GameTreeNode.rand.NextDouble()
            Dim acc As Double = 0
            For i As Integer = 0 To rsn.Length - 1
                If Not Double.IsNaN(rsn(i)) Then acc += rsn(i)
                If r < acc Then _
                    Return m_children(i).TrainProbing(trainplayer, ti, ooq, IIf(player = trainplayer, op, op * rsn(i)), probe)
            Next

        ElseIf (player = trainplayer) Then

            Dim u(m_children.Length - 1) As Double
            Dim ev As Double = 0

            For i As Integer = 0 To m_children.Length - 1

                Dim ap As Double = IIf(i = 0, 1, 0.5)

                If GameTreeNode.rand.NextDouble() < ap Then _
                    u(i) = m_children(i).TrainProbing(trainplayer, ti, ooq * ap, op, probe) _
                Else _
                    u(i) = m_children(i).TrainProbing(trainplayer, ti, ooq, op, True)

                If (Not Double.IsNaN(u(i))) AndAlso (Not Double.IsNaN(rsn(i))) Then _
                    ev += u(i) * rsn(i)
            Next

            If GameTreeNode.r_pooling_only Then
                For i As Integer = 0 To m_children.Length - 1
                    ' just U, as we're modeling EV.
                    nnd(i).outputs = New Double() {IIf(Double.IsNaN(u(i)), Double.NaN, u(i) - ev)}
                Next
            Else
                For i As Integer = 0 To m_children.Length - 1
                    If (Not Double.IsNaN(u(i))) AndAlso (Not Double.IsNaN(rs(i))) Then _
                        nnd(i).outputs = New Double() {rs(i) + (u(i) - ev)} _
                    Else _
                        nnd(i).outputs = New Double() {Double.NaN}
                Next
            End If

            ' R% is the minimum amount to save for training.
            If rand.NextDouble() + GameTreeNode.r_percent > ooq Then

                For i As Integer = 0 To nnd.Length - 1
                    If nnd(i).inputs IsNot Nothing AndAlso nnd(i).outputs IsNot Nothing AndAlso (Not Double.IsNaN(nnd(i).outputs(0))) Then
                        ti.regrets.Add((nnd(i).inputs, nnd(i).outputs))
                    End If
                Next

                If logging Then captainsLog("log_regrets.txt", player, ti, nnd, rsn, u)

            End If

            Return ev

        Else

            ' C% is the minimum amount to save for training, if saving at all.
            If GameTreeNode.c_percent > 0 AndAlso rand.NextDouble() + c_percent > ooq Then

                Dim sc(m_children.Length - 1) As Double

                If ti.c IsNot Nothing Then sc = GetStrategyRaw(ti.c, nnd)

                For i As Integer = 0 To nnd.Length - 1
                    If (Not m_children(i)._is_null) AndAlso (Not Double.IsNaN(sc(i))) AndAlso (Not Double.IsNaN(rsn(i))) Then _
                            nnd(i).outputs = New Double() {sc(i) + rsn(i)}
                Next

                For i As Integer = 0 To nnd.Length - 1
                    If nnd(i).inputs IsNot Nothing AndAlso nnd(i).outputs IsNot Nothing AndAlso Not Double.IsNaN(nnd(i).outputs(0)) Then
                        ti.cummulatives.Add((nnd(i).inputs, nnd(i).outputs))
                    End If
                Next

                If logging Then captainsLog("log_cummulative.txt", player, ti, nnd, sc)

            End If

            Dim r As Double = GameTreeNode.rand.NextDouble()
            Dim acc As Double = 0
            For i As Integer = 0 To rsn.Length - 1
                If Not Double.IsNaN(rsn(i)) Then acc += rsn(i)
                If r < acc Then Return m_children(i).TrainProbing(trainplayer, ti, ooq, op * rsn(i), probe)
            Next

        End If

        Throw New Exception("Something is rotten in the state of Denmark.")
    End Function

    Public Overrides Function TrainProbingCFU(ByVal trainplayer As Integer, ByRef ti As train_info, ByVal ooq As Double, ByVal op As Double, ByVal probe As Boolean) As Double

        Dim nnd(m_children.Length - 1) As nn_data

        SetInputs(player, ti, nnd)

        Dim rs(m_children.Length - 1) As Double

        If (ti.m IsNot Nothing) Then
            rs = GetStrategyRaw(ti.m, nnd)
        ElseIf (ti.mxgb IsNot Nothing) Then
            rs = GetStrategyRaw(ti.mxgb, nnd)
        End If

        Dim highest_i As Integer = -1
        Dim highest As Double = Double.NegativeInfinity

        If probe Then

            ' For larger games (i.e. NL) you can simply simulate out the rest of the game (without betting abstractions).

            If ti.m Is Nothing AndAlso ti.mxgb Is Nothing Then
                ' No U, so just select at random
                highest_i = rand.NewNext(0, m_children.Length)
                While m_children(highest_i)._is_null
                    highest_i = rand.NewNext(0, m_children.Length)
                End While
            Else
                For i As Integer = 0 To m_children.Length - 1
                    If (Not Double.IsNaN(rs(i))) AndAlso (Not m_children(i)._is_null) AndAlso rs(i) > highest Then
                        highest = rs(i)
                        highest_i = i
                    End If
                Next
            End If

            Return m_children(highest_i).TrainProbingCFU(trainplayer, ti, ooq, op, probe)

        ElseIf (player = trainplayer) Then

            Dim u(m_children.Length - 1) As Double

            For i As Integer = 0 To m_children.Length - 1

                Dim ap As Double = IIf(i = 0, 1, 0.5)

                If GameTreeNode.rand.NextDouble() < ap Then _
                    u(i) = m_children(i).TrainProbingCFU(trainplayer, ti, ooq * ap, op, probe) _
                Else _
                    u(i) = m_children(i).TrainProbingCFU(trainplayer, ti, ooq, op, True)

                If GameTreeNode.r_pooling_only Then
                    If ti.m Is Nothing AndAlso ti.mxgb Is Nothing AndAlso (Not Double.IsNaN(u(i))) AndAlso u(i) > highest Then
                        ' No model(s), use U
                        highest = u(i)
                        highest_i = i
                    ElseIf (Not Double.IsNaN(rs(i))) AndAlso (Not m_children(i)._is_null) AndAlso (Not Double.IsNaN(u(i))) AndAlso rs(i) > highest Then
                        highest = rs(i)
                        highest_i = i
                    End If
                    nnd(i).outputs = New Double() {IIf(Double.IsNaN(u(i)), Double.NaN, u(i))}
                Else
                    If (Not Double.IsNaN(u(i))) AndAlso (Not Double.IsNaN(rs(i))) Then
                        ' rs = 0 if no model(s) so it will just be U
                        nnd(i).outputs = New Double() {rs(i) + u(i)}
                        If rs(i) + u(i) > highest Then
                            highest = rs(i) + u(i)
                            highest_i = i
                        End If
                    Else
                        nnd(i).outputs = New Double() {Double.NaN}
                    End If
                End If
            Next

            ' R% is the minimum amount to save for training.
            If rand.NextDouble() + GameTreeNode.r_percent > ooq Then

                For i As Integer = 0 To nnd.Length - 1
                    If nnd(i).inputs IsNot Nothing AndAlso nnd(i).outputs IsNot Nothing AndAlso (Not Double.IsNaN(nnd(i).outputs(0))) Then _
                        ti.regrets.Add((nnd(i).inputs, nnd(i).outputs))
                Next

                If logging Then captainsLog("log_regrets.txt", player, ti, nnd, rs, u)

            End If

            Return u(highest_i)

        Else


            If ti.m Is Nothing AndAlso ti.mxgb Is Nothing Then
                ' No U, so just select at random
                highest_i = rand.NewNext(0, m_children.Length)
                While m_children(highest_i)._is_null
                    highest_i = rand.NewNext(0, m_children.Length)
                End While
            Else
                For i As Integer = 0 To m_children.Length - 1
                    If (Not Double.IsNaN(rs(i))) AndAlso (Not m_children(i)._is_null) AndAlso rs(i) > highest Then
                        highest = rs(i)
                        highest_i = i
                    End If
                Next
            End If

            ' Using only a percentage speeds up sampling
            If GameTreeNode.c_percent > 0 AndAlso GameTreeNode.rand.NextDouble() + GameTreeNode.c_percent > ooq Then

                Dim sc(m_children.Length - 1) As Double

                If ti.c IsNot Nothing Then
                    sc = GetStrategyRaw(ti.c, nnd)
                ElseIf ti.cxgb IsNot Nothing Then
                    sc = GetStrategyRaw(ti.cxgb, nnd)
                End If

                For i As Integer = 0 To nnd.Length - 1
                    nnd(i).outputs = New Double() {IIf(m_children(i)._is_null, Double.NaN, IIf(i = highest_i, sc(i) + (1 / ooq), sc(i)))}
                Next

                For i As Integer = 0 To nnd.Length - 1
                    If nnd(i).inputs IsNot Nothing AndAlso nnd(i).outputs IsNot Nothing AndAlso Not Double.IsNaN(nnd(i).outputs(0)) Then _
                        ti.cummulatives.Add((nnd(i).inputs, nnd(i).outputs))
                Next

                If logging Then captainsLog("log_cummulative.txt", player, ti, nnd, sc)

            End If

            Return m_children(highest_i).TrainProbingCFU(trainplayer, ti, ooq, op, probe)

        End If

        Throw New Exception("Cablewy")
    End Function

    Public Overrides Function TrainExternalSamplingCFU(ByVal trainplayer As Integer, ByRef ti As train_info) As Double

        ' For larger games, set the regrets from NN for ALL actions but only
        ' traverse a couple branches of the tree.  Select the highest regret from 
        ' either the model or the new U.

        Dim nnd(m_children.Length - 1) As nn_data

        SetInputs(player, ti, nnd)

        Dim rs(m_children.Length - 1) As Double

        If (ti.m IsNot Nothing) Then
            rs = GetStrategyRaw(ti.m, nnd)
        ElseIf (ti.mxgb IsNot Nothing) Then
            rs = GetStrategyRaw(ti.mxgb, nnd)
        End If

        Dim highest_i As Integer = -1
        Dim highest As Double = Double.NegativeInfinity

        If (player = trainplayer) Then

            Dim u(m_children.Length - 1) As Double

            For i As Integer = 0 To m_children.Length - 1
                u(i) = m_children(i).TrainExternalSamplingCFU(trainplayer, ti)
                If GameTreeNode.r_pooling_only Then
                    If ti.m Is Nothing AndAlso ti.mxgb Is Nothing AndAlso (Not Double.IsNaN(u(i))) AndAlso u(i) > highest Then
                        ' No model(s), use U
                        highest = u(i)
                        highest_i = i
                    ElseIf (Not Double.IsNaN(rs(i))) AndAlso (Not m_children(i)._is_null) AndAlso (Not Double.IsNaN(u(i))) AndAlso rs(i) > highest Then
                        highest = rs(i)
                        highest_i = i
                    End If
                    nnd(i).outputs = New Double() {IIf(Double.IsNaN(u(i)), Double.NaN, u(i))}
                Else
                    If (Not Double.IsNaN(u(i))) AndAlso (Not Double.IsNaN(rs(i))) Then
                        ' rs = 0 if no model(s) so it will just be U
                        nnd(i).outputs = New Double() {rs(i) + u(i)}
                        If rs(i) + u(i) > highest Then
                            highest = rs(i) + u(i)
                            highest_i = i
                        End If
                    Else
                        nnd(i).outputs = New Double() {Double.NaN}
                    End If
                End If
            Next

            '' Only use a percentage the updates otherwise the output will be very 'game state' heavy,
            '' as oppposed to 'card state' heavy.

            If GameTreeNode.r_percent > 0 AndAlso GameTreeNode.rand.NextDouble() <= GameTreeNode.r_percent Then
                For i As Integer = 0 To nnd.Length - 1
                    If nnd(i).inputs IsNot Nothing AndAlso nnd(i).outputs IsNot Nothing AndAlso Not Double.IsNaN(nnd(i).outputs(0)) Then
                        ti.regrets.Add((nnd(i).inputs, nnd(i).outputs))
                    End If
                Next

                If logging Then captainsLog("log_regrets.txt", player, ti, nnd, rs, u)
            End If

            Return u(highest_i)

        End If

        If ti.m Is Nothing AndAlso ti.mxgb Is Nothing Then
            ' No U, so just select at random
            highest_i = rand.NewNext(0, m_children.Length)
            While m_children(highest_i)._is_null
                highest_i = rand.NewNext(0, m_children.Length)
            End While
        Else
            For i As Integer = 0 To m_children.Length - 1
                If (Not Double.IsNaN(rs(i))) AndAlso (Not m_children(i)._is_null) AndAlso rs(i) > highest Then
                    highest = rs(i)
                    highest_i = i
                End If
            Next
        End If

        ' Using only a percentage speeds up sampling
        If GameTreeNode.c_percent > 0 AndAlso GameTreeNode.rand.NextDouble() <= GameTreeNode.c_percent Then

            Dim sc(m_children.Length - 1) As Double

            If ti.c IsNot Nothing Then
                sc = GetStrategyRaw(ti.c, nnd)
            ElseIf ti.cxgb IsNot Nothing Then
                sc = GetStrategyRaw(ti.cxgb, nnd)
            End If

            For i As Integer = 0 To nnd.Length - 1
                nnd(i).outputs = New Double() {IIf(m_children(i)._is_null, Double.NaN, IIf(i = highest_i, sc(i) + 1, sc(i)))}
            Next

            For i As Integer = 0 To nnd.Length - 1
                If nnd(i).inputs IsNot Nothing AndAlso nnd(i).outputs IsNot Nothing AndAlso Not Double.IsNaN(nnd(i).outputs(0)) Then
                    ti.cummulatives.Add((nnd(i).inputs, nnd(i).outputs))
                End If
            Next

            If logging Then captainsLog("log_cummulative.txt", player, ti, nnd, sc)

        End If

        Return m_children(highest_i).TrainExternalSamplingCFU(trainplayer, ti)

    End Function

    ' These Best Response functions are untested at the moment.
    Public Function BestResponseTotalCFU(ByVal brplayer As Integer, ByRef mxgb As SharpLearning.XGBoost.Models.RegressionXGBoostModel,
                                         ByRef hi As List(Of single_hand_info)) As Double
        Dim op(hi.Count - 1) As Double
        Dim sum As Double = 0

        For hand As Integer = 0 To hi.Count - 1
            For i As Integer = 0 To hi.Count - 1
                op(i) = IIf(i = hand, 0, 1 / (hi.Count - 1))
            Next
            sum += BestResponse(brplayer, mxgb, hi, hand, op)
        Next

        Return sum
    End Function

    Public Overrides Function BestResponse(ByVal brplayer As Integer, ByRef mxgb As SharpLearning.XGBoost.Models.RegressionXGBoostModel,
                                           ByRef hi As List(Of single_hand_info), ByVal hand As Integer, ByVal op() As Double) As Double
        Dim ev As Double = 0

        If player = brplayer Then
            Dim bestev As Double = Double.NegativeInfinity
            For i As Integer = 0 To m_children.Length - 1
                ev = m_children(i).BestResponse(brplayer, mxgb, hi, hand, op)
                If Not Double.IsNaN(ev) AndAlso ev > bestev Then bestev = ev
            Next
            Return bestev
        End If

        For i As Integer = 0 To m_children.Length - 1
            Dim newop(hi.Count - 1) As Double

            For h As Integer = 0 To hi.Count - 1
                If op(h) = 0 Then
                    newop(h) = 0
                Else
                    Dim nnd(m_children.Length - 1) As nn_data
                    SetInputs(player, hi(h), nnd)
                    Dim rs() As Double = GetStrategyRaw(mxgb, nnd)
                    Dim highest_j As Integer = m_children.Length - 1
                    Dim highest As Double = Double.NegativeInfinity
                    For j As Integer = 0 To m_children.Length - 1
                        If (Not m_children(j)._is_null) AndAlso (Not Double.IsNaN(rs(j))) Then
                            If rs(j) > highest Then
                                highest = rs(j)
                                highest_j = j
                            End If
                        End If
                    Next
                    newop(h) = IIf(i = highest_j, 1, 0) * op(h)
                End If
            Next

            Dim br As Double = m_children(i).BestResponse(brplayer, mxgb, hi, hand, newop)
            If Not Double.IsNaN(br) Then ev += br
        Next

        Return ev
    End Function

#End Region

#Region "Misc Functions"

    Public Function Normalize(ByVal s() As Double) As Double()
        ' Need to query more than one model and add the regrets, then normalize

        Dim sum As Double = 0
        Dim count As Integer = 0

        For i As Integer = 0 To s.Length - 1
            If Double.IsNaN(s(i)) Then
            ElseIf s(i) > 0 Then
                sum += s(i)
                count += 1
            Else
                count += 1
            End If
        Next

        Dim out(s.Length - 1) As Double

        If sum > 0 Then
            For i As Integer = 0 To s.Length - 1
                If Double.IsNaN(s(i)) Then
                    out(i) = Double.NaN
                ElseIf s(i) > 0 Then
                    out(i) = s(i) / sum
                Else
                    out(i) = 0
                End If
            Next
        Else
            For i As Integer = 0 To s.Length - 1
                If Double.IsNaN(s(i)) Then out(i) = Double.NaN Else out(i) = 1 / count
            Next
        End If

        Return out
    End Function

    Public Function GetStrategyRaw(ByRef m As Keras.Models.Sequential, ByRef nnd() As nn_data, Optional ByVal init As Boolean = False) As Double()
        ' Need to query more than one model and add the regrets, then normalize

        Dim s(m_children.Length - 1) As Double
        Dim count As Integer = 0
        Dim il As Integer = -1

        For i As Integer = 0 To m_children.Length - 1
            If (Not m_children(i)._is_null) AndAlso (nnd(i).inputs IsNot Nothing) Then
                il = Math.Max(il, nnd(i).inputs.Length)
                count += 1
            End If
        Next

        If init OrElse m Is Nothing Then
            For i As Integer = 0 To m_children.Length - 1
                If Not m_children(i)._is_null Then s(i) = 1 / count Else s(i) = Double.NaN
            Next
            Return s
        End If

        Dim in2d(count, il - 1) As Double

        count = 0
        For i As Integer = 0 To m_children.Length - 1
            If (Not m_children(i)._is_null) AndAlso (nnd(i).inputs IsNot Nothing) Then
                For j As Integer = 0 To nnd(i).inputs.Length - 1
                    in2d(count, j) = nnd(i).inputs(j)
                Next
                count += 1
            Else
                s(i) = Double.NaN
            End If
        Next

        Dim y As Numpy.NDarray = m.Predict(Numpy.np.array(in2d), verbose:=0)

        count = 0
        For i As Integer = 0 To m_children.Length - 1
            If (Not m_children(i)._is_null) AndAlso (nnd(i).inputs IsNot Nothing) Then
                s(i) = y(count)(0)
                count += 1
            Else
                s(i) = Double.NaN
            End If
        Next

        Return s
    End Function

    Public Function GetStrategyRaw(ByRef m As SharpLearning.XGBoost.Models.RegressionXGBoostModel, ByRef nnd() As nn_data, Optional ByVal init As Boolean = False) As Double()
        ' Need to query more than one model and add the regrets, then normalize

        Dim s(m_children.Length - 1) As Double
        Dim count As Integer = 0
        Dim il As Integer = -1

        For i As Integer = 0 To m_children.Length - 1
            If (Not m_children(i)._is_null) AndAlso (nnd(i).inputs IsNot Nothing) Then
                il = Math.Max(il, nnd(i).inputs.Length)
                count += 1
            End If
        Next

        If init OrElse m Is Nothing Then
            For i As Integer = 0 To m_children.Length - 1
                If Not m_children(i)._is_null Then s(i) = 1 / count Else s(i) = Double.NaN
            Next
            Return s
        End If

        For i As Integer = 0 To m_children.Length - 1
            If (Not m_children(i)._is_null) AndAlso (nnd(i).inputs IsNot Nothing) Then
                s(i) = m.Predict(nnd(i).inputs)
            Else
                s(i) = Double.NaN
            End If
        Next

        Return s
    End Function

    Public Sub SetInputs(ByVal trainplayer As Integer, ByRef ti As train_info, ByRef nnd() As nn_data)
        Dim comb As New List(Of Double)

        ' Game State
        comb.AddRange(bet_histories.Clone())

        ' Hand Info

        comb.AddRange(ti.pin(trainplayer).Clone()) ' Pocket

        If tround > 0 Then
            comb.AddRange(ti.fin(trainplayer).Clone()) ' Flop
        Else
            For i As Integer = 1 To 214 : comb.Add(-1) : Next
        End If

        If tround > 1 Then
            comb.AddRange(ti.tin(trainplayer).Clone()) ' Turn
        Else
            For i As Integer = 1 To 147 : comb.Add(-1) : Next
        End If

        If tround > 2 Then
            comb.AddRange(ti.rin(trainplayer).Clone()) ' River
        Else
            For i As Integer = 1 To 68 : comb.Add(-1) : Next
        End If

        ' Fold, Check/Call, and %s' of stack (in BB)

        comb.AddRange(New Double() {-1, -1})

        For i As Integer = 0 To m_children_values.Length - 1 : comb.Add(-1) : Next

        If Not m_children(0)._is_null Then
            nnd(0).inputs = comb.ToArray().Clone()
            nnd(0).inputs(nnd(0).inputs.Length - 3) = 1 ' Fold Flag
        Else
            nnd(0).inputs = Nothing
        End If

        If Not m_children(1)._is_null Then
            nnd(1).inputs = comb.ToArray().Clone()
            nnd(1).inputs(nnd(1).inputs.Length - 2) = 1 ' Check/Call Flag
        Else
            nnd(1).inputs = Nothing
        End If

        For i As Integer = 0 To m_children_values.Length - 1
            If Not m_children(2 + i)._is_null Then
                nnd(2 + i).inputs = comb.ToArray().Clone()
                nnd(2 + i).inputs(nnd(2 + i).inputs.Length - i - 1) = m_children_values(i) ' Raise-to/Stack
            Else
                nnd(2 + i).inputs = Nothing
            End If
        Next

    End Sub

    Public Sub SetInputs(ByVal trainplayer As Integer, ByRef sh As single_hand_info, ByRef nnd() As nn_data)
        Dim comb As New List(Of Double)

        ' Game State
        comb.AddRange(bet_histories.Clone())

        ' Hand Info

        comb.AddRange(sh.pin.Clone()) ' Pocket

        If tround > 0 Then
            comb.AddRange(sh.fin.Clone()) ' Flop
        Else
            For i As Integer = 1 To 214 : comb.Add(-1) : Next
        End If

        If tround > 1 Then
            comb.AddRange(sh.tin.Clone()) ' Turn
        Else
            For i As Integer = 1 To 147 : comb.Add(-1) : Next
        End If

        If tround > 2 Then
            comb.AddRange(sh.rin.Clone()) ' River
        Else
            For i As Integer = 1 To 68 : comb.Add(-1) : Next
        End If

        ' Fold, Check/Call, and %s' of stack (in BB)

        comb.AddRange(New Double() {-1, -1})

        For i As Integer = 0 To m_children_values.Length - 1 : comb.Add(-1) : Next

        If Not m_children(0)._is_null Then
            nnd(0).inputs = comb.ToArray().Clone()
            nnd(0).inputs(nnd(0).inputs.Length - 3) = 1 ' Fold Flag
        Else
            nnd(0).inputs = Nothing
        End If

        If Not m_children(1)._is_null Then
            nnd(1).inputs = comb.ToArray().Clone()
            nnd(1).inputs(nnd(1).inputs.Length - 2) = 1 ' Check/Call Flag
        Else
            nnd(1).inputs = Nothing
        End If

        For i As Integer = 0 To m_children_values.Length - 1
            If Not m_children(2 + i)._is_null Then
                nnd(2 + i).inputs = comb.ToArray().Clone()
                nnd(2 + i).inputs(nnd(2 + i).inputs.Length - i - 1) = m_children_values(i) ' Raise-to/Stack
            Else
                nnd(2 + i).inputs = Nothing
            End If
        Next

    End Sub

    Public Sub captainsLog(ByVal log_file As String, ByVal lplayer As Integer, ByRef ti As train_info, ByRef nnd() As nn_data, ByRef s() As Double, Optional ByRef u() As Double = Nothing)
        Dim log As String = lplayer & vbTab & m_node_string & vbTab

        log &= HoldemHand.Hand.MaskToString(ti.pocket(lplayer)) & vbTab

        Select Case tround
            Case 0 : log &= vbTab : Exit Select
            Case 1 : log &= HoldemHand.Hand.MaskToString(ti.flop) & vbTab : Exit Select
            Case 2 : log &= HoldemHand.Hand.MaskToString(ti.turn) & vbTab : Exit Select
            Case 3 : log &= HoldemHand.Hand.MaskToString(ti.river) & vbTab : Exit Select
        End Select

        For i As Integer = 0 To nnd.Length - 1
            If nnd(i).inputs Is Nothing Or Double.IsNaN(nnd(i).outputs(0)) Then
                log &= "NaN"
            Else
                log &= nnd(i).outputs(0)
            End If
            If i < nnd.Length - 1 Then log &= ", "
        Next

        log &= vbCrLf

        ' Just use the call inputs for now and show all outputs...
        log &= String.Join(",", nnd(1).inputs) & vbTab

        For i As Integer = 0 To s.Length - 1
            If Double.IsNaN(s(i)) Then
                log &= "NaN"
            Else
                log &= s(i)
            End If
            If i < s.Length - 1 Then log &= ", "
        Next

        If u IsNot Nothing Then
            log &= vbTab
            For i As Integer = 0 To u.Length - 1
                If Double.IsNaN(u(i)) Then
                    log &= "NaN"
                Else
                    log &= u(i)
                End If
                If i < u.Length - 1 Then log &= ", "
            Next
        End If

        Try
            System.IO.File.AppendAllText(log_file, log & vbCrLf & vbCrLf)
        Catch ex As Exception

        End Try

    End Sub

#End Region

End Class
