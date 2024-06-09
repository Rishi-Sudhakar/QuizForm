Imports System.Net
Imports Newtonsoft.Json

Public Class QuizForm
    Inherits Form

    Private lblQuestion As Label
    Private rbtnOption1 As RadioButton
    Private rbtnOption2 As RadioButton
    Private rbtnOption3 As RadioButton
    Private rbtnOption4 As RadioButton
    Private btnNext As Button

    Private currentQuestionIndex As Integer = 0
    Private questions As List(Of Question)
    Private correctAnswers As Integer = 0

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Me.lblQuestion = New Label()
        Me.rbtnOption1 = New RadioButton()
        Me.rbtnOption2 = New RadioButton()
        Me.rbtnOption3 = New RadioButton()
        Me.rbtnOption4 = New RadioButton()
        Me.btnNext = New Button()

        Me.SuspendLayout()

        ' lblQuestion
        Me.lblQuestion.Location = New Point(13, 13)
        Me.lblQuestion.Name = "lblQuestion"
        Me.lblQuestion.Size = New Size(775, 23)
        Me.lblQuestion.TabIndex = 0
        Me.lblQuestion.Text = "Question"

        ' rbtnOption1
        Me.rbtnOption1.Location = New Point(13, 40)
        Me.rbtnOption1.Name = "rbtnOption1"
        Me.rbtnOption1.Size = New Size(775, 24)
        Me.rbtnOption1.TabIndex = 1
        Me.rbtnOption1.TabStop = True
        Me.rbtnOption1.Text = "Option 1"
        Me.rbtnOption1.UseVisualStyleBackColor = True

        ' rbtnOption2
        Me.rbtnOption2.Location = New Point(13, 70)
        Me.rbtnOption2.Name = "rbtnOption2"
        Me.rbtnOption2.Size = New Size(775, 24)
        Me.rbtnOption2.TabIndex = 2
        Me.rbtnOption2.TabStop = True
        Me.rbtnOption2.Text = "Option 2"
        Me.rbtnOption2.UseVisualStyleBackColor = True

        ' rbtnOption3
        Me.rbtnOption3.Location = New Point(13, 100)
        Me.rbtnOption3.Name = "rbtnOption3"
        Me.rbtnOption3.Size = New Size(775, 24)
        Me.rbtnOption3.TabIndex = 3
        Me.rbtnOption3.TabStop = True
        Me.rbtnOption3.Text = "Option 3"
        Me.rbtnOption3.UseVisualStyleBackColor = True

        ' rbtnOption4
        Me.rbtnOption4.Location = New Point(13, 130)
        Me.rbtnOption4.Name = "rbtnOption4"
        Me.rbtnOption4.Size = New Size(775, 24)
        Me.rbtnOption4.TabIndex = 4
        Me.rbtnOption4.TabStop = True
        Me.rbtnOption4.Text = "Option 4"
        Me.rbtnOption4.UseVisualStyleBackColor = True

        ' btnNext
        Me.btnNext.Location = New Point(13, 160)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New Size(75, 23)
        Me.btnNext.TabIndex = 5
        Me.btnNext.Text = "Next"
        Me.btnNext.UseVisualStyleBackColor = True

        ' QuizForm
        Me.AutoScaleDimensions = New SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.ClientSize = New Size(800, 450)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.rbtnOption4)
        Me.Controls.Add(Me.rbtnOption3)
        Me.Controls.Add(Me.rbtnOption2)
        Me.Controls.Add(Me.rbtnOption1)
        Me.Controls.Add(Me.lblQuestion)
        Me.Name = "QuizForm"
        Me.Text = "QuizForm"
        Me.ResumeLayout(False)

        AddHandler Me.Load, AddressOf QuizForm_Load
        AddHandler Me.btnNext.Click, AddressOf btnNext_Click
    End Sub

    Private Sub QuizForm_Load(sender As Object, e As EventArgs)
        FetchQuestions()
    End Sub

    Private Sub FetchQuestions()
        Using client As New WebClient()
            Dim json As String = client.DownloadString("https://opentdb.com/api.php?amount=10")
            Dim triviaResponse As TriviaResponse = JsonConvert.DeserializeObject(Of TriviaResponse)(json)
            questions = triviaResponse.results
            DisplayQuestion()
        End Using
    End Sub

    Private Sub DisplayQuestion()
        If currentQuestionIndex >= questions.Count Then
            MessageBox.Show("Quiz completed! Correct answers: " & correctAnswers)
            Me.Close()
            Return
        End If

        Dim question As Question = questions(currentQuestionIndex)
        lblQuestion.Text = WebUtility.HtmlDecode(question.question)
        rbtnOption1.Text = WebUtility.HtmlDecode(question.correct_answer)
        rbtnOption2.Text = WebUtility.HtmlDecode(question.incorrect_answers(0))
        rbtnOption3.Text = WebUtility.HtmlDecode(question.incorrect_answers(1))
        rbtnOption4.Text = WebUtility.HtmlDecode(question.incorrect_answers(2))
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs)
        Dim selectedAnswer As String = If(rbtnOption1.Checked, rbtnOption1.Text, If(rbtnOption2.Checked, rbtnOption2.Text, If(rbtnOption3.Checked, rbtnOption3.Text, If(rbtnOption4.Checked, rbtnOption4.Text, ""))))

        If selectedAnswer = questions(currentQuestionIndex).correct_answer Then
            correctAnswers += 1
        End If

        currentQuestionIndex += 1
        DisplayQuestion()
    End Sub

    Public Class TriviaResponse
        Public Property response_code As Integer
        Public Property results As List(Of Question)
    End Class

    Public Class Question
        Public Property category As String
        Public Property type As String
        Public Property difficulty As String
        Public Property question As String
        Public Property correct_answer As String
        Public Property incorrect_answers As List(Of String)
    End Class
End Class
