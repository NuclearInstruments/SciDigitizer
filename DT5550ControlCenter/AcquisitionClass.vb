Public Class AcquisitionClass


    Public General_settings As New GeneralSettings
    Public CHList As New List(Of Channel)
    Public fit_list As New List(Of Fitting)
    Public currentMAP As New MAP
    Dim x As Integer = 0
    Dim y As Integer = 0

    Public Class MAP

        Public positions(,) As String
        Public rows As Integer
        Public cols As Integer
        Public list() As String

        Public Sub New()
        End Sub

        Public Sub New(rows, cols, channels)

            Me.rows = rows
            Me.cols = cols
            ReDim positions(rows - 1, cols - 1)
            ReDim list(rows * cols - 1)
            Dim k = 0
            For i = 0 To rows - 1
                For j = 0 To cols - 1
                    If i * cols + j + 1 <= channels Then
                        positions(i, j) = i * cols + j + 1
                        list(k) = (i * cols + j + 1).ToString
                        k += 1
                    Else
                        positions(i, j) = "NC"
                    End If
                Next
            Next

        End Sub

    End Class

    Enum signal_polarity
        POSITIVE = 0
        NEGATIVE = 1
    End Enum

    Enum trigger_mode
        THRESHOLD = 1
        DERIVATIVE = 0
    End Enum

    Enum trigger_source
        FREE = 2
        EXTERNAL = 1
        INTERNAL = 0
        LEVEL = 3
    End Enum

    Enum sampling_method
        INDIPENDENT = 0
        COMMON = 1
    End Enum

    Enum edge
        RISING = 0
        FALLING = 1
    End Enum

    Structure GeneralSettings

        Public AFEImpedance As Boolean
        Public AFEOffset As Double
        Public SignalOffset As Double
        Public TriggerMode As trigger_mode
        Public TriggerSource As trigger_source
        Public Sampling As sampling_method
        Public TriggerDelay As Double
        Public TriggerSourceOscilloscope As trigger_source
        Public TriggerChannelOscilloscope As Int32
        Public TriggerOscilloscopeEdges As edge
        Public TriggerOscilloscopeLevel As Double
        Public OscilloscopeDecimator As Double
        Public OscilloscopePreTrigger As Double
        Public AFEShaper As Integer
    End Structure

    Structure AFESettings
        Public Termination As Boolean
        Public Division As Boolean
        Public Offset As Integer
        Public Gain As Double
    End Structure

    Public Class Channel

        Enum energy_filter_mode
            INTEGRATION = 0
            TRAPEZOIDAL = 1
        End Enum

        Public name As String
        Public id As Integer
        Public ch_id As Integer
        Public board_number As Integer
        Public x_position As Integer
        Public y_position As Integer
        Public polarity As signal_polarity
        Public offset As Double
        Public trigger_level As Double
        Public trigger_peaking As Double
        Public trigger_flat As Double
        Public trigger_inhibit As Double
        Public energy_filter As energy_filter_mode
        Public decay_constant As Double
        Public peaking_time As Double
        Public flat_top As Double
        Public energy_sample As Double
        Public gain As Double
        Public integration_time As Double
        Public pre_gate As Double
        Public pileup_enable As Boolean
        Public pileup_time As Double
        Public baseline_inhibit As Double
        Public baseline_sample As Integer
        Public spectra_checked As Boolean
        Public scope_checked As Boolean
        Public Afe_set As AFESettings
        Public TriggerOscilloscopeLevel As Integer

        Public Sub New(name As String, id As Integer, ch_id As Integer, x As Integer, y As Integer, board_type As communication.tModel, board_number As Integer)

            If board_type = communication.tModel.DT5550 Then
                Me.name = name
                Me.id = id
                Me.id = ch_id
                Me.ch_id = ch_id
                Me.board_number = board_number
                Me.x_position = x
                Me.y_position = y
                Me.polarity = signal_polarity.POSITIVE
                Me.offset = 0
                Me.trigger_level = 10000
                Me.trigger_peaking = 0
                Me.trigger_flat = 0
                Me.trigger_inhibit = 1000
                Me.energy_filter = energy_filter_mode.INTEGRATION
                Me.decay_constant = 700
                Me.peaking_time = 1300
                Me.flat_top = 100
                Me.energy_sample = 1600
                Me.gain = 10
                Me.integration_time = 1000.0
                Me.pre_gate = 200
                Me.pileup_enable = True
                Me.pileup_time = 1000
                Me.baseline_inhibit = 2500
                Me.baseline_sample = 256
                spectra_checked = False
                scope_checked = False
            ElseIf board_type = communication.tModel.R5560 Then
                Me.name = name
                Me.id = id
                Me.ch_id = ch_id
                Me.board_number = board_number
                Me.x_position = x
                Me.y_position = y
                Me.polarity = signal_polarity.POSITIVE
                Me.offset = 0
                Me.trigger_level = 20
                Me.trigger_peaking = 40
                Me.trigger_flat = 8
                Me.trigger_inhibit = 0
                Me.energy_filter = energy_filter_mode.TRAPEZOIDAL
                Me.decay_constant = 1000
                Me.peaking_time = 800
                Me.flat_top = 80
                Me.energy_sample = 864
                Me.gain = 2
                Me.integration_time = 0
                Me.pre_gate = 200
                Me.pileup_enable = False
                Me.pileup_time = 0
                Me.baseline_inhibit = 8000
                Me.baseline_sample = 256
                Me.Afe_set.Termination = True
                Me.Afe_set.Division = True
                Me.Afe_set.Offset = 0
                Me.Afe_set.Gain = 1
                spectra_checked = False
                scope_checked = False
            ElseIf board_type = communication.tModel.R5560 Or board_type = communication.tModel.DT5560SE Or board_type = communication.tModel.R5560SE Then
                Me.name = name
                Me.id = id
                Me.ch_id = ch_id
                Me.board_number = board_number
                Me.x_position = x
                Me.y_position = y
                Me.polarity = signal_polarity.POSITIVE
                Me.offset = 0
                Me.trigger_level = 20
                Me.trigger_peaking = 40
                Me.trigger_flat = 8
                Me.trigger_inhibit = 0
                Me.energy_filter = energy_filter_mode.TRAPEZOIDAL
                Me.decay_constant = 1000
                Me.peaking_time = 800
                Me.flat_top = 80
                Me.energy_sample = 864
                Me.gain = 2
                Me.integration_time = 0
                Me.pre_gate = 200
                Me.pileup_enable = False
                Me.pileup_time = 0
                Me.baseline_inhibit = 8000
                Me.baseline_sample = 256
                Me.Afe_set.Termination = True
                Me.Afe_set.Division = True
                Me.Afe_set.Offset = 0
                Me.Afe_set.Gain = 1
                Me.TriggerOscilloscopeLevel = 9000
                spectra_checked = False
                scope_checked = False
            ElseIf board_type = communication.tModel.SCIDK Then
                Me.name = name
                Me.id = id
                Me.id = ch_id
                Me.ch_id = ch_id
                Me.board_number = board_number
                Me.x_position = x
                Me.y_position = y
                Me.polarity = signal_polarity.POSITIVE
                Me.offset = 0
                Me.trigger_level = 25
                Me.trigger_peaking = 100
                Me.trigger_flat = 120
                Me.trigger_inhibit = 1000
                Me.energy_filter = energy_filter_mode.TRAPEZOIDAL
                Me.decay_constant = 50000
                Me.peaking_time = 1300
                Me.flat_top = 100
                Me.energy_sample = 1370
                Me.gain = 0.5
                Me.integration_time = 0
                Me.pre_gate = 200
                Me.pileup_enable = True
                Me.pileup_time = 1000
                Me.baseline_inhibit = 2500
                Me.baseline_sample = 256
                spectra_checked = False
                scope_checked = False
            End If
        End Sub

    End Class

    Public Sub New(nch As Integer, board_type As communication.tModel, nBoard As Integer)

        If board_type = communication.tModel.DT5550 Then
            General_settings.AFEImpedance = True
            General_settings.AFEOffset = 0
            General_settings.SignalOffset = 0
            General_settings.TriggerMode = trigger_mode.THRESHOLD
            General_settings.TriggerSource = trigger_source.INTERNAL
            General_settings.Sampling = sampling_method.COMMON
            General_settings.TriggerDelay = 0
            General_settings.TriggerSourceOscilloscope = trigger_source.FREE
            General_settings.TriggerChannelOscilloscope = 0
            General_settings.TriggerOscilloscopeEdges = edge.RISING
            General_settings.TriggerOscilloscopeLevel = 10000
            General_settings.OscilloscopeDecimator = 1
            General_settings.OscilloscopePreTrigger = 20
            For i = 0 To nch - 1
                FindPosition(16, i + 1, x, y)
                Dim ch = New Channel("CHANNEL " + (i + 1).ToString, i + 1, i + 1, x, y, board_type, 1)
                CHList.Add(ch)
            Next
            currentMAP = New MAP(2, 16, nch)
        ElseIf board_type = communication.tModel.R5560 Then
            General_settings.TriggerSourceOscilloscope = trigger_source.FREE
            General_settings.TriggerChannelOscilloscope = 0
            General_settings.TriggerOscilloscopeEdges = edge.RISING
            General_settings.TriggerOscilloscopeLevel = 10000
            General_settings.OscilloscopeDecimator = 1
            General_settings.OscilloscopePreTrigger = 20

            For b = 0 To nBoard - 1
                For i = 0 To nch - 1
                    FindPosition(16, (i + 1) + (b * nch), x, y)
                    Dim ch = New Channel("CHANNEL " + ((i + 1) + (b * nch)).ToString, (i + 1) + (b * nch), i + 1, x, y, board_type, b)
                    CHList.Add(ch)
                Next
            Next
            currentMAP = New MAP(2 * nBoard, 16, nch * nBoard)
        ElseIf board_type = communication.tModel.DT5560SE Or board_type = communication.tModel.R5560SE Then
            General_settings.TriggerSourceOscilloscope = trigger_source.FREE
            General_settings.TriggerChannelOscilloscope = 0
            General_settings.TriggerOscilloscopeEdges = edge.RISING
            General_settings.TriggerOscilloscopeLevel = 10000
            General_settings.OscilloscopeDecimator = 1
            General_settings.OscilloscopePreTrigger = 20
            General_settings.AFEShaper = 0
            For b = 0 To nBoard - 1
                For i = 0 To nch - 1
                    'FindPosition(16, i + 1, x, y)
                    'Dim ch = New Channel("CHANNEL " + (i).ToString, i + 1, i + 1, x, y, board_type, 1)
                    FindPosition(16, (i + 1) + (b * nch), x, y)
                    Dim ch = New Channel("CHANNEL " + ((i + 1) + (b * nch)).ToString, (i + 1) + (b * nch), i + 1, x, y, board_type, b + 1)
                    CHList.Add(ch)
                Next
            Next
            'currentMAP = New MAP(2, 16, nch)
            currentMAP = New MAP(2 * nBoard, 16, nch * nBoard)

        ElseIf board_type = communication.tModel.SCIDK Then

            General_settings.SignalOffset = 0
            General_settings.TriggerMode = trigger_mode.THRESHOLD
            General_settings.TriggerSource = trigger_source.INTERNAL
            General_settings.Sampling = sampling_method.COMMON
            General_settings.TriggerDelay = 0
            General_settings.TriggerSourceOscilloscope = trigger_source.FREE
            General_settings.TriggerChannelOscilloscope = 0
            General_settings.TriggerOscilloscopeEdges = edge.RISING
            General_settings.TriggerOscilloscopeLevel = 2200
            General_settings.OscilloscopeDecimator = 1
            General_settings.OscilloscopePreTrigger = 20
            For i = 0 To nch - 1

                Dim ch = New Channel("CHANNEL " + (i + 1).ToString, i + 1, i + 1, i, 0, board_type, 1)
                CHList.Add(ch)
            Next
            currentMAP = New MAP(1, 2, nch)
        End If

    End Sub

    Public Sub New()
    End Sub

    Public Sub FindPosition(cols As Integer, index As Integer, ByRef x_pos As Integer, ByRef y_pos As Integer)

        Dim risultato = Math.Floor(index / cols)
        Dim resto = index Mod cols
        If resto = 0 Then
            x_pos = cols - 1
            y_pos = risultato - 1
        Else
            x_pos = resto - 1
            y_pos = risultato
        End If

    End Sub

    Public Class Fitting

        Public cursor1 As Double
        Public cursor2 As Double
        Public channel_number As String

    End Class


End Class
