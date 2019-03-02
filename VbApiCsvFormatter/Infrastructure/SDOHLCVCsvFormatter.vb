Option Strict On
Option Explicit On

Imports System.IO
Imports System.Net.Http
Imports System.Net.Http.Formatting

Public Class SDOHLCVCsvFormatter
    Inherits BufferedMediaTypeFormatter

    Public Sub New()
        SupportedMediaTypes.Add(New System.Net.Http.Headers.MediaTypeHeaderValue("text/csv"))
        SupportedEncodings.Add(New UTF8Encoding(False))
        SupportedEncodings.Add(Encoding.GetEncoding("iso-8859-1"))
    End Sub

    Public Overrides Function CanReadType(type As Type) As Boolean
        Return False
    End Function

    Public Overrides Function CanWriteType(type As Type) As Boolean
        If type.GetType() Is GetType(SDOHLCV) Then
            Return True
        Else
            Dim enumerableType As Type = GetType(IEnumerable(Of SDOHLCV))
            Return enumerableType.IsAssignableFrom(type)
        End If
    End Function

    Public Overrides Sub WriteToStream(ByVal type As Type, ByVal value As Object, ByVal writeStream As Stream, ByVal content As HttpContent)
        Dim effectiveEncoding As Encoding = SelectCharacterEncoding(content.Headers)

        Using writer = New StreamWriter(writeStream)
            Dim SDOHLCVs = TryCast(value, IEnumerable(Of SDOHLCV))

            If SDOHLCVs IsNot Nothing Then

                For Each SDOHLCV In SDOHLCVs
                    WriteItem(SDOHLCV, writer)
                Next
            Else
                Dim singleSDOHLCV = TryCast(value, SDOHLCV)

                If singleSDOHLCV Is Nothing Then
                    Throw New InvalidOperationException("Cannot serialize type")
                End If

                WriteItem(singleSDOHLCV, writer)
            End If
        End Using
    End Sub

    Private Sub WriteItem(ByVal SDOHLCV As SDOHLCV, ByVal writer As StreamWriter)
        writer.WriteLine("{0},{1},{2},{3},{4},{5},{6}", Escape(SDOHLCV.nSymbol), Escape(SDOHLCV.nDate), Escape(SDOHLCV.nOpen), Escape(SDOHLCV.nHigh), Escape(SDOHLCV.nLow), Escape(SDOHLCV.nClose), Escape(SDOHLCV.nVolume))
    End Sub


    Shared _specialChars As Char() = New Char() {","c, Convert.ToChar(vbLf), Convert.ToChar(vbCr), """"c}

    Private Function Escape(ByVal o As Object) As String
        If o Is Nothing Then
            Return ""
        End If

        Dim field As String = o.ToString()

        If field.IndexOfAny(_specialChars) <> -1 Then
            Return String.Format("""{0}""", field.Replace("""", """"""))
        Else
            Return field
        End If
    End Function

End Class
